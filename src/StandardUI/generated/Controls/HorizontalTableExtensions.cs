// This file is generated from IHorizontalTable.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Controls
{
    public static class HorizontalTableExtensions
    {
        public static T RowDefinitions<T>(this T horizontalTable, params IRowDefinition[] value) where T : IHorizontalTable
        {
            horizontalTable.RowDefinitions.Set(value);
            return horizontalTable;
        }
        
        public static T Columns<T>(this T horizontalTable, params IColumn[] value) where T : IHorizontalTable
        {
            horizontalTable.Columns.Set(value);
            return horizontalTable;
        }
    }
}
