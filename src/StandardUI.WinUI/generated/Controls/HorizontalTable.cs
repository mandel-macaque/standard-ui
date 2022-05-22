// This file is generated from IHorizontalTable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class HorizontalTable : GridBase, IHorizontalTable
    {
        public static readonly DependencyProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(UICollection<IRowDefinition>), typeof(HorizontalTable), null);
        public static readonly DependencyProperty ColumnsProperty = PropertyUtils.Register(nameof(Columns), typeof(UICollection<IColumn>), typeof(HorizontalTable), null);
        
        private UICollection<IRowDefinition> _rowDefinitions;
        private UICollection<IColumn> _columns;
        
        public HorizontalTable()
        {
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
            _columns = new UICollection<IColumn>(this);
            SetValue(ColumnsProperty, _columns);
        }
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IHorizontalTable.RowDefinitions => RowDefinitions;
        
        public UICollection<IColumn> Columns => _columns;
        IUICollection<IColumn> IHorizontalTable.Columns => Columns;
        
        protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>
            HorizontalTableLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWindowsFoundationSize();
        
        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize) =>
            HorizontalTableLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWindowsFoundationSize();
    }
}
