using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Retyped.dom;
using System.Windows.Media;
using Granular.Compatibility.Linq;
using Granular.Extensions;

namespace Granular.Host.Render
{
    public class HtmlContainerRenderElement : HtmlRenderElement, IContainerRenderElement
    {
        private List<IHtmlRenderElement> children;
        public IEnumerable<object> Children { get { return children; } }

        private RenderQueue renderQueue;

        private int lastDeferredChildIndex;
        private int deferredChildrenCount;

        public HtmlContainerRenderElement(HTMLElement htmlElement, RenderQueue renderQueue) :
            base(htmlElement)
        {
            this.renderQueue = renderQueue;

            children = new List<IHtmlRenderElement>();

            lastDeferredChildIndex = -1;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            foreach (IHtmlRenderElement child in Children)
            {
                child.Load();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            foreach (IHtmlRenderElement child in Children)
            {
                child.Unload();
            }
        }

        public void InsertChild(int index, object child)
        {
            IHtmlRenderElement childElement = (IHtmlRenderElement)child;

            if (child is IHtmlDeferredRenderElement)
            {
                ((IHtmlDeferredRenderElement)child).HtmlElementCreated += OnChildHtmlElementCreated;
            }

            if (IsLoaded)
            {
                childElement.Load();
            }

            children.Insert(index, childElement);
            if (childElement.HtmlElement != null)
            {
                if (index > lastDeferredChildIndex)
                {
                    index = index - deferredChildrenCount;
                }
                else
                {
                    lastDeferredChildIndex++;
                    index = children.Take(index).Count(c => c.HtmlElement != null);
                }

                renderQueue.InvokeAsync(() => HtmlElement.InsertChild(index, childElement.HtmlElement));
            }
            else
            {
                lastDeferredChildIndex = lastDeferredChildIndex.Max(index);
                deferredChildrenCount++;
            }
        }

        public void RemoveChild(object child)
        {
            IHtmlRenderElement childElement = (IHtmlRenderElement)child;

            if (child is IHtmlDeferredRenderElement)
            {
                ((IHtmlDeferredRenderElement)child).HtmlElementCreated -= OnChildHtmlElementCreated;
            }

            if (IsLoaded)
            {
                childElement.Unload();
            }

            int childIndex = children.IndexOf(childElement);

            if (childIndex == -1)
            {
                return;
            }

            children.RemoveAt(childIndex);

            if (childElement.HtmlElement != null)
            {
                renderQueue.InvokeAsync(() => HtmlElement.removeChild(((HtmlRenderElement)child).HtmlElement));
            }
            else
            {
                deferredChildrenCount--;
            }
        }

        private void OnChildHtmlElementCreated(object sender, EventArgs e)
        {
            IHtmlRenderElement childElement = (IHtmlRenderElement)sender;
            int index = children.IndexOf(childElement);

            index = children.Take(index).Count(c => c.HtmlElement != null);
            renderQueue.InvokeAsync(() => HtmlElement.InsertChild(index, childElement.HtmlElement));

            deferredChildrenCount--;
        }
    }
}
