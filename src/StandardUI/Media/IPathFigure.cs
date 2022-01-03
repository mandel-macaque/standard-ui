using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IPathFigure : IUIObject
    {
        IEnumerable<IPathSegment> Segments { get; set; }

        Point StartPoint { get; set; }

        [DefaultValue(false)]
        bool IsClosed { get; set; }

        [DefaultValue(true)]
        bool IsFilled { get; set; }
    }
}
