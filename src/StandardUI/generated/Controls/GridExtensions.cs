// This file is generated from IGrid.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.Controls
{
    public static class GridExtensions
    {
        public static T ColumnSpacing<T>(this T grid, double value) where T : IGrid
        {
            grid.ColumnSpacing = value;
            return grid;
        }
        
        public static T RowSpacing<T>(this T grid, double value) where T : IGrid
        {
            grid.RowSpacing = value;
            return grid;
        }
    }
}
