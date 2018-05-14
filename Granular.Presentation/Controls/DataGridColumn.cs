
using Granular.Extensions;

namespace System.Windows.Controls
{
    /// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column.</summary>
    public abstract class DataGridColumn : DependencyObject
    {
        private bool _ignoreRedistributionOnWidthChange = false;    // Flag which indicates to ignore recomputation of column widths on width change of column
        private bool _processingWidthChange = false;                // Flag which indicates that execution of width change callback to avoid recursions.
        private const double _starMaxWidth = 10000d;                // Max Width constant for star columns
        private DataGrid _dataGridOwner = null;                     // This property is updated by DataGrid when the column is added to the DataGrid.Columns collection
        /// <summary>
        ///     The owning DataGrid control.
        /// </summary>
        protected internal DataGrid DataGridOwner
        {
            get { return _dataGridOwner; }
            internal set { _dataGridOwner = value; }
        }

        #region Visibility

        /// <summary>
        ///     Dependency property for Visibility
        /// </summary>
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(DataGridColumn),
                new FrameworkPropertyMetadata(Visibility.Visible, new PropertyChangedCallback(OnVisibilityPropertyChanged)));

        /// <summary>
        ///     The property which determines if the column is visible or not.
        /// </summary>
        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        /// <summary>
        ///     Property changed callback for Visibility property
        /// </summary>
        private static void OnVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            Visibility oldVisibility = (Visibility)eventArgs.OldValue;
            Visibility newVisibility = (Visibility)eventArgs.NewValue;

            if (oldVisibility != Visibility.Visible && newVisibility != Visibility.Visible)
            {
                return;
            }

