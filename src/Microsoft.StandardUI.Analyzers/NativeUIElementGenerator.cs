﻿using System.Collections.Immutable;
using System.ComponentModel.Design;
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
            if (GetAttribute(semanticModel, classDeclarationSyntax, out _) != null)
                return classDeclarationSyntax;
            else return null;
        }

        private AttributeSyntax? GetAttribute(SemanticModel semanticModel, ClassDeclarationSyntax classDeclarationSyntax, out UIFrameworkType? uiFrameworkType)
        {
            AttributeSyntax? attributeSyntax = GetTypeAttribute(semanticModel, classDeclarationSyntax, KnownTypes.WpfStandardUIElementAttribute);
            if (attributeSyntax != null)
            {
                uiFrameworkType = UIFrameworkType.Wpf;
                return attributeSyntax;
            }
            else
            {
                uiFrameworkType = null;
                return null;
            }
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

                string classNamespace = GetNamespace(classDeclarationSyntax);

                var className = new TypeName(classNamespace, classDeclarationSyntax.Identifier.Text);

                // Get the parent classes/interfaces, removing the ":" prefix, so the partial class we
                // output derives from the same types as the original class
                string? derivedFrom = classDeclarationSyntax.BaseList?.ToString();
                if (derivedFrom != null && derivedFrom.StartsWith(":"))
                {
                    derivedFrom = derivedFrom.Substring(1);
                    derivedFrom = derivedFrom.TrimStart(' ', '\t');
                }

                var intface = new Interface(context, interfaceType);
                intface.GenerateNativeUIElementPartialClass(uiFramework, className, derivedFrom);
            }
        }
    }
}
