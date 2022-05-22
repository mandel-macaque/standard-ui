namespace Microsoft.StandardUI.Controls
{
    [UIModelObject]
    public interface ITableAttached
    {
        /// <summary>
        /// Total number of rows that the element spans within the parent grid. Zero or negative integer values are not permitted.
        /// Values that are greater than the total number of rows are treated as if they specified the total number and will span
        /// all rows.
        /// </summary>
        [DefaultValue(1)]
        int GetRowSpan(IUIElement element);
        void SetRowSpan(IUIElement element, int rowSpan);

        /// <summary>
        /// Total number of columns that the element spans within the parent grid. Zero or negative integer values are not permitted.
        /// Values that are greater than the total number of columns are treated as if they specified the total number and will span
        /// all columns.
        /// </summary>
        [DefaultValue(1)]
        int GetColumnSpan(IUIElement element);
        void SetColumnSpan(IUIElement element, int columnSpan);
    }
}
