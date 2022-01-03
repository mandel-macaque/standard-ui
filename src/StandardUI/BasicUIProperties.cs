using System.Collections.Generic;
namespace Microsoft.StandardUI
{
    public struct BasicUIProperties
    {
        private Dictionary<BasicUIProperty, object?> _properties;

        public BasicUIProperties(bool init)
        {
            _properties = new Dictionary<BasicUIProperty, object?>();
        }

        public object? GetValue(BasicUIProperty property)
        {
            if (_properties.TryGetValue(property, out object? value))
                return value;

            return property.DefaultValue;
        }

        public object? ReadLocalValue(BasicUIProperty property)
        {
            if (_properties.TryGetValue(property, out object? value))
                return value;
            return UnsetValue.Instance;
        }

        public void SetValue(BasicUIProperty property, object? value)
        {
            _properties[property] = value;
        }

        public void ClearValue(BasicUIProperty property)
        {
            _properties.Remove(property);
        }
    }
}
