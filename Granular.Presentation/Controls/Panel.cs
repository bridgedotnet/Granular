using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using Granular.Collections;
using Granular.Compatibility.Linq;
using Granular.Extensions;

namespace System.Windows.Controls
{
    [ContentProperty("Children")]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public abstract class Panel : FrameworkElement
    {
        public UIElementCollection Children { get; private set; }

        public static readonly DependencyProperty IsItemsHostProperty = DependencyProperty.Register("IsItemsHost", typeof(bool), typeof(Panel), new FrameworkPropertyMetadata(false, (sender, e) => ((Panel)sender).OnIsItemsHostChanged(sender, e)));
        public bool IsItemsHost
        {
            get { return (bool)GetValue(IsItemsHostProperty); }
            set { SetValue(IsItemsHostProperty, value); }
        }

        private IItemContainerGenerator itemContainerGenerator;
        public IItemContainerGenerator ItemContainerGenerator
        {
            get { return itemContainerGenerator; }
            set
            {
                if (itemContainerGenerator == value)
                {
                    return;
                }

                if (itemContainerGenerator != null)
                {
                    itemContainerGenerator.ItemsChanged -= OnGeneratorItemsChanged;
                    Children.Clear();
                }

                itemContainerGenerator = value;

                if (itemContainerGenerator != null)
                {
                    itemContainerGenerator.ItemsChanged += OnGeneratorItemsChanged;
                    AddChildren();
                }
            }
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(Panel), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Panel)sender).OnBackgroundChanged(e)));
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.RegisterAttached("ZIndex", typeof(int), typeof(Panel), new FrameworkPropertyMetadata(propertyChangedCallback: OnZIndexPropertyChanged));

        public static int GetZIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(ZIndexProperty);
        }

        public static void SetZIndex(DependencyObject obj, int value)
        {
            obj.SetValue(ZIndexProperty, value);
        }

        public Panel()
        {
            Children = new UIElementCollection(this);
            Children.CollectionChanged += OnChildrenCollectionChanged;
        }

        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (UIElement child in e.NewItems)
            {
                MoveVisualChild(child, GetZIndex(child));
            }
        }

        protected abstract override Size MeasureOverride(Size availableSize);

        protected abstract override Size ArrangeOverride(Size finalSize);

        protected override bool HitTestOverride(Point position)
        {
            return Background != null && VisualSize.Contains(position);
        }

        protected virtual void OnGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.ItemsCount; i++)
                {
                    Children.Insert(i + e.NewStartingIndex, ItemContainerGenerator.Generate(i + e.NewStartingIndex));
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Children.RemoveRange(e.OldStartingIndex, e.ContainersCount);
            }

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                IEnumerable<UIElement> movedChildren = Children.Skip(e.OldStartingIndex).Take(e.ContainersCount);

                Children.RemoveRange(e.OldStartingIndex, e.ContainersCount);
                Children.InsertRange(e.NewStartingIndex, movedChildren);
            }

            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                Children[e.NewStartingIndex] = ItemContainerGenerator.Generate(e.NewStartingIndex);
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
                AddChildren();
            }
        }

        private void AddChildren()
        {
            for (int i = 0; i < ItemContainerGenerator.ItemsCount; i++)
            {
                Children.Add(ItemContainerGenerator.Generate(i));
            }
        }

        private void OnIsItemsHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemContainerGenerator = IsItemsHost && TemplatedParent is ItemsControl ? ((ItemsControl)TemplatedParent).ItemContainerGenerator : null;

            Panel panel = (Panel)d;

            panel.OnIsItemsHostChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the IsItemsHost property changes.
        /// </summary>
        /// <param name="oldIsItemsHost">The old value of the IsItemsHost property.</param>
        /// <param name="newIsItemsHost">The new value of the IsItemsHost property.</param>
        protected virtual void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {

        }

        private void OnBackgroundChanged(DependencyPropertyChangedEventArgs e)
        {
            VisualBackground = Background;
        }

        private static void OnZIndexPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Visual && ((Visual)dependencyObject).VisualParent is Panel)
            {
                MoveVisualChild((Visual)dependencyObject, (int)e.NewValue);
            }
        }

        private static void MoveVisualChild(Visual child, int childZIndex)
        {
            int childVisualIndex = child.VisualParent.VisualChildren.Count(visual => visual != child && GetZIndex(visual) <= childZIndex);
            child.VisualParent.SetVisualChildIndex(child, childVisualIndex);
        }
    }
}
