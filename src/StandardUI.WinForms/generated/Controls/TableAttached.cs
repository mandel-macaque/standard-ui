// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class TableAttached : ITableAttached
    {
        public static TableAttached Instance = new TableAttached();
        
        public int GetRowSpan(IUIElement element) => Table.GetRowSpan((System.Windows.Forms.Control) element);
        public void SetRowSpan(IUIElement element, int value) => Table.SetRowSpan((System.Windows.Forms.Control) element, value);
        
        public int GetColumnSpan(IUIElement element) => Table.GetColumnSpan((System.Windows.Forms.Control) element);
        public void SetColumnSpan(IUIElement element, int value) => Table.SetColumnSpan((System.Windows.Forms.Control) element, value);
    }
}
