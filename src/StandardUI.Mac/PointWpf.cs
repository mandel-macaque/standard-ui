using System.ComponentModel;
using Microsoft.StandardUI.Mac.Converters;

namespace Microsoft.StandardUI.Mac
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct PointWpf

    {
        public static readonly PointWpf Default = new PointWpf(Point.Default);
        public static readonly PointWpf CenterDefault = new PointWpf(Point.CenterDefault);


        public Point Point { get; }

        public PointWpf(Point point)
        {
            Point = point;
        }
    }
}
