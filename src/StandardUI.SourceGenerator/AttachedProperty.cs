using Microsoft.CodeAnalysis;
using System.Linq;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class AttachedProperty : PropertyBase
    {
        public INamedTypeSymbol AttachedType { get; }
        public IMethodSymbol GetterMethod { get; }
        public IMethodSymbol? SetterMethod { get; }
        public ITypeSymbol TargetType { get; }
        public string TargetTypeName { get; }
        public string? TargetParameterName { get; }

        public AttachedProperty(Context context, Interface intface, INamedTypeSymbol attachedType, IMethodSymbol getterMethod, IMethodSymbol? setterMethod) :
            base(context, intface, getterMethod.Name.Substring("Get".Length), getterMethod.ReturnType, setterMethod == null, attachedType.Name)
        {
            if (getterMethod.Parameters.Length != 1)
                throw new UserViewableException(
                        $"Attached type getter method {attachedType.Name}.{getterMethod.Name} doesn't take a single parameter");
            IParameterSymbol targetParameter = getterMethod.Parameters.First();

            AttachedType = attachedType;
            GetterMethod = getterMethod;
            SetterMethod = setterMethod;
            TargetType = targetParameter.Type;
            TargetTypeName = Utils.ToTypeName(TargetType);
            TargetParameterName = targetParameter.Name;
            SpecifiedDefaultValue = GetSpecifiedDefaultValue(getterMethod.GetAttributes());

            if (setterMethod != null && setterMethod.Parameters.Length != 2)
            {
                throw new UserViewableException(
                    $"Attached type setter method {attachedType.Name}.{setterMethod.Name} doesn't take two parameters as expected");
            }
        }

        public void GenerateExtensionClassMethods(Source source)
        {
            // Get the type base name, without the "I" nor "Attached" suffix
            string typeBaseName = Interface.Name.Substring(1);

            source.AddBlankLineIfNonempty();
            source.AddLine(
                $"public static {TypeName} Get{typeBaseName}{Name}(this IUIElement element) => {typeBaseName}AttachedInstance.Get{Name}(element);");
            if (!IsReadOnly)
            {
                source.AddLine(
                    $"public static void Set{typeBaseName}{Name}(this IUIElement element, {TypeName} value) => {typeBaseName}AttachedInstance.Set{Name}(element, value);");
            }
        }
    }
}
