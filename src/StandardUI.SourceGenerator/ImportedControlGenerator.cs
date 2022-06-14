using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

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
    [Generator]
    internal class ImportedControlGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Enable this to be able to debug the source generator
#if false
            if (!Debugger.IsAttached)
                Debugger.Launch();
#endif

            IncrementalValuesProvider<INamedTypeSymbol> importTypes = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node is AttributeSyntax attrib && attrib.ArgumentList?.Arguments.Count == 1,
                    transform: static (context, _) => GetSemanticTargetForGeneration(context))
                .Where(static importType => importType is not null)!;

            IncrementalValueProvider<(Compilation, ImmutableArray<INamedTypeSymbol>)> compilationAndTypes
                = context.CompilationProvider.Combine(importTypes.Collect());

            context.RegisterSourceOutput(compilationAndTypes,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));

            //context.RegisterForPostInitialization((pi) => pi.AddSource("StandardUI_AssemblyAttributes", AttributeSource));
            //context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        static INamedTypeSymbol? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var attributeSyntax = (AttributeSyntax)context.Node;

            // If we couldn't get the symbol for some reason, ignore it
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                return null;

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            string fullName = attributeContainingTypeSymbol.ToDisplayString();

            if (fullName != "Microsoft.StandardUI.ImportStandardControlAttribute")
                return null;

            ExpressionSyntax? attributeArg = attributeSyntax.ArgumentList!.Arguments[0].Expression;
            if (attributeArg is not TypeOfExpressionSyntax typeOfExpressionSyntax)
                return null;

            TypeSyntax? importType = typeOfExpressionSyntax.Type;

            return context.SemanticModel.GetSymbolInfo(importType).Symbol as INamedTypeSymbol;
        }

        public static void Execute(Compilation compilation, ImmutableArray<INamedTypeSymbol> importTypes, SourceProductionContext context)
        {
            try
            {
                if (importTypes.IsDefaultOrEmpty)
                    return;

                HashSet<string> generatedInterfaces = new HashSet<string>();
                foreach (INamedTypeSymbol importType in importTypes)
                {
                    string fullTypeName = importType.ToString();
                    if (generatedInterfaces.Contains(fullTypeName))
                        continue;

                    GenerateSourceFile(compilation, context, fullTypeName);
                    generatedInterfaces.Add(fullTypeName);

                    // Generate any ancestor types
                    INamedTypeSymbol? ancestorType = GetBaseInterface(importType);
                    while (ancestorType != null)
                    {
                        var symbolDisplayFormat = new SymbolDisplayFormat(
                            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                        string ancestorFullTypeName = ancestorType.ToDisplayString(symbolDisplayFormat);

                        if (ancestorFullTypeName == KnownTypes.IStandardControl || generatedInterfaces.Contains(ancestorFullTypeName))
                            break;

                        GenerateSourceFile(compilation, context, ancestorFullTypeName);
                        generatedInterfaces.Add(ancestorFullTypeName);

                        ancestorType = GetBaseInterface(ancestorType);
                    }
                }
            }
            catch (UserViewableException e)
            {
                var diagnosticDescriptor = new DiagnosticDescriptor(e.Id, "StandardUI source generation failed",
                    e.Message, Utils.StandardUIRootNamespace, DiagnosticSeverity.Error, isEnabledByDefault: true);
                context.ReportDiagnostic(Diagnostic.Create(diagnosticDescriptor, e.Location));
            }
            catch (Exception e)
            {
                var diagnosticDescriptor = new DiagnosticDescriptor(UserVisibleErrors.InternalErrorId, "StandardUI source generation failed with internal",
                    e.ToString(), Utils.StandardUIRootNamespace, DiagnosticSeverity.Error, isEnabledByDefault: true);
                context.ReportDiagnostic(Diagnostic.Create(diagnosticDescriptor, null));
            }
        }

        private static UIFramework GetUIFramework(Compilation compilation, Context context)
        {
            foreach (AssemblyIdentity referencedAssembly in compilation.ReferencedAssemblyNames)
            {
                string assemblyName = referencedAssembly.Name;

                if (assemblyName == "StandardUI.Wpf")
                    return new WpfUIFramework(context);
                else if (assemblyName == "StandardUI.WinForms")
                    return new WinFormsUIFramework(context);
            }

            throw UserVisibleErrors.CouldNotIdentifyUIFramework();
        }

        private static void GenerateSourceFile(Compilation compilation, SourceProductionContext sourceProductionContext, string interfaceFullTypeName)
        {
            INamedTypeSymbol? interfaceSymbol = compilation.GetTypeByMetadataName(interfaceFullTypeName);
            if (interfaceSymbol == null)
                return;

            if (!TryGetTypeNamesFromInterface(interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName))
                return;

            Context context = new Context(compilation, new GeneratorExecutionOutput(sourceProductionContext));
            UIFramework uiFramework = GetUIFramework(compilation, context);

            var intface = new Interface(context, interfaceSymbol);
            intface.Generate(uiFramework);
        }

        /// <summary>
        /// Return the first base interface or null if there aren't any
        /// </summary>
        private static INamedTypeSymbol? GetBaseInterface(INamedTypeSymbol interfaceSymbol)
        {
            foreach (INamedTypeSymbol baseInterface in interfaceSymbol.Interfaces)
            {
                return baseInterface;
            }

            return null;
        }

        /// <summary>
        /// Given the full name (with namespace) of an interface type, extracts various other related strings.
        /// </summary>
        /// <param name="interfaceFullTypeName">The full name (with namespace) of an interface type. For example, Contoso.Controls.IControl</param>
        /// <param name="interfaceNamespace">The interface's namespace. For example, Contoso.Controls</param>
        /// <param name="controlTypeName">The default name of the class implementing the interface (by convention). For example, Control</param>
        /// <returns>True if the output strings were successfully determined, otherwise false.</returns>
        private static bool TryGetTypeNamesFromInterface(string interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName)
        {
            int lastDotIndex = interfaceFullTypeName.LastIndexOf('.');
            if (lastDotIndex < 3)
            {
                interfaceNamespace = "";
                controlTypeName = "";
                return false;
            }

            controlTypeName = interfaceFullTypeName.Substring(lastDotIndex + 2);
            interfaceNamespace = interfaceFullTypeName.Substring(0, lastDotIndex);
            return true;
        }
    }
}
