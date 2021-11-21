using Microsoft.CodeAnalysis;
using System.Linq;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class AttachedProperty : PropertyBase
    {
        public INamedTypeSymbol SourceInterfaceAttached { get; }
        public IMethodSymbol GetterMethod { get; }
        public IMethodSymbol? SetterMethod { get; }
        public ITypeSymbol TargetType { get; }
        public string TargetTypeName { get; }
        public string? TargetParameterName { get; }

        public AttachedProperty(Context context, Interface intface, INamedTypeSymbol sourceInterfaceAttached, IMethodSymbol getterMethod, IMethodSymbol? setterMethod) :
            base(context, intface, getterMethod.Name.Substring("Get".Length), getterMethod.ReturnType, setterMethod == null, sourceInterfaceAttached.Name)
        {
            if (getterMethod.Parameters.Length != 1)
                throw new UserViewableException(
                        $"Attached type getter method {sourceInterfaceAttached.Name}.{getterMethod.Name} doesn't take a single parameter");
            IParameterSymbol targetParameter = getterMethod.Parameters.First();

            SourceInterfaceAttached = sourceInterfaceAttached;
            GetterMethod = getterMethod;
            SetterMethod = setterMethod;
            TargetType = targetParameter.Type;
            TargetTypeName = Utils.ToTypeName(TargetType);
            TargetParameterName = targetParameter.Name;
            SpecifiedDefaultValue = GetSpecifiedDefaultValue(getterMethod.GetAttributes());

            if (setterMethod != null && setterMethod.Parameters.Length != 2)
            {
                throw new UserViewableException(
                    $"Attached type setter method {sourceInterfaceAttached.Name}.{setterMethod.Name} doesn't take two parameters as expected");
            }
        }
    }
}
