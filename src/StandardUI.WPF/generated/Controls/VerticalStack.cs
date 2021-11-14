// This file is generated from IVerticalStack.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class VerticalStack : StackBase, IVerticalStack
    {
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            VerticalStackLayoutManager.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(constraint)).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            VerticalStackLayoutManager.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(arrangeSize)).ToWpfSize();
    }
}
