namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IGradientStop : IUIObject
    {
        // The default is Transparent
        Color Color { get; set; }

        [DefaultValue(0.0)]
        double Offset { get; set; }
    }
}
