using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Context
    {
        public const string RootNamespace = "Microsoft.StandardUI";
        public readonly SymbolDisplayFormat TypeFullNameSymbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public int IndentSize { get; } = 4;
        public Compilation Compilation { get; }
        public string RootDirectory { get; }
        public OutputType OutputType { get; }

        public Context(Compilation compilation, string rootDirectory, OutputType outputType)
        {
            Compilation = compilation;
            RootDirectory = rootDirectory;
            OutputType = outputType;
        }

        public string ToFrameworkNamespaceName(INamespaceSymbol namespc)
        {
            string frameworkRootNamespace = OutputType.RootNamespace;

            // Map e.g. Microsoft.StandardUI.Media source namespace => Microsoft.StandardUI.Wpf.Media destination namespace
            // If the source namespace is just Microsoft.StandardUI, don't change anything here
            string? childNamespaceName = GetChildNamespaceName(GetNamespaceFullName(namespc));
            if (childNamespaceName == null)
                return frameworkRootNamespace;
            else return frameworkRootNamespace + "." + childNamespaceName;
        }

        public string ToTypeName(ITypeSymbol type) =>
            AddNullableSuffixIfNeeded(ToTypeNameNonNullable(type), type.NullableAnnotation);

        public string AddNullableSuffixIfNeeded(string typeName, NullableAnnotation nullableAnnotation) =>
            nullableAnnotation == NullableAnnotation.Annotated ? typeName + "?" : typeName;

        public string ToTypeNameNonNullable(ITypeSymbol type)
        {
            string? builtInTypeName = GetBuiltInTypeName(type);
            if (builtInTypeName != null)
                return builtInTypeName;

            string typeName = type.Name;
            if (typeName.Length == 0)
                throw new UserViewableException($"Type {type} has no type name");

            if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var buffer = new StringBuilder(typeName);
                buffer.Append("<");

                bool firstArgument = true;
                foreach (ITypeSymbol typeArgument in namedType.TypeArguments)
                {
                    if (! firstArgument)
                        buffer.Append(",");
                    buffer.Append(typeArgument.Name);

                    firstArgument = false;
                }

                buffer.Append(">");
                return buffer.ToString();
            }

            return typeName;
        }

        public string? GetBuiltInTypeName(ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Boolean:
                    return "bool";
                case SpecialType.System_SByte:
                    return "sbyte";
                case SpecialType.System_Int16:
                    return "short";
                case SpecialType.System_Int32:
                    return "int";
                case SpecialType.System_Int64:
                    return "long";
                case SpecialType.System_Byte:
                    return "byte";
                case SpecialType.System_UInt16:
                    return "ushort";
                case SpecialType.System_UInt32:
                    return "uint";
                case SpecialType.System_UInt64:
                    return "ulong";
                case SpecialType.System_Single:
                    return "ulong";
                case SpecialType.System_Double:
                    return "double";
                case SpecialType.System_Char:
                    return "char";
                case SpecialType.System_Object:
                    return "object";
                case SpecialType.System_String:
                    return "string";
                default:
                    return null;
            }
        }

        public string ToFrameworkTypeName(ITypeSymbol type)
        {
            string typeName = type.Name;

            string destinationTypeName;
            if (IsThisType(type, "Microsoft.StandardUI.IUIElement"))
                destinationTypeName = OutputType.DefaultUIElementBaseClassName;
            else if (IsUIModelInterfaceType(type))
                destinationTypeName = typeName.Substring(1);
            else if (IsWrappedType(type))
                destinationTypeName = GetTypeNameWrapIfNeeded(type);
            else destinationTypeName = ToTypeName(type);

            return AddNullableSuffixIfNeeded(destinationTypeName, type.NullableAnnotation);
        }

        public string ToNonnullableType(string type)
        {
            if (type.EndsWith("?"))
                return type.Substring(0, type.Length - 1);
            else return type;
        }

        /// <summary>
        /// Convert the type name, normally starting with an upper case character, to a variable name,
        /// lowercasing the initial character(s) per convention.
        /// 
        /// element => element
        /// Element => element
        /// AElement => aElement
        /// UIElement => uiElement
        /// </summary>
        public string TypeNameToVariableName(string typeName)
        {
            int upperCasePrefixCharCount = 0;
            int length = typeName.Length;
            for (int i = 0; i < length; ++i)
            {
                if (char.IsUpper(typeName[i]))
                    ++upperCasePrefixCharCount;
                else break;
            }

            if (upperCasePrefixCharCount == 0)
                return typeName;
            else if (upperCasePrefixCharCount == 1)
                return typeName.Substring(0, 1).ToLower() + typeName.Substring(1);
            else
                return typeName.Substring(0, upperCasePrefixCharCount - 1).ToLower() + typeName.Substring(upperCasePrefixCharCount - 1);
        }

        public static bool IsTransformType(TypeSyntax type)
        {
            if (type is NullableTypeSyntax nullableType)
                type = nullableType.ElementType;

            return type is IdentifierNameSyntax identifierName && identifierName.Identifier.Text.EndsWith("Transform");
        }

        public bool IsWrappableType(ITypeSymbol type)
        {
            if (!(type is INamedTypeSymbol namedType))
                return false;

            string typeName = type.Name;
            return typeName == "Color" || typeName == "Point" || typeName == "Points" || typeName == "Size" || typeName == "DataSource" || typeName == "FontWeight";
        }

        public bool IsThisType(ITypeSymbol type, string typeFullName) => GetTypeFullName(type) == typeFullName;

        public string GetTypeFullName(ITypeSymbol type) => type.ToDisplayString(TypeFullNameSymbolDisplayFormat);

        public string GetNamespaceFullName(INamespaceSymbol namespce) => namespce.ToDisplayString(TypeFullNameSymbolDisplayFormat);

        public bool IsPanelSubclass(ITypeSymbol type)
        {
            if (!(type is INamedTypeSymbol namedType))
                return false;

            foreach (INamedTypeSymbol intface in namedType.Interfaces)
            {
                if (IsThisType(intface, "Microsoft.StandardUI.Controls.IPanel"))
                    return true;
            }

            return false;
        }

        public bool IncludeDraw(ITypeSymbol type)
        {
            if (!(type is INamedTypeSymbol namedType))
                return false;

            if (IsThisType(type, "Microsoft.StandardUI.Controls.ITextBlock"))
                return true;

            foreach (INamedTypeSymbol intface in namedType.Interfaces)
            {
                if (IsThisType(intface, "Microsoft.StandardUI.Shapes.IShape"))
                    return true;
            }

            return false;
        }

        public bool IsUIModelInterfaceType(ITypeSymbol type)
        {
            return type.TypeKind == TypeKind.Interface && type.Name != "IEnumerable";  // TODO: Use attribute check instead
        }

        public bool IsWrappedType(ITypeSymbol type)
        {
            return OutputType is XamlOutputType && IsWrappableType(type);
        }

        public string GetWrapperTypeName(string typeName)
        {
            return typeName + ((XamlOutputType)OutputType).WrapperSuffix;
        }

        public string GetTypeNameWrapIfNeeded(ITypeSymbol type)
        {
            if (IsWrappedType(type))
                return type.Name + ((XamlOutputType)OutputType).WrapperSuffix;
            else return type.Name;
        }

        public bool IsUIElementType(ITypeSymbol type) => IsThisType(type, "Microsoft.StandardUI.IUIElement");

        public static string? IsCollectionType(ITypeSymbol type)
        {
            string typeName = type.Name;
            const string collectionSuffix = "Collection";

            if (typeName.EndsWith(collectionSuffix))
                return typeName.Substring(0, typeName.Length - collectionSuffix.Length);
            else
                return null;
        }

        // TODO: Remove when no longer used
        public static string? IsCollectionType(string typeName)
        {
            const string collectionSuffix = "Collection";

            if (typeName.EndsWith(collectionSuffix))
                return typeName.Substring(0, typeName.Length - collectionSuffix.Length);
            else
                return null;
        }

        public string GetDefaultValue(ImmutableArray<AttributeData> attributes, string fullPropertyName, ITypeSymbol propertyType)
        {
            string typeFullName = GetTypeFullName(propertyType);

            foreach (AttributeData attribute in attributes)
            {
                var attributeTypeFullName = GetTypeFullName(attribute.AttributeClass);
                if (attributeTypeFullName != "Microsoft.StandardUI.DefaultValueAttribute")
                    continue;

                ImmutableArray<TypedConstant> constructorArguments = attribute.ConstructorArguments;

                if (constructorArguments.Length != 1)
                    throw new UserViewableException($"Property {fullPropertyName} should have a single argument for the [DefaultValue] attribute");
                TypedConstant argument = constructorArguments[0];

                var kind = argument.Kind;

                if (kind == TypedConstantKind.Primitive)
                {
                    if (argument.Value is string stringArgumentValue)
                    {
                        if (typeFullName == "Microsoft.StandardUI.Point" && stringArgumentValue == "0.5,0.5")
                            return $"{ToFrameworkTypeName(propertyType)}.CenterDefault";
                        else if (stringArgumentValue == "")
                            return "\"\"";
                        else new UserViewableException($"Unknown string literal based default value: {stringArgumentValue}");
                    }
                    else if (argument.Value is double doubleArgumentValue)
                    {
                        if (doubleArgumentValue - Math.Truncate(doubleArgumentValue) == 0)
                            return doubleArgumentValue.ToString("F1", CultureInfo.InvariantCulture);
                        else return doubleArgumentValue.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (argument.Value is bool boolArgumentValue)
                    {
                        return boolArgumentValue ? "true" : "false";
                    }
                    else if (argument.IsNull)
                    {
                        return "null";
                    }

                    throw new UserViewableException($"{fullPropertyName} default value {argument.Value.ToString()} not yet supported");
                }
                else if (kind == TypedConstantKind.Enum)
                {
                    object enumValue = argument.Value;
                    ITypeSymbol type = argument.Type!;
                    ImmutableArray<ISymbol> enumMembers = type.GetMembers();

                    foreach (IFieldSymbol enumFieldMember in enumMembers)
                    {
                        if (enumFieldMember.ConstantValue.Equals(enumValue))
                            return $"{type.Name}.{enumFieldMember.Name}";
                    }

                    throw new UserViewableException($"No symbol found in enum {type.Name} for value {enumValue}");
                }

                // TODO: add explicit checks for different expression types
                return argument.Value.ToString();
                // throw new UserViewableException($"Default value type {argument} not yet supported");
            }

            if (typeFullName == "System.Collections.Generic.IEnumerable")
                return "null";

            if  (IsCollectionType(propertyType) != null)
                return "null";

            if (propertyType is INamedTypeSymbol namedTypeSymbol &&
                namedTypeSymbol.Name is string typeName &&
                (typeName == "Color" ||
                typeName == "Point" ||
                typeName == "Points" ||
                typeName == "Size" ||
                typeName == "Thickness" ||
                typeName == "CornerRadius" ||
                typeName == "FontWeight"))
            {
                return $"{GetTypeNameWrapIfNeeded(propertyType)}.Default";
            }
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

            throw new UserViewableException($"Property {fullPropertyName} has no [DefaultValue] attribute nor hardcoded default");
        }

        public string GetSharedOutputDirectory(string? childNamespaceName)
        {
            string outputDirectory = Path.Combine(RootDirectory, "src", "StandardUI", "generated");
            if (childNamespaceName != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespaceName);

            return outputDirectory;
        }

        public string GetPlatformOutputDirectory(string? childNamespaceName)
        {
            string outputDirectory = Path.Combine(RootDirectory, "src", OutputType.ProjectBaseDirectory, "generated");
            if (childNamespaceName != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespaceName);

            return outputDirectory;
        }

        /// <summary>
        /// Return the child namespace (e.g. "Shapes", "Transforms", etc. or null if there is no child
        /// and classes should be at the root.
        /// </summary>
        /// <param name="sourceNamespace">source namespace</param>
        /// <returns>child namespace</returns>
        public static string? GetChildNamespace(NameSyntax sourceNamespace)
        {
            string sourceNamespaceString = sourceNamespace.ToString();

            if (!sourceNamespaceString.StartsWith(RootNamespace))
                throw new InvalidOperationException($"Source namespace {sourceNamespace} doesn't start with '{RootNamespace}' as expected");

            if (!sourceNamespaceString.StartsWith(RootNamespace + "."))
                return null;

            return sourceNamespaceString.Substring(sourceNamespaceString.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// Return the child namespace (e.g. "Shapes", "Transforms", etc. or null if there is no child
        /// and classes should be at the root.
        /// </summary>
        /// <param name="sourceNamespace">source namespace</param>
        /// <returns>child namespace</returns>
        public static string? GetChildNamespaceName(string namespaceName)
        {
            if (!namespaceName.StartsWith(RootNamespace))
                throw new InvalidOperationException($"namespace {namespaceName} doesn't start with '{RootNamespace}' as expected");

            if (!namespaceName.StartsWith(RootNamespace + "."))
                return null;

            return namespaceName.Substring(namespaceName.LastIndexOf('.') + 1);
        }
    }
}
