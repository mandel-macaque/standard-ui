// This file is generated from IColumn.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Column : Panel, IColumn
    {
        public static new readonly DependencyProperty WidthProperty = PropertyUtils.Register(nameof(Width), typeof(GridLength), typeof(ColumnDefinition), GridLength.Default);
        public static new readonly DependencyProperty MinWidthProperty = PropertyUtils.Register(nameof(MinWidth), typeof(double), typeof(ColumnDefinition), 0.0);
        public static new readonly DependencyProperty MaxWidthProperty = PropertyUtils.Register(nameof(MaxWidth), typeof(double), typeof(ColumnDefinition), double.PositiveInfinity);
        public static new readonly DependencyProperty ActualWidthProperty = PropertyUtils.Register(nameof(ActualWidth), typeof(double), typeof(ColumnDefinition), 0.0);
        
        public new GridLength Width
        {
            get => (GridLength) GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        
        public new double MinWidth
        {
            get => (double) GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }
        
        public new double MaxWidth
        {
            get => (double) GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }
        
        public new double ActualWidth => (double) GetValue(ActualWidthProperty);
    }
}
