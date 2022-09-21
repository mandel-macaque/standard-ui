// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Blazor.Media;
using Microsoft.StandardUI.Text;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Blazor.Controls
{
    public class TextBlock : BuiltInUIElement, ITextBlock
    {
        public static readonly UIProperty ForegroundProperty = new UIProperty(nameof(Foreground), null);
        public static readonly UIProperty TextProperty = new UIProperty(nameof(Text), "");
        public static readonly UIProperty FontFamilyProperty = new UIProperty(nameof(FontFamily), "");
        public static readonly UIProperty FontStyleProperty = new UIProperty(nameof(FontStyle), FontStyle.Normal);
        public static readonly UIProperty FontWeightProperty = new UIProperty(nameof(FontWeight), FontWeight.Default);
        public static readonly UIProperty FontSizeProperty = new UIProperty(nameof(FontSize), 11.0);
        public static readonly UIProperty FontStretchProperty = new UIProperty(nameof(FontStretch), FontStretch.Normal);
        public static readonly UIProperty TextAlignmentProperty = new UIProperty(nameof(TextAlignment), TextAlignment.Left);
        
        public Brush Foreground
        {
            get => (Brush) GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        IBrush ITextBlock.Foreground
        {
            get => Foreground;
            set => Foreground = (Brush) value;
        }
        
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public FontFamily FontFamily
        {
            get => (FontFamily) GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        
        public FontStyle FontStyle
        {
            get => (FontStyle) GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }
        
        public FontWeight FontWeight
        {
            get => (FontWeight) GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }
        
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        
        public FontStretch FontStretch
        {
            get => (FontStretch) GetValue(FontStretchProperty);
            set => SetValue(FontStretchProperty, value);
        }
        
        public TextAlignment TextAlignment
        {
            get => (TextAlignment) GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
    }
}
