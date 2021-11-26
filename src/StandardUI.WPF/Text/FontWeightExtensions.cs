using Microsoft.StandardUI.Text;

namespace Microsoft.StandardUI.Wpf.Text
{
    public static class FontWeightExtensions
    {
        public static System.Windows.FontWeight ToWpfFontWeight(this FontWeight fontWeight) =>
            System.Windows.FontWeight.FromOpenTypeWeight(fontWeight.Weight);

        public static FontWeight FromWpfFontWeight(System.Windows.FontWeight fontWeight) =>
            new FontWeight((ushort) fontWeight.ToOpenTypeWeight());
    }
}
