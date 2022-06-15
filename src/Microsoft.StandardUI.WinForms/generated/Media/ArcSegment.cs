// This file is generated from IArcSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.WinForms.Media
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public static readonly UIProperty PointProperty = new UIProperty(nameof(Point), Point.Default);
        public static readonly UIProperty SizeProperty = new UIProperty(nameof(Size), Size.Default);
        public static readonly UIProperty RotationAngleProperty = new UIProperty(nameof(RotationAngle), 0.0);
        public static readonly UIProperty IsLargeArcProperty = new UIProperty(nameof(IsLargeArc), false);
        public static readonly UIProperty SweepDirectionProperty = new UIProperty(nameof(SweepDirection), SweepDirection.Counterclockwise);
        
        public Point Point
        {
            get => (Point) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        
        public Size Size
        {
            get => (Size) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        
        public double RotationAngle
        {
            get => (double) GetValue(RotationAngleProperty);
            set => SetValue(RotationAngleProperty, value);
        }
        
        public bool IsLargeArc
        {
            get => (bool) GetValue(IsLargeArcProperty);
            set => SetValue(IsLargeArcProperty, value);
        }
        
        public SweepDirection SweepDirection
        {
            get => (SweepDirection) GetValue(SweepDirectionProperty);
            set => SetValue(SweepDirectionProperty, value);
        }
    }
}
