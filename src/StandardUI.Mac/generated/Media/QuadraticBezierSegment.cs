// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public static readonly UIProperty Point1Property = new UIProperty(nameof(Point1), Point.Default);
        public static readonly UIProperty Point2Property = new UIProperty(nameof(Point2), Point.Default);
        
        public Point Point1
        {
            get => (Point) GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }
        
        public Point Point2
        {
            get => (Point) GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
    }
}
