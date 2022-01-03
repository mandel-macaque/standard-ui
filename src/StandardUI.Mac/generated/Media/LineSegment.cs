// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly UIProperty PointProperty = new UIProperty(nameof(Point), Point.Default);
        
        public Point Point
        {
            get => (Point) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
    }
}
