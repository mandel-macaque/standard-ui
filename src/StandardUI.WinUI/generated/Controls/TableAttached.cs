// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class TableAttached : ITableAttached
    {
        public static TableAttached Instance = new TableAttached();
        
        public int GetRowSpan(IUIElement element) => Table.GetRowSpan((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetRowSpan(IUIElement element, int value) => Table.SetRowSpan((Microsoft.UI.Xaml.FrameworkElement) element, value);
        
        public int GetColumnSpan(IUIElement element) => Table.GetColumnSpan((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetColumnSpan(IUIElement element, int value) => Table.SetColumnSpan((Microsoft.UI.Xaml.FrameworkElement) element, value);
    }
}
