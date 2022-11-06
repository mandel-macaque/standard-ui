using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
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
    internal class ControlLibraryGenerator : SourceGeneratorBase<SyntaxNode>
    {
        protected override bool Filter(SyntaxNode node) =>
            node is InterfaceDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0 ||
            node is AttributeSyntax attrib && attrib.ArgumentList?.Arguments.Count == 1;

        protected override SyntaxNode? Transform(SemanticModel semanticModel, SyntaxNode node)
        {
            if (node is InterfaceDeclarationSyntax interfaceDeclarationSyntax &&
                GetTypeAttribute(semanticModel, interfaceDeclarationSyntax, KnownTypes.StandardControlAttribute,
                    KnownTypes.StandardUIElementAttribute, KnownTypes.StandardUISingletonAttribute) != null)
            {
                return interfaceDeclarationSyntax;
            }
            else if (node is AttributeSyntax attributeSyntax && IsMatchingAttribute(semanticModel, attributeSyntax, KnownTypes.ControlLibraryAttribute))
            {
                return attributeSyntax;
            }

            return null;
        }

        protected override void Generate(Context context, ImmutableArray<SyntaxNode> inputs)
        {
            AttributeSyntax? controlLibraryAttributeSyntax = null;
            string? controlLibraryName = null;
            List<Interface> interfaces = new();

            foreach (SyntaxNode input in inputs)
            {
                if (input is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
                {
                    SemanticModel semanticModel = context.Compilation.GetSemanticModel(interfaceDeclarationSyntax.SyntaxTree);
                    ISymbol? symbol = semanticModel.GetDeclaredSymbol(interfaceDeclarationSyntax);
                    if (symbol is INamedTypeSymbol interfaceTypeSymbol)
                    {
                        var intface = new Interface(context, interfaceTypeSymbol);
                        interfaces.Add(intface);
                    }
                }
                else if (input is AttributeSyntax attributeSyntax)
                {
                    controlLibraryAttributeSyntax = attributeSyntax;
                    controlLibraryName = GetAttributeStringArgument(attributeSyntax, 0);
                }
            }

            if (controlLibraryName == null || controlLibraryAttributeSyntax == null)
                return;

            var controlLibrary = new ControlLibrary(context, controlLibraryName,
                controlLibraryAttributeSyntax.GetReference(), interfaces);
            controlLibrary.GenerateStaticsClass();
            controlLibrary.GenerateFactoryClass();
        }
    }
}
