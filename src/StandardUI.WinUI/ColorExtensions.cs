namespace Microsoft.StandardUI.WinUI
{
    public static class ColorExtensions
    {
        public static global::Windows.UI.Color ToWinUIColor(this Color color) => global::Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
