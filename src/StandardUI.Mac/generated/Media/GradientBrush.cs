// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly UIProperty GradientStopsProperty = new UIProperty(nameof(GradientStops), null, readOnly:true);
        public static readonly UIProperty MappingModeProperty = new UIProperty(nameof(MappingMode), BrushMappingMode.RelativeToBoundingBox);
        public static readonly UIProperty SpreadMethodProperty = new UIProperty(nameof(SpreadMethod), GradientSpreadMethod.Pad);
        
        private UICollection<IGradientStop> _gradientStops;
        
        public GradientBrush()
        {
            _gradientStops = new UICollection<IGradientStop>(this);
            SetValue(GradientStopsProperty, _gradientStops);
        }
        
        public UICollection<IGradientStop> GradientStops => _gradientStops;
        IUICollection<IGradientStop> IGradientBrush.GradientStops => GradientStops;
        
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
