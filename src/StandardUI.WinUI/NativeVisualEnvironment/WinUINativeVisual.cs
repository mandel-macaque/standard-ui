using Microsoft.UI.Composition;

namespace Microsoft.StandardUI.WinUI.NativeVisualEnvironment
{
    public class WinUINativeVisual : IVisual
    {
        public Visual Visual { get; }

        public WinUINativeVisual(Visual visual)
        {
            Visual = visual;
        }

        public object NativeVisual => Visual;
    }
}
