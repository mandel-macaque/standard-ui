// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly System.Windows.DependencyProperty LeftProperty = PropertyUtils.RegisterAttached("Left", typeof(double), typeof(System.Windows.UIElement), 0.0);
        public static readonly System.Windows.DependencyProperty TopProperty = PropertyUtils.RegisterAttached("Top", typeof(double), typeof(System.Windows.UIElement), 0.0);
        
        public static double GetLeft(System.Windows.UIElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(System.Windows.UIElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(System.Windows.UIElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(System.Windows.UIElement element, double value) => element.SetValue(TopProperty, value);
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            CanvasLayoutManager.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(constraint)).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            CanvasLayoutManager.Instance.ArrangeOverride(this, SizeExtensions.FromWpfSize(arrangeSize)).ToWpfSize();
    }
}
