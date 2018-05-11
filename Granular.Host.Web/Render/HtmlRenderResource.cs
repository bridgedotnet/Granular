using static Retyped.dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Granular.Host.Render
{
    public class HtmlRenderResource
    {
        public HTMLElement HtmlElement { get; private set; }

        public bool IsLoaded { get { return referenceCount > 0; } }

        private int referenceCount;

        public HtmlRenderResource(HTMLElement htmlElement)
        {
            this.HtmlElement = htmlElement;
        }

        public void Load()
        {
            referenceCount++;

            if (referenceCount == 1)
            {
                OnLoad();
            }
        }

        public void Unload()
        {
            referenceCount--;

            if (referenceCount == 0)
            {
                OnUnload();
            }
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
