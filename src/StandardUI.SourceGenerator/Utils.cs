using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.StandardUI.SourceGenerator
{
    public static class Utils
    {
        public const string RootNamespace = "Microsoft.StandardUI";
        public static readonly SymbolDisplayFormat TypeFullNameSymbolDisplayFormat =
            new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public static string ToTypeName(ITypeSymbol type) =>
            AddNullableSuffixIfNeeded(ToTypeNameNonNullable(type), type.NullableAnnotation);

        public static string AddNullableSuffixIfNeeded(string typeName, NullableAnnotation nullableAnnotation) =>
            nullableAnnotation == NullableAnnotation.Annotated ? typeName + "?" : typeName;

        public static string ToTypeNameNonNullable(ITypeSymbol type)
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

        public static string? GetBuiltInTypeName(ITypeSymbol type)
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

        public static string ToNonnullableType(string type)
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
        public static string PascalCaseToCamelCase(string typeName)
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

        public static bool IsThisType(ITypeSymbol type, string typeFullName) => GetTypeFullName(type) == typeFullName;

        public static bool IsPanelType(ITypeSymbol type, string typeFullName) => GetTypeFullName(type) == "Microsoft.StandardUI.Controls.IPanel";

        public static string GetTypeFullName(ITypeSymbol type) => type.ToDisplayString(TypeFullNameSymbolDisplayFormat);

        public static string GetNamespaceFullName(INamespaceSymbol namespce) => namespce.ToDisplayString(TypeFullNameSymbolDisplayFormat);

        public static bool IncludeDraw(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol namedType)
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

        public static bool IsUIModelInterfaceType(ITypeSymbol type)
        {
            return type.TypeKind == TypeKind.Interface && type.Name != "IEnumerable";  // TODO: Use attribute check instead
        }

        public static bool IsUIElementType(ITypeSymbol type) => IsThisType(type, "Microsoft.StandardUI.IUIElement");

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
