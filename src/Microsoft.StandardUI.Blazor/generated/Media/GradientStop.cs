// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Blazor.Media
{
    public class GradientStop : StandardUIObject, IGradientStop
    {
        public static readonly UIProperty ColorProperty = new UIProperty(nameof(Color), Color.Default);
        public static readonly UIProperty OffsetProperty = new UIProperty(nameof(Offset), 0.0);
        
        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        
        public double Offset
        {
            get => (double) GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
    }
}
