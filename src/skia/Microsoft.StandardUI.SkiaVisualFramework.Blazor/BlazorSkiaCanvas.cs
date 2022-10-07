using SkiaSharp;
using SkiaSharp.Views.Blazor;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.StandardUI.SkiaVisualFramework.Blazor;

namespace Microsoft.StandardUI.SkiaVisualFramework
{
    public class BlazorSkiaCanvas : IVisualHostControl
    {
        SkiaVisual? _content;

        public BlazorSkiaCanvas(RenderTreeBuilder builder, int index)
        {
            builder.OpenComponent<SKCanvasView>(index);
            builder.AddAttribute(index, "OnPaintSurface", (System.Action<SKPaintSurfaceEventArgs>)(OnPaintSurface));

            builder.AddAttribute(index + 1, "height", "200");
            builder.AddAttribute(index + 2, "width", "200");
            builder.AddAttribute(index + 3, "style", "position:absolute");

            builder.CloseComponent();
        }

        public IVisual? Content { set => _content = (SkiaVisual?) value; }

        public object? NativeControl => throw new System.NotImplementedException();

        private void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            //canvas.Clear();

#if false
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                TextSize = 24
            };
#endif

            if (_content != null)
                canvas.DrawPicture((SKPicture)_content.NativeVisual);

            //canvas.DrawText("SkiaSharp", 0, 24, paint);
        }
    }
}
