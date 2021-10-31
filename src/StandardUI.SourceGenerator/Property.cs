using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Property
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public IPropertySymbol SourceProperty { get; }
        public bool HasSetter { get; }
        public string Name { get; }
        public ITypeSymbol Type { get; }
        public string TypeName { get; }
        public string FrameworkTypeName { get; }
        public string DefaultValue { get; }
        public bool IsCollection { get; }
        public string? FieldNameIfExists { get; }

        public Property(Context context, Interface intface, IPropertySymbol propertySymbol)
        {
            Context = context;
            Interface = intface;
            SourceProperty = propertySymbol;
            HasSetter = !propertySymbol.IsReadOnly;
            Name = propertySymbol.Name;
            Type = propertySymbol.Type;
            TypeName = context.ToTypeName(Type);
            FrameworkTypeName = context.ToFrameworkTypeName(Type);
            DefaultValue = context.GetDefaultValue(propertySymbol.GetAttributes(), $"{Interface.Name}.{Name}", propertySymbol.Type);
            IsCollection = Context.IsCollectionType(Type) != null;

            // Only collections have a field currently
            if (IsCollection)
                FieldNameIfExists = "_" + Context.TypeNameToVariableName(FrameworkTypeName);
            else FieldNameIfExists = null;
        }

        public void GenerateDescriptor(Source destinationStaticMembers)
        {
            if (!(Context.OutputType is XamlOutputType xamlOutputType))
                return;

            string nonNullablePropertyType = Context.ToNonnullableType(FrameworkTypeName);
            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);
            destinationStaticMembers.AddLine(
                $"public static readonly {xamlOutputType.DependencyPropertyClassName} {descriptorName} = PropertyUtils.Register(nameof({Name}), typeof({nonNullablePropertyType}), typeof({Interface.FrameworkClassName}), {DefaultValue});");
        }

        public void GenerateFieldIfNeeded(Source nonstaticFields)
        {
            if (FieldNameIfExists == null)
                return;

            nonstaticFields.AddLine(
                $"private {FrameworkTypeName} {FieldNameIfExists};");
        }

        public void GenerateConstructorLinesIfNeeded(Source constuctorBody)
        {
            if (!(Context.OutputType is XamlOutputType xamlOutputType))
                return;

            if (FieldNameIfExists == null)
                return;

            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

            // Add a special case to pass parent object to UIElementCollection constructor
            string constructorParameters = "";
            if (FrameworkTypeName.ToString() == "UIElementCollection")
                constructorParameters = "this";

            constuctorBody.AddLines(
                $"{FieldNameIfExists} = new {FrameworkTypeName}({constructorParameters});",
                $"SetValue({descriptorName}, {FieldNameIfExists});");
        }

        public string? GetFieldNameIfExists()
        {
            // Only collections, for XAML output, have a field currently
            if (!IsCollection)
                return null;

            return "_" + Context.TypeNameToVariableName(FrameworkTypeName);
        }

        public void GenerateMethods(Source source)
        {
            bool classPropertyTypeDiffersFromInterface = TypeName != FrameworkTypeName;

#if false
            SyntaxTrivia xmlCommentTrivia = Declaration.GetLeadingTrivia().FirstOrDefault(t =>
                t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia ||
                t.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia);
            bool includeXmlComment = classPropertyTypeDiffersFromInterface && xmlCommentTrivia.Kind() != SyntaxKind.None;
#endif

#if LATER
            SyntaxTokenList modifiers;
            if (includeXmlComment)
                modifiers = TokenList(
                    Token(
                        TriviaList(xmlCommentTrivia),
                        SyntaxKind.PublicKeyword,
                        TriviaList()));
            else
                modifiers = TokenList(Token(SyntaxKind.PublicKeyword));
#endif

            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlOutputType xamlOutputType)
            {
                string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

                string getterValue;
                if (FieldNameIfExists != null)
                    getterValue = $"{FieldNameIfExists}";
                else
                    getterValue = $"({FrameworkTypeName}) GetValue({descriptorName})";

                if (!HasSetter)
                    source.AddLine($"public {FrameworkTypeName} {Name} => {getterValue};");
                else
                {
                    source.AddLines(
                        $"public {FrameworkTypeName} {Name}",
                        "{");
                    using (source.Indent())
                    {
                        source.AddLine(
                            $"get => {getterValue};");
                        source.AddLine(
                            $"set => SetValue({descriptorName}, value);");
                    }
                    source.AddLine(
                        "}");
                }
            }
            else
            {
#if LATER
                ExpressionSyntax defaultValue = GetDefaultValue(modelProperty, destinationPropertyType);

                propertyDeclaration = PropertyDeclaration(destinationPropertyType, propertyName)
                    .WithModifiers(modifiers)
                    .WithAccessorList(
                        AccessorList(
                            List(new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            })))
                    .WithInitializer(
                        EqualsValueClause(defaultValue))
                    .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken));
#endif
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif

            // If the interface property has a different type, add another property that explicitly implements it
            if (classPropertyTypeDiffersFromInterface)
            {
                string getterValue;
                string setterAssignment;
                if (Context.IsWrappedType(Type))
                {
                    getterValue = $"{Name}.{TypeName}";
                    setterAssignment = $"{Name} = new {FrameworkTypeName}(value)";
                }
                else
                {
                    getterValue = Name;
                    setterAssignment = $"{Name} = ({FrameworkTypeName}) value";
                }

                if (!HasSetter)
                {
                    source.AddLine(
                        $"{TypeName} {Interface.Name}.{Name} => {getterValue};");
                }
                else
                {
                    source.AddLines(
                        $"{TypeName} {Interface.Name}.{Name}",
                        "{");
                    using (source.Indent())
                    {
                        source.AddLine(
                            $"get => {getterValue};");
                        source.AddLine(
                            $"set => {setterAssignment};");
                    }
                    source.AddLine(
                        "}");
                }
            }
        }

        public void GenerateExtensionClassMethods(Source source)
        {
            if (!HasSetter)
                return;

            string interfaceVariableName = Interface.VariableName;

            source.AddBlankLineIfNonempty();
            source.AddLines(
                $"public static T {Name}<T>(this T {interfaceVariableName}, {TypeName} value) where T : {Interface.Name}",
                "{");
            using (source.Indent())
            {
                source.AddLines(
                    $"{interfaceVariableName}.{Name} = value;",
                    $"return {interfaceVariableName};");
            }
            source.AddLine(
                "}");
        }
    }
}
