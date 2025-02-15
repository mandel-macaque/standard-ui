// This file is generated from IPolyline.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;
using Microsoft.AspNetCore.Components;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Blazor.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public static readonly UIProperty FillRuleProperty = new UIProperty(nameof(FillRule), FillRule.EvenOdd);
        public static readonly UIProperty PointsProperty = new UIProperty(nameof(Points), Points.Default);
        
        [Parameter]
        public FillRule FillRule
        {
            get => (FillRule) GetNonNullValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
        
        [Parameter]
        public Points Points
        {
            get => (Points) GetNonNullValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawPolyline(this);
    }
}
