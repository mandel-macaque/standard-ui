using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.StandardUI.SkiaVisualFramework.Blazor
{
    public class BlazorSkiaVisualFramework : SkiaVisualFramework
    {
        public override IVisualHostControl CreateHostControl(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            return new BlazorSkiaCanvas((RenderTreeBuilder) arg1!, (int) arg2!);
        }
    }
}
