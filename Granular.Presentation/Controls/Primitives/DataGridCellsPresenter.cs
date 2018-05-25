using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using Granular.Collections;

namespace System.Windows.Controls.Primitives
{
    /// <summary>
    ///     A control that will be responsible for generating cells.
    ///     This control is meant to be specified within the template of a DataGridRow.
    ///     The APIs from ItemsControl do not match up nicely with the meaning of a
    ///     row, which is why this is being factored out.
    ///
    ///     The data item for the row is added n times to the Items collection,
    ///     where n is the number of columns in the DataGrid. This is implemented
    ///     using a special collection to avoid keeping multiple references to the
    ///     same object.
    /// </summary>
    public class DataGridCellsPresenter : ItemsControl
    {
        static DataGridCellsPresenter()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridCellsPresenter))));
            FocusableProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(false));

            ItemsPanelProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new ItemsPanelTemplate
            {
                FrameworkElementFactory = new FrameworkElementFactory(new ElementFactory(typeof(DataGridCellsPanel), ElementInitializer.Empty), new InitializeContext(XamlNamespaces.Empty))
            }));
        }

        public DataGridCellsPresenter()
        {
        }

        /// <summary>It's invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DataGridRow dataGridRowOwner = this.DataGridRowOwner;
            if (dataGridRowOwner != null)
            {
                dataGridRowOwner.CellsPresenter = this;
                this.Item = dataGridRowOwner.Item;
            }
            this.SyncProperties(false);
        }

        #region Cell Container Generation

        /// <summary>
        ///     Determines if an item is its own container.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>true if the item is a DataGridCell, false otherwise.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DataGridCell;
        }

        /// <summary>
        ///     Method which returns the result of IsItemItsOwnContainerOverride to be used internally
        /// </summary>
        internal bool IsItemItsOwnContainerInternal(object item)
        {
            return IsItemItsOwnContainerOverride(item);
        }

        /// <summary>
        ///     Instantiates an instance of a container.
        /// </summary>
        /// <returns>A new DataGridCell.</returns>
        protected override FrameworkElement GetContainerForItemOverride()
        {
            return new DataGridCell();
        }

        #endregion

        private ObservableCollection<DataGridColumn> Columns
        {
            get
            {
                DataGridRow owningRow = DataGridRowOwner;
                DataGrid owningDataGrid = (owningRow != null) ? owningRow.DataGridOwner : null;
                return (owningDataGrid != null) ? owningDataGrid.Columns : null;
            }
        }

        #region Data Item

        private object _item;
        /// <summary>
        ///     The item that the row represents. This item is an entry in the list of items from the DataGrid.
        ///     From this item, cells are generated for each column in the DataGrid.
        /// </summary>
        public object Item
        {
            get
            {
                return _item;
            }

            internal set
            {
                if (_item != value)
                {
                    object oldItem = _item;
                    _item = value;
                    OnItemChanged(oldItem, _item);
                }
            }
        }

        /// <summary>
        ///     Called when the value of the Item property changes.
        /// </summary>
        /// <param name="oldItem">The old value of Item.</param>
        /// <param name="newItem">The new value of Item.</param>
        protected virtual void OnItemChanged(object oldItem, object newItem)
        {
            ObservableCollection<DataGridColumn> columns = Columns;

            if (columns != null)
            {
                // Either update or create a collection that will return the row's data item
                // n number of times, where n is the number of columns.
                ObservableCollection<DataGridCell> cellItems = ItemsSource as ObservableCollection<DataGridCell>;
                if (cellItems == null)
                {
                    cellItems = new ObservableCollection<DataGridCell>();
                    ItemsSource = cellItems;
                }
                else
                {
                    cellItems.Clear();
                }

                DataGridRow row = DataGridRowOwner;

                foreach (var dataGridColumn in Columns)
                {
                    var cell = new DataGridCell
                    {
                        Column = dataGridColumn,
                    };

                    cell.PrepareCell(row.Item, this, row);

                    cellItems.Add(cell);
                }
            }
        }

        #endregion

        //#region Layout

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    Size childConstraint = new Size(Double.PositiveInfinity, availableSize.Height);
        //    var desiredSize = base.MeasureOverride(childConstraint);

        //    desiredSize = new Size(Math.Min(availableSize.Width, desiredSize.Width), desiredSize.Height);

        //    return desiredSize;
        //}

        //#endregion

        /// <summary>
        ///     The DataGrid that owns this control
        /// </summary>
        internal DataGrid DataGridOwner
        {
            get
            {
                DataGridRow parent = DataGridRowOwner;
                if (parent != null)
                {
                    return parent.DataGridOwner;
                }

                return null;
            }
        }

        /// <summary>
        ///     The DataGridRow that owns this control.
        /// </summary>
        internal DataGridRow DataGridRowOwner
        {
            get { return DataGridHelper.FindParent<DataGridRow>(this); }
        }

        /// <summary>
        ///     Update all properties that get a value from the DataGrid
        /// </summary>
        /// <remarks>
        ///     See comment on DataGridRow.SyncProperties
        /// </remarks>
        internal void SyncProperties(bool forcePrepareCells)
        {
            var dataGridOwner = DataGridOwner;
            if (dataGridOwner == null)
            {
                return;
            }
        }
    }
}
