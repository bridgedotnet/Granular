using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Windows.Controls
{
    /// <summary>
    ///     Helper code for DataGrid.
    /// </summary>
    internal static class DataGridHelper
    {
        #region Notification Propagation

        public static bool ShouldNotifyCells(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Cells);
        }

        public static bool ShouldNotifyCellsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.CellsPresenter);
        }

        public static bool ShouldNotifyColumns(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Columns);
        }

        public static bool ShouldNotifyColumnHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeaders);
        }

        public static bool ShouldNotifyColumnHeadersPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeadersPresenter);
        }

        public static bool ShouldNotifyColumnCollection(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnCollection);
        }

        public static bool ShouldNotifyDataGrid(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DataGrid);
        }

        public static bool ShouldNotifyDetailsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DetailsPresenter);
        }

        public static bool ShouldRefreshCellContent(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RefreshCellContent);
        }

        public static bool ShouldNotifyRowHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RowHeaders);
        }

        public static bool ShouldNotifyRows(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Rows);
        }

        public static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
        {
            DataGridNotificationTarget value =
                DataGridNotificationTarget.Rows |
                DataGridNotificationTarget.RowHeaders |
                DataGridNotificationTarget.CellsPresenter |
                DataGridNotificationTarget.Cells |
                DataGridNotificationTarget.RefreshCellContent |
                DataGridNotificationTarget.DetailsPresenter;

            return TestTarget(target, value);
        }

        private static bool TestTarget(DataGridNotificationTarget target, DataGridNotificationTarget value)
        {
            return (target & value) != 0;
        }

        #endregion

        #region Tree Helpers

        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }


        #endregion

        #region  Property Helpers

        /// <summary>
        ///     Causes the given DependencyProperty to be coerced in transfer mode.
        /// </summary>
        /// <remarks>
        ///     This should be called from within the target object's NotifyPropertyChanged.  It MUST be called in
        ///     response to a change in the target property.
        /// </remarks>
        /// <param name="d">The DependencyObject which contains the property that needs to be transfered.</param>
        /// <param name="p">The DependencyProperty that is the target of the property transfer.</param>
        public static void TransferProperty(DependencyObject d, DependencyProperty p)
        {
            var transferEnabledMap = GetPropertyTransferEnabledMapForObject(d);
            transferEnabledMap[p] = true;
            d.CoerceValue(p);
            transferEnabledMap[p] = false;
        }

        /// <summary>
        ///     Tracks which properties are currently being transfered.  This information is needed when GetPropertyTransferEnabledMapForObject
        ///     is called inside of Coercion.
        /// </summary>
        private static Dictionary<DependencyObject, Dictionary<DependencyProperty, bool>> _propertyTransferEnabledMap = new Dictionary<DependencyObject, Dictionary<DependencyProperty, bool>>();


        private static Dictionary<DependencyProperty, bool> GetPropertyTransferEnabledMapForObject(DependencyObject d)
        {
            Dictionary<DependencyProperty, bool> propertyTransferEnabledForObject;

            if (!_propertyTransferEnabledMap.TryGetValue(d, out propertyTransferEnabledForObject))
            {
                propertyTransferEnabledForObject = new Dictionary<DependencyProperty, bool>();
                _propertyTransferEnabledMap.Add(d, propertyTransferEnabledForObject);
            }

            return propertyTransferEnabledForObject;
        }

        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject,
            object baseValue,
            DependencyProperty baseProperty,
            DependencyObject parentObject,
            DependencyProperty parentProperty)
        {
            return GetCoercedTransferPropertyValue(
                baseObject,
                baseValue,
                baseProperty,
                parentObject,
                parentProperty,
                null,
                null);
        }

        /// <summary>
        ///     Computes the value of a given property based on the DataGrid property transfer rules.
        /// </summary>
        /// <remarks>
        ///     This is intended to be called from within the coercion of the baseProperty.
        /// </remarks>
        /// <param name="baseObject">The target object which recieves the transferred property</param>
        /// <param name="baseValue">The baseValue that was passed into the coercion delegate</param>
        /// <param name="baseProperty">The property that is being coerced</param>
        /// <param name="parentObject">The object that contains the parentProperty</param>
        /// <param name="parentProperty">A property who's value should be transfered (via coercion) to the baseObject if it has a higher precedence.</param>
        /// <param name="grandParentObject">Same as parentObject but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <param name="grandParentProperty">Same as parentProperty but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <returns></returns>
        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject,
            object baseValue,
            DependencyProperty baseProperty,
            DependencyObject parentObject,
            DependencyProperty parentProperty,
            DependencyObject grandParentObject,
            DependencyProperty grandParentProperty)
        {
            // Transfer Property Coercion rules:
            //
            // Determine if this is a 'Transfer Property Coercion'.  If so:
            //   We can safely get the BaseValueSource because the property change originated from another
            //   property, and thus this BaseValueSource wont be stale.
            //   Pick a value to use based on who has the greatest BaseValueSource
            // If not a 'Transfer Property Coercion', simply return baseValue.  This will cause a property change if the value changes, which
            // will trigger a 'Transfer Property Coercion', and we will no longer have a stale BaseValueSource
            var coercedValue = baseValue;

            if (IsPropertyTransferEnabled(baseObject, baseProperty))
            {
                var propertySource = baseObject.GetValueSource(baseProperty);
                var maxBaseValueSource = propertySource.BaseValueSource;

                if (parentObject != null)
                {                  
                    var parentPropertySource = parentObject.GetValueSource(parentProperty);

                    if (parentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = parentObject.GetValue(parentProperty);
                        maxBaseValueSource = parentPropertySource.BaseValueSource;
                    }
                }

                if (grandParentObject != null)
                {
                    var grandParentPropertySource = grandParentObject.GetValueSource(grandParentProperty);

                    if (grandParentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = grandParentObject.GetValue(grandParentProperty);
                        maxBaseValueSource = grandParentPropertySource.BaseValueSource;
                    }
                }
            }

            return coercedValue;
        }


        internal static bool IsPropertyTransferEnabled(DependencyObject d, DependencyProperty p)
        {
            Dictionary<DependencyProperty, bool> propertyTransferEnabledForObject;

            if (_propertyTransferEnabledMap.TryGetValue(d, out propertyTransferEnabledForObject))
            {
                bool isPropertyTransferEnabled;
                if (propertyTransferEnabledForObject.TryGetValue(p, out isPropertyTransferEnabled))
                {
                    return isPropertyTransferEnabled;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        ///     Helper method which coerces a value such that it satisfies min and max restrictions
        /// </summary>
        public static double CoerceToMinMax(double value, double minValue, double maxValue)
        {
            value = Math.Max(value, minValue);
            value = Math.Min(value, maxValue);
            return value;
        }

        internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
        {
            if (IsDefaultValue(column, columnProperty))
            {
                content.ClearValue(contentProperty);
            }
            else
            {
                content.SetValue(contentProperty, column.GetValue(columnProperty));
            }
        }

        public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return d.GetValueSource(dp).BaseValueSource == BaseValueSource.Default;
        }
    }
}
