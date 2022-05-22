// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class TableAttached : ITableAttached
    {
        public static TableAttached Instance = new TableAttached();
        
        public int GetRowSpan(IUIElement element) => Table.GetRowSpan((System.Windows.UIElement) element);
        public void SetRowSpan(IUIElement element, int value) => Table.SetRowSpan((System.Windows.UIElement) element, value);
        
        public int GetColumnSpan(IUIElement element) => Table.GetColumnSpan((System.Windows.UIElement) element);
        public void SetColumnSpan(IUIElement element, int value) => Table.SetColumnSpan((System.Windows.UIElement) element, value);
    }
}
