using System.Collections.Generic;

namespace Microsoft.StandardUI
{
    public struct PropertyValues
    {
        private Dictionary<UIProperty, object?> _propertiesToValue;

        public PropertyValues(bool init)
        {
            _propertiesToValue = new Dictionary<UIProperty, object?>();
        }

        public object? GetValue(UIProperty property)
        {
            if (_propertiesToValue.TryGetValue(property, out object? value))
                return value;

            return property.DefaultValue;
        }

        public object? ReadLocalValue(UIProperty property)
        {
            if (_propertiesToValue.TryGetValue(property, out object? value))
                return value;
            return UnsetValue.Instance;
        }

        public void SetValue(UIProperty property, object? value)
        {
            _propertiesToValue[property] = value;
        }

        public void ClearValue(UIProperty property)
        {
            _propertiesToValue.Remove(property);
        }
    }
}
