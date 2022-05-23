// This file is generated from ITable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Table : GridBase, ITable
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(UICollection<IColumnDefinition>), typeof(Table), null);
        public static readonly DependencyProperty RowsProperty = PropertyUtils.Register(nameof(Rows), typeof(UIElementCollection<Row,IRow>), typeof(Table), null);
        public static readonly DependencyProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 1);
        public static readonly DependencyProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 1);
        
        public static int GetRowSpan(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UIElementCollection<Row,IRow> _rows;
        
        public Table()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rows = new UIElementCollection<Row,IRow>(this);
            SetValue(RowsProperty, _rows);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> ITable.ColumnDefinitions => ColumnDefinitions;
        
        public UIElementCollection<Row,IRow> Rows => _rows;
        IUICollection<IRow> ITable.Rows => Rows.ToStandardUIElementCollection();
        
        protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>
            TableLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWindowsFoundationSize();
        
        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize) =>
            TableLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWindowsFoundationSize();
    }
}
