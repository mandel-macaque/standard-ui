// This file is generated from IRowDefinition.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class RowDefinition : StandardUIDependencyObject, IRowDefinition
    {
        public static readonly System.Windows.DependencyProperty HeightProperty = PropertyUtils.Register(nameof(Height), typeof(GridLength), typeof(RowDefinition), GridLength.Default);
        public static readonly System.Windows.DependencyProperty MinHeightProperty = PropertyUtils.Register(nameof(MinHeight), typeof(double), typeof(RowDefinition), 0.0);
        public static readonly System.Windows.DependencyProperty MaxHeightProperty = PropertyUtils.Register(nameof(MaxHeight), typeof(double), typeof(RowDefinition), double.PositiveInfinity);
        public static readonly System.Windows.DependencyProperty ActualHeightProperty = PropertyUtils.Register(nameof(ActualHeight), typeof(double), typeof(RowDefinition), 0.0);
        
        public GridLength Height
        {
            get => (GridLength) GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }
        
        public double MinHeight
        {
            get => (double) GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }
        
        public double MaxHeight
        {
            get => (double) GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }
        
        public double ActualHeight => (double) GetValue(ActualHeightProperty);
    }
}
