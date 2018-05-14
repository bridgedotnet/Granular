using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Granular.Collections;

namespace Granular.Presentation.Tests.Controls
{
    [TestClass]
    public class ItemContainerGeneratorTest
    {
        private class TestGeneratorHost : IGeneratorHost
        {
            public static readonly DependencyProperty ItemForItemContainerProperty = DependencyProperty.RegisterAttached("ItemForItemContainer", typeof(object), typeof(TestGeneratorHost), new FrameworkPropertyMetadata());

            public ItemCollection View { get; private set; }

            public Dictionary<object, FrameworkElement> Containers { get; private set; }

            public TestGeneratorHost()
            {
                View = new ItemCollection();
                Containers = new Dictionary<object, FrameworkElement>();
            }

            public FrameworkElement GetContainerForItem(object item)
            {
                Assert.IsFalse(Containers.ContainsKey(item));

                FrameworkElement container = new FrameworkElement();
                Containers[item] = container;
                return container;
            }

            public void ClearContainerForItem(object item, FrameworkElement container)
            {
                container.ClearValue(ItemForItemContainerProperty);
                Containers.Remove(item);
            }

            public bool IsItemItsOwnContainer(object item)
            {
                throw new NotImplementedException();
            }

            public void PrepareContainerForItem(object item, FrameworkElement container)
            {
                container.SetValue(ItemForItemContainerProperty, item);
            }
        }

        [TestMethod]
        public void ItemContainerGeneratorBasicTest()
        {
            TestGeneratorHost host = new TestGeneratorHost();
            ItemContainerGenerator generator = new ItemContainerGenerator(host);

            ItemsChangedEventArgs lastChangedArg = null;

            generator.ItemsChanged += (sender, e) => lastChangedArg = e;

            host.View.Add("item1");
            Assert.AreEqual(NotifyCollectionChangedAction.Add, lastChangedArg.Action);
            Assert.AreEqual(0, lastChangedArg.NewStartingIndex);
            Assert.AreEqual(1, lastChangedArg.ItemsCount);
            Assert.AreEqual(0, lastChangedArg.ContainersCount);

            FrameworkElement container1 = generator.Generate(0);
            Assert.IsTrue(host.Containers.ContainsKey("item1"));
            Assert.AreEqual("item1", container1.GetValue(TestGeneratorHost.ItemForItemContainerProperty));

            host.View[0] = "item2";
            Assert.IsFalse(host.Containers.ContainsKey("item1"));
            Assert.IsTrue(host.Containers.ContainsKey("item2"));
            Assert.IsNull(container1.GetValue(TestGeneratorHost.ItemForItemContainerProperty));

            FrameworkElement container2 = generator.Generate(0);
            Assert.IsTrue(host.Containers.ContainsKey("item2"));
            Assert.AreEqual("item2", container2.GetValue(TestGeneratorHost.ItemForItemContainerProperty));

            host.View.Remove("item2");
            Assert.IsFalse(host.Containers.ContainsKey("item2"));
            Assert.IsNull(container2.GetValue(TestGeneratorHost.ItemForItemContainerProperty));

            host.View.Add("item3");
            Assert.IsFalse(host.Containers.ContainsKey("item3"));

            FrameworkElement container3 = generator.Generate(0);
            Assert.IsTrue(host.Containers.ContainsKey("item3"));
            Assert.AreEqual("item3", container3.GetValue(TestGeneratorHost.ItemForItemContainerProperty));

            generator.Dispose();
            Assert.IsFalse(host.Containers.ContainsKey("item3"));
            Assert.IsNull(container3.GetValue(TestGeneratorHost.ItemForItemContainerProperty));
        }
    }
}
