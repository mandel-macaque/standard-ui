using Microsoft.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Microsoft.StandardUI.SourceGenerator
{
    public static class UserVisibleErrors
    {
        public static UserViewableException CouldNotIdentifyUIFramework() =>
            CreateError(1, message: "No reference to StandardUI platform assembly found");

        public static UserViewableException AttachedTypeMethodMustStartWithGetOrSet(string className, string methodName) =>
            CreateError(2, message: $"Attached type method '{className}.{methodName}' doesn't start with Get or Set");

        public static UserViewableException StandardUIInterfaceMustStartWithI(INamedTypeSymbol interfaceType) =>
            CreateError(3, $"StandardUI interface '{interfaceType.Name}' must start with 'I'",
                GetLocation(interfaceType));

        public static UserViewableException StandardUIMissingPurposeAttribute(INamedTypeSymbol interfaceType) =>
            CreateError(4, message: $"StandardUI interface '{interfaceType.Name}' missing attribute defining its purpose",
                GetLocation(interfaceType));

        public static UserViewableException NoLayoutManagerClassFound(string layoutManagerName, string interfaceName) =>
            CreateError(5, message: $"No class '{layoutManagerName}' found for StandardPanel interface '{interfaceName}'");

        public static UserViewableException NoStandardControlImplementationClassFound(string standardControlImplementationName, string interfaceName) =>
            CreateError(5, message: $"No implementation class '{standardControlImplementationName}' found for StandardControl interface '{interfaceName}'");

        public static UserViewableException PropertyHasNoDefaultValue(ISymbol symbol, string fullPropertyName) =>
            CreateError(6, message: $"Property {fullPropertyName} has no [DefaultValue] attribute nor hardcoded default",
                GetLocation(symbol));

        public static UserViewableException AttributeShouldHaveSingleArgument(ISymbol symbol, string attributeName) =>
            CreateError(7, message: $"{symbol.Name} should have a single argument for the [{attributeName}] attribute",
                GetLocation(symbol));

        public static UserViewableException AttributeShouldHaveAStringArgument(ISymbol symbol, string attributeName) =>
            CreateError(8, message: $"{symbol.Name} should have a string value for the [{attributeName}] attribute",
                GetLocation(symbol));

        public static UserViewableException ControlLibraryAttributeInvalid(AttributeData attribute) =>
            CreateError(9, message: $"The [ControlLibrary] attribute should have a single argument with the fully qualified library name",
                GetLocation(attribute.ApplicationSyntaxReference));

        public static UserViewableException MissingControlLibraryAttribute() =>
            CreateError(10, message: $"No [ControlLibrary] assembly attribute found; it should be added");

        public static string InternalErrorId = ErrorIdFromInt(99);

        public static Location? GetLocation(ISymbol symbol)
        {
            SyntaxReference? syntaxReference = symbol.DeclaringSyntaxReferences.FirstOrDefault();
            return GetLocation(syntaxReference);
        }

        public static Location? GetLocation(SyntaxReference? syntaxReference)
        {
            if (syntaxReference == null)
                return null;

            return Location.Create(syntaxReference.SyntaxTree, syntaxReference.Span);
        }

        public static string ErrorIdFromInt(int id) => "SUI" + id.ToString("0000", CultureInfo.InvariantCulture);

        public static UserViewableException CreateError(int id, string message, Location? location = null) =>
            new UserViewableException(ErrorIdFromInt(id), message, location);
    }
}
