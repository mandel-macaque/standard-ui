// This file is generated from ITargetPropertyPath.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Mac
{
    public class TargetPropertyPath : StandardUIObject, ITargetPropertyPath
    {
        public static readonly UIProperty PropertyProperty = new UIProperty(nameof(Property), null);
        public static readonly UIProperty TargetProperty = new UIProperty(nameof(Target), null);
        
        public PropertyPath Property
        {
            get => (PropertyPath) GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }
        IPropertyPath ITargetPropertyPath.Property
        {
            get => Property;
            set => Property = (PropertyPath) value;
        }
        
        public object Target
        {
            get => (object) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
    }
}
