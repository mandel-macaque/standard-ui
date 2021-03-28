﻿namespace System.StandardUI
{
    [UIModelObject]
    public interface IVisualStateGroup : IUIPropertyObject
    {
        /// <summary>
        /// Gets the most recently set VisualState from a successful call to the GoToState method.
        /// </summary>
        [DefaultValue(null)]
        public IVisualState CurrentState { get; }

        /// <summary>
        /// Gets the name of the VisualStateGroup.
        /// </summary>
        [DefaultValue("")]
        public string Name { get; }

        /// <summary>
        /// Gets the collection of mutually exclusive VisualState objects.
        /// </summary>
        public IVisualStateCollection States { get; }
    }
}
