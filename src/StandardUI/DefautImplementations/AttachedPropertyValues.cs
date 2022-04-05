using System.Runtime.CompilerServices;

namespace Microsoft.StandardUI.DefaultImplementations
{
    public class AttachedPropertyValues
    {
        private AttachedUIProperty _property;
        private ConditionalWeakTable<object, object?> _objectToValue;

        public AttachedPropertyValues(AttachedUIProperty property)
        {
            _property = property;
        }

        public object? GetValue(object obj)
        {
            if (!_objectToValue.TryGetValue(obj, out object? value))
                return _property.DefaultValue;
            return value;
        }

        public object? ReadLocalValue(object obj)
        {
            if (!_objectToValue.TryGetValue(obj, out object? value))
                return UnsetValue.Instance;
            return value;
        }

        public void SetValue(object obj, object? value)
        {
            // Do a Remove/Add as the AddOrUpdate method is only available in .NET Core and above
            _objectToValue.Remove(obj);
            _objectToValue.Add(obj, value);
        }

        public void ClearValue(object obj)
        {
            _objectToValue.Remove(obj);
        }
    }
}
