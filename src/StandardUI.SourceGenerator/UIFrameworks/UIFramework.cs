using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;

namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public abstract class UIFramework
    {
        Context Context { get; }

        protected UIFramework(Context context)
        {
            Context = context;
        }

        public abstract string ProjectBaseDirectory { get; }
        public abstract string RootNamespace { get; }
        public abstract string FrameworkTypeForUIElementAttachedTarget { get; }
        public abstract string? DefaultBaseClassName { get; }
        public abstract string DefaultUIElementBaseClassName { get; }
        public virtual void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute) { }
        public virtual void AddTypeAliasUsingIfNeeded(Usings usings, string destinationtypeFullName) { }

        public virtual void GeneratePropertyDescriptor(Property property, Source staticMembers) { }
        public virtual void GenerateAttachedPropertyDescriptor(AttachedProperty attachedProperty, Source staticMembers) { }
        public virtual void GeneratePropertyField(Property property, Source nonstaticFields) { }
        public virtual void GeneratePropertyConstructorLines(Property property, Source constuctorBody) { }
        public virtual void GeneratePropertyMethods(Property property, Source methods) { }
        public virtual void GenerateAttachedPropertyMethods(AttachedProperty attachedProperty, Source methods) { }
        public virtual void GenerateAttachedPropertyAttachedClassMethods(AttachedProperty attachedProperty, Source methods) { }
        public virtual void GenerateStandardPanelLayoutMethods(string layoutManagerTypeName, Source methods) { }
        public virtual void GeneratePanelMethods(Source methods) { }
        public virtual void GenerateDrawableObjectMethods(Interface intface, Source methods) { }

        public string ToFrameworkNamespaceName(INamespaceSymbol namespc)
        {
            // Map e.g. Microsoft.StandardUI.Media source namespace => Microsoft.StandardUI.Wpf.Media destination namespace
            // If the source namespace is just Microsoft.StandardUI, don't change anything here
            string? childNamespaceName = Utils.GetChildNamespaceName(Utils.GetNamespaceFullName(namespc));
            if (childNamespaceName == null)
                return RootNamespace;
            else return RootNamespace + "." + childNamespaceName;
        }

        public string GetOutputDirectory(string? childNamespaceName)
        {
            string outputDirectory = Path.Combine(Context.RootDirectory, "src", ProjectBaseDirectory, "generated");
            if (childNamespaceName != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespaceName);

            return outputDirectory;
        }

        public string OutputTypeName(ITypeSymbol type)
        {
            string typeName = type.Name;

            string destinationTypeName;
            if (Utils.IsThisType(type, "Microsoft.StandardUI.IUIElement"))
                destinationTypeName = DefaultUIElementBaseClassName;
            else if (Utils.IsUIModelInterfaceType(type))
                destinationTypeName = typeName.Substring(1);
            else if (IsWrappedType(type))
                destinationTypeName = GetTypeNameWrapIfNeeded(type);
            else destinationTypeName = Utils.ToTypeName(type);

            return Utils.AddNullableSuffixIfNeeded(destinationTypeName, type.NullableAnnotation);
        }

        public string PropertyOutputTypeName(PropertyBase property) => OutputTypeName(property.Type);

        public string PropertyFieldName(PropertyBase property)
        {
            return "_" + Utils.PascalCaseToCamelCase(property.Name);
        }

        public virtual string AttachedTargetOutputTypeName(AttachedProperty attachedProperty)
        {
            return Utils.IsThisType(attachedProperty.TargetType, KnownTypes.IUIElement) ?
                FrameworkTypeForUIElementAttachedTarget :
                OutputTypeName(attachedProperty.TargetType);
        }

        public virtual string WrapperSuffix => throw new NotImplementedException("WrapperSuffix doesn't have a default implementation; implement it if IsWrappedType can ever return true");

        public virtual bool IsWrappedType(ITypeSymbol type) => false;

        public string GetTypeNameWrapIfNeeded(ITypeSymbol type)
        {
            if (IsWrappedType(type))
                return type.Name + WrapperSuffix;
            else return type.Name;
        }

        public string DefaultValue(PropertyBase property)
        {
            ITypeSymbol propertyType = property.Type;
            string typeFullName = Utils.GetTypeFullName(propertyType);

            if (property.SpecifiedDefaultValue != null)
            {
                TypedConstant specifiedDefaultValue = property.SpecifiedDefaultValue.Value;

                if (specifiedDefaultValue.IsNull)
                {
                    return "null";
                }

                var kind = specifiedDefaultValue.Kind;
                object value = specifiedDefaultValue.Value!;

                if (kind == TypedConstantKind.Primitive)
                {
                    if (value is string stringArgumentValue)
                    {
                        if (typeFullName == "Microsoft.StandardUI.Point" && stringArgumentValue == "0.5,0.5")
                            return $"{OutputTypeName(propertyType)}.CenterDefault";
                        else if (stringArgumentValue == "")
                            return "\"\"";
                        else new UserViewableException($"Unknown string literal based default value: {stringArgumentValue}");
                    }
                    else if (value is double doubleArgumentValue)
                    {
                        if (double.IsPositiveInfinity(doubleArgumentValue))
                            return "double.PositiveInfinity";
                        else if (double.IsNegativeInfinity(doubleArgumentValue))
                            return "double.NegativeInfinity";
                        else if (double.IsNaN(doubleArgumentValue))
                            return "double.NaN";
                        else if (doubleArgumentValue - Math.Truncate(doubleArgumentValue) == 0)
                            return doubleArgumentValue.ToString("F1", CultureInfo.InvariantCulture);
                        else return doubleArgumentValue.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (value is int intArgumentValue)
                    {
                        return intArgumentValue.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (value is bool boolArgumentValue)
                    {
                        return boolArgumentValue ? "true" : "false";
                    }

                    throw new UserViewableException($"{property.FullPropertyName} default value {value.ToString()} not yet supported");
                }
                else if (kind == TypedConstantKind.Enum)
                {
                    object enumValue = value;
                    ITypeSymbol type = specifiedDefaultValue.Type!;
                    ImmutableArray<ISymbol> enumMembers = type.GetMembers();

                    foreach (IFieldSymbol enumFieldMember in enumMembers)
                    {
                        if (enumFieldMember.ConstantValue.Equals(value))
                            return $"{type.Name}.{enumFieldMember.Name}";
                    }

                    throw new UserViewableException($"No symbol found in enum {type.Name} for value {enumValue}");
                }

                // TODO: add explicit checks for different expression types
                return value.ToString();
                // throw new UserViewableException($"Default value type {argument} not yet supported");
            }

            if (typeFullName == "System.Collections.Generic.IEnumerable")
                return "null";

            if (Utils.IsCollectionType(propertyType) != null)
                return "null";

            if (typeFullName == "Microsoft.StandardUI.Color" ||
                typeFullName == "Microsoft.StandardUI.Point" ||
                typeFullName == "Microsoft.StandardUI.Points" ||
                typeFullName == "Microsoft.StandardUI.Size" ||
                typeFullName == "Microsoft.StandardUI.Thickness" ||
                typeFullName == "Microsoft.StandardUI.CornerRadius" ||
                typeFullName == "Microsoft.StandardUI.Text.FontWeight" ||
                typeFullName == "Microsoft.StandardUI.GridLength")
            {
                return $"{GetTypeNameWrapIfNeeded(propertyType)}.Default";
            }

            if (typeFullName == "Microsoft.StandardUI.Media.FontFamily")
                return FontFamilyDefaultValue;

            // TODO: Implement this
#if false
            else if (propertyType is IArrayTypeSymbol arrayTypeSymbol)
            {
                return
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("Array"),
                            GenericName(
                                    Identifier("Empty"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(arrayType.ElementType)))));
            }
#endif

            throw new UserViewableException($"Property {property.FullPropertyName} has no [DefaultValue] attribute nor hardcoded default");
        }

        protected virtual string? PropertyTypeDefaultValue(INamedTypeSymbol propertyType)
        {
            string typeFullName = Utils.GetTypeFullName(propertyType);

            if (propertyType is INamedTypeSymbol namedTypeSymbol &&
                namedTypeSymbol.Name is string typeName &&
                (typeName == "Microsoft.StandardUI.Color" ||
                typeName == "Microsoft.StandardUI.Point" ||
                typeName == "Microsoft.StandardUI.Points" ||
                typeName == "Microsoft.StandardUI.Size" ||
                typeName == "Microsoft.StandardUI.Thickness" ||
                typeName == "Microsoft.StandardUI.CornerRadius" ||
                typeName == "Microsoft.StandardUI.FontWeight"))
            {
                return $"{GetTypeNameWrapIfNeeded(propertyType)}.Default";
            }

            return null;
        }

        protected abstract string FontFamilyDefaultValue { get; }
    }
}
