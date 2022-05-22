// This file is generated from IRow.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Controls
{
    public static class RowExtensions
    {
        public static T Children<T>(this T row, params IUIElement[] value) where T : IRow
        {
            row.Children.Set(value);
            return row;
        }
    }
}
