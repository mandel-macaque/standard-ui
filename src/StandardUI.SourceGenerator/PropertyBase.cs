using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class PropertyBase
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public string Name { get; }
        public ITypeSymbol Type { get; }
        public bool IsReadOnly { get; }
        public string TypeName { get; }
        public ITypeSymbol? UICollectionElementType { get; }
        public TypedConstant? SpecifiedDefaultValue { get; protected set; }
        public string FullPropertyName { get; }

        public PropertyBase(Context context, Interface intface, string name, ITypeSymbol type, bool isReadOnly, string containingTypeName)
        {
            Context = context;
            Interface = intface;
            Name = name;
            Type = type;
            IsReadOnly = isReadOnly;
            FullPropertyName = $"{containingTypeName}.{name}";
            TypeName = Utils.ToTypeName(Type);

            if (Utils.IsUICollectionType(Type, out ITypeSymbol uiCollectionElementType))
                UICollectionElementType = uiCollectionElementType;
            else UICollectionElementType = null;
        }

        public bool IsUICollection => UICollectionElementType != null;

        public TypedConstant? GetSpecifiedDefaultValue(ImmutableArray<AttributeData> attributes)
        {
            foreach (AttributeData attribute in attributes)
            {
                var attributeTypeFullName = Utils.GetTypeFullName(attribute.AttributeClass!);
                if (attributeTypeFullName != "Microsoft.StandardUI.DefaultValueAttribute")
                    continue;

                ImmutableArray<TypedConstant> constructorArguments = attribute.ConstructorArguments;

                if (constructorArguments.Length != 1)
                    throw new UserViewableException($"Property {FullPropertyName} should have a single argument for the [DefaultValue] attribute");
                return constructorArguments[0];
            }

            return null;
        }
    }
}
