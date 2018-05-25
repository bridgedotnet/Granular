using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Controls.Primitives
{
    public class DataGridRowsPresenter : StackPanel
    {
        public DataGridRowsPresenter()
        {
        }


        /// <summary>
        ///     This method is invoked when the IsItemsHost property changes.
        /// </summary>
        /// <param name="oldIsItemsHost">The old value of the IsItemsHost property.</param>
        /// <param name="newIsItemsHost">The new value of the IsItemsHost property.</param>
        protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {
            base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);

            if (newIsItemsHost)
            {
                DataGrid dataGrid = Owner;
                if (dataGrid != null)
                {
                    // ItemsHost should be the "root" element which has
                    // IsItemsHost = true on it.  In the case of grouping,
                    // IsItemsHost is true on all panels which are generating
                    // content.  Thus, we care only about the panel which
                    // is generating content for the ItemsControl.
                    IItemContainerGenerator generator = dataGrid.ItemContainerGenerator as IItemContainerGenerator;
                    if (generator != null && generator == generator.GetItemContainerGeneratorForPanel(this))
                    {
                        dataGrid.InternalItemsHost = this;
                    }
                }
            }
            else
            {
                // No longer the items host, clear out the property on the DataGrid
                if ((_owner != null) && (_owner.InternalItemsHost == this))
                {
                    _owner.InternalItemsHost = null;
                }

                _owner = null;
            }
        }

        #region Helpers

        private DataGrid _owner;
        internal DataGrid Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = ItemsControl.GetItemsOwner(this) as DataGrid;
                }

                return _owner;
            }
        }

        #endregion
    }
}
