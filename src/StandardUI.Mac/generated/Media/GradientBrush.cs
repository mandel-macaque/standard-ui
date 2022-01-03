// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly UIProperty GradientStopsProperty = new UIProperty(nameof(GradientStops), null);
        public static readonly UIProperty MappingModeProperty = new UIProperty(nameof(MappingMode), BrushMappingMode.RelativeToBoundingBox);
        public static readonly UIProperty SpreadMethodProperty = new UIProperty(nameof(SpreadMethod), GradientSpreadMethod.Pad);
        
        public IEnumerable<IGradientStop> GradientStops
        {
            get => (IEnumerable<IGradientStop>) GetValue(GradientStopsProperty);
            set => SetValue(GradientStopsProperty, value);
        }
        
        public BrushMappingMode MappingMode
        {
            get => (BrushMappingMode) GetValue(MappingModeProperty);
            set => SetValue(MappingModeProperty, value);
        }
        
        public GradientSpreadMethod SpreadMethod
        {
            get => (GradientSpreadMethod) GetValue(SpreadMethodProperty);
            set => SetValue(SpreadMethodProperty, value);
        }
    }
}
