// This file is generated from IRow.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class Row : RowDefinition, IRow
    {
        public static readonly UIProperty ChildrenProperty = new UIProperty(nameof(Children), null, readOnly:true);
        
        private UIElementCollection<Microsoft.StandardUI.IUIElement> _children;
        
        public Row()
        {
            _children = new UIElementCollection<Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection<Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IRow.Children => Children.ToStandardUIElementCollection();
    }
}
