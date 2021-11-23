// This file is generated from IVisualState.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Wpf
{
    public class VisualState : UIPropertyObject, IVisualState
    {
        public static readonly System.Windows.DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualState), "");
        public static readonly System.Windows.DependencyProperty SettersProperty = PropertyUtils.Register(nameof(Setters), typeof(SetterCollection), typeof(VisualState), null);
        
        private SetterCollection _setters;
        
        public VisualState()
        {
            _setters = new SetterCollection();
            SetValue(SettersProperty, _setters);
        }
        
        public string Name => (string) GetValue(NameProperty);
        
        public SetterCollection Setters => _setters;
        ISetterCollection IVisualState.Setters => Setters;
    }
}
