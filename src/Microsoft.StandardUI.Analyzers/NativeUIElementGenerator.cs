using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    [Generator]
    internal class NativeUIElementGenerator : SourceGeneratorBase<ClassDeclarationSyntax>
    {
        protected override bool Filter(SyntaxNode node) =>
            node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0;

        protected override ClassDeclarationSyntax? Transform(SemanticModel semanticModel, SyntaxNode node)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)node;

            AttributeSyntax? attributeSyntax = GetAttribute(semanticModel, classDeclarationSyntax,
                out UIFrameworkType? uiFrameworkType);
            if (attributeSyntax == null)
                return null;

            return classDeclarationSyntax;
        }

        private AttributeSyntax? GetAttribute(SemanticModel semanticModel, ClassDeclarationSyntax classDeclarationSyntax, out UIFrameworkType? uiFrameworkType)
        {
            foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    string? currentAttributeTypeName = GetAttributeFullTypeName(semanticModel, attributeSyntax);
                    if (currentAttributeTypeName == KnownTypes.WpfStandardUIElementAttribute)
                    {
                        uiFrameworkType = UIFrameworkType.Wpf;
                        return attributeSyntax;
                    }
                }
            }

            uiFrameworkType = null;
            return null;
        }

        protected override void Generate(Context context, ImmutableArray<ClassDeclarationSyntax> inputs)
        {
            foreach (ClassDeclarationSyntax classDeclarationSyntax in inputs)
            {
                SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

                AttributeSyntax? attributeSyntax = GetAttribute(semanticModel, classDeclarationSyntax,
                    out UIFrameworkType? uiFrameworkType);
                if (attributeSyntax == null)
                    continue;

                UIFramework uiFramework = uiFrameworkType!.CreateUIFramework(context);

                INamedTypeSymbol? interfaceType = GetAttributeTypeArgument(semanticModel, attributeSyntax, 0);
                if (interfaceType == null)
                    continue;

                SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(classDeclarationSyntax);
                if (symbolInfo.Symbol is not INamedTypeSymbol classType)
                    continue;

                var className = new TypeName(classType);
                string? derivedFrom = classDeclarationSyntax.BaseList?.ToString();

                var intface = new Interface(context, interfaceType);
                intface.GenerateNativeUIElementPartialClass(uiFramework, className, derivedFrom);
            }
        }
    }
}
