namespace Microsoft.StandardUI.Controls   
{
    /// <summary>
    /// A Table is a type of Grid expressed as a list of Rows, with each Row
    /// listing the cells for that row.
    /// </summary>
    [StandardPanel]
    public interface ITable : IGridBase
    {
        public IUICollection<IColumnDefinition> ColumnDefinitions { get; }

        IUICollection<IRow> Rows { get; }
    }
}
