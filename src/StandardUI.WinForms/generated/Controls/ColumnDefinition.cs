// This file is generated from IColumnDefinition.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class ColumnDefinition : StandardUIObject, IColumnDefinition
    {
        public static readonly UIProperty WidthProperty = new UIProperty(nameof(Width), GridLength.Default);
        public static readonly UIProperty MinWidthProperty = new UIProperty(nameof(MinWidth), 0.0);
        public static readonly UIProperty MaxWidthProperty = new UIProperty(nameof(MaxWidth), double.PositiveInfinity);
        public static readonly UIProperty ActualWidthProperty = new UIProperty(nameof(ActualWidth), 0.0, readOnly:true);
        
        public GridLength Width
        {
            get => (GridLength) GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        
        public double MinWidth
        {
            get => (double) GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }
        
        public double MaxWidth
        {
            get => (double) GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }
        
        public double ActualWidth => (double) GetValue(ActualWidthProperty);
    }
}
