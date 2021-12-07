// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Grid : Panel, IGrid
    {
        public static readonly System.Windows.DependencyProperty ColumnSpacingProperty = PropertyUtils.Register(nameof(ColumnSpacing), typeof(double), typeof(Grid), 0.0);
        public static readonly System.Windows.DependencyProperty RowSpacingProperty = PropertyUtils.Register(nameof(RowSpacing), typeof(double), typeof(Grid), 0.0);
        public static readonly System.Windows.DependencyProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(ColumnDefinitionCollection), typeof(Grid), null);
        public static readonly System.Windows.DependencyProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(RowDefinitionCollection), typeof(Grid), null);
        public static readonly System.Windows.DependencyProperty RowProperty = PropertyUtils.RegisterAttached("Row", typeof(int), typeof(System.Windows.UIElement), 0);
        public static readonly System.Windows.DependencyProperty ColumnProperty = PropertyUtils.RegisterAttached("Column", typeof(int), typeof(System.Windows.UIElement), 0);
        public static readonly System.Windows.DependencyProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        public static readonly System.Windows.DependencyProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        
        public static int GetRow(System.Windows.UIElement element) => (int) element.GetValue(RowProperty);
        public static void SetRow(System.Windows.UIElement element, int value) => element.SetValue(RowProperty, value);
        
        public static int GetColumn(System.Windows.UIElement element) => (int) element.GetValue(ColumnProperty);
        public static void SetColumn(System.Windows.UIElement element, int value) => element.SetValue(ColumnProperty, value);
        
        public static int GetRowSpan(System.Windows.UIElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(System.Windows.UIElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(System.Windows.UIElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(System.Windows.UIElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private ColumnDefinitionCollection _columnDefinitions;
        private RowDefinitionCollection _rowDefinitions;
        
        public Grid()
        {
            _columnDefinitions = new ColumnDefinitionCollection();
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new RowDefinitionCollection();
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
        
        public ColumnDefinitionCollection ColumnDefinitions => _columnDefinitions;
        IColumnDefinitionCollection IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public RowDefinitionCollection RowDefinitions => _rowDefinitions;
        IRowDefinitionCollection IGrid.RowDefinitions => RowDefinitions;
    }
}
