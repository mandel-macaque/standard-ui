namespace Microsoft.StandardUI.Controls   
{
    [StandardPanel]   
    public interface IGrid : IGridBase, IPanel
    {
        /// <summary>
        /// List of column definitions defined on this instance of the grid.
        /// </summary>
        public IUICollection<IColumnDefinition> ColumnDefinitions { get; }

        /// <summary>
        /// List of row definitions defined on this instance of the grid.
        /// </summary>
        public IUICollection<IRowDefinition> RowDefinitions { get; }
    }
}
