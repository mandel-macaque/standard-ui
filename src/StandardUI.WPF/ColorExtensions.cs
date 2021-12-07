namespace Microsoft.StandardUI.Wpf
{
    public static class ColorExtensions
    {
        public static System.Windows.Media.Color ToWpfColor(this Color color) => System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
