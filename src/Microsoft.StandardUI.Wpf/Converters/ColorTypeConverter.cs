using System.ComponentModel;
using System.Globalization;
using Microsoft.StandardUI.Converters;

namespace Microsoft.StandardUI.Wpf.Converters
{
    public class ColorTypeConverter : TypeConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new ColorWpf(ColorConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
