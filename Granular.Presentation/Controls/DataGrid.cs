using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Granular.Collections;
using Granular.Extensions;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a control that displays data in a customizable grid.
    /// </summary>
    public class DataGrid : MultiSelector
    {
        static DataGrid()
        {
            Type ownerType = typeof(DataGrid);

            Control.IsTabStopProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGrid))));

            var dataGridRowPresenterFactory = new FrameworkElementFactory(new ElementFactory(typeof(DataGridRowsPresenter), ElementInitializer.Empty), new InitializeContext(XamlNamespaces.Empty));
            ItemsPanelProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(new ItemsPanelTemplate
            {
                FrameworkElementFactory = dataGridRowPresenterFactory
            }));
        }

        public DataGrid()
        {
            _columns = new DataGridColumnCollection(this);
            _columns.CollectionChanged += OnColumnsChanged;

            SetSelectionBehavior();
        }

        #region Width

        /// <summary>
        ///     Specifies the width of the header and cells within all the columns.
        /// </summary>
        public DataGridLength ColumnWidth
        {
            get { return (DataGridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the ColumnWidth property.
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register("ColumnWidth", typeof(DataGridLength), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridLength.SizeToHeader));


        /// <summary>
        ///     Specifies the minimum width of the header and cells within all columns.
        /// </summary>
        public double MinColumnWidth
        {
            get { return (double)GetValue(MinColumnWidthProperty); }
            set { SetValue(MinColumnWidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the MinColumnWidth property.
        /// </summary>
        public static readonly DependencyProperty MinColumnWidthProperty =
            DependencyProperty.Register(
                "MinColumnWidth",
                typeof(double),
                typeof(DataGrid),
                new FrameworkPropertyMetadata(20d, new PropertyChangedCallback(OnColumnSizeConstraintChanged)),
                new ValidateValueCallback(ValidateMinColumnWidth));

        /// <summary>
        ///     Specifies the maximum width of the header and cells within all columns.
        /// </summary>
        public double MaxColumnWidth
        {
            get { return (double)GetValue(MaxColumnWidthProperty); }
            set { SetValue(MaxColumnWidthProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty that represents the  MaxColumnWidth property.
        /// </summary>
        public static readonly DependencyProperty MaxColumnWidthProperty =
            DependencyProperty.Register(
                "MaxColumnWidth",
                typeof(double),
                typeof(DataGrid),
                new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnColumnSizeConstraintChanged)),
                new ValidateValueCallback(ValidateMaxColumnWidth));

        private static void OnColumnSizeConstraintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
        }

        /// <summary>
        /// Validates that the minimum column width is an acceptable value
        /// </summary>
        private static bool ValidateMinColumnWidth(object v)
        {
            double value = (double)v;
            return !(value < 0d || value.IsNaN() || Double.IsPositiveInfinity(value));
        }

        /// <summary>
        /// Validates that the maximum column width is an acceptable value
        /// </summary>
        private static bool ValidateMaxColumnWidth(object v)
        {
            double value = (double)v;
            return !(value < 0d || value.IsNaN());
        }

        #endregion

        #region Column Auto Generation

        /// <summary>
        ///     The DependencyProperty that represents the AutoGenerateColumns property.
        /// </summary>
        public static readonly DependencyProperty AutoGenerateColumnsProperty =
            DependencyProperty.Register("AutoGenerateColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoGenerateColumnsPropertyChanged)));

        /// <summary>
        /// The property which determines whether the columns are to be auto generated or not.
        /// Setting of the property actually generates or deletes columns.
        /// </summary>
        public bool AutoGenerateColumns
        {
            get { return (bool)GetValue(AutoGenerateColumnsProperty); }
            set { SetValue(AutoGenerateColumnsProperty, value); }
        }

        /// <summary>
        /// The event listener which listens to the change in the AutoGenerateColumns flag
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnAutoGenerateColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            DataGrid dataGrid = (DataGrid)d;

            if (newValue)
            {
                // dataGrid.AddAutoColumns();
            }
            else
            {
                // dataGrid.DeleteAutoColumns();
            }
        }


        #endregion


        #region Editing

        /// <summary>
        ///     Whether the DataGrid's rows and cells can be placed in edit mode.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for IsReadOnly.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsReadOnlyChanged)));

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue)
            //{
            //    // When going from R/W to R/O, cancel any current edits
            //    ((DataGrid)d).CancelAnyEdit();
            //}

            //// re-evalutate the BeginEdit command's CanExecute.
            //CommandManager.InvalidateRequerySuggested();

            //d.CoerceValue(CanUserAddRowsProperty);
            //d.CoerceValue(CanUserDeleteRowsProperty);

            // Affects the IsReadOnly property on cells
            OnNotifyColumnAndCellPropertyChanged(d, e);
        }

        #endregion

        #region Columns

        private DataGridColumnHeadersPresenter _columnHeadersPresenter; // headers presenter for sending down notifications

        /// <summary>
        /// Reference to the ColumnHeadersPresenter. The presenter sets this when it is created.
        /// </summary>
        internal DataGridColumnHeadersPresenter ColumnHeadersPresenter
        {
            get { return _columnHeadersPresenter; }
            set { _columnHeadersPresenter = value; }
        }

        private readonly DataGridColumnCollection _columns;                          // Stores the columns

        /// <summary>
        ///     A collection of column definitions describing the individual
        ///     columns of each row.
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        ///     Returns the column collection without having to upcast from ObservableCollection
        /// </summary>
        internal DataGridColumnCollection InternalColumns
        {
            get { return _columns; }
        }

        /// <summary>
        ///     Called when the Columns collection changes.
        /// </summary>
        private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update the reference to this DataGrid on the affected column(s)
            // and update the SelectedCells collection.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateDataGridReference(e.NewItems, /* clear = */ false);
                    UpdateColumnSizeConstraints(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UpdateDataGridReference(e.OldItems, /* clear = */ true);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    UpdateDataGridReference(e.OldItems, /* clear = */ true);
                    UpdateDataGridReference(e.NewItems, /* clear = */ false);
                    UpdateColumnSizeConstraints(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // We can't clear column references on Reset: _columns has 0 items and e.OldItems is empty.
                    // _selectedCells.Clear();
                    break;
            }

            //// FrozenColumns rely on column DisplayIndex
            //// Delay the coercion if necessary
            //if (InternalColumns.DisplayIndexMapInitialized)
            //{
            //    CoerceValue(FrozenColumnCountProperty);
            //}

            bool visibleColumnsChanged = HasVisibleColumns(e.OldItems);
            visibleColumnsChanged |= HasVisibleColumns(e.NewItems);
            visibleColumnsChanged |= (e.Action == NotifyCollectionChangedAction.Reset);

            //if (visibleColumnsChanged)
            //{
            //    InternalColumns.InvalidateColumnRealization(true);
            //}

            // UpdateColumnsOnRows(e);

            // Recompute the column width if required, but wait until the first load
            if (visibleColumnsChanged && e.Action != NotifyCollectionChangedAction.Move)
            {
                InternalColumns.InvalidateColumnWidthsComputation();
            }
        }


        /// <summary>
        ///     General notification for DependencyProperty changes from the grid or from columns.
        /// </summary>
        /// <remarks>
        ///     This can be called from a variety of sources, such as from column objects
        ///     or from this DataGrid itself when there is a need to notify the rows and/or
        ///     the cells in the DataGrid about a property change. Down-stream handlers
        ///     can check the source of the change using the "d" parameter.
        /// </remarks>
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            NotifyPropertyChanged(d, string.Empty, e, target);
        }

        /// <summary>
        ///     General notification for DependencyProperty changes from the grid or from columns.
        /// </summary>
        /// <remarks>
        ///     This can be called from a variety of sources, such as from column objects
        ///     or from this DataGrid itself when there is a need to notify the rows and/or
        ///     the cells in the DataGrid about a property change. Down-stream handlers
        ///     can check the source of the change using the "d" parameter.
        /// </remarks>
        internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            //if (DataGridHelper.ShouldNotifyDataGrid(target))
            //{
            //    if (e.Property == AlternatingRowBackgroundProperty)
            //    {
            //        // If the alternate row background is set, the count may be coerced to 2
            //        CoerceValue(AlternationCountProperty);
            //    }
            //    else if ((e.Property == DataGridColumn.VisibilityProperty) || (e.Property == DataGridColumn.WidthProperty) || (e.Property == DataGridColumn.DisplayIndexProperty))
            //    {
            //        // DataGridCellsPanel needs to be re-measured when column visibility changes
            //        // Recyclable containers may not be fully remeasured when they are brought in
            //        foreach (DependencyObject container in ItemContainerGenerator.RecyclableContainers)
            //        {
            //            DataGridRow row = container as DataGridRow;
            //            if (row != null)
            //            {
            //                var cellsPresenter = row.CellsPresenter;
            //                if (cellsPresenter != null)
            //                {
            //                    cellsPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
            //                }
            //            }
            //        }
            //    }
            //}

            // Rows, Cells, CellsPresenter, DetailsPresenter or RowHeaders
            if (DataGridHelper.ShouldNotifyRowSubtree(target))
            {
                // Notify the Rows about the property change
                ContainerTracking<DataGridRow> tracker = _rowTrackingRoot;
                while (tracker != null)
                {
                    tracker.Container.NotifyPropertyChanged(d, propertyName, e, target);
                    tracker = tracker.Next;
                }
            }

            //if (DataGridHelper.ShouldNotifyColumnCollection(target) || DataGridHelper.ShouldNotifyColumns(target))
            //{
            //    InternalColumns.NotifyPropertyChanged(d, propertyName, e, target);
            //}

            //if ((DataGridHelper.ShouldNotifyColumnHeadersPresenter(target) || DataGridHelper.ShouldNotifyColumnHeaders(target)) && ColumnHeadersPresenter != null)
            //{
            //    ColumnHeadersPresenter.NotifyPropertyChanged(d, propertyName, e, target);
            //}
        }

        /// <summary>
        ///     Notifies each Column and Cell about property changes.
        /// </summary>
        private static void OnNotifyColumnAndCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.Cells);
        }

        internal void OnHasVisibleStarColumnsChanged()
        {
            DetermineItemsHostStarBehavior();
        }

        private ContainerTracking<DataGridRow> _rowTrackingRoot;            // Root of a linked list of active row containers

        private void DetermineItemsHostStarBehavior()
        {
            //VirtualizingStackPanel panel = _internalItemsHost as VirtualizingStackPanel;
            //if (panel != null)
            //{
            //    panel.IgnoreMaxDesiredSize = InternalColumns.HasVisibleStarColumns;
            //}
        }

        private Panel _internalItemsHost;                                   // Workaround for not having access to ItemsHost
        /// <summary>
        ///     Workaround for not having access to ItemsControl.ItemsHost.
        /// </summary>
        internal Panel InternalItemsHost
        {
            get { return _internalItemsHost; }
            set
            {
                if (_internalItemsHost != value)
                {
                    _internalItemsHost = value;
                    if (_internalItemsHost != null)
                    {
                        DetermineItemsHostStarBehavior();
                        EnsureInternalScrollControls();
                    }
                }
            }
        }

        private ScrollViewer _internalScrollHost;                           // Scroll viewer of the datagrid
        /// <summary>
        ///     Workaround for not having access to ItemsControl.ScrollHost.
        /// </summary>
        internal ScrollViewer InternalScrollHost
        {
            get
            {
                EnsureInternalScrollControls();
                return _internalScrollHost;
            }
        }

        private ScrollContentPresenter _internalScrollContentPresenter = null; // Scroll Content Presenter of DataGrid's ScrollViewer
        /// <summary>
        ///     Workaround for not having access to ScrollContentPresenter
        /// </summary>
        internal ScrollContentPresenter InternalScrollContentPresenter
        {
            get
            {
                EnsureInternalScrollControls();
                return _internalScrollContentPresenter;
            }
        }

        /// <summary>
        ///     Helper method which ensures the initialization of scroll controls.
        /// </summary>
        private void EnsureInternalScrollControls()
        {
            if (_internalScrollContentPresenter == null)
            {
                if (_internalItemsHost != null)
                {
                    _internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(_internalItemsHost);
                }
                else if (_rowTrackingRoot != null)
                {
                    DataGridRow row = _rowTrackingRoot.Container;
                    _internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(row);
                }
                if (_internalScrollContentPresenter != null)
                {
                    //_internalScrollContentPresenter.SizeChanged += new SizeChangedEventHandler(OnInternalScrollContentPresenterSizeChanged);
                }
            }

            if (_internalScrollHost == null)
            {
                if (_internalItemsHost != null)
                {
                    _internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(_internalItemsHost);
                }
                else if (_rowTrackingRoot != null)
                {
                    DataGridRow row = _rowTrackingRoot.Container;
                    _internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(row);
                }
                //if (_internalScrollHost != null)
                //{
                //    Binding horizontalOffsetBinding = new Binding("ContentHorizontalOffset");
                //    horizontalOffsetBinding.Source = _internalScrollHost;
                //    SetBinding(HorizontalScrollOffsetProperty, horizontalOffsetBinding);
                //}
            }
        }

        /// <summary>
        /// Helper method which returns the width of the viewport which is available for the columns to render
        /// </summary>
        /// <returns></returns>
        internal double GetViewportWidthForColumns()
        {
            if (InternalScrollHost == null)
            {
                return 0.0;
            }

            double totalAvailableWidth = InternalScrollHost.ViewportWidth;
            //totalAvailableWidth -= CellsPanelHorizontalOffset;
            return totalAvailableWidth;
        }

        /// <summary>
        ///     Updates the reference to this DataGrid on the list of columns.
        /// </summary>
        /// <param name="list">The list of affected columns.</param>
        /// <param name="clear">Whether to add or remove the reference to this grid.</param>
        internal void UpdateDataGridReference(IEnumerable list, bool clear)
        {
            foreach (var c in list)
            {
                DataGridColumn column = (DataGridColumn)c;
                if (clear)
                {
                    // Set the owner to null only if the current owner is this grid
                    if (column.DataGridOwner == this)
                    {
                        column.DataGridOwner = null;
                    }
                }
                else
                {
                    // Remove the column from any old owner
                    if (column.DataGridOwner != null && column.DataGridOwner != this)
                    {
                        column.DataGridOwner.Columns.Remove(column);
                    }

                    column.DataGridOwner = this;
                }
            }
        }

        /// <summary>
        ///     Updates the transferred size constraints from DataGrid on the columns.
        /// </summary>
        /// <param name="list">The list of affected columns.</param>
        private static void UpdateColumnSizeConstraints(IEnumerable list)
        {
            foreach (var c in list)
            {
                var column = (DataGridColumn)c;
                column.SyncProperties();
            }
        }

        /// <summary>
        ///     Helper method which determines if the
        ///     given list has visible columns
        /// </summary>
        private static bool HasVisibleColumns(IEnumerable columns)
        {
            if (columns != null)
            {
                foreach (DataGridColumn column in columns)
                {
                    if (column.IsVisible)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Row Generation

        /// <summary>
        ///     Determines if an item is its own container.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>true if the item is a DataGridRow, false otherwise.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DataGridRow;
        }

        /// <summary>
        ///     Instantiates an instance of a container.
        /// </summary>
        /// <returns>A new DataGridRow.</returns>
        protected override FrameworkElement GetContainerForItemOverride()
        {
            return new DataGridRow();
        }

        /// <summary>
        ///     Prepares a new container for a given item.
        /// </summary>
        /// <param name="element">The new container.</param>
        /// <param name="item">The item that the container represents.</param>
        protected override void PrepareContainerForItemOverride(object item, FrameworkElement element)
        {
            base.PrepareContainerForItemOverride(item, element);

            DataGridRow row = (DataGridRow)element;
            row.PreviewMouseDown += OnItemContainerPreviewMouseDown; // handled too
            row.MouseDown += OnItemContainerMouseDown;
            row.KeyDown += OnItemContainerKeyDown;
            row.GotKeyboardFocus += OnItemContainerGotKeyboardFocus;

            if (row.DataGridOwner != this)
            {
                row.Tracker.StartTracking(ref _rowTrackingRoot);
                //if (item == CollectionView.NewItemPlaceholder ||
                //    (IsAddingNewItem && item == EditableItems.CurrentAddItem))
                //{
                //    row.IsNewItem = true;
                //}
                //else
                //{
                //    row.ClearValue(DataGridRow.IsNewItemPropertyKey);
                //}
                EnsureInternalScrollControls();
                //EnqueueNewItemMarginComputation();
            }

            row.PrepareRow(item, this);
            //OnLoadingRow(new DataGridRowEventArgs(row));
        }

        /// <summary>
        ///     Clears a container of references.
        /// </summary>
        /// <param name="element">The container being cleared.</param>
        /// <param name="item">The data item that the container represented.</param>
        protected override void ClearContainerForItemOverride(object item, FrameworkElement element)
        {
            base.ClearContainerForItemOverride(item, element);

            DataGridRow row = (DataGridRow)element;
            row.PreviewMouseDown -= OnItemContainerPreviewMouseDown; // handled too
            row.MouseDown -= OnItemContainerMouseDown;
            row.KeyDown -= OnItemContainerKeyDown;
            row.GotKeyboardFocus -= OnItemContainerGotKeyboardFocus;

            if (row.DataGridOwner == this)
            {
                row.Tracker.StopTracking(ref _rowTrackingRoot);
            }
        }

        #endregion

        private bool _isItemContainerBeingClicked;

        private void OnItemContainerPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                _isItemContainerBeingClicked = true;
            }
        }

        private void OnItemContainerMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                _selectionBehavior.SetClickSelection((DataGridRow)sender, ApplicationHost.Current.GetKeyboardDeviceFromElement(this).Modifiers);
                _isItemContainerBeingClicked = false;
            }
        }

        private void OnItemContainerKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                _selectionBehavior.SetClickSelection((DataGridRow)sender, e.KeyboardDevice.Modifiers);
            }
        }

        private void OnItemContainerGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!_isItemContainerBeingClicked)
            {
                _selectionBehavior.SetFocusChangeSelection((DataGridRow)sender, e.KeyboardDevice.Modifiers);
            }
        }

        #region Selection

        private interface ISelectionBehavior
        {
            void SetClickSelection(DataGridRow item, ModifierKeys modifiers);
            void SetFocusChangeSelection(DataGridRow item, ModifierKeys modifiers);
        }

        private class SingleSelectionBehavior : ISelectionBehavior
        {
            private readonly DataGrid _dataGrid;

            public SingleSelectionBehavior(DataGrid dataGrid)
            {
                this._dataGrid = dataGrid;
            }

            public void SetClickSelection(DataGridRow item, ModifierKeys modifiers)
            {
                _dataGrid.SetSingleSelection(item);

                //if (modifiers != ModifierKeys.Control)
                //{

                //}
                //else
                //{
                ////_dataGrid.SetSelectionAnchor(item);
                ////_dataGrid.ToggleSingleSelection(item);
                //}
            }

            public void SetFocusChangeSelection(DataGridRow item, ModifierKeys modifiers)
            {
                if (modifiers != ModifierKeys.Control)
                {
                    _dataGrid.SetSingleSelection(item);
                }
            }
        }

        /// <summary>
        ///     Defines the selection behavior.
        /// </summary>
        /// <remarks>
        ///     The SelectionMode and the SelectionUnit properties together define
        ///     the selection behavior for the DataGrid.
        /// </remarks>
        public DataGridSelectionUnit SelectionUnit
        {
            get { return (DataGridSelectionUnit)GetValue(SelectionUnitProperty); }
            set { SetValue(SelectionUnitProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the SelectionUnit property.
        /// </summary>
        public static readonly DependencyProperty SelectionUnitProperty =
            DependencyProperty.Register("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridSelectionUnit.FullRow, new PropertyChangedCallback(OnSelectionUnitChanged)));

        private static void OnSelectionUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)d;
            DataGridSelectionUnit oldUnit = (DataGridSelectionUnit)e.OldValue;

            ////// Full wipe on unit change
            ////if (oldUnit != DataGridSelectionUnit.Cell)
            ////{
            ////    dataGrid.UnselectAll();
            ////}

            ////if (oldUnit != DataGridSelectionUnit.FullRow)
            ////{
            ////    using (dataGrid.UpdateSelectedCells())
            ////    {
            ////        dataGrid._selectedCells.Clear();
            ////    }
            ////}

            dataGrid.CoerceValue(IsSynchronizedWithCurrentItemProperty);
        }

        private void SetSingleSelection(DataGridRow item)
        {
            SelectedItem = ItemContainerGenerator.ItemFromContainer(item);

            for (int i = 0; i < ItemContainerGenerator.ItemsCount; i++)
            {
                DependencyObject itemContainer = ItemContainerGenerator.Generate(i);
                itemContainer.SetCurrentValue(Selector.IsSelectedProperty, itemContainer == item);
            }
        }


        private ISelectionBehavior _selectionBehavior;

        private void SetSelectionBehavior()
        {
            _selectionBehavior = new SingleSelectionBehavior(this);
        }

        #endregion

        private bool _columnWidthsComputed;
        protected override Size MeasureOverride(Size availableSize)
        {
            var retValue = base.MeasureOverride(availableSize);

            if (!_columnWidthsComputed)
            {
                InternalColumns.InvalidateColumnWidthsComputation();
                _columnWidthsComputed = true;
            }

            return retValue;
        }
        
        protected override void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            InternalColumns.InvalidateColumnWidthsComputation();

            base.OnItemsSourceChanged(e);
        }
    }
}
