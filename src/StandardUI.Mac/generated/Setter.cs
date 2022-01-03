// This file is generated from ISetter.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Mac
{
    public class Setter : StandardUIObject, ISetter
    {
        public static readonly UIProperty PropertyProperty = new UIProperty(nameof(Property), null);
        public static readonly UIProperty TargetProperty = new UIProperty(nameof(Target), null);
        public static readonly UIProperty ValueProperty = new UIProperty(nameof(Value), null);
        
        public UIProperty? Property
        {
            get => (UIProperty?) GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }
        IUIProperty? ISetter.Property
        {
            get => Property;
            set => Property = (UIProperty?) value;
        }
        
        public TargetPropertyPath Target
        {
            get => (TargetPropertyPath) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
        ITargetPropertyPath ISetter.Target
        {
            get => Target;
            set => Target = (TargetPropertyPath) value;
        }
        
        public object Value
        {
            get => (object) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
