// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class Table : GridBase, ITable
    {
        public static readonly UIProperty ColumnDefinitionsProperty = new UIProperty(nameof(ColumnDefinitions), null, readOnly:true);
        public static readonly UIProperty RowsProperty = new UIProperty(nameof(Rows), null, readOnly:true);
        public static readonly AttachedUIProperty RowSpanProperty = new AttachedUIProperty("RowSpan", 1);
        public static readonly AttachedUIProperty ColumnSpanProperty = new AttachedUIProperty("ColumnSpan", 1);
        
        public static int GetRowSpan(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, RowSpanProperty);
        public static void SetRowSpan(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, RowSpanProperty, value);
        
        public static int GetColumnSpan(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, ColumnSpanProperty);
        public static void SetColumnSpan(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRow> _rows;
        
        public Table()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rows = new UICollection<IRow>(this);
            SetValue(RowsProperty, _rows);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> ITable.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRow> Rows => _rows;
        IUICollection<IRow> ITable.Rows => Rows;
    }
}
