using Microsoft.StandardUI.Media;
using System.ComponentModel;

namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPolyline : IShape
    {
        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; set; }

        Points Points { get; set; }
    }
}
