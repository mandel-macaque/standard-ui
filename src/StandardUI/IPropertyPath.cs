﻿namespace Microsoft.StandardUI
{
    /// <summary>
    /// Implements a data structure for describing a property as a path below another property, or
    /// below an owning type. Property paths are used in data binding to objects.
    /// </summary>
    [UIModelObject]
    public interface IPropertyPath
    {
        /// <summary>
        /// Gets the path value held by this PropertyPath.
        /// </summary>
        [DefaultValue("")]
        public string Path { get; }
    }
}
