using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retyped.dom;
using Granular.Host.Render;

namespace Granular.Host
{
    public class SvgDefinitionContainer
    {
        public HTMLElement HtmlElement { get; private set; }

        private RenderQueue renderQueue;
        private HTMLElement definitionsElement;
        private int id;

        public SvgDefinitionContainer(RenderQueue renderQueue)
        {
            this.renderQueue = renderQueue;

            HtmlElement = SvgDocument.CreateElement("svg");
            HtmlElement.style.setProperty("overflow", "hidden");
            HtmlElement.style.width = "0px";
            HtmlElement.style.height = "0px";

            definitionsElement = SvgDocument.CreateElement("defs");
            HtmlElement.appendChild(definitionsElement);
        }

        public int GetNextId()
        {
            id++;
            return id;
        }

        public void Add(HtmlRenderResource svgDefinition)
        {
            renderQueue.InvokeAsync(() => definitionsElement.appendChild(svgDefinition.HtmlElement));
        }

        public void Remove(HtmlRenderResource svgDefinition)
        {
            renderQueue.InvokeAsync(() => definitionsElement.removeChild(svgDefinition.HtmlElement));
        }
    }
}
