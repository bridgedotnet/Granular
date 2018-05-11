using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static Retyped.dom;

namespace Granular.Host.Render
{
    public abstract class HtmlBrushRenderResource : HtmlRenderResource, IBrushRenderResource
    {
        public string Uri { get; private set; }

        private double opacity;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (opacity == value)
                {
                    return;
                }

                opacity = value;
                OnOpacityChanged();
            }
        }

        private SvgDefinitionContainer svgDefinitionContainer;

        public HtmlBrushRenderResource(string tagName, SvgDefinitionContainer svgDefinitionContainer) :
            base(SvgDocument.CreateElement(tagName))
        {
            this.svgDefinitionContainer = svgDefinitionContainer;

            string elementName = $"{tagName}{svgDefinitionContainer.GetNextId()}";
            this.Uri = $"url(#{elementName})";
            this.HtmlElement.setAttribute("id", elementName);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            svgDefinitionContainer.Add(this);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            svgDefinitionContainer.Remove(this);
        }

        protected virtual void OnOpacityChanged()
        {
            //
        }
    }
}
