// This file is generated from IRow.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Row : RowDefinition, IRow
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>), typeof(Row), null);
        
        private UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> _children;
        
        public Row()
        {
            _children = new UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection<Microsoft.UI.Xaml.FrameworkElement,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IRow.Children => Children.ToStandardUIElementCollection();
    }
}
