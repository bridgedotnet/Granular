using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace System.Windows.Controls
{
    /// <summary>
    ///     A control for displaying a cell of the DataGrid.
    /// </summary>
    public class DataGridCell : ContentControl, IProvideDataGridColumn
    {
        /// <summary>
        ///     Instantiates global information.
        /// </summary>
        static DataGridCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridCell))));         
        }

        public DataGridCell()
        {
        }

        private DataGridRow _owner;
        internal DataGrid DataGridOwner
        {
            get
            {
                if (_owner != null)
                {
                    DataGrid dataGridOwner = _owner.DataGridOwner;
                    if (dataGridOwner == null)
                    {
                        dataGridOwner = ItemsControl.ItemsControlFromItemContainer(_owner) as DataGrid;
                    }

                    return dataGridOwner;
                }

                return null;
            }
        }

        internal DataGridRow RowOwner
        {
            get { return _owner; }
        }

        internal object RowDataItem
        {
            get
            {
                DataGridRow row = RowOwner;
                if (row != null)
                {
                    return row.Item;
                }

                return DataContext;
            }
        }

        private bool NeedsVisualTree
        {
            get
            {
                return (ContentTemplate == null) && (ContentTemplateSelector == null);
            }
        }

        /// <summary>
        ///     Whether the cell can be placed in edit mode.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
        }

        private static readonly DependencyPropertyKey IsReadOnlyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsReadOnly", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, OnNotifyIsReadOnlyChanged, OnCoerceIsReadOnly));

        /// <summary>
        ///     The DependencyProperty for IsReadOnly.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = IsReadOnlyPropertyKey.DependencyProperty;

        private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
        {
            var cell = d as DataGridCell;
            var column = cell.Column;
            var dataGrid = cell.DataGridOwner;

            // We dont use the cell & 'baseValue' here because this property is read only on cell.
            // the column may coerce a default value to 'true', so we'll use it's effective value for IsReadOnly
            // as the baseValue.
            return DataGridHelper.GetCoercedTransferPropertyValue(
                column,
                column.IsReadOnly,
                DataGridColumn.IsReadOnlyProperty,
                dataGrid,
                DataGrid.IsReadOnlyProperty);
        }

        /// <summary>
        ///     Cancels editing the current cell & notifies the cell of a change to IsReadOnly.
        /// </summary>
        private static void OnNotifyIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var cell = (DataGridCell)d;
            //var dataGrid = cell.DataGridOwner;
            //if ((bool)e.NewValue && dataGrid != null)
            //{
            //    dataGrid.CancelEdit(cell);
            //}

            //// re-evalutate the BeginEdit command's CanExecute.
            //CommandManager.InvalidateRequerySuggested();

            //cell.NotifyPropertyChanged(d, string.Empty, e, DataGridNotificationTarget.Cells);
        }

        /// <summary>
        ///     The column that defines how this cell should appear.
        /// </summary>
        public DataGridColumn Column
        {
            get { return (DataGridColumn)GetValue(ColumnProperty); }
            internal set { SetValue(ColumnPropertyKey, value); }
        }

        /// <summary>
        ///     The DependencyPropertyKey that allows writing the Column property value.
        /// </summary>
        private static readonly DependencyPropertyKey ColumnPropertyKey =
            DependencyProperty.RegisterReadOnly("Column", typeof(DataGridColumn), typeof(DataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnColumnChanged)));

        /// <summary>
        ///     The DependencyProperty for the Columns property.
        /// </summary>
        public static readonly DependencyProperty ColumnProperty = ColumnPropertyKey.DependencyProperty;

        /// <summary>
        ///     Called when the Column property changes.
        ///     Calls the protected virtual OnColumnChanged.
        /// </summary>
        private static void OnColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                cell.OnColumnChanged((DataGridColumn)e.OldValue, (DataGridColumn)e.NewValue);
            }
        }

        /// <summary>
        ///     Called due to the cell's column definition changing.
        ///     Not called due to changes within the current column definition.
        /// </summary>
        /// <remarks>
        ///     Coerces ContentTemplate and ContentTemplateSelector.
        /// </remarks>
        /// <param name="oldColumn">The old column definition.</param>
        /// <param name="newColumn">The new column definition.</param>
        protected virtual void OnColumnChanged(DataGridColumn oldColumn, DataGridColumn newColumn)
        {
            // We need to call BuildVisualTree after changing the column (PrepareCell does this).
            Content = null;
            DataGridHelper.TransferProperty(this, StyleProperty);
            DataGridHelper.TransferProperty(this, IsReadOnlyProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var baseSize = base.MeasureOverride(availableSize);
            return new Size(Column.ActualWidth, baseSize.Height);
        }

        /// <summary>
        ///     Prepares a cell for use.
        /// </summary>
        /// <remarks>
        ///     Updates the column reference.
        ///     This overload computes the column index from the ItemContainerGenerator.
        /// </remarks>
        internal void PrepareCell(object item, ItemsControl cellsPresenter, DataGridRow ownerRow)
        {
            PrepareCell(item, ownerRow, cellsPresenter.ItemContainerGenerator.IndexFromContainer(this));
        }

        /// <summary>
        ///     Prepares a cell for use.
        /// </summary>
        /// <remarks>
        ///     Updates the column reference.
        /// </remarks>
        internal void PrepareCell(object item, DataGridRow ownerRow, int index)
        {
            _owner = ownerRow;

            DataGrid dataGrid = _owner.DataGridOwner;
            if (dataGrid != null)
            {
                // The index of the container should correspond to the index of the column
                if ((index >= 0) && (index < dataGrid.Columns.Count))
                {
                    // Retrieve the column definition and pass it to the cell container
                    DataGridColumn column = dataGrid.Columns[index];
                    Column = column;
                    // TabIndex = column.DisplayIndex;
                }

                if ((Content as FrameworkElement) == null)
                {
                    // If there isn't already a visual tree, then create one.
                    BuildVisualTree();

                    if (!NeedsVisualTree)
                    {
                        Content = item;
                    }
                }

                //// Update cell Selection
                //bool isSelected = dataGrid.SelectedCellsInternal.Contains(this);
                //SyncIsSelected(isSelected);
            }

            DataGridHelper.TransferProperty(this, StyleProperty);
            DataGridHelper.TransferProperty(this, IsReadOnlyProperty);
            CoerceValue(ClipProperty);
        }

        /// <summary>
        ///     Builds a column's visual tree if not using templates.
        /// </summary>
        internal void BuildVisualTree()
        {
            if (NeedsVisualTree)
            {
                var column = Column;
                if (column != null)
                {
                    //// Work around a problem with BindingGroup not removing BindingExpressions.
                    //var row = RowOwner;
                    //if (row != null)
                    //{
                    //    var bindingGroup = row.BindingGroup;
                    //    if (bindingGroup != null)
                    //    {
                    //        RemoveBindingExpressions(bindingGroup, Content as DependencyObject);
                    //    }
                    //}

                    // Ask the column to build a visual tree
                    FrameworkElement newContent = column.BuildVisualTree(false, RowDataItem, this);

                    // Before discarding the old visual tree, disconnect all its
                    // bindings, as in ItemContainerGenerator.UnlinkContainerFromItem.
                    // This prevents aliasing that can arise in recycling mode (DDVSO 405066)
                    FrameworkElement oldContent = Content as FrameworkElement;
                    if (oldContent != null && oldContent != newContent)
                    {
                        ContentPresenter cp = oldContent as ContentPresenter;
                        if (cp == null)
                        {
                            oldContent.SetValue(FrameworkElement.DataContextProperty, BindingExpression.DisconnectedItem);
                        }
                        else
                        {
                            // for a template column, disconnect by setting the
                            // Content, to override the binding set up in
                            // DataGridTemplateColumn.LoadTemplateContent.
                            cp.Content = BindingExpression.DisconnectedItem;
                        }
                    }

                    // hook the visual tree up through the Content property.
                    Content = newContent;
                }
            }
        }
    }
}
