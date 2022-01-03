// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Mac.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly AttachedUIProperty LeftProperty = new AttachedUIProperty("Left", 0.0);
        public static readonly AttachedUIProperty TopProperty = new AttachedUIProperty("Top", 0.0);
        
        public static double GetLeft(StandardUIElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(StandardUIElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(StandardUIElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(StandardUIElement element, double value) => element.SetValue(TopProperty, value);
    }
}
