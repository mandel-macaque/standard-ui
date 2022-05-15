namespace Microsoft.StandardUI.Controls   
{
    /// <summary>
    /// A HorizontalTable is similar to a Table, except that it's expressed as a list of Columns
    /// instead of a list of Rows. HorizontalTable is normally used much less often than Table.
    /// It's appropriate to use when desiring a layout where the first column (instead of the first
    /// row) contains the labels and each additional column corresponds to a record.
    /// </summary>
    [StandardPanel]
    public interface IHorizontalTable : IGridBase
    {
        public IUICollection<IRowDefinition> RowDefinitions { get; }

        IUICollection<IColumn> Columns { get; }
    }
}
