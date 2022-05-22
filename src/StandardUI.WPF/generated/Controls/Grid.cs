// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Grid : GridBase, IGrid
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(UICollection<IColumnDefinition>), typeof(Grid), null);
        public static readonly DependencyProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(UICollection<IRowDefinition>), typeof(Grid), null);
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement>), typeof(Panel), null);
        public static readonly DependencyProperty RowProperty = PropertyUtils.RegisterAttached("Row", typeof(int), typeof(System.Windows.UIElement), 0);
        public static readonly DependencyProperty ColumnProperty = PropertyUtils.RegisterAttached("Column", typeof(int), typeof(System.Windows.UIElement), 0);
        public static readonly DependencyProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        public static readonly DependencyProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(System.Windows.UIElement), 1);
        
        public static int GetRow(System.Windows.UIElement element) => (int) element.GetValue(RowProperty);
        public static void SetRow(System.Windows.UIElement element, int value) => element.SetValue(RowProperty, value);
        
        public static int GetColumn(System.Windows.UIElement element) => (int) element.GetValue(ColumnProperty);
        public static void SetColumn(System.Windows.UIElement element, int value) => element.SetValue(ColumnProperty, value);
        
        public static int GetRowSpan(System.Windows.UIElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(System.Windows.UIElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(System.Windows.UIElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(System.Windows.UIElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRowDefinition> _rowDefinitions;
        private UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement> _children;
        
        public Grid()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
            _children = new UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IGrid.RowDefinitions => RowDefinitions;
        
        public UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IPanel.Children => Children.ToStandardUIElementCollection();
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            GridLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            GridLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWpfSize();
    }
}
