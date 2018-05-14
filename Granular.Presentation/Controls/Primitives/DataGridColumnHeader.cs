
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
    /// <summary>
    /// Container for the Column header object.  This is instantiated by the DataGridColumnHeadersPresenter.
    /// </summary>
    public class DataGridColumnHeader : ButtonBase, IProvideDataGridColumn
    {
        static DataGridColumnHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(new StyleKey(typeof(DataGridColumnHeader))));
            ContentProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceContent));
            ContentTemplateProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, OnNotifyPropertyChanged, OnCoerceContentTemplate));

            FocusableProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(false));
        }

        public DataGridColumnHeader()
        {
        }

        private DataGridColumnHeadersPresenter _parentPresenter;
        internal DataGridColumnHeadersPresenter ParentPresenter
        {
            get
            {
                if (_parentPresenter == null)
                {
                    // _parentPresenter = ItemsControl.ItemsControlFromItemContainer(this) as DataGridColumnHeadersPresenter;
                }

                return _parentPresenter;
            }
        }

        private DataGridColumn _column;
        public DataGridColumn Column
        {
            get { return _column; }
        }

        private Panel ParentPanel
        {
            get
            {
                return VisualParent as Panel;
            }
        }

        /// <summary>
        ///     Notifies the Header of a property change.
        /// </summary>
        private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridColumnHeader)d).NotifyPropertyChanged(d, e);
        }

        /// <summary>
        ///     Coerces the Content property.  We're choosing a value between Column.Header and the Content property on ColumnHeader.
        /// </summary>
        private static object OnCoerceContent(DependencyObject d, object baseValue)
        {
            var header = d as DataGridColumnHeader;

            object content = DataGridHelper.GetCoercedTransferPropertyValue(
                header,
                baseValue,
                ContentProperty,
                header.Column,
                DataGridColumn.HeaderProperty);

            // if content is a WPF element with a logical parent, disconnect it now
            // so that it can be connected to the DGColumnHeader.   This happens during
            // a theme change - see Dev11 146729.
            ////FrameworkObject fo = new FrameworkObject(content as DependencyObject);
            ////if (fo.Parent != null && fo.Parent != header)
            ////{
            ////    fo.ChangeLogicalParent(null);
            ////}
            ///

            return content;
        }

        /// <summary>
        ///     Coerces the ContentTemplate property based on the templates defined on the Column.
        /// </summary>
        private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
        {
            var columnHeader = d as DataGridColumnHeader;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                columnHeader,
                baseValue,
                ContentTemplateProperty,
                columnHeader.Column,
                DataGridColumn.HeaderTemplateProperty);
        }

        /// <summary>
        ///     Notification for column header-related DependencyProperty changes from the grid or from columns.
        /// </summary>
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumn column = d as DataGridColumn;
            if ((column != null) && (column != Column))
            {
                // This notification does not apply to this column header
                return;
            }

            //if (e.Property == DataGridColumn.WidthProperty)
            //{
            //    DataGridHelper.OnColumnWidthChanged(this, e);
            //}
            //else if (e.Property == DataGridColumn.HeaderProperty || e.Property == ContentProperty)
            //{
            //    DataGridHelper.TransferProperty(this, ContentProperty);
            //}
            //else if (e.Property == DataGridColumn.HeaderTemplateProperty || e.Property == ContentTemplateProperty)
            //{
            //    DataGridHelper.TransferProperty(this, ContentTemplateProperty);
            //}
            //else if (e.Property == DataGridColumn.HeaderTemplateSelectorProperty || e.Property == ContentTemplateSelectorProperty)
            //{
            //    DataGridHelper.TransferProperty(this, ContentTemplateSelectorProperty);
            //}
            //else if (e.Property == DataGridColumn.HeaderStringFormatProperty || e.Property == ContentStringFormatProperty)
            //{
            //    DataGridHelper.TransferProperty(this, ContentStringFormatProperty);
            //}
            //else if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGridColumn.HeaderStyleProperty || e.Property == StyleProperty)
            //{
            //    DataGridHelper.TransferProperty(this, StyleProperty);
            //}
            //else if (e.Property == DataGrid.ColumnHeaderHeightProperty || e.Property == HeightProperty)
            //{
            //    DataGridHelper.TransferProperty(this, HeightProperty);
            //}
            //else if (e.Property == DataGridColumn.DisplayIndexProperty)
            //{
            //    CoerceValue(DisplayIndexProperty);
            //    TabIndex = column.DisplayIndex;
            //}
            //else if (e.Property == DataGrid.CanUserResizeColumnsProperty)
            //{
            //    OnCanUserResizeColumnsChanged();
            //}
            //else if (e.Property == DataGridColumn.CanUserSortProperty)
            //{
            //    CoerceValue(CanUserSortProperty);
            //}
            //else if (e.Property == DataGridColumn.SortDirectionProperty)
            //{
            //    CoerceValue(SortDirectionProperty);
            //}
            //else if (e.Property == DataGridColumn.IsFrozenProperty)
            //{
            //    CoerceValue(IsFrozenProperty);
            //}
            //else if (e.Property == DataGridColumn.CanUserResizeProperty)
            //{
            //    OnCanUserResizeChanged();
            //}
            //else if (e.Property == DataGridColumn.VisibilityProperty)
            //{
            //    OnColumnVisibilityChanged(e);
            //}
        }

        #region Column Header Generation

        /// <summary>
        /// Prepares a column header to be used.  Sets up the association between the column header and its column.
        /// </summary>
        internal void PrepareColumnHeader(object item, DataGridColumn column)
        {
            _column = column;
            // TabIndex = column.DisplayIndex;

            DataGridHelper.TransferProperty(this, ContentProperty);
            DataGridHelper.TransferProperty(this, ContentTemplateProperty);
            DataGridHelper.TransferProperty(this, ContentTemplateSelectorProperty);
            // DataGridHelper.TransferProperty(this, ContentStringFormatProperty);
            DataGridHelper.TransferProperty(this, StyleProperty);
            DataGridHelper.TransferProperty(this, HeightProperty);
            // 
            // CoerceValue(CanUserSortProperty);
            // CoerceValue(SortDirectionProperty);
            // CoerceValue(IsFrozenProperty);
            CoerceValue(ClipProperty);
            // CoerceValue(DisplayIndexProperty);
        }

        internal void ClearHeader()
        {
            _column = null;
        }


        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            var baseSize = base.MeasureOverride(availableSize);
            return new Size(Column.ActualWidth, baseSize.Height);
        }
    }
}
