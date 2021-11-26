// This file is generated from IStack.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Stack : StackBase, IStack
    {
        public static readonly System.Windows.DependencyProperty OrientationProperty = PropertyUtils.Register(nameof(Orientation), typeof(Orientation), typeof(Stack), Orientation.Vertical);
        
        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            StackLayoutManager.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(constraint)).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            StackLayoutManager.Instance.ArrangeOverride(this, SizeExtensions.FromWpfSize(arrangeSize)).ToWpfSize();
    }
}
