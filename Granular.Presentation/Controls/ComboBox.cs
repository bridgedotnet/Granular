using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Granular.Extensions;

namespace System.Windows.Controls
{
    public class ComboBox : Selector
    {
        private interface ISelectionBehavior
        {
            void SetClickSelection(ComboBoxItem item, ModifierKeys modifiers);
            void SetFocusChangeSelection(ComboBoxItem item, ModifierKeys modifiers);
        }

        private class SingleSelectionBehavior : ISelectionBehavior
        {
            private ComboBox comboBox;

            public SingleSelectionBehavior(ComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            public void SetClickSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                if (modifiers != ModifierKeys.Control)
                {
                    comboBox.SetSingleSelection(item);
                }
                else
                {
                    comboBox.SetSelectionAnchor(item);
                    comboBox.ToggleSingleSelection(item);
                }

                comboBox.IsDropDownOpen = false;
            }

            public void SetFocusChangeSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                if (modifiers != ModifierKeys.Control)
                {
                    comboBox.SetSingleSelection(item);
                }
            }
        }

        private class MultipleSelectionBehavior : ISelectionBehavior
        {
            private ComboBox comboBox;

            public MultipleSelectionBehavior(ComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            public void SetClickSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                comboBox.SetSelectionAnchor(item);
                comboBox.ToggleSelection(item);
            }

