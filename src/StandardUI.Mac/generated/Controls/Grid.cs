// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Mac.Controls
{
    public class Grid : Panel, IGrid
    {
        public static readonly UIProperty ColumnSpacingProperty = new UIProperty(nameof(ColumnSpacing), 0.0);
        public static readonly UIProperty RowSpacingProperty = new UIProperty(nameof(RowSpacing), 0.0);
        public static readonly UIProperty ColumnDefinitionsProperty = new UIProperty(nameof(ColumnDefinitions), null, readOnly:true);
        public static readonly UIProperty RowDefinitionsProperty = new UIProperty(nameof(RowDefinitions), null, readOnly:true);
        public static readonly AttachedUIProperty RowProperty = new AttachedUIProperty("Row", 0);
        public static readonly AttachedUIProperty ColumnProperty = new AttachedUIProperty("Column", 0);
        public static readonly AttachedUIProperty RowSpanProperty = new AttachedUIProperty("RowSpan", 1);
        public static readonly AttachedUIProperty ColumnSpanProperty = new AttachedUIProperty("ColumnSpan", 1);
        
        public static int GetRow(StandardUIElement element) => (int) element.GetValue(RowProperty);
        public static void SetRow(StandardUIElement element, int value) => element.SetValue(RowProperty, value);
        
        public static int GetColumn(StandardUIElement element) => (int) element.GetValue(ColumnProperty);
        public static void SetColumn(StandardUIElement element, int value) => element.SetValue(ColumnProperty, value);
        
        public static int GetRowSpan(StandardUIElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(StandardUIElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(StandardUIElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(StandardUIElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRowDefinition> _rowDefinitions;
        
        public Grid()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
        }
        
        public double ColumnSpacing
        {
            get => (double) GetValue(ColumnSpacingProperty);
            set => SetValue(ColumnSpacingProperty, value);
        }
        
        public double RowSpacing
        {
            get => (double) GetValue(RowSpacingProperty);
            set => SetValue(RowSpacingProperty, value);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IGrid.RowDefinitions => RowDefinitions;
    }
}
