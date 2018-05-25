using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace System.Windows.Controls
{


    /// <summary>
    ///     A control for displaying a row of the DataGrid.
    ///     A row represents a data item in the DataGrid.
    ///     A row displays a cell for each column of the DataGrid.
    /// 
    ///     The data item for the row is added n times to the row's Items collection, 
    ///     where n is the number of columns in the DataGrid.
    /// </summary>
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.NormalState)]
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.MouseOverState)]
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.DisabledState)]
    [TemplateVisualState(VisualStates.SelectionStates, VisualStates.SelectedState)]
    [TemplateVisualState(VisualStates.SelectionStates, VisualStates.SelectedUnfocusedState)]
    [TemplateVisualState(VisualStates.SelectionStates, VisualStates.UnselectedState)]
    [TemplateVisualState(VisualStates.FocusStates, VisualStates.FocusedState)]
    [TemplateVisualState(VisualStates.FocusStates, VisualStates.UnfocusedState)]
    public class DataGridRow : Control
    {
        static DataGridRow()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridRow))));

            UIElement.IsEnabledProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsVisualState));
            UIElement.IsMouseOverProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsVisualState));
            Selector.IsSelectionActiveProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsVisualState));
        }

        public DataGridRow()
        {
            _tracker = new ContainerTracking<DataGridRow>(this);
        }

        private DataGrid _owner;
        internal DataGrid DataGridOwner
        {
            get { return _owner; }
        }

        private DataGridCellsPresenter _cellsPresenter;
        /// <summary>
        ///     Set by the CellsPresenter when it is created.  Used by the Row to send down property change notifications.
        /// </summary>
        internal DataGridCellsPresenter CellsPresenter
        {
            get { return _cellsPresenter; }
            set { _cellsPresenter = value; }
        }

        /// <summary>
        ///     Acceses the CellsPresenter and attempts to get the cell at the given index.
        ///     This is not necessarily the display order.
        /// </summary>
        internal DataGridCell TryGetCell(int index)
        {
            DataGridCellsPresenter cellsPresenter = CellsPresenter;
            if (cellsPresenter != null)
            {
                return cellsPresenter.ItemContainerGenerator.ContainerFromIndex(index) as DataGridCell;
            }

            return null;
        }

        private ContainerTracking<DataGridRow> _tracker;
        /// <summary>
        ///     Used by the DataGrid owner to send notifications to the row container.
        /// </summary>
        internal ContainerTracking<DataGridRow> Tracker
        {
            get { return _tracker; }
        }

        #region Data Item

        /// <summary>
        ///     The item that the row represents. This item is an entry in the list of items from the DataGrid.
        ///     From this item, cells are generated for each column in the DataGrid.
        /// </summary>
        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the Item property.
        /// </summary>
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(object), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnNotifyRowPropertyChanged)));

        /// <summary>
        ///     Called when the value of the Item property changes.
        /// </summary>
        /// <param name="oldItem">The old value of Item.</param>
        /// <param name="newItem">The new value of Item.</param>
        protected virtual void OnItemChanged(object oldItem, object newItem)
        {
            DataGridCellsPresenter cellsPresenter = CellsPresenter;
            if (cellsPresenter != null)
            {
                cellsPresenter.Item = newItem;
            }

            DataContext = newItem;
        }

        #endregion

        private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
        }

        /// <summary>
        ///     General notification for DependencyProperty changes from the grid or from columns.
        /// </summary>
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            NotifyPropertyChanged(d, string.Empty, e, target);
        }

        /// <summary>
        ///     General notification for DependencyProperty changes from the grid or from columns.
        /// </summary>
        internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
        {
            if (e.Property == ItemProperty)
            {
                OnItemChanged(e.OldValue, e.NewValue);
            }
        }

        /// <summary>
        ///     Prepares a row container for active use.
        /// </summary>
        /// <remarks>
        ///     Instantiates or updates a MultipleCopiesCollection ItemsSource in
        ///     order that cells be generated.
        /// </remarks>
        /// <param name="item">The data item that the row represents.</param>
        /// <param name="owningDataGrid">The DataGrid owner.</param>
        internal void PrepareRow(object item, DataGrid owningDataGrid)
        {
            bool fireOwnerChanged = (_owner != owningDataGrid);
            
            bool forcePrepareCells = false;
            _owner = owningDataGrid;

            if (this != item)
            {
                if (Item != item)
                {
                    Item = item;
                }
                else
                {
                    forcePrepareCells = true;
                }
            }

            // Since we just changed _owner we need to invalidate all child properties that rely on a value supplied by the DataGrid.
            // A common scenario is when a recycled Row was detached from the visual tree and has just been reattached (we always clear out the 
            // owner when recycling a container).
            if (fireOwnerChanged)
            {
                // SyncProperties(forcePrepareCells);
            }
        }

        #region Selection

        /// <summary>
        ///     Indicates whether this DataGridRow is selected.
        /// </summary>
        /// <remarks>
        ///     When IsSelected is set to true, an InvalidOperationException may be
        ///     thrown if the value of the SelectionUnit property on the parent DataGrid 
        ///     prevents selection or rows.
        /// </remarks>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the IsSelected property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(
            typeof(DataGridRow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsSelectedChanged)));

        private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;

            bool isSelected = (bool)e.NewValue;

            if (isSelected && !row.IsSelectable)
            {
                throw new Granular.Exception("Cannot select Row");
            }

            DataGrid grid = row.DataGridOwner;
            if (grid != null && row.DataContext != null)
            {
                //DataGridAutomationPeer gridPeer = UIElementAutomationPeer.FromElement(grid) as DataGridAutomationPeer;
                //if (gridPeer != null)
                //{
                //    DataGridItemAutomationPeer rowItemPeer = gridPeer.FindOrCreateItemAutomationPeer(row.DataContext) as DataGridItemAutomationPeer;
                //    if (rowItemPeer != null)
                //    {
                //        rowItemPeer.RaisePropertyChangedEvent(
                //            System.Windows.Automation.SelectionItemPatternIdentifiers.IsSelectedProperty,
                //            (bool)e.OldValue,
                //            isSelected);
                //    }
                //}
            }

            // Update the header's IsRowSelected property
            row.NotifyPropertyChanged(row, e, DataGridNotificationTarget.Rows | DataGridNotificationTarget.RowHeaders);

            // This will raise the appropriate selection event, which will
            // bubble to the DataGrid. The base class Selector code will listen
            // for these events and will update SelectedItems as necessary.
            row.RaiseSelectionChangedEvent(isSelected);

            row.UpdateVisualState();

            // Update the header's IsRowSelected property
            row.NotifyPropertyChanged(row, e, DataGridNotificationTarget.Rows | DataGridNotificationTarget.RowHeaders);
        }

        private void RaiseSelectionChangedEvent(bool isSelected)
        {
            if (isSelected)
            {
                OnSelected(new RoutedEventArgs(SelectedEvent, this));
            }
            else
            {
                OnUnselected(new RoutedEventArgs(UnselectedEvent, this));
            }
        }

        /// <summary>
        ///     Raised when the item's IsSelected property becomes true.
        /// </summary>
        public static readonly RoutedEvent SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(DataGridRow));

        /// <summary>
        ///     Raised when the item's IsSelected property becomes true.
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add
            {
                AddHandler(SelectedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedEvent, value);
            }
        }

        /// <summary>
        ///     Called when IsSelected becomes true. Raises the Selected event.
        /// </summary>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void OnSelected(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        ///     Raised when the item's IsSelected property becomes false.
        /// </summary>
        public static readonly RoutedEvent UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(DataGridRow));

        /// <summary>
        ///     Raised when the item's IsSelected property becomes false.
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add
            {
                AddHandler(UnselectedEvent, value);
            }

            remove
            {
                RemoveHandler(UnselectedEvent, value);
            }
        }

        /// <summary>
        ///     Called when IsSelected becomes false. Raises the Unselected event.
        /// </summary>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        ///     Determines if a row can be selected, based on the DataGrid's SelectionUnit property.
        /// </summary>
        private bool IsSelectable
        {
            get
            {
                DataGrid dataGrid = DataGridOwner;
                if (dataGrid != null)
                {
                    DataGridSelectionUnit unit = dataGrid.SelectionUnit;
                    return (unit == DataGridSelectionUnit.FullRow) ||
                        (unit == DataGridSelectionUnit.CellOrRowHeader);
                }

                return true;
            }
        }

        #endregion

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
        }

    }
}
