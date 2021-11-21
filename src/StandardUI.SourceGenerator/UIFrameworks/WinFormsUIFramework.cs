namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WinFormsUIFramework : UIFramework
    {
        public static readonly WinFormsUIFramework Instance = new WinFormsUIFramework();

        public override string ProjectBaseDirectory => "StandardUI.WinForms";
        public override string RootNamespace => "Microsoft.StandardUI.WinForms";
        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.WinForms.Control";
        public override string? DefaultBaseClassName => null;
        public override string DefaultUIElementBaseClassName => "StandardUIControl";

        public override void GeneratePropertyField(Property property, Source nonstaticFields)
        {
            nonstaticFields.AddLine(
                $"private {property.FrameworkTypeName} {property.FieldName};");
        }

        public override void GeneratePropertyMethods(Property property, Source source)
        {
            var usings = source.Usings;

            // Add the type - for interface type and the framework type (if different)
            usings.AddTypeNamespace(property.Type);
            if (property.Context.IsWrappedType(property.Type) || property.Context.IsUIModelInterfaceType(property.Type))
                usings.AddNamespace(property.Context.ToFrameworkNamespaceName(property.Type.ContainingNamespace));

            AddTypeAliasUsingIfNeeded(usings, property.FrameworkTypeName);

            bool classPropertyTypeDiffersFromInterface = property.TypeName != property.FrameworkTypeName;

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

            string getterValue = $"{property.FieldName}";

            if (!property.HasSetter)
                source.AddLine($"public {property.FrameworkTypeName} {property.Name} => {getterValue};");
            else
            {
                source.AddLines(
                    $"public {property.FrameworkTypeName} {property.Name}",
                    "{");
                using (source.Indent())
                {
                    source.AddLine(
                        $"get => {getterValue};");
                    source.AddLine(
                        $"set => {property.FieldName} = value;");
                }
                source.AddLine(
                    "}");
            }

#if LATER
            // If the interface property has a different type, add another property that explicitly implements it
            if (classPropertyTypeDiffersFromInterface)
            {
                string otherGetterValue;
                string setterAssignment;
                if (property.Context.IsWrappedType(property.Type))
                {
                    otherGetterValue = $"{property.Name}.{property.TypeName}";
                    setterAssignment = $"{property.Name} = new {property.FrameworkTypeName}(value)";
                }
                else
                {
                    otherGetterValue = property.Name;
                    setterAssignment = $"{property.Name} = ({property.FrameworkTypeName}) value";
                }

                if (!property.HasSetter)
                {
                    source.AddLine(
                        $"{property.TypeName} {property.Interface.Name}.{property.Name} => {otherGetterValue};");
                }
                else
                {
                    source.AddLines(
                        $"{property.TypeName} {property.Interface.Name}.{property.Name}",
                        "{");
                    using (source.Indent())
                    {
                        source.AddLine(
                            $"get => {otherGetterValue};");
                        source.AddLine(
                            $"set => {setterAssignment};");
                    }
                    source.AddLine(
                        "}");
                }
            }
#endif
        }
    }
}
