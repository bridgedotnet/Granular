using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Granular.Collections;
using Granular.Extensions;

namespace System.Windows.Controls
{
    public class ItemContainerGenerator : IItemContainerGenerator, IDisposable
    {
        private class GeneratedItemContainer
        {
            public FrameworkElement Container { get; set; }
            public object Item { get; set; }
            public int Index { get; set; }

            public GeneratedItemContainer(FrameworkElement container, object item, int index)
            {
                this.Container = container;
                this.Item = item;
                this.Index = index;
            }
        }

        public event EventHandler<ItemsChangedEventArgs> ItemsChanged;

        public int ItemsCount { get { return host.View.Count; } }

        private IGeneratorHost host;
        private List<GeneratedItemContainer> generatedContainers;

        public ItemContainerGenerator(IGeneratorHost host)
        {
            this.host = host;
            this.host.View.CollectionChanged += OnViewCollectionChanged;

            this.generatedContainers = new List<GeneratedItemContainer>();
        }

        public void Dispose()
        {
            host.View.CollectionChanged -= OnViewCollectionChanged;
            RemoveRange(0, ItemsCount);
        }

        private void OnViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int oldItemsCount = e.OldItems.Count();
            int newItemsCount = e.NewItems.Count();
            int oldContainersCount = generatedContainers.Where(container => container.Index >= e.OldStartingIndex && container.Index < e.OldStartingIndex + oldItemsCount).Count();

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ShiftGeneratedContainersIndex(e.NewStartingIndex, newItemsCount);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveRange(e.OldStartingIndex, oldItemsCount);
                ShiftGeneratedContainersIndex(e.OldStartingIndex, -oldItemsCount);
            }

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                SwitchGeneratedContainersIndex(e.OldStartingIndex, e.NewStartingIndex, newItemsCount);
            }

            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                RefershRange(e.NewStartingIndex, newItemsCount);
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RemoveRange(0, ItemsCount);
            }

            ItemsChanged.Raise(this, new ItemsChangedEventArgs(e.Action, e.OldStartingIndex, e.NewStartingIndex, newItemsCount, oldContainersCount));
        }

        public FrameworkElement Generate(int index)
        {
            GeneratedItemContainer container = generatedContainers.FirstOrDefault(c => c.Index == index);

            if (container == null)
            {
                container = new GeneratedItemContainer(host.GetContainerForItem(host.View[index]), host.View[index], index);
                generatedContainers.Add(container);
                host.PrepareContainerForItem(container.Item, container.Container);              
            }

            return container.Container;
        }

        public void RemoveRange(int startIndex, int count)
        {
            int i = 0;

            while (i < generatedContainers.Count)
            {
                GeneratedItemContainer container = generatedContainers[i];

                if (container.Index >= startIndex && container.Index < startIndex + count)
                {
                    Remove(container);
                    continue;
                }

                i++;
            }
        }

        private void Remove(GeneratedItemContainer container)
        {
            generatedContainers.Remove(container);
            host.ClearContainerForItem(container.Item, container.Container);
        }

        private void RefershRange(int startIndex, int count)
        {
            foreach (GeneratedItemContainer container in generatedContainers)
            {
                if (container.Index >= startIndex && container.Index < startIndex + count)
                {
                    Refersh(container);
                }
            }
        }

        private void Refersh(GeneratedItemContainer container)
        {
            host.ClearContainerForItem(container.Item, container.Container);

            container.Item = host.View[container.Index];
            container.Container = host.GetContainerForItem(host.View[container.Index]);

            host.PrepareContainerForItem(container.Item, container.Container);
        }

        public DependencyObject ContainerFromItem(object item)
        {
            GeneratedItemContainer generatedItemContainer = generatedContainers.FirstOrDefault(c => c.Item == item);
            return generatedItemContainer != null ? generatedItemContainer.Container : null;
        }

        public object ItemFromContainer(DependencyObject container)
        {
            GeneratedItemContainer generatedItemContainer = generatedContainers.FirstOrDefault(c => c.Container == container);
            return generatedItemContainer != null ? generatedItemContainer.Item : null;
        }

        public DependencyObject ContainerFromIndex(int index)
        {
            GeneratedItemContainer generatedItemContainer = generatedContainers.ElementAtOrDefault(index);
            return generatedItemContainer != null ? generatedItemContainer.Container : null;
        }

        public int IndexFromContainer(DependencyObject container)
        {
            GeneratedItemContainer generatedItemContainer = generatedContainers.FirstOrDefault(c => c.Container == container);
            return generatedItemContainer != null ? generatedItemContainer.Index : -1;
        }

        private void ShiftGeneratedContainersIndex(int startIndex, int offset)
        {
            foreach (GeneratedItemContainer container in generatedContainers)
            {
                if (container.Index >= startIndex)
                {
                    container.Index += offset;
                }
            }
        }

        private void SwitchGeneratedContainersIndex(int sourceIndex, int targetIndex, int count)
        {
            int offset = targetIndex - sourceIndex;

            foreach (GeneratedItemContainer container in generatedContainers)
            {
                if (container.Index >= sourceIndex && container.Index < sourceIndex + count)
                {
                    container.Index += offset;
                }
                else if (container.Index >= sourceIndex && container.Index < targetIndex ||
                         container.Index >= targetIndex && container.Index < sourceIndex)
                {
                    container.Index -= offset;
                }
            }
        }
    }
}
