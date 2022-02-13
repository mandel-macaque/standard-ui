// This file is generated from IBorder.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.WinForms.Media;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class Border : BuiltInUIElement, IBorder
    {
        public static readonly UIProperty BackgroundProperty = new UIProperty(nameof(Background), null);
        public static readonly UIProperty BackgroundSizingProperty = new UIProperty(nameof(BackgroundSizing), BackgroundSizing.InnerBorderEdge);
        public static readonly UIProperty BorderBrushProperty = new UIProperty(nameof(BorderBrush), null);
        public static readonly UIProperty BorderThicknessProperty = new UIProperty(nameof(BorderThickness), Thickness.Default);
        public static readonly UIProperty ChildProperty = new UIProperty(nameof(Child), null);
        public static readonly UIProperty CornerRadiusProperty = new UIProperty(nameof(CornerRadius), CornerRadius.Default);
        public static readonly UIProperty PaddingProperty = new UIProperty(nameof(Padding), Thickness.Default);
        
        public Brush Background
        {
            get => (Brush) GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        IBrush IBorder.Background
        {
            get => Background;
            set => Background = (Brush) value;
        }
        
        public BackgroundSizing BackgroundSizing
        {
            get => (BackgroundSizing) GetValue(BackgroundSizingProperty);
            set => SetValue(BackgroundSizingProperty, value);
        }
        
        public Brush BorderBrush
        {
            get => (Brush) GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        IBrush IBorder.BorderBrush
        {
            get => BorderBrush;
            set => BorderBrush = (Brush) value;
        }
        
        public Thickness BorderThickness
        {
            get => (Thickness) GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        
        public BuiltInUIElement Child
        {
            get => (BuiltInUIElement) GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }
        IUIElement IBorder.Child
        {
            get => Child;
            set => Child = (BuiltInUIElement) value;
        }
        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}
