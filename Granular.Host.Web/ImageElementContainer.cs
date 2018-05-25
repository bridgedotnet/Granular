using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retyped.dom;

namespace Granular.Host
{
    public class ImageElementContainer
    {
        public HTMLElement HtmlElement { get; private set; }

        public ImageElementContainer()
        {
            HtmlElement = document.createElement("div");
            HtmlElement.style.setProperty("visibility", "hidden");
            HtmlElement.style.setProperty("overflow", "hidden");
            HtmlElement.style.width = "0px";
            HtmlElement.style.height = "0px";
        }

        public void Add(HTMLElement element)
        {
            HtmlElement.appendChild(element);
        }

        public void Remove(HTMLElement element)
        {
            HtmlElement.removeChild(element);
        }
    }
}
