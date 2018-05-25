using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace System.Windows.Controls
{
    /// <summary>
    ///     A base class for specifying column definitions for certain standard
    ///     types that do not allow arbitrary templates.
    /// </summary>
    public abstract class DataGridBoundColumn : DataGridColumn
    {
        #region Constructors

        static DataGridBoundColumn()
        {
        }

        #endregion

        #region Styling

        /// <summary>
        ///     A style that is applied to the generated element when not editing.
        ///     The TargetType of the style depends on the derived column class.
        /// </summary>
        public Style ElementStyle
        {
            get { return (Style)GetValue(ElementStyleProperty); }
            set { SetValue(ElementStyleProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the ElementStyle property.
        /// </summary>
        public static readonly DependencyProperty ElementStyleProperty =
            DependencyProperty.Register(
                "ElementStyle",
                typeof(Style),
                typeof(DataGridBoundColumn),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

        /// <summary>
        ///     A style that is applied to the generated element when editing.
        ///     The TargetType of the style depends on the derived column class.
        /// </summary>
        public Style EditingElementStyle
        {
            get { return (Style)GetValue(EditingElementStyleProperty); }
            set { SetValue(EditingElementStyleProperty, value); }
        }

        /// <summary>
        ///     The DependencyProperty for the EditingElementStyle property.
        /// </summary>
        public static readonly DependencyProperty EditingElementStyleProperty =
            DependencyProperty.Register(
                "EditingElementStyle",
                typeof(Style),
                typeof(DataGridBoundColumn),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

        /// <summary>
        ///     Assigns the ElementStyle to the desired property on the given element.
        /// </summary>
        internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }

        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style style = isEditing ? EditingElementStyle : ElementStyle;
            if (isEditing && defaultToElementStyle && (style == null))
            {
                style = ElementStyle;
            }

            return style;
        }

        #endregion

        private Binding _binding;

        /// <summary>
        ///     The binding that will be applied to the generated element.
        /// </summary>
        /// <remarks>
        ///     This isn't a DP because if it were getting the value would evaluate the binding.
        /// </remarks>
        public virtual Binding Binding
        {
            get
            {
                return _binding;
            }

            set
            {
                if (_binding != value)
                {
                    Binding oldBinding = _binding;
                    _binding = value;
                    CoerceValue(IsReadOnlyProperty);
                    //CoerceValue(SortMemberPathProperty);
                    OnBindingChanged(oldBinding, _binding);
                }
            }
        }

        /// <summary>
        ///     Called when Binding changes.
        /// </summary>
        /// <remarks>
        ///     Default implementation notifies the DataGrid and its subtree about the change.
        /// </remarks>
        /// <param name="oldBinding">The old binding.</param>
        /// <param name="newBinding">The new binding.</param>
        protected virtual void OnBindingChanged(Binding oldBinding, Binding newBinding)
        {
            NotifyPropertyChanged("Binding");
        }

        /// <summary>
        ///     Assigns the Binding to the desired property on the target object.
        /// </summary>
        internal void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            Binding binding = Binding;
            if (binding != null)
            {
                // IExpression bindExpr = binding.CreateExpression(target, property);
                target.SetValue(property, binding);
            }
            else
            {
                target.ClearValue(property);
            }
        }
    }
}
