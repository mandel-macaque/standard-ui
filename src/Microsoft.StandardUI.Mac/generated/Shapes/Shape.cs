// This file is generated from IShape.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Mac.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Mac.Shapes
{
    public class Shape : BuiltInUIElement, IShape
    {
        public static readonly UIProperty FillProperty = new UIProperty(nameof(Fill), null);
        public static readonly UIProperty StrokeProperty = new UIProperty(nameof(Stroke), null);
        public static readonly UIProperty StrokeThicknessProperty = new UIProperty(nameof(StrokeThickness), 1.0);
        public static readonly UIProperty StrokeMiterLimitProperty = new UIProperty(nameof(StrokeMiterLimit), 10.0);
        public static readonly UIProperty StrokeLineCapProperty = new UIProperty(nameof(StrokeLineCap), PenLineCap.Flat);
        public static readonly UIProperty StrokeLineJoinProperty = new UIProperty(nameof(StrokeLineJoin), PenLineJoin.Miter);
        
        public Brush? Fill
        {
            get => (Brush?) GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        IBrush? IShape.Fill
        {
            get => Fill;
            set => Fill = (Brush?) value;
        }
        
        public Brush? Stroke
        {
            get => (Brush?) GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }
        IBrush? IShape.Stroke
        {
            get => Stroke;
            set => Stroke = (Brush?) value;
        }
        
        public double StrokeThickness
        {
            get => (double) GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        
        public double StrokeMiterLimit
        {
            get => (double) GetValue(StrokeMiterLimitProperty);
            set => SetValue(StrokeMiterLimitProperty, value);
        }
        
        public PenLineCap StrokeLineCap
        {
            get => (PenLineCap) GetValue(StrokeLineCapProperty);
            set => SetValue(StrokeLineCapProperty, value);
        }
        
        public PenLineJoin StrokeLineJoin
        {
            get => (PenLineJoin) GetValue(StrokeLineJoinProperty);
            set => SetValue(StrokeLineJoinProperty, value);
        }
    }
}
