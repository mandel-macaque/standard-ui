// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Table : GridBase, ITable
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(UICollection<IColumnDefinition>), typeof(Table), null);
        public static readonly DependencyProperty RowsProperty = PropertyUtils.Register(nameof(Rows), typeof(UICollection<IRow>), typeof(Table), null);
        public static readonly DependencyProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        public static readonly DependencyProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        
        public static int GetRowSpan(System.Windows.UIElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(System.Windows.UIElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(System.Windows.UIElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(System.Windows.UIElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
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
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            TableLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            TableLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWpfSize();
    }
}
