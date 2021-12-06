// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System;

namespace Microsoft.StandardUI.Controls
{
    public static class CanvasAttachedExtensions
    {
        private static readonly Lazy<ICanvasAttached> s_lazyCanvasAttached = new Lazy<ICanvasAttached>(() => StandardUIEnvironment.Instance.Factory.CanvasAttachedInstance);
        public static ICanvasAttached CanvasAttachedInstance => s_lazyCanvasAttached.Value;
        
        public static double GetCanvasLeft(this IUIElement element) => CanvasAttachedInstance.GetLeft(element);
        public static void SetCanvasLeft(this IUIElement element, double value) => CanvasAttachedInstance.SetLeft(element, value);
        
        public static double GetCanvasTop(this IUIElement element) => CanvasAttachedInstance.GetTop(element);
        public static void SetCanvasTop(this IUIElement element, double value) => CanvasAttachedInstance.SetTop(element, value);
    }
}
