// This file is generated from IVisualStateGroup.cs. Update the source file to change its contents.

using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI
{
    public class VisualStateGroup : UIPropertyObject, IVisualStateGroup
    {
        public static readonly DependencyProperty CurrentStateProperty = PropertyUtils.Register(nameof(CurrentState), typeof(VisualState), typeof(VisualStateGroup), null);
        public static readonly DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualStateGroup), "");
        public static readonly DependencyProperty StatesProperty = PropertyUtils.Register(nameof(States), typeof(VisualStateCollection), typeof(VisualStateGroup), null);
        
        private VisualStateCollection _states;
        
        public VisualStateGroup()
        {
            _states = new VisualStateCollection();
            SetValue(StatesProperty, _states);
        }
        
        public VisualState CurrentState => (VisualState) GetValue(CurrentStateProperty);
        IVisualState IVisualStateGroup.CurrentState => CurrentState;
        
        public string Name => (string) GetValue(NameProperty);
        
        public VisualStateCollection States => _states;
        IVisualStateCollection IVisualStateGroup.States => States;
    }
}
