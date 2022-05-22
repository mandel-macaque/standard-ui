// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class Grid : GridBase, IGrid
    {
        public static readonly UIProperty ColumnDefinitionsProperty = new UIProperty(nameof(ColumnDefinitions), null, readOnly:true);
        public static readonly UIProperty RowDefinitionsProperty = new UIProperty(nameof(RowDefinitions), null, readOnly:true);
        public static readonly UIProperty ChildrenProperty = new UIProperty(nameof(Children), null, readOnly:true);
        public static readonly AttachedUIProperty RowProperty = new AttachedUIProperty("Row", 0);
        public static readonly AttachedUIProperty ColumnProperty = new AttachedUIProperty("Column", 0);
        public static readonly AttachedUIProperty RowSpanProperty = new AttachedUIProperty("RowSpan", 1);
        public static readonly AttachedUIProperty ColumnSpanProperty = new AttachedUIProperty("ColumnSpan", 1);
        
        public static int GetRow(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, RowProperty);
        public static void SetRow(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, RowProperty, value);
        
        public static int GetColumn(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, ColumnProperty);
        public static void SetColumn(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, ColumnProperty, value);
        
        public static int GetRowSpan(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, RowSpanProperty);
        public static void SetRowSpan(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, RowSpanProperty, value);
        
        public static int GetColumnSpan(System.Windows.Forms.Control element) => (int) AttachedPropertiesValues.GetValue(element, ColumnSpanProperty);
        public static void SetColumnSpan(System.Windows.Forms.Control element, int value) => AttachedPropertiesValues.SetValue(element, ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRowDefinition> _rowDefinitions;
        private UIElementCollection<Microsoft.StandardUI.IUIElement> _children;
        
        public Grid()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
            _children = new UIElementCollection<Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IGrid.RowDefinitions => RowDefinitions;
        
        public UIElementCollection<Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IPanel.Children => Children.ToStandardUIElementCollection();
    }
}
