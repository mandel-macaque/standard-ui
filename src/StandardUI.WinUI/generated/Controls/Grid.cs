// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Grid : GridBase, IGrid
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(UICollection<IColumnDefinition>), typeof(Grid), null);
        public static readonly DependencyProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(UICollection<IRowDefinition>), typeof(Grid), null);
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>), typeof(Panel), null);
        public static readonly DependencyProperty RowProperty = PropertyUtils.RegisterAttached("Row", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 0);
        public static readonly DependencyProperty ColumnProperty = PropertyUtils.RegisterAttached("Column", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 0);
        public static readonly DependencyProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 1);
        public static readonly DependencyProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(Microsoft.UI.Xaml.FrameworkElement), 1);
        
        public static int GetRow(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(RowProperty);
        public static void SetRow(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(RowProperty, value);
        
        public static int GetColumn(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(ColumnProperty);
        public static void SetColumn(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(ColumnProperty, value);
        
        public static int GetRowSpan(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(Microsoft.UI.Xaml.FrameworkElement element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(Microsoft.UI.Xaml.FrameworkElement element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRowDefinition> _rowDefinitions;
        private UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> _children;
        
        public Grid()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
            _children = new UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IGrid.RowDefinitions => RowDefinitions;
        
        public UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IPanel.Children => Children.ToStandardUIElementCollection();
        
        protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>
            GridLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWindowsFoundationSize();
        
        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize) =>
            GridLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWindowsFoundationSize();
    }
}
