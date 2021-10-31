using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class AttachedProperty
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public INamedTypeSymbol SourceInterfaceAttached { get; }
        public IMethodSymbol GetterMethod { get; }
        public IMethodSymbol? SetterMethod { get; }
        public string Name { get; }
        public ITypeSymbol Type { get; }
        public string FrameworkTypeName { get; }
        public ITypeSymbol TargetType { get; }
        public string TargetTypeName { get; }
        public string TargetFrameworkTypeName { get; }
        public string? TargetParameterName { get; }
        public string DefaultValue { get; }

        public AttachedProperty(Context context, Interface intface, INamedTypeSymbol sourceInterfaceAttached, IMethodSymbol getterMethod, IMethodSymbol? setterMethod)
        {
            string getterMethodName = getterMethod.Name;

            if (getterMethod.Parameters.Length != 1)
                throw new UserViewableException(
                        $"Attached type getter method {sourceInterfaceAttached.Name}.{getterMethodName} doesn't take a single parameter");
            IParameterSymbol targetParameter = getterMethod.Parameters.First();

            Context = context;
            Interface = intface;
            SourceInterfaceAttached = sourceInterfaceAttached;
            GetterMethod = getterMethod;
            SetterMethod = setterMethod;
            Name = getterMethodName.Substring("Get".Length);
            Type = getterMethod.ReturnType;
            FrameworkTypeName = context.ToFrameworkTypeName(Type);
            TargetType = targetParameter.Type;
            TargetTypeName = context.ToTypeName(TargetType);
            TargetFrameworkTypeName =  Context.IsUIElementType(TargetType) ? context.OutputType.FrameworkTypeForUIElementAttachedTarget : context.ToFrameworkTypeName(TargetType);
            TargetParameterName = targetParameter.Name;
            DefaultValue = context.GetDefaultValue(getterMethod.GetAttributes(), $"{SourceInterfaceAttached.Name}.{Name}", Type);

            if (setterMethod != null && setterMethod.Parameters.Length != 2)
            {
                throw new UserViewableException(
                    $"Attached type setter method {sourceInterfaceAttached.Name}.{setterMethod.Name} doesn't take two parameters as expected");
            }
        }

        public void GenerateMainClassDescriptor(Source source)
        {
            if (!(Context.OutputType is XamlFrameworkType xamlOutputType))
                return;

            string nonNullablePropertyType = Context.ToNonnullableType(FrameworkTypeName);
            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);
            source.AddLine(
                $"public static readonly {xamlOutputType.DependencyPropertyClassName} {descriptorName} = PropertyUtils.RegisterAttached(\"{Name}\", typeof({nonNullablePropertyType}), typeof({TargetFrameworkTypeName}), {DefaultValue});");
        }

        public void GenerateMainClassMethods(Source source)
        {
            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlFrameworkType xamlOutputType)
            {
                string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

                source.AddLine($"public static {FrameworkTypeName} Get{Name}({TargetFrameworkTypeName} {TargetParameterName}) => ({FrameworkTypeName}) {TargetParameterName}.GetValue({descriptorName});");

                if (SetterMethod != null)
                    source.AddLine($"public static void Set{Name}({TargetFrameworkTypeName} {TargetParameterName}, {FrameworkTypeName} value) => {TargetParameterName}.SetValue({descriptorName}, value);");
            }
            else
            {
                // TODO: Support this
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif
        }

        public void GenerateAttachedClassMethods(Source source)
        {
            bool classPropertyTypeDiffersFromInterface = Type.ToString() != FrameworkTypeName;

            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlFrameworkType xamlOutputType)
            {
                source.AddLine($"public {FrameworkTypeName} Get{Name}({TargetTypeName} {TargetParameterName}) => {Interface.FrameworkClassName}.Get{Name}(({TargetFrameworkTypeName}) {TargetParameterName});");
                if (SetterMethod != null)
                    source.AddLine($"public void Set{Name}({TargetTypeName} {TargetParameterName}, {FrameworkTypeName} value) => {Interface.FrameworkClassName}.Set{Name}(({TargetFrameworkTypeName}) {TargetParameterName}, value);");
            }
            else
            {
                // TODO: Support this
            }
        }
    }
}
