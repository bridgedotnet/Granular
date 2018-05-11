using System;
using System.Collections.Generic;
using static Retyped.dom;
using System.Linq;
using System.Text;
using Granular.Extensions;

namespace Granular.Host.Render
{
    public interface IHtmlRenderElement
    {
        HTMLElement HtmlElement { get; }
        void Load();
        void Unload();
    }

    public interface IHtmlDeferredRenderElement : IHtmlRenderElement
    {
        event EventHandler HtmlElementCreated;
    }

    public class HtmlRenderElement : IHtmlRenderElement
    {
        public HTMLElement HtmlElement { get; private set; }
        public bool IsLoaded { get; private set; }

        public HtmlRenderElement() :
            this(document.createElement("div"))
        {
            //
        }

        public HtmlRenderElement(HTMLElement htmlElement)
        {
            this.HtmlElement = htmlElement;
        }

        public void Load()
        {
            if (IsLoaded)
            {
                return;
            }

            IsLoaded = true;
            OnLoad();
        }

        public void Unload()
        {
            if (!IsLoaded)
            {
                return;
            }

            IsLoaded = false;
            OnUnload();
        }

        protected virtual void OnLoad()
        {
            //
        }

        protected virtual void OnUnload()
        {
            //
        }
    }
}
