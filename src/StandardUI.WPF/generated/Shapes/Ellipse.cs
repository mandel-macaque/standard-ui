// This file is generated from IEllipse.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Ellipse : Shape, IEllipse
    {
        public override void OnVisualize(IVisualizer visualizer) => visualizer.DrawEllipse(this);
    }
}
