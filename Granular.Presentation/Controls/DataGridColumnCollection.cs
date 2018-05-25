
using System.Collections.Generic;
using Granular.Collections;
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

            CollectionChanged += DataGridColumnCollection_CollectionChanged;
        }

        private void DataGridColumnCollection_CollectionChanged(object sender, Granular.Collections.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //if (DisplayIndexMapInitialized)
                    //{
                    //    UpdateDisplayIndexForNewColumns(e.NewItems, e.NewStartingIndex);
                    //}

                    InvalidateHasVisibleStarColumns();
                    break;

                case NotifyCollectionChangedAction.Move:
                    //if (DisplayIndexMapInitialized)
                    //{
                    //    UpdateDisplayIndexForMovedColumn(e.OldStartingIndex, e.NewStartingIndex);
                    //}

                    break;

                case NotifyCollectionChangedAction.Remove:
                    //if (DisplayIndexMapInitialized)
                    //{
                    //    UpdateDisplayIndexForRemovedColumns(e.OldItems, e.OldStartingIndex);
                    //}

                    //ClearDisplayIndex(e.OldItems, e.NewItems);
                    InvalidateHasVisibleStarColumns();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    //if (DisplayIndexMapInitialized)
                    //{
                    //    UpdateDisplayIndexForReplacedColumn(e.OldItems, e.NewItems);
                    //}

                    //ClearDisplayIndex(e.OldItems, e.NewItems);
                    InvalidateHasVisibleStarColumns();
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // We dont ClearDisplayIndex here because we no longer have access to the old items.
                    // Instead this is handled in ClearItems.
                    //if (DisplayIndexMapInitialized)
                    //{
                    //    DisplayIndexMap.Clear();
                    //    DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Reset, -1, null, -1);
                    //}

                    HasVisibleStarColumns = false;
                    break;
            }

            InvalidateAverageColumnWidth();
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
            if (ColumnWidthsComputationPending)
            {
                return;
            }

            DataGridLength width = changedColumn.Width;
            bool hasStarColumns = HasVisibleStarColumns;
            if (oldWidth.IsStar && !width.IsStar && !hasStarColumns)
            {
                ExpandAllColumnWidthsToDesiredValue();
            }
            else if (width.IsStar && !oldWidth.IsStar)
            {
                if (!HasVisibleStarColumnsInternal(changedColumn))
                {
                    ComputeColumnWidths();
                }
                else
                {
                    double minWidth = changedColumn.MinWidth;
                    double leftOverSpace = GiveAwayWidthToNonStarColumns(null, oldWidth.DisplayValue - minWidth);
                    changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth + leftOverSpace));
                    RecomputeStarColumnWidths();
                }
            }
            else if (width.IsStar && oldWidth.IsStar)
            {
                RecomputeStarColumnWidths();
            }
            else if (hasStarColumns)
            {
                RedistributeColumnWidthsOnNonStarWidthChange(
                    changedColumn,
                    oldWidth);
            }
        }

        /// <summary>
        ///     Method which redistributes the column widths based on change in MaxWidth of a column
        /// </summary>
        internal void RedistributeColumnWidthsOnMaxWidthChangeOfColumn(DataGridColumn changedColumn, double oldMaxWidth)
        {
            if (ColumnWidthsComputationPending)
            {
                return;
            }

            DataGridLength width = changedColumn.Width;
            double maxWidth = changedColumn.MaxWidth;
            if (maxWidth.LessThan(width.DisplayValue))
            {
                if (HasVisibleStarColumns)
                {
                    GiveAwayWidthToColumns(changedColumn, width.DisplayValue - maxWidth);
                }

                changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, maxWidth));
            }
            else if (maxWidth.GreaterThan(oldMaxWidth))
            {
                if (width.IsStar)
                {
                    RecomputeStarColumnWidths();
                }
                else if (oldMaxWidth.LessThan(width.DesiredValue))
                {
                    double displayValue = Math.Min(width.DesiredValue, maxWidth);
                    if (HasVisibleStarColumns)
                    {
                        double leftOverSpace = TakeAwayWidthFromUnusedSpace(false, displayValue - oldMaxWidth);
                        leftOverSpace = TakeAwayWidthFromStarColumns(changedColumn, leftOverSpace);
                        displayValue -= leftOverSpace;
                    }

                    changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, displayValue));
                }
            }
        }

        /// <summary>
        ///     Method which redistributes widths of columns on change of a column's width
        ///     when datagrid itself has star columns, but neither the oldwidth or the newwidth
        ///     of changed column is star.
        /// </summary>
        private void RedistributeColumnWidthsOnNonStarWidthChange(DataGridColumn changedColumn, DataGridLength oldWidth)
        {
            DataGridLength width = changedColumn.Width;
            if (width.DesiredValue.GreaterThan(oldWidth.DisplayValue))
            {
                double nonRetrievableSpace = TakeAwayWidthFromColumns(changedColumn, width.DesiredValue - oldWidth.DisplayValue, changedColumn != null);
                if (nonRetrievableSpace.GreaterThan(0.0))
                {
                    changedColumn.SetWidthInternal(new DataGridLength(
                        width.Value,
                        width.UnitType,
                        width.DesiredValue,
                        Math.Max(width.DisplayValue - nonRetrievableSpace, changedColumn.MinWidth)));
                }
            }
            else if (width.DesiredValue.LessThan(oldWidth.DisplayValue))
            {
                double newDesiredValue = DataGridHelper.CoerceToMinMax(width.DesiredValue, changedColumn.MinWidth, changedColumn.MaxWidth);
                GiveAwayWidthToColumns(changedColumn, oldWidth.DisplayValue - newDesiredValue);
            }
        }

        /// <summary>
        ///     Method which tries to give away the given amount of width 
        ///     among all the columns except the ignored column
        /// </summary>
        /// <param name="ignoredColumn">The column which is giving away the width</param>
        /// <param name="giveAwayWidth">The amount of giveaway width</param>
        private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
        {
            return GiveAwayWidthToColumns(ignoredColumn, giveAwayWidth, false);
        }

        /// <summary>
        ///     Method which tries to give away the given amount of width 
        ///     among all the columns except the ignored column
        /// </summary>
        /// <param name="ignoredColumn">The column which is giving away the width</param>
        /// <param name="giveAwayWidth">The amount of giveaway width</param>
        private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth, bool recomputeStars)
        {
            double originalGiveAwayWidth = giveAwayWidth;
            giveAwayWidth = GiveAwayWidthToScrollViewerExcess(giveAwayWidth, /*includedInColumnsWidth*/ ignoredColumn != null);
            giveAwayWidth = GiveAwayWidthToNonStarColumns(ignoredColumn, giveAwayWidth);

            if (giveAwayWidth.GreaterThan(0.0) || recomputeStars)
            {
                double sumOfStarDisplayWidths = 0.0;
                double sumOfStarMaxWidths = 0.0;
                bool giveAwayWidthIncluded = false;
                foreach (DataGridColumn column in this)
                {
                    DataGridLength width = column.Width;
                    if (width.IsStar && column.IsVisible)
                    {
                        if (column == ignoredColumn)
                        {
                            giveAwayWidthIncluded = true;
                        }

                        sumOfStarDisplayWidths += width.DisplayValue;
                        sumOfStarMaxWidths += column.MaxWidth;
                    }
                }

                double expectedStarSpace = sumOfStarDisplayWidths;
                if (!giveAwayWidthIncluded)
                {
                    expectedStarSpace += giveAwayWidth;
                }
                else if (!originalGiveAwayWidth.IsClose(giveAwayWidth))
                {
                    expectedStarSpace -= (originalGiveAwayWidth - giveAwayWidth);
                }

                double usedStarSpace = ComputeStarColumnWidths(Math.Min(expectedStarSpace, sumOfStarMaxWidths));
                giveAwayWidth = Math.Max(usedStarSpace - expectedStarSpace, 0.0);
            }

            return giveAwayWidth;
        }

        /// <summary>
        ///     Helper method which gives away width to scroll viewer
        ///     if its extent width is greater than viewport width
        /// </summary>
        private double GiveAwayWidthToScrollViewerExcess(double giveAwayWidth, bool includedInColumnsWidth)
        {
            double totalSpace = _dataGridOwner.GetViewportWidthForColumns();
            double usedSpace = 0.0;
            foreach (DataGridColumn column in this)
            {
                if (column.IsVisible)
                {
                    usedSpace += column.Width.DisplayValue;
                }
            }

            if (includedInColumnsWidth)
            {
                if (usedSpace.GreaterThan(totalSpace))
                {
                    double contributingSpace = usedSpace - totalSpace;
                    giveAwayWidth -= Math.Min(contributingSpace, giveAwayWidth);
                }
            }
            else
            {
                // If the giveAwayWidth is not included in columns, then the new
                // giveAwayWidth should be derived the total available space and used space
                giveAwayWidth = Math.Min(giveAwayWidth, Math.Max(0d, totalSpace - usedSpace));
            }

            return giveAwayWidth;
        }

        /// <summary>
        ///     Method which tries to take away the given amount of width from columns
        ///     except the ignored column
        /// </summary>
        private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized)
        {
            double totalAvailableWidth = _dataGridOwner.GetViewportWidthForColumns();
            return TakeAwayWidthFromColumns(ignoredColumn, takeAwayWidth, widthAlreadyUtilized, totalAvailableWidth);
        }

        /// <summary>
        ///     Method which tries to take away the given amount of width from columns
        ///     except the ignored column
        /// </summary>
        private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized, double totalAvailableWidth)
        {
            takeAwayWidth = TakeAwayWidthFromUnusedSpace(widthAlreadyUtilized, takeAwayWidth, totalAvailableWidth);

            takeAwayWidth = TakeAwayWidthFromStarColumns(ignoredColumn, takeAwayWidth);

            takeAwayWidth = TakeAwayWidthFromNonStarColumns(ignoredColumn, takeAwayWidth);
            return takeAwayWidth;
        }

        /// <summary>
        ///     Helper method which tries to take away width from unused space
        /// </summary>
        private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth)
        {
            double totalAvailableWidth = _dataGridOwner.GetViewportWidthForColumns();
            if (totalAvailableWidth.GreaterThan(0.0))
            {
                return TakeAwayWidthFromUnusedSpace(spaceAlreadyUtilized, takeAwayWidth, totalAvailableWidth);
            }

            return takeAwayWidth;
        }

        /// <summary>
        ///     Helper method which tries to take away width from unused space
        /// </summary>
        private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth, double totalAvailableWidth)
        {
            double usedSpace = 0.0;
            foreach (DataGridColumn column in this)
            {
                if (column.IsVisible)
                {
                    usedSpace += column.Width.DisplayValue;
                }
            }

            if (spaceAlreadyUtilized)
            {
                if (totalAvailableWidth.GreaterThan(usedSpace) || totalAvailableWidth.IsClose(usedSpace))
                {
                    return 0.0;
                }
                else
                {
                    return Math.Min(usedSpace - totalAvailableWidth, takeAwayWidth);
                }
            }
            else
            {
                double unusedSpace = totalAvailableWidth - usedSpace;
                if (unusedSpace.GreaterThan(0.0))
                {
                    takeAwayWidth = Math.Max(0.0, takeAwayWidth - unusedSpace);
                }

                return takeAwayWidth;
            }
        }

        /// <summary>
        ///     Method which tries to take away the given amount of width form
        ///     the star columns
        /// </summary>
        private double TakeAwayWidthFromStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
        {
            if (takeAwayWidth.GreaterThan(0.0))
            {
                double sumOfStarDisplayWidths = 0.0;
                double sumOfStarMinWidths = 0.0;
                foreach (DataGridColumn column in this)
                {
                    DataGridLength width = column.Width;
                    if (width.IsStar && column.IsVisible)
                    {
                        if (column == ignoredColumn)
                        {
                            sumOfStarDisplayWidths += takeAwayWidth;
                        }

                        sumOfStarDisplayWidths += width.DisplayValue;
                        sumOfStarMinWidths += column.MinWidth;
                    }
                }

                double expectedStarSpace = sumOfStarDisplayWidths - takeAwayWidth;
                double usedStarSpace = ComputeStarColumnWidths(Math.Max(expectedStarSpace, sumOfStarMinWidths));
                takeAwayWidth = Math.Max(usedStarSpace - expectedStarSpace, 0.0);
            }

            return takeAwayWidth;
        }

        /// <summary>
        ///     Method which tries to give away the given amount of width
        ///     among all non star columns except the ignored column
        /// </summary>
        private double GiveAwayWidthToNonStarColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
        {
            while (giveAwayWidth.GreaterThan(0.0))
            {
                int countOfParticipatingColumns = 0;
                double minLagWidth = FindMinimumLaggingWidthOfNonStarColumns(
                    ignoredColumn,
                    out countOfParticipatingColumns);

                if (countOfParticipatingColumns == 0)
                {
                    break;
                }

                double minTotalLagWidth = minLagWidth * countOfParticipatingColumns;
                if (minTotalLagWidth.GreaterThan(giveAwayWidth) || minTotalLagWidth.IsClose(giveAwayWidth))
                {
                    minLagWidth = giveAwayWidth / countOfParticipatingColumns;
                    giveAwayWidth = 0.0;
                }
                else
                {
                    giveAwayWidth -= minTotalLagWidth;
                }

                GiveAwayWidthToEveryNonStarColumn(ignoredColumn, minLagWidth);
            }

            return giveAwayWidth;
        }

        /// <summary>
        ///     Helper method which gives away the given amount of width to
        ///     every non star column whose display value is less than its desired value
        /// </summary>
        private void GiveAwayWidthToEveryNonStarColumn(DataGridColumn ignoredColumn, double perColumnGiveAwayWidth)
        {
            foreach (DataGridColumn column in this)
            {
                if (ignoredColumn == column ||
                    !column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (width.IsStar)
                {
                    continue;
                }

                if (width.DisplayValue.LessThan(Math.Min(width.DesiredValue, column.MaxWidth)))
                {
                    column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue + perColumnGiveAwayWidth));
                }
            }
        }

        /// <summary>
        ///     Helper method which finds the minimum non-zero difference between displayvalue and desiredvalue
        ///     among all non star columns
        /// </summary>
        private double FindMinimumLaggingWidthOfNonStarColumns(
            DataGridColumn ignoredColumn,
            out int countOfParticipatingColumns)
        {
            double minLagWidth = Double.PositiveInfinity;
            countOfParticipatingColumns = 0;
            foreach (DataGridColumn column in this)
            {
                if (ignoredColumn == column ||
                    !column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (width.IsStar)
                {
                    continue;
                }

                double columnMaxWidth = column.MaxWidth;
                if (width.DisplayValue.LessThan(width.DesiredValue) &&
                    !width.DisplayValue.IsClose(columnMaxWidth))
                {
                    countOfParticipatingColumns++;
                    double lagWidth = Math.Min(width.DesiredValue, columnMaxWidth) - width.DisplayValue;
                    if (lagWidth.LessThan(minLagWidth))
                    {
                        minLagWidth = lagWidth;
                    }
                }
            }

            return minLagWidth;
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

        /// <summary>
        ///     Method which redistributes the width of star columns among them selves
        /// </summary>
        private void RecomputeStarColumnWidths()
        {
            double totalDisplaySpace = _dataGridOwner.GetViewportWidthForColumns();
            double nonStarSpace = 0.0;
            foreach (DataGridColumn column in this)
            {
                DataGridLength width = column.Width;
                if (column.IsVisible && !width.IsStar)
                {
                    nonStarSpace += width.DisplayValue;
                }
            }

            if (double.IsNaN(nonStarSpace))
            {
                return;
            }

            ComputeStarColumnWidths(totalDisplaySpace - nonStarSpace);
        }

        private double ComputeStarColumnWidths(double availableStarSpace)
        {
            List<DataGridColumn> unResolvedColumns = new List<DataGridColumn>();
            List<DataGridColumn> partialResolvedColumns = new List<DataGridColumn>();
            double totalFactors = 0.0;
            double totalMinWidths = 0.0;
            double totalMaxWidths = 0.0;
            double utilizedStarSpace = 0.0;

            // Accumulate all the star columns into unResolvedColumns in the beginning
            foreach (DataGridColumn column in this)
            {
                DataGridLength width = column.Width;
                if (column.IsVisible && width.IsStar)
                {
                    unResolvedColumns.Add(column);
                    totalFactors += width.Value;
                    totalMinWidths += column.MinWidth;
                    totalMaxWidths += column.MaxWidth;
                }
            }

            if (availableStarSpace.LessThan(totalMinWidths))
            {
                availableStarSpace = totalMinWidths;
            }

            if (availableStarSpace.GreaterThan(totalMaxWidths))
            {
                availableStarSpace = totalMaxWidths;
            }

            while (unResolvedColumns.Count > 0)
            {
                double starValue = availableStarSpace / totalFactors;

                // Find all the columns whose star share is less than thier min width and move such columns
                // into partialResolvedColumns giving them atleast the minwidth and there by reducing the availableSpace and totalFactors
                for (int i = 0, count = unResolvedColumns.Count; i < count; i++)
                {
                    DataGridColumn column = unResolvedColumns[i];
                    DataGridLength width = column.Width;

                    double columnMinWidth = column.MinWidth;
                    double starColumnWidth = availableStarSpace * width.Value / totalFactors;

                    if (columnMinWidth.GreaterThan(starColumnWidth))
                    {
                        availableStarSpace = Math.Max(0.0, availableStarSpace - columnMinWidth);
                        totalFactors -= width.Value;
                        unResolvedColumns.RemoveAt(i);
                        i--;
                        count--;
                        partialResolvedColumns.Add(column);
                    }
                }

                // With the remaining space determine in any columns star share is more than maxwidth.
                // If such columns are found give them their max width and remove them from unResolvedColumns
                // there by reducing the availablespace and totalfactors. If such column is found, the remaining columns are to be recomputed
                bool iterationRequired = false;
                for (int i = 0, count = unResolvedColumns.Count; i < count; i++)
                {
                    DataGridColumn column = unResolvedColumns[i];
                    DataGridLength width = column.Width;

                    double columnMaxWidth = column.MaxWidth;
                    double starColumnWidth = availableStarSpace * width.Value / totalFactors;

                    if (columnMaxWidth.LessThan(starColumnWidth))
                    {
                        iterationRequired = true;
                        unResolvedColumns.RemoveAt(i);
                        availableStarSpace -= columnMaxWidth;
                        utilizedStarSpace += columnMaxWidth;
                        totalFactors -= width.Value;
                        column.UpdateWidthForStarColumn(columnMaxWidth, starValue * width.Value, width.Value);
                        break;
                    }
                }

                // If it was determined by the previous step that another iteration is needed
                // then move all the partialResolvedColumns back to unResolvedColumns and there by
                // restoring availablespace and totalfactors.
                // If another iteration is not needed then allocate min widths to all columns in
                // partial resolved columns and star share to all unresolved columns there by
                // ending the loop
                if (iterationRequired)
                {
                    for (int i = 0, count = partialResolvedColumns.Count; i < count; i++)
                    {
                        DataGridColumn column = partialResolvedColumns[i];

                        unResolvedColumns.Add(column);
                        availableStarSpace += column.MinWidth;
                        totalFactors += column.Width.Value;
                    }

                    partialResolvedColumns.Clear();
                }
                else
                {
                    for (int i = 0, count = partialResolvedColumns.Count; i < count; i++)
                    {
                        DataGridColumn column = partialResolvedColumns[i];
                        DataGridLength width = column.Width;
                        double columnMinWidth = column.MinWidth;
                        column.UpdateWidthForStarColumn(columnMinWidth, width.Value * starValue, width.Value);
                        utilizedStarSpace += columnMinWidth;
                    }

                    partialResolvedColumns.Clear();
                    for (int i = 0, count = unResolvedColumns.Count; i < count; i++)
                    {
                        DataGridColumn column = unResolvedColumns[i];
                        DataGridLength width = column.Width;
                        double starColumnWidth = availableStarSpace * width.Value / totalFactors;
                        column.UpdateWidthForStarColumn(starColumnWidth, width.Value * starValue, width.Value);
                        utilizedStarSpace += starColumnWidth;
                    }

                    unResolvedColumns.Clear();
                }
            }

            return utilizedStarSpace;
        }

        /// <summary>
        ///     Helper method to invalidate the column width computation
        /// </summary>
        internal void InvalidateColumnWidthsComputation()
        {
            if (_columnWidthsComputationPending)
            {
                return;
            }

            _columnWidthsComputationPending = true;

            ComputeColumnWidths();
        }

        /// <summary>
        ///     Flag set when an operation has invalidated auto-width columns so they are no longer expected to be their desired width.
        /// </summary>
        internal bool RefreshAutoWidthColumns { get; set; }

        private void ComputeColumnWidths()
        {
            if (HasVisibleStarColumns)
            {
                InitializeColumnDisplayValues();
                DistributeSpaceAmongColumns(_dataGridOwner.GetViewportWidthForColumns());
            }
            else
            {
                ExpandAllColumnWidthsToDesiredValue();
            }

            if (RefreshAutoWidthColumns)
            {
                foreach (DataGridColumn column in this)
                {
                    if (column.Width.IsAuto)
                    {
                        // This operation resets desired and display widths to 0.0.
                        column.Width = DataGridLength.Auto;
                    }
                }

                RefreshAutoWidthColumns = false;
            }

            _columnWidthsComputationPending = false;
        }

        /// <summary>
        ///     Method which expands the display values of widths of all columns to
        ///     their desired values. Usually used when the last star column's width
        ///     is changed to non-star
        /// </summary>
        private void ExpandAllColumnWidthsToDesiredValue()
        {
            foreach (DataGridColumn column in this)
            {
                if (!column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                double maxWidth = column.MaxWidth;
                if (width.DesiredValue.GreaterThan(width.DisplayValue) &&
                    !width.DisplayValue.IsClose(maxWidth))
                {
                    column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, Math.Min(width.DesiredValue, maxWidth)));
                }
            }
        }

        /// <summary>
        ///     Method which initializes the column width's diplay value to its desired value
        /// </summary>
        private void InitializeColumnDisplayValues()
        {
            foreach (DataGridColumn column in this)
            {
                if (!column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (!width.IsStar)
                {
                    double minWidth = column.MinWidth;
                    double displayValue = DataGridHelper.CoerceToMinMax(double.IsNaN(width.DesiredValue) ? minWidth : width.DesiredValue, minWidth, column.MaxWidth);
                    if (!width.DisplayValue.IsClose(displayValue))
                    {
                        column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, displayValue));
                    }
                }
            }
        }

        /// <summary>
        ///     Method which distributes a given amount of width among all the columns
        /// </summary>
        private void DistributeSpaceAmongColumns(double availableSpace)
        {
            double sumOfMinWidths = 0.0;
            double sumOfMaxWidths = 0.0;
            double sumOfStarMinWidths = 0.0;
            foreach (DataGridColumn column in this)
            {
                if (!column.IsVisible)
                {
                    continue;
                }

                sumOfMinWidths += column.MinWidth;
                sumOfMaxWidths += column.MaxWidth;
                if (column.Width.IsStar)
                {
                    sumOfStarMinWidths += column.MinWidth;
                }
            }

            if (availableSpace.LessThan(sumOfMinWidths))
            {
                availableSpace = sumOfMinWidths;
            }

            if (availableSpace.GreaterThan(sumOfMaxWidths))
            {
                availableSpace = sumOfMaxWidths;
            }

            double nonStarSpaceLeftOver = DistributeSpaceAmongNonStarColumns(availableSpace - sumOfStarMinWidths);

            ComputeStarColumnWidths(sumOfStarMinWidths + nonStarSpaceLeftOver);
        }

        /// <summary>
        ///     Helper method which distributes a given amount of width among all non star columns
        /// </summary>
        private double DistributeSpaceAmongNonStarColumns(double availableSpace)
        {
            double requiredSpace = 0.0;
            foreach (DataGridColumn column in this)
            {
                DataGridLength width = column.Width;
                if (!column.IsVisible ||
                    width.IsStar)
                {
                    continue;
                }

                requiredSpace += width.DisplayValue;
            }

            if (availableSpace.LessThan(requiredSpace))
            {
                double spaceDeficit = requiredSpace - availableSpace;
                TakeAwayWidthFromNonStarColumns(null, spaceDeficit);
            }

            return Math.Max(availableSpace - requiredSpace, 0.0);
        }

        /// <summary>
        ///     Method which tries to take away the given amount of width
        ///     among all non star columns except the ignored column
        /// </summary>
        private double TakeAwayWidthFromNonStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
        {
            while (takeAwayWidth.GreaterThan(0.0))
            {
                int countOfParticipatingColumns = 0;
                double minExcessWidth = FindMinimumExcessWidthOfNonStarColumns(
                    ignoredColumn,
                    out countOfParticipatingColumns);

                if (countOfParticipatingColumns == 0)
                {
                    break;
                }

                double minTotalExcessWidth = minExcessWidth * countOfParticipatingColumns;
                if (minTotalExcessWidth.GreaterThan(takeAwayWidth) || minTotalExcessWidth.IsClose(takeAwayWidth))
                {
                    minExcessWidth = takeAwayWidth / countOfParticipatingColumns;
                    takeAwayWidth = 0.0;
                }
                else
                {
                    takeAwayWidth -= minTotalExcessWidth;
                }

                TakeAwayWidthFromEveryNonStarColumn(ignoredColumn, minExcessWidth);
            }

            return takeAwayWidth;
        }

        private double FindMinimumExcessWidthOfNonStarColumns(
            DataGridColumn ignoredColumn,
            out int countOfParticipatingColumns)
        {
            double minExcessWidth = Double.PositiveInfinity;
            countOfParticipatingColumns = 0;
            foreach (DataGridColumn column in this)
            {
                if (ignoredColumn == column ||
                    !column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (width.IsStar)
                {
                    continue;
                }

                double minWidth = column.MinWidth;
                if (width.DisplayValue.GreaterThan(minWidth))
                {
                    countOfParticipatingColumns++;
                    double excessWidth = width.DisplayValue - minWidth;
                    if (excessWidth.LessThan(minExcessWidth))
                    {
                        minExcessWidth = excessWidth;
                    }
                }
            }

            return minExcessWidth;
        }

        private void TakeAwayWidthFromEveryNonStarColumn(
            DataGridColumn ignoredColumn,
            double perColumnTakeAwayWidth)
        {
            foreach (DataGridColumn column in this)
            {
                if (ignoredColumn == column ||
                    !column.IsVisible)
                {
                    continue;
                }

                DataGridLength width = column.Width;
                if (width.IsStar)
                {
                    continue;
                }

                if (width.DisplayValue.GreaterThan(column.MinWidth))
                {
                    column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue - perColumnTakeAwayWidth));
                }
            }
        }


    }
}