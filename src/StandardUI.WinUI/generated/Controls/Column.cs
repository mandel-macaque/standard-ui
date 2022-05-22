// This file is generated from IColumn.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Column : ColumnDefinition, IColumn
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>), typeof(Column), null);
        
        private UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> _children;
        
        public Column()
        {
            _children = new UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IColumn.Children => Children.ToStandardUIElementCollection();
    }
}
