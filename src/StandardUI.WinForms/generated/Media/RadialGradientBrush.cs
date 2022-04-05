// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.WinForms.Media
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly UIProperty CenterProperty = new UIProperty(nameof(Center), Point.CenterDefault);
        public static readonly UIProperty GradientOriginProperty = new UIProperty(nameof(GradientOrigin), Point.CenterDefault);
        public static readonly UIProperty RadiusXProperty = new UIProperty(nameof(RadiusX), 0.5);
        
        public Point Center
        {
            get => (Point) GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        
        public Point GradientOrigin
        {
            get => (Point) GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }
        
        public double RadiusX
        {
            get => (double) GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
    }
}
