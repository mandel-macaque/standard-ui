using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Property
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public IPropertySymbol SourceProperty { get; }
        public bool HasSetter { get; }
        public string Name { get; }
        public ITypeSymbol Type { get; }
        public string TypeName { get; }
        public string FrameworkTypeName { get; }
        public string DefaultValue { get; }
        public bool IsCollection { get; }
        public string FieldName { get; }

        public Property(Context context, Interface intface, IPropertySymbol propertySymbol)
        {
            Context = context;
            Interface = intface;
            SourceProperty = propertySymbol;
            HasSetter = !propertySymbol.IsReadOnly;
            Name = propertySymbol.Name;
            Type = propertySymbol.Type;
            TypeName = context.ToTypeName(Type);
            FrameworkTypeName = context.ToFrameworkTypeName(Type);
            DefaultValue = context.GetDefaultValue(propertySymbol.GetAttributes(), $"{Interface.Name}.{Name}", propertySymbol.Type);
            IsCollection = Context.IsCollectionType(Type) != null;
            FieldName = "_" + Context.TypeNameToVariableName(FrameworkTypeName);
        }

        public void GenerateExtensionClassMethods(Source source)
        {
            if (!HasSetter)
                return;

            source.Usings.AddTypeNamespace(Type);

            string interfaceVariableName = Interface.VariableName;

            source.AddBlankLineIfNonempty();
            source.AddLines(
                $"public static T {Name}<T>(this T {interfaceVariableName}, {TypeName} value) where T : {Interface.Name}",
                "{");
            using (source.Indent())
            {
                source.AddLines(
                    $"{interfaceVariableName}.{Name} = value;",
                    $"return {interfaceVariableName};");
            }
            source.AddLine(
                "}");
        }
    }
}
