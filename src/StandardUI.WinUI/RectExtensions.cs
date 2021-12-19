namespace Microsoft.StandardUI.WinUI
{
    public static class RectExtensions
    {
        public static global::Windows.Foundation.Rect ToWindowsFoundationRect(this Rect rect)
        {
            return new global::Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
