// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Panel : StandardUIFrameworkElement, IPanel
    {
        public static readonly System.Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        private UIElementCollection _children;
        
        public Panel()
        {
            _children = new UIElementCollection(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection Children => _children;
        IUIElementCollection IPanel.Children => Children;
        
        protected override int VisualChildrenCount => _children.Count;
        
        protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _children[index];
    }
}
