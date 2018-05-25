using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Granular.Collections;
using Granular.Extensions;

namespace System.Windows.Controls.Primitives
{
    public interface IItemContainerGenerator
    {
        event EventHandler<ItemsChangedEventArgs> ItemsChanged;
        int ItemsCount { get; }
        FrameworkElement Generate(int index);
        void RemoveRange(int startIndex, int count);
        /// <summary>
        /// Return the ItemContainerGenerator appropriate for use by the given panel
        /// </summary>
        IItemContainerGenerator GetItemContainerGeneratorForPanel(Panel panel);
        DependencyObject ContainerFromItem(object item);
        object ItemFromContainer(DependencyObject container);
        DependencyObject ContainerFromIndex(int index);
        int IndexFromContainer(DependencyObject container);
    }

    public static class ItemContainerGeneratorExtensions
    {
        public static void GenerateRange(this IItemContainerGenerator generator, int startIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                generator.Generate(startIndex + i);
            }
        }

        public static void Remove(this IItemContainerGenerator generator, int index)
        {
            generator.RemoveRange(index, 1);
        }
    }

    public interface IGeneratorHost
    {
        ItemCollection View { get; }

        FrameworkElement GetContainerForItem(object item);
        void PrepareContainerForItem(object item, FrameworkElement container);
        void ClearContainerForItem(object item, FrameworkElement container);
        /// <summary>
        /// Return true if the item is (or should be) its own item container
        /// </summary>
        bool IsItemItsOwnContainer(object item);
    }

    public class ItemsChangedEventArgs : EventArgs
    {
        public NotifyCollectionChangedAction Action { get; private set; }
        public int OldStartingIndex { get; private set; }
        public int NewStartingIndex { get; private set; }
        public int ItemsCount { get; private set; }
        public int ContainersCount { get; private set; }

        public ItemsChangedEventArgs(NotifyCollectionChangedAction action, int oldStartingIndex, int newStartingIndex, int itemsCount, int containersCount)
        {
            this.Action = action;
            this.OldStartingIndex = oldStartingIndex;
            this.NewStartingIndex = newStartingIndex;
            this.ItemsCount = itemsCount;
            this.ContainersCount = containersCount;
        }
    }
}
