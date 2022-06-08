// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class TableAttached : ITableAttached
    {
        public static TableAttached Instance = new TableAttached();
        
        public int GetRowSpan(IUIElement element) => Table.GetRowSpan(element.ToFrameworkElement());
        public void SetRowSpan(IUIElement element, int value) => Table.SetRowSpan(element.ToFrameworkElement(), value);
        
        public int GetColumnSpan(IUIElement element) => Table.GetColumnSpan(element.ToFrameworkElement());
        public void SetColumnSpan(IUIElement element, int value) => Table.SetColumnSpan(element.ToFrameworkElement(), value);
    }
}
