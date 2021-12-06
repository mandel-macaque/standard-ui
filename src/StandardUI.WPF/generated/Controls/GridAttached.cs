// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class GridAttached : IGridAttached
    {
        public static GridAttached Instance = new GridAttached();
        
        public int GetRow(IUIElement element) => Grid.GetRow((System.Windows.UIElement) element);
        public void SetRow(IUIElement element, int value) => Grid.SetRow((System.Windows.UIElement) element, value);
        
        public int GetColumn(IUIElement element) => Grid.GetColumn((System.Windows.UIElement) element);
        public void SetColumn(IUIElement element, int value) => Grid.SetColumn((System.Windows.UIElement) element, value);
        
        public int GetRowSpan(IUIElement element) => Grid.GetRowSpan((System.Windows.UIElement) element);
        public void SetRowSpan(IUIElement element, int value) => Grid.SetRowSpan((System.Windows.UIElement) element, value);
        
        public int GetColumnSpan(IUIElement element) => Grid.GetColumnSpan((System.Windows.UIElement) element);
        public void SetColumnSpan(IUIElement element, int value) => Grid.SetColumnSpan((System.Windows.UIElement) element, value);
    }
}
