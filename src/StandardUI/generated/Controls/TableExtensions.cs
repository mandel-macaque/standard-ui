// This file is generated from ITable.cs. Update the source file to change its contents.

using System;

namespace Microsoft.StandardUI.Controls
{
    public static class TableExtensions
    {
        private static readonly Lazy<ITableAttached> s_TableAttached = new Lazy<ITableAttached>(() => HostEnvironment.Factory.TableAttachedInstance);
        public static ITableAttached TableAttachedInstance => s_TableAttached.Value;
        
        public static T ColumnDefinitions<T>(this T table, params IColumnDefinition[] value) where T : ITable
        {
            table.ColumnDefinitions.Set(value);
            return table;
        }
        
        public static T Rows<T>(this T table, params IRow[] value) where T : ITable
        {
            table.Rows.Set(value);
            return table;
        }
        
        // Attached properties
        
        public static int TableRowSpan<T>(this T uiElement) where T : IUIElement => TableAttachedInstance.GetRowSpan(uiElement);
        public static T TableRowSpan<T>(this T uiElement, int value) where T : IUIElement
        {
            TableAttachedInstance.SetRowSpan(uiElement, value);
            return uiElement;
        }
        
        public static int TableColumnSpan<T>(this T uiElement) where T : IUIElement => TableAttachedInstance.GetColumnSpan(uiElement);
        public static T TableColumnSpan<T>(this T uiElement, int value) where T : IUIElement
        {
            TableAttachedInstance.SetColumnSpan(uiElement, value);
            return uiElement;
        }
    }
}
