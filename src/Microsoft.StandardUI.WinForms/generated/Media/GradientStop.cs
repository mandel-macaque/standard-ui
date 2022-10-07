// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.WinForms.Media
{
    public class GradientStop : StandardUIObject, IGradientStop
    {
        public static readonly UIProperty ColorProperty = new UIProperty(nameof(Color), Color.Default);
        public static readonly UIProperty OffsetProperty = new UIProperty(nameof(Offset), 0.0);
        
        public Color Color
        {
            get => (Color) GetNonNullValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        
        public double Offset
        {
            get => (double) GetNonNullValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
    }
}
