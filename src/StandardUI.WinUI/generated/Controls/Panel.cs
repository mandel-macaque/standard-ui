// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class Panel : StandardUIFrameworkElement, IPanel
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        private UIElementCollection _children;
        
        public Panel()
        {
            _children = new UIElementCollection(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection Children => _children;
        IUIElementCollection IPanel.Children => Children;
    }
}
