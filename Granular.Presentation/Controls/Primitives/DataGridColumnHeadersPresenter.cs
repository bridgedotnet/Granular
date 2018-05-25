using System.Windows.Markup;

namespace System.Windows.Controls.Primitives
{
    class DataGridColumnHeadersPresenter : ItemsControl
    {
        static DataGridColumnHeadersPresenter()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnHeadersPresenter), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridColumnHeadersPresenter))));
            FocusableProperty.OverrideMetadata(typeof(DataGridColumnHeadersPresenter), new FrameworkPropertyMetadata(false));

            ItemsPanelProperty.OverrideMetadata(typeof(DataGridColumnHeadersPresenter), new FrameworkPropertyMetadata(new ItemsPanelTemplate
            {
                FrameworkElementFactory = new FrameworkElementFactory(new ElementFactory(typeof(DataGridCellsPanel), ElementInitializer.Empty), new InitializeContext(XamlNamespaces.Empty))
            }));
        }

        public DataGridColumnHeadersPresenter()
        {
        }

        private DataGrid _parentDataGrid = null;
        internal DataGrid ParentDataGrid
        {
            get
            {
                if (_parentDataGrid == null)
                {
                    _parentDataGrid = DataGridHelper.FindParent<DataGrid>(this);
                }

                return _parentDataGrid;
            }
        }

        private DataGridColumnHeaderCollection HeaderCollection
        {
            get
            {
                return ItemsSource as DataGridColumnHeaderCollection;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Find the columns collection and set the ItemsSource.
            DataGrid grid = ParentDataGrid;

            if (grid != null)
            {
                ItemsSource = new DataGridColumnHeaderCollection(grid.Columns);
                grid.ColumnHeadersPresenter = this;
                //DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);

                //DataGridColumnHeader fillerColumnHeader = GetTemplateChild(ElementFillerColumnHeader) as DataGridColumnHeader;
                //if (fillerColumnHeader != null)
                //{
                //    DataGridHelper.TransferProperty(fillerColumnHeader, DataGridColumnHeader.StyleProperty);
                //    DataGridHelper.TransferProperty(fillerColumnHeader, DataGridColumnHeader.HeightProperty);
                //}
            }
            else
            {
                ItemsSource = null;
            }
        }

        #region Column Header Generation

        protected override FrameworkElement GetContainerForItemOverride()
        {
            return new DataGridColumnHeader();
        }

        /// <summary>
        ///     Determines if an item is its own container.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>true if the item is a DataGridColumnHeader, false otherwise.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DataGridColumnHeader;
        }

        /// <summary>
        ///     Method which returns the result of IsItemItsOwnContainerOverride to be used internally
        /// </summary>
        internal bool IsItemItsOwnContainerInternal(object item)
        {
            return IsItemItsOwnContainerOverride(item);
        }

        protected override void PrepareContainerForItemOverride(object item, FrameworkElement container)
        {
            DataGridColumnHeader header = container as DataGridColumnHeader;

            if (header != null)
            {
                DataGridColumn column = ColumnFromContainer(header);

                if (header.Column == null)
                {
                    // A null column means this is a fresh container.  PrepareContainer will also be called simply if the column's
                    // Header property has changed and this container needs to be given a new item.  In that case it'll already be tracked.
                    ////header.Tracker.Debug_AssertNotInList(_headerTrackingRoot);
                    ////header.Tracker.StartTracking(ref _headerTrackingRoot);
                }

                ////header.Tracker.Debug_AssertIsInList(_headerTrackingRoot);

                header.PrepareColumnHeader(item, column);
            }
        }

        protected override void ClearContainerForItemOverride(object item, FrameworkElement container)
        {
            DataGridColumnHeader header = container as DataGridColumnHeader;

            base.ClearContainerForItemOverride(item, container);

            if (header != null)
            {
                ////header.Tracker.StopTracking(ref _headerTrackingRoot);
                header.ClearHeader();
            }
        }

        private DataGridColumn ColumnFromContainer(DataGridColumnHeader container)
        {
            int index = ItemContainerGenerator.IndexFromContainer(container);
            return HeaderCollection.ColumnFromIndex(index);
        }

        #endregion

        #region Layout

        protected override Size MeasureOverride(Size availableSize)
        {
            Size childConstraint = new Size(Double.PositiveInfinity, availableSize.Height);
            var desiredSize = base.MeasureOverride(childConstraint);

            desiredSize = new Size(Math.Min(availableSize.Width, desiredSize.Width), desiredSize.Height);

            return desiredSize;
        }

        #endregion
    }
}
