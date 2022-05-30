using Microsoft.StandardUI.Media;
using System.ComponentModel;

namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPath : IShape
    {
        [DefaultValue(null)]
        IGeometry Data { get; set; }
    }
}
