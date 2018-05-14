
using Granular.Extensions;

namespace System.Windows.Controls
{
    /// <summary>
    ///     Internal class that holds the DataGrid's column collection.  Handles error-checking columns as they come in.
    /// </summary>
    internal class DataGridColumnCollection : Granular.Collections.ObservableCollection<DataGridColumn>
    {
        private readonly DataGrid _dataGridOwner;

        public DataGridColumnCollection(DataGrid dataGridOwner)
        {
            _dataGridOwner = dataGridOwner;
        }

        private bool _columnWidthsComputationPending; // Flag indicating whether the columns width computaion operation is pending

        /// <summary>
        ///     Property indicating whether the column width computation opertaion is pending
        /// </summary>
        internal bool ColumnWidthsComputationPending
        {
            get
            {
                return _columnWidthsComputationPending;
            }
        }

        /// <summary>
        ///     Method which redistributes the column widths based on change in MinWidth of a column
        /// </summary>
        internal void RedistributeColumnWidthsOnMinWidthChangeOfColumn(DataGridColumn changedColumn, double oldMinWidth)
        {
            if (ColumnWidthsComputationPending)
            {
                return;
            }

            //DataGridLength width = changedColumn.Width;
            //double minWidth = changedColumn.MinWidth;
            //if (DoubleUtil.GreaterThan(minWidth, width.DisplayValue))
            //{
            //    if (HasVisibleStarColumns)
            //    {
            //        TakeAwayWidthFromColumns(changedColumn, minWidth - width.DisplayValue, false);
            //    }

            //    changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth));
            //}
            //else if (DoubleUtil.LessThan(minWidth, oldMinWidth))
            //{
            //    if (width.IsStar)
            //    {
            //        if (DoubleUtil.AreClose(width.DisplayValue, oldMinWidth))
            //        {
            //            GiveAwayWidthToColumns(changedColumn, oldMinWidth - minWidth, true);
            //        }
            //    }
            //    else if (DoubleUtil.GreaterThan(oldMinWidth, width.DesiredValue))
            //    {
            //        double displayValue = Math.Max(width.DesiredValue, minWidth);
            //        if (HasVisibleStarColumns)
            //        {
            //            GiveAwayWidthToColumns(changedColumn, oldMinWidth - displayValue);
            //        }

            //        changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, displayValue));
            //    }
            //}
        }

        /// <summary>
        ///     Method which redistributes the column widths based on change in Width of a column
        /// </summary>
        internal void RedistributeColumnWidthsOnWidthChangeOfColumn(DataGridColumn changedColumn, DataGridLength oldWidth)
        {
            //if (ColumnWidthsComputationPending)
            //{
            //    return;
            //}

            //DataGridLength width = changedColumn.Width;
            //bool hasStarColumns = HasVisibleStarColumns;
            //if (oldWidth.IsStar && !width.IsStar && !hasStarColumns)
            //{
            //    ExpandAllColumnWidthsToDesiredValue();
            //}
            //else if (width.IsStar && !oldWidth.IsStar)
            //{
            //    if (!HasVisibleStarColumnsInternal(changedColumn))
            //    {
            //        ComputeColumnWidths();
            //    }
            //    else
            //    {
            //        double minWidth = changedColumn.MinWidth;
            //        double leftOverSpace = GiveAwayWidthToNonStarColumns(null, oldWidth.DisplayValue - minWidth);
            //        changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth + leftOverSpace));
            //        RecomputeStarColumnWidths();
            //    }
            //}
            //else if (width.IsStar && oldWidth.IsStar)
            //{
            //    RecomputeStarColumnWidths();
            //}
            //else if (hasStarColumns)
            //{
            //    RedistributeColumnWidthsOnNonStarWidthChange(
            //        changedColumn,
            //        oldWidth);
            //}
        }

        /// <summary>
        ///     Helper method to invalidate the average width computation
        /// </summary>
        internal void InvalidateAverageColumnWidth()
        {
            //_averageColumnWidth = null;

            //// changing a column width should also invalidate the maximum desired
            //// size of the row presenter (Dev11 821019)
            //VirtualizingStackPanel vsp = (DataGridOwner == null) ? null :
            //    DataGridOwner.InternalItemsHost as VirtualizingStackPanel;
            //if (vsp != null)
            //{
            //    vsp.ResetMaximumDesiredSize();
            //}
        }

        /// <summary>
        ///     Method which redetermines if the collection has any star columns are not.
        /// </summary>
        internal void InvalidateHasVisibleStarColumns()
        {
            HasVisibleStarColumns = HasVisibleStarColumnsInternal(null);
        }

        /// <summary>
        ///     Method which determines if there are any
        ///     star columns in datagrid except the given column
        /// </summary>
        private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn)
        {
            double perStarWidth;
            return HasVisibleStarColumnsInternal(ignoredColumn, out perStarWidth);
        }

        /// <summary>
        ///     Method which determines if there are any
        ///     star columns in datagrid except the given column and also returns perStarWidth
        /// </summary>
        private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn, out double perStarWidth)
        {
            bool hasStarColumns = false;
            perStarWidth = 0.0;
            foreach (DataGridColumn column in this)
            {
                if (column == ignoredColumn ||
                    !column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (width.IsStar)
                {
                    hasStarColumns = true;
                    if (!width.Value.IsClose(0.0) &&
                        !width.DesiredValue.IsClose(0.0))
                    {
                        perStarWidth = width.DesiredValue / width.Value;
                        break;
                    }
                }
            }

            return hasStarColumns;
        }

        private bool _hasVisibleStarColumns = false;
        /// <summary>
        ///     Property which determines if there are any star columns
        ///     in the datagrid.
        /// </summary>
        internal bool HasVisibleStarColumns
        {
            get
            {
                return _hasVisibleStarColumns;
            }
            private set
            {
                if (_hasVisibleStarColumns != value)
                {
                    _hasVisibleStarColumns = value;
                    _dataGridOwner.OnHasVisibleStarColumnsChanged();
                }
            }
        }
    }
}