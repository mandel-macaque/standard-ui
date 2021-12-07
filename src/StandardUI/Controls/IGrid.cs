namespace Microsoft.StandardUI.Controls
{
    [UIModelObject]
    public interface IGrid : IPanel
    {
        /// <summary>
        /// Uniform distance between grid columns.
        /// </summary>
        [DefaultValue(0.0)]
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// Uniform distance between grid rows.
        /// </summary>
        [DefaultValue(0.0)]
        public double RowSpacing { get; set; }

        /// <summary>
        /// List of column definitions defined on this instance of the grid.
        /// </summary>
        public IColumnDefinitionCollection ColumnDefinitions { get; }

        /// <summary>
        /// List of row definitions defined on this instance of the grid.
        /// </summary>
        public IRowDefinitionCollection RowDefinitions { get; }
    }
}
