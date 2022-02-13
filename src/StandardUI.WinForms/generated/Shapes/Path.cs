// This file is generated from IPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.WinForms.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.WinForms.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly UIProperty DataProperty = new UIProperty(nameof(Data), null);
        
        public Geometry Data
        {
            get => (Geometry) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        IGeometry IPath.Data
        {
            get => Data;
            set => Data = (Geometry) value;
        }
    }
}
