// This file is generated from IRow.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Row : RowDefinition, IRow
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement>), typeof(Row), null);
        
        private UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement> _children;
        
        public Row()
        {
            _children = new UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection<System.Windows.FrameworkElement,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IRow.Children => Children.ToStandardUIElementCollection();
    }
}
