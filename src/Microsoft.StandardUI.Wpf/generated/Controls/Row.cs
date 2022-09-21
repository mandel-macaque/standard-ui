// This file is generated from IRow.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Row : Panel, IRow
    {
        public static new readonly DependencyProperty HeightProperty = PropertyUtils.Register(nameof(Height), typeof(GridLength), typeof(RowDefinition), GridLength.Default);
        public static new readonly DependencyProperty MinHeightProperty = PropertyUtils.Register(nameof(MinHeight), typeof(double), typeof(RowDefinition), 0.0);
        public static new readonly DependencyProperty MaxHeightProperty = PropertyUtils.Register(nameof(MaxHeight), typeof(double), typeof(RowDefinition), double.PositiveInfinity);
        public static new readonly DependencyProperty ActualHeightProperty = PropertyUtils.Register(nameof(ActualHeight), typeof(double), typeof(RowDefinition), 0.0);
        
        public new GridLength Height
        {
            get => (GridLength) GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }
        
        public new double MinHeight
        {
            get => (double) GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }
        
        public new double MaxHeight
        {
            get => (double) GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }
        
        public new double ActualHeight => (double) GetValue(ActualHeightProperty);
    }
}
