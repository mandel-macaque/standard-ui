// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class GridAttached : IGridAttached
    {
        public static GridAttached Instance = new GridAttached();
        
        public int GetRow(IUIElement element) => Grid.GetRow((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetRow(IUIElement element, int value) => Grid.SetRow((Microsoft.UI.Xaml.FrameworkElement) element, value);
        
        public int GetColumn(IUIElement element) => Grid.GetColumn((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetColumn(IUIElement element, int value) => Grid.SetColumn((Microsoft.UI.Xaml.FrameworkElement) element, value);
        
        public int GetRowSpan(IUIElement element) => Grid.GetRowSpan((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetRowSpan(IUIElement element, int value) => Grid.SetRowSpan((Microsoft.UI.Xaml.FrameworkElement) element, value);
        
        public int GetColumnSpan(IUIElement element) => Grid.GetColumnSpan((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetColumnSpan(IUIElement element, int value) => Grid.SetColumnSpan((Microsoft.UI.Xaml.FrameworkElement) element, value);
    }
}
