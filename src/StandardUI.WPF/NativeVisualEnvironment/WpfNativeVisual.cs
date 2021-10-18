using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf.NativeVisualEnvironment
{
    public class WpfNativeVisual : IVisual
    {
        public DrawingGroup DrawingGroup { get; }

        public WpfNativeVisual(DrawingGroup drawingGroup)
        {
            DrawingGroup = drawingGroup;
        }

        public object NativeVisual => DrawingGroup;
    }
}
