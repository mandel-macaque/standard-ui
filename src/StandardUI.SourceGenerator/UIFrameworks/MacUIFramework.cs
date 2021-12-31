namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class MacUIFramework : UIFramework
    {
        public MacUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "StandardUI.Mac";
        public override string RootNamespace => "Microsoft.StandardUI.Mac";
        public override string FrameworkTypeForUIElementAttachedTarget => "AppKit.NSView";
        public override string NativeUIElementType => "AppKit.NSView";
        public override string? DefaultBaseClassName => null;
        public override string BuiltInUIElementBaseClassName => "StandardUIControl";
        protected override string FontFamilyDefaultValue => throw new System.NotImplementedException();

        public override void GeneratePropertyField(Property property, Source nonstaticFields)
        {
            nonstaticFields.AddLine(
                $"private {PropertyOutputTypeName(property)} {PropertyFieldName(property)};");
        }

        public override void GeneratePropertyMethods(Property property, Source source)
        {
            var usings = source.Usings;

            // Add the type - for interface type and the framework type (if different)
            usings.AddTypeNamespace(property.Type);
            if (IsWrappedType(property.Type) || Utils.IsUIModelInterfaceType(property.Type))
                usings.AddNamespace(ToFrameworkNamespaceName(property.Type.ContainingNamespace));

            var propertyOutputTypeName = PropertyOutputTypeName(property);
            AddTypeAliasUsingIfNeeded(usings, propertyOutputTypeName);

            bool classPropertyTypeDiffersFromInterface = property.TypeName != propertyOutputTypeName;

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

            string propertyFieldName = PropertyFieldName(property);

            if (property.IsReadOnly)
                source.AddLine($"public {propertyOutputTypeName} {property.Name} => {propertyFieldName};");
            else
            {
                source.AddLines(
                    $"public {propertyOutputTypeName} {property.Name}",
                    "{");
                using (source.Indent())
                {
                    source.AddLine(
                        $"get => {propertyFieldName};");
                    source.AddLine(
                        $"set => {propertyFieldName} = value;");
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
