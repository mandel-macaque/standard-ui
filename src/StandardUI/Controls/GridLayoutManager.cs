using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Controls
{
    public class GridLayoutManager : LayoutManager<IGrid>
    {
        public static GridLayoutManager Instance = new GridLayoutManager();

        public override Size MeasureOverride(IGrid grid, Size constraint)
        {
            var gridStructure = new GridStructure(grid, constraint.Width, constraint.Height);

            double measuredWidth = gridStructure.MeasuredGridWidth();
            double measuredHeight = gridStructure.MeasuredGridHeight();

            return new Size(measuredWidth, measuredHeight);
        }

        public override Size ArrangeOverride(IGrid grid, Size finalSize)
        {
            var gridStructure = new GridStructure(grid, finalSize.Width, finalSize.Height);

            foreach (IUIElement? child in grid.Children)
            {
                if (! child.Visible)
                {
                    continue;
                }

                // TODO: Determine if need to support non zero position for top/left
                //var cell = gridStructure.GetCellBoundsFor(child, bounds.Left, bounds.Top);
                var cell = gridStructure.GetCellBoundsFor(child, 0, 0);

                child.Arrange(cell);
            }

            var actual = new Size(gridStructure.MeasuredGridWidth(), gridStructure.MeasuredGridHeight());

            return AdjustForStretchToFill(grid, actual.Width, actual.Height, finalSize);
        }

        private class GridStructure
        {
            private readonly IGrid _grid;
            private readonly double _gridWidthConstraint;
            private readonly double _gridHeightConstraint;
            private readonly double _explicitGridHeight;
            private readonly double _explicitGridWidth;
            private readonly double _gridMaxHeight;
            private readonly double _gridMinHeight;
            private readonly double _gridMaxWidth;
            private readonly double _gridMinWidth;

            private Row[] _rows { get; }
            private Column[] _columns { get; }
            private readonly IUIElement[] _childrenToLayOut;
            private Cell[] _cells { get; }

            private readonly Thickness _padding;
            private readonly double _rowSpacing;
            private readonly double _columnSpacing;
            private readonly IList<IRowDefinition> _rowDefinitions;
            private readonly IList<IColumnDefinition> _columnDefinitions;

            private readonly Dictionary<SpanKey, Span> _spans = new();

            public GridStructure(IGrid grid, double widthConstraint, double heightConstraint)
            {
                _grid = grid;

                _gridWidthConstraint = widthConstraint;
                _gridHeightConstraint = heightConstraint;

                _explicitGridHeight = _grid.Height;
                _explicitGridWidth = _grid.Width;
                _gridMaxHeight = _grid.MaxHeight;
                _gridMinHeight = _grid.MinHeight;
                _gridMaxWidth = _grid.MaxWidth;
                _gridMinWidth = _grid.MinWidth;


                // Cache these GridLayout properties so we don't have to keep looking them up via _grid
                // (Property access via _grid may have performance implications for some SDKs.)
                //_padding = grid.Padding;
                _columnSpacing = grid.ColumnSpacing;
                _rowSpacing = grid.RowSpacing;
                _rowDefinitions = grid.RowDefinitions;
                _columnDefinitions = grid.ColumnDefinitions;

                if (_rowDefinitions.Count == 0)
                {
                    // Since no rows are specified, we'll create an implied row 0 
                    _rows = new Row[1];
                    _rows[0] = new Row(new ImpliedRow());
                }
                else
                {
                    _rows = new Row[_rowDefinitions.Count];

                    for (int n = 0; n < _rowDefinitions.Count; n++)
                    {
                        _rows[n] = new Row(_rowDefinitions[n]);
                    }
                }

                if (_columnDefinitions.Count == 0)
                {
                    // Since no columns are specified, we'll create an implied column 0 
                    _columns = new Column[1];
                    _columns[0] = new Column(new ImpliedColumn());
                }
                else
                {
                    _columns = new Column[_columnDefinitions.Count];

                    for (int n = 0; n < _columnDefinitions.Count; n++)
                    {
                        _columns[n] = new Column(_columnDefinitions[n]);
                    }
                }

                IUICollection<IUIElement> gridChildren = grid.Children;

                // We could work out the _childrenToLayOut array (with the Collapsed items filtered out) with a Linq 1-liner
                // but doing it the hard way means we don't allocate extra enumerators, especially if we're in the 
                // happy path where _none_ of the children are Collapsed.
                var gridChildCount = gridChildren.Count;

                _childrenToLayOut = new IUIElement[gridChildCount];
                int currentChild = 0;
                for (int n = 0; n < gridChildCount; n++)
                {
                    IUIElement child = gridChildren[n];

                    if (child.Visible)
                    {
                        _childrenToLayOut[currentChild] = child;
                        currentChild += 1;
                    }
                }

                if (currentChild < gridChildCount)
                {
                    Array.Resize(ref _childrenToLayOut, currentChild);
                }

                // We'll ignore any collapsed child views during layout
                _cells = new Cell[_childrenToLayOut.Length];

                InitializeCells();

                MeasureCells();
            }

            private void InitializeCells()
            {
                // If the width/height constraints are infinity, then Star rows/columns won't really make any sense.
                // When that happens, we need to tag the cells so they can be measured as Auto cells instead.
                bool isGridWidthConstraintInfinite = double.IsInfinity(_gridWidthConstraint);
                bool isGridHeightConstraintInfinite = double.IsInfinity(_gridHeightConstraint);

                for (int n = 0; n < _childrenToLayOut.Length; n++)
                {
                    IUIElement? child = _childrenToLayOut[n];

                    if (!child.Visible)
                    {
                        continue;
                    }

                    int column = child.GetGridColumn().Clamp(0, _columns.Length - 1);
                    int columnSpan = _grid.GetGridColumnSpan().Clamp(1, _columns.Length - column);

                    GridLengthType columnGridLengthType = GridLengthType.None;

                    for (int columnIndex = column; columnIndex < column + columnSpan; columnIndex++)
                    {
                        columnGridLengthType |= ToGridLengthType(_columns[columnIndex].ColumnDefinition.Width.GridUnitType);
                    }

                    var row = child.GetGridRow().Clamp(0, _rows.Length - 1);
                    var rowSpan = child.GetGridRowSpan().Clamp(1, _rows.Length - row);

                    var rowGridLengthType = GridLengthType.None;

                    for (int rowIndex = row; rowIndex < row + rowSpan; rowIndex++)
                    {
                        rowGridLengthType |= ToGridLengthType(_rows[rowIndex].RowDefinition.Height.GridUnitType);
                    }

                    // Check for infinite constraints and Stars, so we can mark them for measurement as if they were Auto
                    var measureStarAsAuto = (isGridHeightConstraintInfinite && IsStar(rowGridLengthType))
                        || (isGridWidthConstraintInfinite && IsStar(columnGridLengthType));

                    _cells[n] = new Cell(n, row, column, rowSpan, columnSpan, columnGridLengthType, rowGridLengthType, measureStarAsAuto);
                }
            }

            public Rect GetCellBoundsFor(IUIElement uiElement, double xOffset, double yOffset)
            {
                int firstColumn = uiElement.GetGridColumn().Clamp(0, _columns.Length - 1);
                int columnSpan = uiElement.GetGridColumnSpan().Clamp(1, _columns.Length - firstColumn);
                int lastColumn = firstColumn + columnSpan;

                int firstRow = uiElement.GetGridRow().Clamp(0, _rows.Length - 1);
                int rowSpan = uiElement.GetGridRowSpan().Clamp(1, _rows.Length - firstRow);
                int lastRow = firstRow + rowSpan;

                double top = TopEdgeOfRow(firstRow);
                double left = LeftEdgeOfColumn(firstColumn);

                double width = 0;
                double height = 0;

                for (int n = firstColumn; n < lastColumn; n++)
                {
                    width += _columns[n].Size;
                }

                for (int n = firstRow; n < lastRow; n++)
                {
                    height += _rows[n].Size;
                }

                // Account for any space between spanned rows/columns
                width += (columnSpan - 1) * _columnSpacing;
                height += (rowSpan - 1) * _rowSpacing;

                return new Rect(left + xOffset, top + yOffset, width, height);
            }

            public double GridHeight()
            {
                return SumDefinitions(_rows, _rowSpacing) + _padding.VerticalThickness;
            }

            public double GridWidth()
            {
                return SumDefinitions(_columns, _columnSpacing) + _padding.HorizontalThickness;
            }

            public double MeasuredGridHeight()
            {
                var height = _explicitGridHeight > -1 ? _explicitGridHeight : GridHeight();

                if (_gridMaxHeight >= 0 && height > _gridMaxHeight)
                {
                    height = _gridMaxHeight;
                }

                if (_gridMinHeight >= 0 && height < _gridMinHeight)
                {
                    height = _gridMinHeight;
                }

                return height;
            }

            public double MeasuredGridWidth()
            {
                var width = _explicitGridWidth > -1 ? _explicitGridWidth : GridWidth();

                if (_gridMaxWidth >= 0 && width > _gridMaxWidth)
                {
                    width = _gridMaxWidth;
                }

                if (_gridMinWidth >= 0 && width < _gridMinWidth)
                {
                    width = _gridMinWidth;
                }

                return width;
            }

            private double SumDefinitions(Definition[] definitions, double spacing)
            {
                double sum = 0;

                for (int n = 0; n < definitions.Length; n++)
                {
                    double current = definitions[n].Size;

                    if (current <= 0)
                    {
                        continue;
                    }

                    sum += current;

                    if (n > 0)
                    {
                        sum += spacing;
                    }
                }

                return sum;
            }

            private void MeasureCells()
            {
                for (int n = 0; n < _cells.Length; n++)
                {
                    var cell = _cells[n];

                    if (cell.ColumnGridLengthType == GridLengthType.Pixel
                        && cell.RowGridLengthType == GridLengthType.Pixel)
                    {
                        continue;
                    }

                    double availableWidth = AvailableWidth(cell);
                    double availableHeight = AvailableHeight(cell);

                    IUIElement child = _childrenToLayOut[cell.ChildIndex];

                    if (cell.IsColumnSpanAuto || cell.IsRowSpanAuto || cell.MeasureStarAsAuto)
                    {
                        child.Measure(new Size(availableWidth, availableHeight));
                        var measure = child.DesiredSize;

                        if (cell.IsColumnSpanAuto)
                        {
                            if (cell.ColumnSpan == 1)
                            {
                                _columns[cell.Column].Update(measure.Width);
                            }
                            else
                            {
                                var span = new Span(cell.Column, cell.ColumnSpan, true, measure.Width);
                                TrackSpan(span);
                            }
                        }

                        if (cell.IsRowSpanAuto)
                        {
                            if (cell.RowSpan == 1)
                            {
                                _rows[cell.Row].Update(measure.Height);
                            }
                            else
                            {
                                var span = new Span(cell.Row, cell.RowSpan, false, measure.Height);
                                TrackSpan(span);
                            }
                        }
                    }
                }

                ResolveSpans();

                ResolveStarColumns();
                ResolveStarRows();

                EnsureFinalMeasure();
            }

            private void TrackSpan(Span span)
            {
                if (_spans.TryGetValue(span.Key, out Span? otherSpan))
                {
                    // This span may replace an equivalent but smaller span
                    if (span.Requested > otherSpan.Requested)
                    {
                        _spans[span.Key] = span;
                    }
                }
                else
                {
                    _spans[span.Key] = span;
                }
            }

            private void ResolveSpans()
            {
                foreach (var span in _spans.Values)
                {
                    if (span.IsColumn)
                    {
                        ResolveSpan(_columns, span.Start, span.Length, _columnSpacing, span.Requested);
                    }
                    else
                    {
                        ResolveSpan(_rows, span.Start, span.Length, _rowSpacing, span.Requested);
                    }
                }
            }

            private void ResolveSpan(Definition[] definitions, int start, int length, double spacing, double requestedSize)
            {
                double currentSize = 0;
                var end = start + length;

                // Determine how large the spanned area currently is
                for (int n = start; n < end; n++)
                {
                    currentSize += definitions[n].Size;

                    if (n > start)
                    {
                        currentSize += spacing;
                    }
                }

                if (requestedSize <= currentSize)
                {
                    // If our request fits in the current size, we're good
                    return;
                }

                // Figure out how much more space we need in this span
                double required = requestedSize - currentSize;

                // And how many parts of the span to distribute that space over
                int autoCount = 0;
                for (int n = start; n < end; n++)
                {
                    if (definitions[n].IsAuto)
                    {
                        autoCount += 1;
                    }
                }

                double distribution = required / autoCount;

                // And distribute that over the rows/columns in the span
                for (int n = start; n < end; n++)
                {
                    if (definitions[n].IsAuto)
                    {
                        definitions[n].Size += distribution;
                    }
                }
            }

            private double LeftEdgeOfColumn(int column)
            {
                double left = _padding.Left;

                for (int n = 0; n < column; n++)
                {
                    left += _columns[n].Size;
                    left += _columnSpacing;
                }

                return left;
            }

            private double TopEdgeOfRow(int row)
            {
                double top = _padding.Top;

                for (int n = 0; n < row; n++)
                {
                    top += _rows[n].Size;
                    top += _rowSpacing;
                }

                return top;
            }

            private void ResolveStars(Definition[] defs, double availableSpace, Func<Cell, bool> cellCheck, Func<Size, double> dimension)
            {
                // Count up the total weight of star columns (e.g., "*, 3*, *" == 5)

                var starCount = 0.0;

                foreach (var definition in defs)
                {
                    if (definition.IsStar)
                    {
                        starCount += definition.GridLength.Value;
                    }
                }

                if (starCount == 0)
                {
                    return;
                }

                double starSize = 0;

                if (double.IsInfinity(availableSpace))
                {
                    // If the available space we're measuring is infinite, then the 'star' doesn't really mean anything
                    // (each one would be infinite). So instead we'll use the size of the actual view in the star row/column.
                    // This means that an empty star row/column goes to zero if the available space is infinite. 

                    foreach (var cell in _cells)
                    {
                        if (cellCheck(cell)) // Check whether this cell should count toward the type of star value were measuring
                        {
                            // Update the star width if the view in this cell is bigger
                            starSize = Math.Max(starSize, dimension(_childrenToLayOut[cell.ChildIndex].DesiredSize));
                        }
                    }
                }
                else
                {
                    // If we have a finite space, we can divvy it up among the full star weight
                    starSize = availableSpace / starCount;
                }

                foreach (var definition in defs)
                {
                    if (definition.IsStar)
                    {
                        // Give the star row/column the appropriate portion of the space based on its weight
                        definition.Size = starSize * definition.GridLength.Value;
                    }
                }
            }

            private void ResolveStarColumns()
            {
                var availableSpace = _gridWidthConstraint - GridWidth();
                static bool cellCheck(Cell cell) => cell.IsColumnSpanStar;
                static double getDimension(Size size) => size.Width;

                ResolveStars(_columns, availableSpace, cellCheck, getDimension);
            }

            private void ResolveStarRows()
            {
                var availableSpace = _gridHeightConstraint - GridHeight();
                static bool cellCheck(Cell cell) => cell.IsRowSpanStar;
                static double getDimension(Size size) => size.Height;

                ResolveStars(_rows, availableSpace, cellCheck, getDimension);
            }

            private void EnsureFinalMeasure()
            {
                foreach (var cell in _cells)
                {
                    double width = 0;
                    double height = 0;

                    for (int n = cell.Row; n < cell.Row + cell.RowSpan; n++)
                    {
                        height += _rows[n].Size;
                    }

                    for (int n = cell.Column; n < cell.Column + cell.ColumnSpan; n++)
                    {
                        width += _columns[n].Size;
                    }

                    _childrenToLayOut[cell.ChildIndex].Measure(new Size(width, height));
                }
            }

            private double AvailableWidth(Cell cell)
            {
                var alreadyUsed = GridWidth();
                var available = _gridWidthConstraint - alreadyUsed;

                // Because our cell may overlap columns that are already measured (and counted in GridWidth()),
                // we'll need to add the size of those columns back into our available space
                double cellColumnsWidth = 0;

                for (int c = cell.Column; c < cell.Column + cell.ColumnSpan; c++)
                {
                    cellColumnsWidth += _columns[c].Size;
                }

                cellColumnsWidth += (cell.ColumnSpan - 1) * _columnSpacing;

                return available + cellColumnsWidth;
            }

            private double AvailableHeight(Cell cell)
            {
                var alreadyUsed = GridHeight();
                var available = _gridHeightConstraint - alreadyUsed;

                // Because our cell may overlap rows that are already measured (and counted in GridHeight()),
                // we'll need to add the size of those rows back into our available space
                double cellRowsHeight = 0;

                for (int c = cell.Row; c < cell.Row + cell.RowSpan; c++)
                {
                    cellRowsHeight += _rows[c].Size;
                }

                cellRowsHeight += (cell.RowSpan - 1) * _rowSpacing;

                return available + cellRowsHeight;
            }
        }

        // Dictionary key for tracking a Span
        private struct SpanKey
        {
            public int Start { get; }
            public int Length { get; }
            public bool IsColumn { get; }

            public SpanKey(int start, int length, bool isColumn)
            {
                Start = start;
                Length = length;
                IsColumn = isColumn;
            }

            public override bool Equals(object? obj)
            {
                return obj is SpanKey key &&
                       Start == key.Start &&
                       Length == key.Length &&
                       IsColumn == key.IsColumn;
            }

            public override int GetHashCode()
            {
                int hashCode = -1566178749;
                hashCode = hashCode * -1521134295 + Start.GetHashCode();
                hashCode = hashCode * -1521134295 + Length.GetHashCode();
                hashCode = hashCode * -1521134295 + IsColumn.GetHashCode();
                return hashCode;
            }
        }

        private class Span
        {
            public int Start { get; }
            public int Length { get; }
            public bool IsColumn { get; }
            public double Requested { get; }

            public SpanKey Key { get; }

            public Span(int start, int length, bool isColumn, double requestedLength)
            {
                Start = start;
                Length = length;
                IsColumn = isColumn;
                Requested = requestedLength;

                Key = new SpanKey(Start, Length, IsColumn);
            }
        }

        private class Cell
        {
            public int ChildIndex { get; }
            public int Row { get; }
            public int Column { get; }
            public int RowSpan { get; }
            public int ColumnSpan { get; }

            /// <summary>
            /// A combination of all the measurement types in the columns this cell spans
            /// </summary>
            public GridLengthType ColumnGridLengthType { get; }

            /// <summary>
            /// A combination of all the measurement types in the rows this cell spans
            /// </summary>
            public GridLengthType RowGridLengthType { get; }

            /// <summary>
            /// Marks the cell as requiring initial measurement even though the measurement type is Star
            /// Star measurements don't make sense when the axis constraint is infinity; when that happens, we treat them 
            /// Auto instead. We need to tag that situation in the Cell so the Auto measurement can happen; otherwise, we 
            /// can end up with un-measured controls when resolving the Star cells.
            /// </summary>
            public bool MeasureStarAsAuto { get; }

            public Cell(int childIndex, int row, int column, int rowSpan, int columnSpan,
                GridLengthType columnGridLengthType, GridLengthType rowGridLengthType, bool measureStarAsAuto)
            {
                ChildIndex = childIndex;
                Row = row;
                Column = column;
                RowSpan = rowSpan;
                ColumnSpan = columnSpan;
                ColumnGridLengthType = columnGridLengthType;
                RowGridLengthType = rowGridLengthType;
                MeasureStarAsAuto = measureStarAsAuto;
            }

            public bool IsColumnSpanAuto => HasFlag(ColumnGridLengthType, GridLengthType.Auto);
            public bool IsRowSpanAuto => HasFlag(RowGridLengthType, GridLengthType.Auto);
            public bool IsColumnSpanStar => HasFlag(ColumnGridLengthType, GridLengthType.Star);
            public bool IsRowSpanStar => HasFlag(RowGridLengthType, GridLengthType.Star);

            private bool HasFlag(GridLengthType a, GridLengthType b)
            {
                // Avoiding Enum.HasFlag here for performance reasons; we don't need the type check
                return (a & b) == b;
            }
        }

        [Flags]

        private enum GridLengthType
        {
            None = 0,
            Pixel = 1,
            Auto = 2,
            Star = 4
        }

        private static GridLengthType ToGridLengthType(GridUnitType gridUnitType)
        {
            return gridUnitType switch
            {
                GridUnitType.Pixel => GridLengthType.Pixel,
                GridUnitType.Star => GridLengthType.Star,
                GridUnitType.Auto => GridLengthType.Auto,
                _ => GridLengthType.None,
            };
        }

        private static bool IsStar(GridLengthType gridLengthType)
        {
            return (gridLengthType & GridLengthType.Star) == GridLengthType.Star;
        }

        private abstract class Definition
        {
            public double Size { get; set; }

            public void Update(double size)
            {
                if (size > Size)
                {
                    Size = size;
                }
            }

            public abstract bool IsAuto { get; }
            public abstract bool IsStar { get; }

            public abstract GridLength GridLength { get; }
        }

        private class Column : Definition
        {
            public IColumnDefinition ColumnDefinition { get; set; }

            public override bool IsAuto => ColumnDefinition.Width.IsAuto;
            public override bool IsStar => ColumnDefinition.Width.IsStar;
            public override GridLength GridLength => ColumnDefinition.Width;

            public Column(IColumnDefinition columnDefinition)
            {
                ColumnDefinition = columnDefinition;
                if (columnDefinition.Width.IsAbsolute)
                {
                    Size = columnDefinition.Width.Value;
                }
            }
        }

        private class Row : Definition
        {
            public IRowDefinition RowDefinition { get; set; }

            public override bool IsAuto => RowDefinition.Height.IsAuto;
            public override bool IsStar => RowDefinition.Height.IsStar;
            public override GridLength GridLength => RowDefinition.Height;

            public Row(IRowDefinition rowDefinition)
            {
                RowDefinition = rowDefinition;
                if (rowDefinition.Height.IsAbsolute)
                {
                    Size = rowDefinition.Height.Value;
                }
            }
        }

        // If the IGrid doesn't have any rows/columns defined, the manager will use an implied single row or column
        // in their place. 

        private class ImpliedRow : IRowDefinition
        {
            public GridLength Height { get => GridLength.Default; set => throw new NotImplementedException(); }

            double IRowDefinition.MinHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            double IRowDefinition.MaxHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            double IRowDefinition.ActualHeight => throw new NotImplementedException();

            void IUIObject.ClearValue(IUIProperty property) => throw new NotImplementedException();
            object? IUIObject.GetValue(IUIProperty property) => throw new NotImplementedException();
            object? IUIObject.ReadLocalValue(IUIProperty property) => throw new NotImplementedException();
            void IUIObject.SetValue(IUIProperty property, object? value) => throw new NotImplementedException();
        }

        private class ImpliedColumn : IColumnDefinition
        {
            public GridLength Width { get => GridLength.Default; set => throw new NotImplementedException(); }

            double IColumnDefinition.MinWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            double IColumnDefinition.MaxWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            double IColumnDefinition.ActualWidth => throw new NotImplementedException();

            void IUIObject.ClearValue(IUIProperty property) => throw new NotImplementedException();
            object? IUIObject.GetValue(IUIProperty property) => throw new NotImplementedException();
            object? IUIObject.ReadLocalValue(IUIProperty property) => throw new NotImplementedException();
            void IUIObject.SetValue(IUIProperty property, object? value) => throw new NotImplementedException();
        }
    }
}
