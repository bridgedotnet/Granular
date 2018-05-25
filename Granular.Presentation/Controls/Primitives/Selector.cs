using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls.Primitives
{
    public abstract class Selector : ItemsControl
    {
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(Selector));
        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
        public event RoutedEventHandler Unselected
        {
            add { AddHandler(UnselectedEvent, value); }
            remove { RemoveHandler(UnselectedEvent, value); }
        }

        public static readonly DependencyProperty IsSynchronizedWithCurrentItemProperty = DependencyProperty.Register("IsSynchronizedWithCurrentItem", typeof(bool?), typeof(Selector), new FrameworkPropertyMetadata());
        public bool? IsSynchronizedWithCurrentItem
        {
            get { return (bool?)GetValue(IsSynchronizedWithCurrentItemProperty); }
            set { SetValue(IsSynchronizedWithCurrentItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Selector), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Selector)sender).OnSelectedIndexChanged(e)));
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Selector)sender).OnSelectedItemChanged(e)));
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Selector)sender).OnSelectedValueChanged(e)));
        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(Selector), new FrameworkPropertyMetadata());
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata());
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.Inherits));
        public static readonly DependencyProperty IsSelectionActiveProperty = IsSelectionActivePropertyKey.DependencyProperty;

        public static bool GetIsSelectionActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectionActiveProperty);
        }

        private static void SetIsSelectionActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectionActivePropertyKey, value);
        }

        public Selector()
        {
            SetIsSelectionActive(this, false);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            SetIsSelectionActive(this, true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            SetIsSelectionActive(this, false);
        }

        private void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            //
        }


        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ItemContainerGenerator.ContainerFromItem(e.OldValue).SetCurrentValue(Selector.IsSelectedProperty, false);
            }

            if (e.NewValue != null)
            {
                ItemContainerGenerator.ContainerFromItem(e.NewValue).SetCurrentValue(Selector.IsSelectedProperty, true);
            }
        }

        private void OnSelectedValueChanged(DependencyPropertyChangedEventArgs e)
        {
            //
        }
    }
}
