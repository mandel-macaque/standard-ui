// This file is generated from IGrid.cs. Update the source file to change its contents.

using System;

namespace Microsoft.StandardUI.Controls
{
    public static class GridAttachedExtensions
    {
        private static readonly Lazy<IGridAttached> s_lazyGridAttached = new Lazy<IGridAttached>(() => StandardUIEnvironment.Instance.Factory.GridAttachedInstance);
        public static IGridAttached GridAttachedInstance => s_lazyGridAttached.Value;
        
        public static int GetGridRow(this IUIElement element) => GridAttachedInstance.GetRow(element);
        public static void SetGridRow(this IUIElement element, int value) => GridAttachedInstance.SetRow(element, value);
        
        public static int GetGridColumn(this IUIElement element) => GridAttachedInstance.GetColumn(element);
        public static void SetGridColumn(this IUIElement element, int value) => GridAttachedInstance.SetColumn(element, value);
        
        public static int GetGridRowSpan(this IUIElement element) => GridAttachedInstance.GetRowSpan(element);
        public static void SetGridRowSpan(this IUIElement element, int value) => GridAttachedInstance.SetRowSpan(element, value);
        
        public static int GetGridColumnSpan(this IUIElement element) => GridAttachedInstance.GetColumnSpan(element);
        public static void SetGridColumnSpan(this IUIElement element, int value) => GridAttachedInstance.SetColumnSpan(element, value);
    }
}
