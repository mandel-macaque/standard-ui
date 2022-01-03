using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public abstract class NonXamlUIFramework : UIFramework
    {
        protected NonXamlUIFramework(Context context) : base(context)
        {
        }

        public override void GeneratePropertyDescriptor(Property property, Source staticMembers)
        {
            string readOnlyParam = property.IsReadOnly ? ", readOnly:true" : "";
            staticMembers.AddLine(
                $"public static readonly UIProperty {PropertyDescriptorName(property)} = new UIProperty(nameof({property.Name}), {DefaultValue(property)}{readOnlyParam});");
        }

        public override void GenerateAttachedPropertyDescriptor(AttachedProperty attachedProperty, Source staticMembers)
        {
            string readOnlyParam = attachedProperty.IsReadOnly ? ", readOnly:true" : "";
            staticMembers.AddLine(
                $"public static readonly AttachedUIProperty {PropertyDescriptorName(attachedProperty)} = new AttachedUIProperty(\"{attachedProperty.Name}\", {DefaultValue(attachedProperty)}{readOnlyParam});");
        }

        public override void GeneratePropertyField(Property property, Source nonstaticFields)
        {
            if (property.IsUICollection)
                nonstaticFields.AddLine(
                    $"private {PropertyOutputTypeName(property)} {PropertyFieldName(property)};");
        }

        public override void GeneratePropertyInit(Property property, Source constuctorBody)
        {
            if (property.IsUICollection)
            {
                string descriptorName = PropertyDescriptorName(property);
                string propertyFieldName = PropertyFieldName(property);

                constuctorBody.AddLines(
                    $"{propertyFieldName} = new {PropertyOutputTypeName(property)}(this);",
                    $"SetValue({descriptorName}, {propertyFieldName});");
            }
        }

        public override void GeneratePropertyMethods(Property property, Source source)
        {
            var usings = source.Usings;
            string propertyOutputTypeName = PropertyOutputTypeName(property);

            // Add the type - for interface type and the framework type (if different)
            usings.AddTypeNamespace(property.Type);
            if (IsWrappedType(property.Type) || Utils.IsUIModelInterfaceType(property.Type))
                usings.AddNamespace(ToFrameworkNamespaceName(property.Type.ContainingNamespace));

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
            string descriptorName = PropertyDescriptorName(property);

            string getterValue;
            if (property.IsUICollection)
                getterValue = $"{PropertyFieldName(property)}";
            else
                getterValue = $"({propertyOutputTypeName}) GetValue({descriptorName})";

            if (property.IsReadOnly)
                source.AddLine($"public {propertyOutputTypeName} {property.Name} => {getterValue};");
            else
            {
                source.AddLines(
                    $"public {propertyOutputTypeName} {property.Name}",
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
                if (IsWrappedType(property.Type))
                {
                    otherGetterValue = $"{property.Name}.{property.TypeName}";
                    setterAssignment = $"{property.Name} = new {propertyOutputTypeName}(value)";
                }
                else if (Utils.IsUICollectionType(property.Type, out var elementType) && propertyOutputTypeName.StartsWith("UIElementCollection<"))
                {
                    otherGetterValue = $"{property.Name}.ToStandardUIElementCollection()";
                    setterAssignment = ""; // Not used
                }
                else
                {
                    otherGetterValue = property.Name;
                    setterAssignment = $"{property.Name} = ({propertyOutputTypeName}) value";
                }

                if (property.IsReadOnly)
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

        public override void GenerateAttachedPropertyMethods(AttachedProperty attachedProperty, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            string descriptorName = PropertyDescriptorName(attachedProperty);
            string targetOutputTypeName = AttachedTargetOutputTypeName(attachedProperty);
            string propertyOutputTypeName = PropertyOutputTypeName(attachedProperty);

            methods.AddLine($"public static {propertyOutputTypeName} Get{attachedProperty.Name}({targetOutputTypeName} {attachedProperty.TargetParameterName}) => ({propertyOutputTypeName}) {attachedProperty.TargetParameterName}.GetValue({descriptorName});");

            if (attachedProperty.SetterMethod != null)
                methods.AddLine($"public static void Set{attachedProperty.Name}({targetOutputTypeName} {attachedProperty.TargetParameterName}, {propertyOutputTypeName} value) => {attachedProperty.TargetParameterName}.SetValue({descriptorName}, value);");

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif
        }

        public override void GenerateAttachedPropertyAttachedClassMethods(AttachedProperty attachedProperty, Source methods)
        {
            string targetOutputTypeName = AttachedTargetOutputTypeName(attachedProperty);
            string propertyOutputTypeName = PropertyOutputTypeName(attachedProperty);
            bool classPropertyTypeDiffersFromInterface = attachedProperty.Type.ToString() != propertyOutputTypeName;

            methods.AddBlankLineIfNonempty();
            methods.AddLine($"public {propertyOutputTypeName} Get{attachedProperty.Name}({attachedProperty.TargetTypeName} {attachedProperty.TargetParameterName}) => {attachedProperty.Interface.FrameworkClassName}.Get{attachedProperty.Name}(({targetOutputTypeName}) {attachedProperty.TargetParameterName});");
            if (attachedProperty.SetterMethod != null)
                methods.AddLine($"public void Set{attachedProperty.Name}({attachedProperty.TargetTypeName} {attachedProperty.TargetParameterName}, {propertyOutputTypeName} value) => {attachedProperty.Interface.FrameworkClassName}.Set{attachedProperty.Name}(({targetOutputTypeName}) {attachedProperty.TargetParameterName}, value);");
        }
    }
}
