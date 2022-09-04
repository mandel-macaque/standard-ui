using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.StandardUI.SourceGenerator
{
    /// <summary>
    /// Generates framework-specific implementations of StandardUI control interfaces. The
    /// consuming app specifies the control interfaces that it consumes via StandardUIControl
    /// assembly attributes.
    /// </summary>
    /// <example>
    /// [assembly: StandardUIControl("Namespace.IControlName")]
    /// </example>
    internal abstract class SourceGeneratorBase<TGenerateInput> : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
        {
            // Enable this to be able to debug the source generators
#if false
            if (!Debugger.IsAttached)
                Debugger.Launch();
#endif

            IncrementalValuesProvider<TGenerateInput> syntaxProvider = initializationContext.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => Filter(node),
                    transform: (generatorSyntaxContext, _) => Transform(generatorSyntaxContext.SemanticModel, generatorSyntaxContext.Node))
                .Where(static transformResult => transformResult is not null)!;

            IncrementalValueProvider<(Compilation, ImmutableArray<TGenerateInput>)> generateInputProvider
                = initializationContext.CompilationProvider.Combine(syntaxProvider.Collect());

            initializationContext.RegisterSourceOutput(generateInputProvider,
                (sourceProductioContext, source) => GenerateAction(source.Item1, source.Item2, sourceProductioContext));
        }

        protected abstract bool Filter(SyntaxNode node);

        protected abstract TGenerateInput? Transform(SemanticModel semanticModel, SyntaxNode node);

        protected abstract void Generate(Context context, ImmutableArray<TGenerateInput> generateInput);

        private void GenerateAction(Compilation compilation, ImmutableArray<TGenerateInput> generateInput, SourceProductionContext sourceProductionContext)
        {
            try
            {
                if (generateInput.IsDefaultOrEmpty)
                    return;

                Context context = new Context(compilation, new GeneratorExecutionOutput(sourceProductionContext));
                Generate(context, generateInput);
            }
            catch (UserViewableException e)
            {
                var diagnosticDescriptor = new DiagnosticDescriptor(e.Id, "StandardUI source generation failed",
                    e.Message, Utils.StandardUIRootNamespace, DiagnosticSeverity.Error, isEnabledByDefault: true);
                sourceProductionContext.ReportDiagnostic(Diagnostic.Create(diagnosticDescriptor, e.Location));
            }
            catch (Exception e)
            {
                var diagnosticDescriptor = new DiagnosticDescriptor(UserVisibleErrors.InternalErrorId, "StandardUI source generation failed with internal error",
                    e.ToString(), Utils.StandardUIRootNamespace, DiagnosticSeverity.Error, isEnabledByDefault: true);
                sourceProductionContext.ReportDiagnostic(Diagnostic.Create(diagnosticDescriptor, null));
            }
        }

        public static string? GetAttributeFullTypeName(SemanticModel semanticModel, AttributeSyntax attributeSyntax)
        {
            if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                return null;

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            return attributeContainingTypeSymbol.ToDisplayString();
        }

        public static INamedTypeSymbol? GetAttributeTypeArgument(SemanticModel semanticModel, AttributeSyntax attributeSyntax, int argIndex)
        {
            ExpressionSyntax? attributeArg = attributeSyntax.ArgumentList!.Arguments[argIndex].Expression;
            if (attributeArg is not TypeOfExpressionSyntax typeOfExpressionSyntax)
                return null;

            TypeSyntax? typeArgument = typeOfExpressionSyntax.Type;
            return semanticModel.GetSymbolInfo(typeArgument).Symbol as INamedTypeSymbol;
        }

        public static INamedTypeSymbol? GetAttributedType(SemanticModel semanticModel, AttributeSyntax attributeSyntax)
        {
            SyntaxNode? attributedTypeSyntax = attributeSyntax.Parent;
            if (attributedTypeSyntax == null)
                return null;

            SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(attributedTypeSyntax);
            return symbolInfo.Symbol as INamedTypeSymbol;
        }

        public static AttributeSyntax? GetAttribute(SemanticModel semanticModel, SyntaxList<AttributeListSyntax> attributeListsSyntax, string attributeTypeName)
        {
            foreach (AttributeListSyntax attributeListSyntax in attributeListsSyntax)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    string? currentAttributeTypeName = GetAttributeFullTypeName(semanticModel, attributeSyntax);
                    if (currentAttributeTypeName == attributeTypeName)
                        return attributeSyntax;
                }
            }

            return null;
        }
    }
}