            public void SetFocusChangeSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                //
            }
        }

        private class ExtendedSelectionBehavior : ISelectionBehavior
        {
            private ComboBox comboBox;

            public ExtendedSelectionBehavior(ComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            public void SetClickSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                if (modifiers == ModifierKeys.None)
                {
                    comboBox.SetSelectionAnchor(item);
                    comboBox.SetSingleSelection(item);
                }
                else if (modifiers == ModifierKeys.Shift)
                {
                    comboBox.SetRangeSelection(item);
                }
                else if (modifiers == ModifierKeys.Control)
                {
                    comboBox.SetSelectionAnchor(item);
                    comboBox.ToggleSelection(item);
                }
            }

            public void SetFocusChangeSelection(ComboBoxItem item, ModifierKeys modifiers)
            {
                if (modifiers == ModifierKeys.None)
                {
                    comboBox.SetSelectionAnchor(item);
                    comboBox.SetSingleSelection(item);
                }
                else if (modifiers == ModifierKeys.Shift)
                {
                    comboBox.SetRangeSelection(item);
                }
            }
        }

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IEnumerable<object>), typeof(ComboBox), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;
        public IEnumerable<object> SelectedItems
        {
            get { return (IEnumerable<object>)GetValue(SelectedItemsPropertyKey); }
            private set { SetValue(SelectedItemsPropertyKey, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(ComboBox), new FrameworkPropertyMetadata(SelectionMode.Single, (sender, e) => ((ComboBox)sender).SetSelectionBehavior()));
        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }


        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(false,
            (sender, e) =>
            {
                
            }));

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set
            {
                
                SetValue(IsDropDownOpenProperty, value);
            }
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(ComboBox), new FrameworkPropertyMetadata(5000));
        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }


        private ISelectionBehavior selectionBehavior;

        private ComboBoxItem selectionAnchor;

        private bool isItemContainerBeingClicked;

        static ComboBox()
        {
            Control.IsTabStopProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(new StyleKey(typeof(ComboBox))));
        }

        public ComboBox()
        {
            SetSelectionBehavior();
        }

        //public void ScrollIntoView(object item);
        //public void SelectAll();
        //protected bool SetSelectedItems(IEnumerable selectedItems);
        //public void UnselectAll();

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ComboBoxItem;
        }

        protected override FrameworkElement GetContainerForItemOverride()
        {
            return new ComboBoxItem();
        }

        protected override void OnPrepareContainerForItem(object item, FrameworkElement container)
        {
            container.PreviewMouseDown += OnItemContainerPreviewMouseDown; // handled too
            container.MouseDown += OnItemContainerMouseDown;
            container.KeyDown += OnItemContainerKeyDown;
            container.GotKeyboardFocus += OnItemContainerGotKeyboardFocus;
        }

        protected override void OnClearContainerForItem(object item, FrameworkElement container)
        {
            container.PreviewMouseDown -= OnItemContainerPreviewMouseDown; // handled too
            container.MouseDown -= OnItemContainerMouseDown;
            container.KeyDown -= OnItemContainerKeyDown;
            container.GotKeyboardFocus -= OnItemContainerGotKeyboardFocus;
        }

        private void OnItemContainerPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                isItemContainerBeingClicked = true;
            }
        }

        private void OnItemContainerMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                selectionBehavior.SetClickSelection((ComboBoxItem)sender, ApplicationHost.Current.GetKeyboardDeviceFromElement(this).Modifiers);
                isItemContainerBeingClicked = false;
            }
        }

        private void OnItemContainerKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                selectionBehavior.SetClickSelection((ComboBoxItem)sender, e.KeyboardDevice.Modifiers);
            }
        }

        private void OnItemContainerGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!isItemContainerBeingClicked)
            {
                selectionBehavior.SetFocusChangeSelection((ComboBoxItem)sender, e.KeyboardDevice.Modifiers);
            }
        }

        private void SetSelectionBehavior()
        {
            selectionBehavior = CreateSelectionBehavior(this, SelectionMode);
        }

        private static ISelectionBehavior CreateSelectionBehavior(ComboBox comboBox, SelectionMode selectionMode)
        {
            switch (selectionMode)
            {
                case SelectionMode.Single: return new SingleSelectionBehavior(comboBox);
                case SelectionMode.Multiple: return new MultipleSelectionBehavior(comboBox);
                case SelectionMode.Extended: return new ExtendedSelectionBehavior(comboBox);
            }

            throw new Granular.Exception("Unexpected SelectionMode \"{0}\"", selectionMode);
        }

        private void SetSelectionAnchor(ComboBoxItem item)
        {
            selectionAnchor = item;
        }

        private void SetSingleSelection(ComboBoxItem item)
        {
            SelectedItem = ItemContainerGenerator.ItemFromContainer(item);

            for (int i = 0; i < ItemContainerGenerator.ItemsCount; i++)
            {
                DependencyObject itemContainer = ItemContainerGenerator.Generate(i);
                itemContainer.SetCurrentValue(Selector.IsSelectedProperty, itemContainer == item);
            }
        }

        private void SetRangeSelection(ComboBoxItem item)
        {
            int itemIndex = ItemContainerGenerator.IndexFromContainer(item);
            int selectionAnchorIndex = ItemContainerGenerator.IndexFromContainer(selectionAnchor);

            int rangeStartIndex = itemIndex.Min(selectionAnchorIndex);
            int rangeEndIndex = itemIndex.Max(selectionAnchorIndex);

            for (int i = 0; i < ItemContainerGenerator.ItemsCount; i++)
            {
                DependencyObject itemContainer = ItemContainerGenerator.Generate(i);
                itemContainer.SetCurrentValue(Selector.IsSelectedProperty, rangeStartIndex <= i && i <= rangeEndIndex);
            }
        }

        private void ToggleSelection(ComboBoxItem item)
        {
            item.SetCurrentValue(Selector.IsSelectedProperty, !(bool)item.GetValue(Selector.IsSelectedProperty));
        }

        private void ToggleSingleSelection(ComboBoxItem item)
        {
            bool isSelected = !(bool)item.GetValue(Selector.IsSelectedProperty);

            SelectedItem = isSelected ? ItemContainerGenerator.ItemFromContainer(item) : null;

            for (int i = 0; i < ItemContainerGenerator.ItemsCount; i++)
            {
                DependencyObject itemContainer = ItemContainerGenerator.Generate(i);
                itemContainer.SetCurrentValue(Selector.IsSelectedProperty, itemContainer == item && isSelected);
            }
        }
    }
}
