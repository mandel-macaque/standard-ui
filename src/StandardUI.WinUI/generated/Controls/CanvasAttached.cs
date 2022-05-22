// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinUI.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        public static CanvasAttached Instance = new CanvasAttached();
        
        public double GetLeft(IUIElement element) => Canvas.GetLeft((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetLeft(IUIElement element, double value) => Canvas.SetLeft((Microsoft.UI.Xaml.FrameworkElement) element, value);
        
        public double GetTop(IUIElement element) => Canvas.GetTop((Microsoft.UI.Xaml.FrameworkElement) element);
        public void SetTop(IUIElement element, double value) => Canvas.SetTop((Microsoft.UI.Xaml.FrameworkElement) element, value);
    }
}
