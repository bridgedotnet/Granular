using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

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
    public class DataGridRow : Control
    {
        static DataGridRow()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridRow))));
            FocusableProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(false));
        }

        public DataGridRow()
        {
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

    }
}
