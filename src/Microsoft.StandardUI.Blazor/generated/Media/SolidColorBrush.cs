// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Blazor.Media
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly UIProperty ColorProperty = new UIProperty(nameof(Color), Color.Default);
        
        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
