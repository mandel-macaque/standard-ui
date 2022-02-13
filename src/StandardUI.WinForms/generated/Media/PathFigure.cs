// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.WinForms.Media
{
    public class PathFigure : StandardUIObject, IPathFigure
    {
        public static readonly UIProperty SegmentsProperty = new UIProperty(nameof(Segments), null, readOnly:true);
        public static readonly UIProperty StartPointProperty = new UIProperty(nameof(StartPoint), Point.Default);
        public static readonly UIProperty IsClosedProperty = new UIProperty(nameof(IsClosed), false);
        public static readonly UIProperty IsFilledProperty = new UIProperty(nameof(IsFilled), true);
        
        private UICollection<IPathSegment> _segments;
        
        public PathFigure()
        {
            _segments = new UICollection<IPathSegment>(this);
            SetValue(SegmentsProperty, _segments);
        }
        
        public UICollection<IPathSegment> Segments => _segments;
        IUICollection<IPathSegment> IPathFigure.Segments => Segments;
        
        public Point StartPoint
        {
            get => (Point) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        
        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            set => SetValue(IsClosedProperty, value);
        }
        
        public bool IsFilled
        {
            get => (bool) GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, value);
        }
    }
}
