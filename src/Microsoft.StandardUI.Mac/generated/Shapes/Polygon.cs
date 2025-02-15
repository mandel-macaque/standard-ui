// This file is generated from IPolygon.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Mac.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public static readonly UIProperty FillRuleProperty = new UIProperty(nameof(FillRule), FillRule.EvenOdd);
        public static readonly UIProperty PointsProperty = new UIProperty(nameof(Points), Points.Default);
        
        public FillRule FillRule
        {
            get => (FillRule) GetNonNullValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
        
        public Points Points
        {
            get => (Points) GetNonNullValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawPolygon(this);
    }
}
