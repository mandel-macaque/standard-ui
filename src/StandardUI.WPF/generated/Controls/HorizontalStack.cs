// This file is generated from IHorizontalStack.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class HorizontalStack : StackBase, IHorizontalStack
    {
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            HorizontalStackLayoutManager.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(constraint)).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            HorizontalStackLayoutManager.Instance.ArrangeOverride(this, SizeExtensions.FromWpfSize(arrangeSize)).ToWpfSize();
    }
}