            ((DataGridColumn)d).NotifyPropertyChanged(
                d,
                eventArgs,
                DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.DataGrid | DataGridNotificationTarget.ColumnHeaders);
        }

        /// <summary>
        ///     Helper IsVisible property
        /// </summary>
        internal bool IsVisible
        {
            get
            {
                return Visibility == Visibility.Visible;
            }
        }

        #endregion

        #region Header

        /// <summary>
        ///     The DependencyProperty that represents the Header property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnNotifyColumnHeaderPropertyChanged)));


        /// <summary>
        ///     An object that represents the header of this column.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        ///     The template that defines the visual representation of the header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the HeaderTemplate property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, OnNotifyColumnHeaderPropertyChanged));


        #endregion

        #region Width

        /// <summary>
        ///     Specifies the width of the header and cells within this column.
        /// </summary>
        public DataGridLength Width
        {
            get { return (DataGridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the Width property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                "Width",
                typeof(DataGridLength),
                typeof(DataGridColumn),
                new FrameworkPropertyMetadata(DataGridLength.Auto, new PropertyChangedCallback(OnWidthPropertyChanged), new CoerceValueCallback(OnCoerceWidth)));


        /// <summary>
        /// Property changed call back for Width property which notification propagation
        /// and does the redistribution of widths among other columns if needed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumn column = (DataGridColumn)d;
            DataGridLength oldWidth = (DataGridLength)e.OldValue;
            DataGridLength newWidth = (DataGridLength)e.NewValue;
            DataGrid dataGrid = column.DataGridOwner;

            if (dataGrid != null && !oldWidth.DisplayValue.IsClose(newWidth.DisplayValue))
            {
                dataGrid.InternalColumns.InvalidateAverageColumnWidth();
            }

            if (column._processingWidthChange)
            {
                column.CoerceValue(ActualWidthProperty);
                return;
            }

            column._processingWidthChange = true;
            if (oldWidth.IsStar != newWidth.IsStar)
            {
                column.CoerceValue(MaxWidthProperty);
            }

            try
            {
                if (dataGrid != null && (newWidth.IsStar ^ oldWidth.IsStar))
                {
                    dataGrid.InternalColumns.InvalidateHasVisibleStarColumns();
                }

                column.NotifyPropertyChanged(
                    d,
                    e,
                    DataGridNotificationTarget.ColumnCollection |
                    DataGridNotificationTarget.Columns |
                    DataGridNotificationTarget.Cells |
                    DataGridNotificationTarget.ColumnHeaders |
                    DataGridNotificationTarget.CellsPresenter |
                    DataGridNotificationTarget.ColumnHeadersPresenter |
                    DataGridNotificationTarget.DataGrid);

                if (dataGrid != null)
                {
                    if (!column._ignoreRedistributionOnWidthChange && column.IsVisible)
                    {
                        if (!newWidth.IsStar && !newWidth.IsAbsolute)
                        {
                            DataGridLength changedWidth = column.Width;
                            double displayValue = DataGridHelper.CoerceToMinMax(changedWidth.DesiredValue, column.MinWidth, column.MaxWidth);
                            column.SetWidthInternal(new DataGridLength(changedWidth.Value, changedWidth.UnitType, changedWidth.DesiredValue, displayValue));
                        }

                        dataGrid.InternalColumns.RedistributeColumnWidthsOnWidthChangeOfColumn(column, (DataGridLength)e.OldValue);
                    }
                }
            }
            finally
            {
                column._processingWidthChange = false;
            }
        }

        /// <summary>
        /// Internal method which sets the column's width
        /// without actual redistribution of widths among other
        /// columns
        /// </summary>
        /// <param name="width"></param>
        internal void SetWidthInternal(DataGridLength width)
        {
            bool originalValue = _ignoreRedistributionOnWidthChange;
            _ignoreRedistributionOnWidthChange = true;
            try
            {
                Width = width;
            }
            finally
            {
                _ignoreRedistributionOnWidthChange = originalValue;
            }
        }

        /// <summary>
        ///     Coerces the WidthProperty based on the DataGrid transferred property rules
        /// </summary>
        private static object OnCoerceWidth(DependencyObject d, object baseValue)
        {
            var column = d as DataGridColumn;
            DataGridLength width = (DataGridLength)DataGridHelper.GetCoercedTransferPropertyValue(
                column,
                baseValue,
                WidthProperty,
                column.DataGridOwner,
                DataGrid.ColumnWidthProperty);

            double newDesiredValue = CoerceDesiredOrDisplayWidthValue(width.Value, width.DesiredValue, width.UnitType);
            double newDisplayValue = CoerceDesiredOrDisplayWidthValue(width.Value, width.DisplayValue, width.UnitType);
            newDisplayValue = (newDisplayValue.IsNaN() ? newDisplayValue : DataGridHelper.CoerceToMinMax(newDisplayValue, column.MinWidth, column.MaxWidth));
            if (newDisplayValue.IsNaN() || newDisplayValue.IsClose(width.DisplayValue))
            {
                return width;
            }

            return new DataGridLength(
                width.Value,
                width.UnitType,
                newDesiredValue,
                newDisplayValue);
        }

        /// <summary>
        ///     Helper method which coerces the DesiredValue or DisplayValue
        ///     of the width.
        /// </summary>
        private static double CoerceDesiredOrDisplayWidthValue(double widthValue, double memberValue, DataGridLengthUnitType type)
        {
            if (memberValue.IsNaN())
            {
                if (type == DataGridLengthUnitType.Pixel)
                {
                    memberValue = widthValue;
                }
                else if (type == DataGridLengthUnitType.Auto ||
                         type == DataGridLengthUnitType.SizeToCells ||
                         type == DataGridLengthUnitType.SizeToHeader)
                {
                    memberValue = 0d;
                }
            }
            return memberValue;
        }

        /// <summary>
        ///     Specifies the minimum width of the header and cells within this column.
        /// </summary>
        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the MinWidth property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(
                "MinWidth",
                typeof(double),
                typeof(DataGridColumn),
                new FrameworkPropertyMetadata(20d, new PropertyChangedCallback(OnMinWidthPropertyChanged), new CoerceValueCallback(OnCoerceMinWidth)),
                new ValidateValueCallback(ValidateMinWidth));

        /// <summary>
        /// Property changed call back for MinWidth property which notification propagation
        /// and does the redistribution of widths among other columns if needed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumn column = (DataGridColumn)d;
            DataGrid dataGrid = column.DataGridOwner;

            column.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);

            if (dataGrid != null && column.IsVisible)
            {
                dataGrid.InternalColumns.RedistributeColumnWidthsOnMinWidthChangeOfColumn(column, (double)e.OldValue);
            }
        }

        /// <summary>
        ///     Coerces the MinWidthProperty based on the DataGrid transferred property rules
        /// </summary>
        private static object OnCoerceMinWidth(DependencyObject d, object baseValue)
        {
            var column = d as DataGridColumn;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                column,
                baseValue,
                MinWidthProperty,
                column.DataGridOwner,
                DataGrid.MinColumnWidthProperty);
        }

        /// <summary>
        ///     Validates that the minimum width is an acceptable value
        /// </summary>
        private static bool ValidateMinWidth(object v)
        {
            double value = (double)v;
            return !(value < 0d || value.IsNaN() || Double.IsPositiveInfinity(value));
        }

        /// <summary>
        ///     Validates that the maximum width is an acceptable value
        /// </summary>
        private static bool ValidateMaxWidth(object v)
        {
            double value = (double)v;
            return !(value < 0d || value.IsNaN());
        }

        /// <summary>
        ///     Specifies the maximum width of the header and cells within this column.
        /// </summary>
        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the MaxWidth property.
        /// </summary>
        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register(
                "MaxWidth",
                typeof(double),
                typeof(DataGridColumn),
                new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnMaxWidthPropertyChanged), new CoerceValueCallback(OnCoerceMaxWidth)),
                new ValidateValueCallback(ValidateMaxWidth));

        /// <summary>
        /// Property changed call back for MaxWidth property which notification propagation
        /// and does the redistribution of widths among other columns if needed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumn column = (DataGridColumn)d;
            DataGrid dataGrid = column.DataGridOwner;

            column.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);

            if (dataGrid != null && column.IsVisible)
            {
                // dataGrid.InternalColumns.RedistributeColumnWidthsOnMaxWidthChangeOfColumn(column, (double)e.OldValue);
            }
        }

        /// <summary>
        ///     Coerces the MaxWidthProperty based on the DataGrid transferred property rules
        /// </summary>
        private static object OnCoerceMaxWidth(DependencyObject d, object baseValue)
        {
            var column = d as DataGridColumn;
            double transferValue = (double)DataGridHelper.GetCoercedTransferPropertyValue(
                column,
                baseValue,
                MaxWidthProperty,
                column.DataGridOwner,
                DataGrid.MaxColumnWidthProperty);

            //// Coerce the Max Width to 10k pixels if infinity on a star column
            //if (double.IsPositiveInfinity(transferValue) &&
            //    column.Width.IsStar)
            //{
            //    return _starMaxWidth;
            //}

            return transferValue;
        }

        /// <summary>
        ///      This is the width that cells and headers should use in Arrange.
        /// </summary>
        public double ActualWidth
        {
            get { return (double)GetValue(ActualWidthProperty); }
            private set { SetValue(ActualWidthPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ActualWidthPropertyKey =
            DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(0.0, null, new CoerceValueCallback(OnCoerceActualWidth)));

        public static readonly DependencyProperty ActualWidthProperty = ActualWidthPropertyKey.DependencyProperty;

        private static object OnCoerceActualWidth(DependencyObject d, object baseValue)
        {
            DataGridColumn column = ((DataGridColumn)d);
            double actualWidth = (double)baseValue;
            double minWidth = column.MinWidth;
            double maxWidth = column.MaxWidth;

            // If the width is an absolute pixel value, then ActualWidth should be that value
            DataGridLength width = column.Width;
            if (width.IsAbsolute)
            {
                actualWidth = width.DisplayValue;
            }

            if (actualWidth < minWidth)
            {
                actualWidth = minWidth;
            }
            else if (actualWidth > maxWidth)
            {
                actualWidth = maxWidth;
            }

            return actualWidth;
        }

        #endregion

        #region IsReadOnly

        /// <summary>
        ///     Whether cells in this column can enter edit mode.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the IsReadOnly property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false, OnNotifyCellPropertyChanged, OnCoerceIsReadOnly));

        private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
        {
            var column = d as DataGridColumn;
            return column.OnCoerceIsReadOnly((bool)baseValue);
        }

        /// <summary>
        ///     Subtypes can override this to force IsReadOnly to be coerced to true.
        /// </summary>
        protected virtual bool OnCoerceIsReadOnly(bool baseValue)
        {
            return (bool)DataGridHelper.GetCoercedTransferPropertyValue(
                this,
                baseValue,
                IsReadOnlyProperty,
                DataGridOwner,
                DataGrid.IsReadOnlyProperty);
        }

        #endregion

        #region Visual Tree Generation

        /// <summary>
        ///     Retrieves the visual tree that was generated for a particular row and column.
        /// </summary>
        /// <param name="dataItem">The row that corresponds to the desired cell.</param>
        /// <returns>The element if found, null otherwise.</returns>
        public FrameworkElement GetCellContent(object dataItem)
        {
            if (dataItem == null)
            {
                throw new ArgumentNullException("dataItem");
            }

            if (_dataGridOwner != null)
            {
                DataGridRow row = _dataGridOwner.ItemContainerGenerator.ContainerFromItem(dataItem) as DataGridRow;
                if (row != null)
                {
                    return GetCellContent(row);
                }
            }

            return null;
        }

        /// <summary>
        ///     Retrieves the visual tree that was generated for a particular row and column.
        /// </summary>
        /// <param name="dataGridRow">The row that corresponds to the desired cell.</param>
        /// <returns>The element if found, null otherwise.</returns>
        public FrameworkElement GetCellContent(DataGridRow dataGridRow)
        {
            if (dataGridRow == null)
            {
                throw new ArgumentNullException("dataGridRow");
            }

            if (_dataGridOwner != null)
            {
                int columnIndex = _dataGridOwner.Columns.IndexOf(this);
                if (columnIndex >= 0)
                {
                    DataGridCell cell = dataGridRow.TryGetCell(columnIndex);
                    if (cell != null)
                    {
                        return cell.Content as FrameworkElement;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Method which updates the cell for property changes
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propertyName"></param>
        protected internal virtual void RefreshCellContent(FrameworkElement element, string propertyName)
        {
        }

        /// <summary>
        ///     Creates the visual tree that will become the content of a cell.
        /// </summary>
        /// <param name="isEditing">Whether the editing version is being requested.</param>
        /// <param name="dataItem">The data item for the cell.</param>
        /// <param name="cell">The cell container that will receive the tree.</param>
        internal FrameworkElement BuildVisualTree(bool isEditing, object dataItem, DataGridCell cell)
        {
            if (isEditing)
            {
                return GenerateEditingElement(cell, dataItem);
            }
            else
            {
                return GenerateElement(cell, dataItem);
            }
        }

        /// <summary>
        ///     Creates the visual tree that will become the content of a cell.
        /// </summary>
        protected abstract FrameworkElement GenerateElement(DataGridCell cell, object dataItem);

        /// <summary>
        ///     Creates the visual tree that will become the content of a cell.
        /// </summary>
        protected abstract FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem);


        #endregion

        #region Editing

        /// <summary>
        ///     Called when a cell has just switched to edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <returns>The unedited value of the cell.</returns>
        protected virtual object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            return null;
        }

        /// <summary>
        ///     Called when a cell's value is to be restored to its original value,
        ///     just before it exits edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <param name="uneditedValue">The original, unedited value of the cell.</param>
        protected virtual void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            // DataGridHelper.UpdateTarget(editingElement);
        }

        /// <summary>
        ///     Called when a cell's value is to be committed, just before it exits edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <returns>false if there is a validation error. true otherwise.</returns>
        protected virtual bool CommitCellEdit(FrameworkElement editingElement)
        {
            return true;
            // return DataGridHelper.ValidateWithoutUpdate(editingElement);
        }


        #endregion

        /// <summary>
        ///     Notifies the DataGrid and the Column Headers about property changes.
        /// </summary>
        private static void OnNotifyColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnHeaders);
        }

        /// <summary>
        ///   General notification for DependencyProperty changes from the grid and/or column.
        /// </summary>
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            if (DataGridHelper.ShouldNotifyColumns(target))
            {
                // Remove columns target since we're handling it.  If we're targeting multiple targets it may also need to get
                // sent to the DataGrid.
                target &= ~DataGridNotificationTarget.Columns;

                //if (e.Property == DataGrid.MaxColumnWidthProperty || e.Property == MaxWidthProperty)
                //{
                //    DataGridHelper.TransferProperty(this, MaxWidthProperty);
                //}
                //else if (e.Property == DataGrid.MinColumnWidthProperty || e.Property == MinWidthProperty)
                //{
                //    DataGridHelper.TransferProperty(this, MinWidthProperty);
                //}
                //else if (e.Property == DataGrid.ColumnWidthProperty || e.Property == WidthProperty)
                //{
                //    DataGridHelper.TransferProperty(this, WidthProperty);
                //}
                //else if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == HeaderStyleProperty)
                //{
                //    DataGridHelper.TransferProperty(this, HeaderStyleProperty);
                //}
                //else if (e.Property == DataGrid.CellStyleProperty || e.Property == CellStyleProperty)
                //{
                //    DataGridHelper.TransferProperty(this, CellStyleProperty);
                //}
                //else if (e.Property == DataGrid.IsReadOnlyProperty || e.Property == IsReadOnlyProperty)
                //{
                //    DataGridHelper.TransferProperty(this, IsReadOnlyProperty);
                //}
                //else if (e.Property == DataGrid.DragIndicatorStyleProperty || e.Property == DragIndicatorStyleProperty)
                //{
                //    DataGridHelper.TransferProperty(this, DragIndicatorStyleProperty);
                //}
                //else if (e.Property == DisplayIndexProperty)
                //{
                //    CoerceValue(IsFrozenProperty);
                //}
                //else if (e.Property == DataGrid.CanUserSortColumnsProperty)
                //{
                //    DataGridHelper.TransferProperty(this, CanUserSortProperty);
                //}
                //else if (e.Property == DataGrid.CanUserResizeColumnsProperty || e.Property == CanUserResizeProperty)
                //{
                //    DataGridHelper.TransferProperty(this, CanUserResizeProperty);
                //}
                //else if (e.Property == DataGrid.CanUserReorderColumnsProperty || e.Property == CanUserReorderProperty)
                //{
                //    DataGridHelper.TransferProperty(this, CanUserReorderProperty);
                //}

                if (e.Property == WidthProperty || e.Property == MinWidthProperty || e.Property == MaxWidthProperty)
                {
                    CoerceValue(ActualWidthProperty);
                }
            }

            if (target != DataGridNotificationTarget.None)
            {
                // Everything else gets sent to the DataGrid so it can propogate back down
                // to the targets that need notification.
                DataGridColumn column = (DataGridColumn)d;
                DataGrid dataGridOwner = column.DataGridOwner;
                if (dataGridOwner != null)
                {
                    dataGridOwner.NotifyPropertyChanged(d, e, target);
                }
            }
        }

        /// <summary>
        /// Method which propogates the property changed notification to datagrid
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (DataGridOwner != null)
            {
                DataGridOwner.NotifyPropertyChanged(this, propertyName, new DependencyPropertyChangedEventArgs(), DataGridNotificationTarget.RefreshCellContent);
            }
        }

        /// <summary>
        ///     Notifies the DataGrid and the Cells about property changes.
        /// </summary>
        internal static void OnNotifyCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.Cells);
        }

        /// <summary>
        /// Method used as property changed callback for properties which need RefreshCellContent to be called
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        internal static void NotifyPropertyChangeForRefreshContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridColumn)d).NotifyPropertyChanged(e.Property.Name);
        }

    }
}
