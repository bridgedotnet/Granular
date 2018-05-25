using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Granular.Compatibility.Linq;
using Granular.Extensions;
using static Retyped.dom;

namespace Granular.Host.Render
{
    public class HtmlDrawingRenderElement : IHtmlDeferredRenderElement, IContainerRenderElement
    {
        public event EventHandler HtmlElementCreated;
        public HTMLElement HtmlElement { get { return container?.HtmlElement; } }

        public IEnumerable<object> Children { get { return container != null ? container.Children : new object[0]; } }

        private HtmlContainerRenderElement container;
        private RenderQueue renderQueue;
        private SvgValueConverter converter;
        private bool isLoaded;

        public HtmlDrawingRenderElement(RenderQueue renderQueue, SvgValueConverter converter)
        {
            this.renderQueue = renderQueue;
            this.converter = converter;
        }

        public void Load()
        {
            isLoaded = true;

            if (container != null)
            {
                container.Load();
            }
        }

        public void Unload()
        {
            isLoaded = false;

            if (container != null)
            {
                container.Unload();
            }
        }

        public void InsertChild(int index, object child)
        {
            if (container == null)
            {
                HTMLElement element = SvgDocument.CreateElement("svg");
                element.setAttribute("overflow", "visible");

                container = new HtmlContainerRenderElement(element, renderQueue);

                if (isLoaded)
                {
                    container.Load();
                }

                HtmlElementCreated.Raise(this);
            }

            container.InsertChild(index, child);
        }

        public void RemoveChild(object child)
        {
            if (container != null)
            {
                container.RemoveChild(child);
            }
        }
    }
}
