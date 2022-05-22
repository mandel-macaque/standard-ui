// This file is generated from IColumn.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Controls
{
    public static class ColumnExtensions
    {
        public static T Children<T>(this T column, params IUIElement[] value) where T : IColumn
        {
            column.Children.Set(value);
            return column;
        }
    }
}
