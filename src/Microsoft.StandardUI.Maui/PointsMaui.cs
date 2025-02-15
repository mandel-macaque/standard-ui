using System.ComponentModel;
using Microsoft.StandardUI.Maui.Converters;

namespace Microsoft.StandardUI.Maui
{
    [TypeConverter(typeof(PointsTypeConverter))]
    public struct PointsMaui
    {
        public static readonly PointsMaui Default = new PointsMaui(Points.Default);

        public Points Points { get; }

        public PointsMaui(Points points)
        {
            Points = points;
        }
    }
}
