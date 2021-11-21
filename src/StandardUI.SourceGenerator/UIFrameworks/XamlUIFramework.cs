namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public abstract class XamlUIFramework : UIFramework
    {
        public abstract string DependencyPropertyClassName { get; }
        public virtual string GetPropertyDescriptorName(string propertyName) => propertyName + "Property";
        public abstract string WrapperSuffix { get; }
        public virtual void GeneratePanelSubclassMethods(Source methods) { }

        public override void GeneratePropertyDescriptor(Property property, Source destinationStaticMembers)
        {
            string nonNullablePropertyType = property.Context.ToNonnullableType(property.FrameworkTypeName);
            string descriptorName = GetPropertyDescriptorName(property.Name);
            destinationStaticMembers.AddLine(
                $"public static readonly {DependencyPropertyClassName} {descriptorName} = PropertyUtils.Register(nameof({property.Name}), typeof({nonNullablePropertyType}), typeof({property.Interface.FrameworkClassName}), {property.DefaultValue});");
        }

        public override void GeneratePropertyField(Property property, Source nonstaticFields)
        {
            if (property.IsCollection)
                nonstaticFields.AddLine(
                    $"private {property.FrameworkTypeName} {property.FieldName};");
        }

        public override void GeneratePropertyConstructorLines(Property property, Source constuctorBody)
        {
            if (property.IsCollection)
            {
                string descriptorName = GetPropertyDescriptorName(property.Name);

                // Add a special case to pass parent object to UIElementCollection constructor
                string constructorParameters = "";
                if (property.FrameworkTypeName.ToString() == "UIElementCollection")
                    constructorParameters = "this";

                constuctorBody.AddLines(
                    $"{property.FieldName} = new {property.FrameworkTypeName}({constructorParameters});",
                    $"SetValue({descriptorName}, {property.FieldName});");
            }
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
            string descriptorName = GetPropertyDescriptorName(property.Name);

            string getterValue;
            if (property.IsCollection)
                getterValue = $"{property.FieldName}";
            else
                getterValue = $"({property.FrameworkTypeName}) GetValue({descriptorName})";

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
                        $"set => SetValue({descriptorName}, value);");
                }
                source.AddLine(
                    "}");
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
        }
    }
}
