// This file is generated from IVisualStateGroup.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;

namespace Microsoft.StandardUI.Mac
{
    public class VisualStateGroup : StandardUIObject, IVisualStateGroup
    {
        public static readonly UIProperty CurrentStateProperty = new UIProperty(nameof(CurrentState), null, readOnly:true);
        public static readonly UIProperty NameProperty = new UIProperty(nameof(Name), "", readOnly:true);
        public static readonly UIProperty StatesProperty = new UIProperty(nameof(States), null, readOnly:true);
        
        private UICollection<IVisualState> _states;
        
        public VisualStateGroup()
        {
            _states = new UICollection<IVisualState>(this);
            SetValue(StatesProperty, _states);
        }
        
        public VisualState CurrentState => (VisualState) GetValue(CurrentStateProperty);
        IVisualState IVisualStateGroup.CurrentState => CurrentState;
        
        public string Name => (string) GetValue(NameProperty);
        
        public UICollection<IVisualState> States => _states;
        IUICollection<IVisualState> IVisualStateGroup.States => States;
    }
}
