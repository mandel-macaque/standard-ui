using System.Collections.Generic;

namespace Microsoft.StandardUI
{
    public static class AttachedPropertiesValues
    {
        private static Dictionary<AttachedUIProperty, AttachedPropertyValues> _propertiesToValues = new();

        public static object? GetValue(object obj, AttachedUIProperty property)
        {
            if (!_propertiesToValues.TryGetValue(property, out AttachedPropertyValues values))
                return property.DefaultValue;

            return values.GetValue(obj);
        }

        public static object? ReadLocalValue(object obj, AttachedUIProperty property)
        {
            if (!_propertiesToValues.TryGetValue(property, out AttachedPropertyValues values))
                return UnsetValue.Instance;

            return values.ReadLocalValue(obj);
        }

        public static void SetValue(object obj, AttachedUIProperty property, object? value)
        {
            if (!_propertiesToValues.TryGetValue(property, out AttachedPropertyValues values))
            {
                values = new AttachedPropertyValues(property);
                _propertiesToValues.Add(property, values);
            }

            values.SetValue(obj, value);
        }

        public static void ClearValue(object obj, AttachedUIProperty property)
        {
            if (!_propertiesToValues.TryGetValue(property, out AttachedPropertyValues values))
                return;

            values.ClearValue(obj);
        }
    }
}
