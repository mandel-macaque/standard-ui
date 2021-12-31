using System.ComponentModel;
using Microsoft.StandardUI.Mac.Converters;

namespace Microsoft.StandardUI.Mac
{
    [TypeConverter(typeof(SizeTypeConverter))]
    public struct SizeWpf
    {
        public static readonly SizeWpf Default = new SizeWpf(Size.Default);


        public Size Size { get; }

        public SizeWpf(Size size)
        {
            Size = size;
        }
    }
}
