using System;
using System.Collections.Generic;
using System.Windows.Media;
using static Retyped.dom;
using Granular.Compatibility.Linq;

namespace Granular.Host.Render
{
    public class HtmlGradientBrushRenderResource : HtmlBrushRenderResource, IGradientBrushRenderResource
    {
        private IEnumerable<RenderGradientStop> gradientStops;
        public IEnumerable<RenderGradientStop> GradientStops
        {
            get { return gradientStops; }
            set
            {
                if (gradientStops == value)
                {
                    return;
                }

                gradientStops = value;
                renderQueue.InvokeAsync(SetGradientStops);
            }
        }

        private GradientSpreadMethod spreadMethod;
        public GradientSpreadMethod SpreadMethod
        {
            get { return spreadMethod; }
            set
            {
                if (spreadMethod == value)
                {
                    return;
                }

                spreadMethod = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgSpreadMethod(spreadMethod, converter));
            }
        }

        private BrushMappingMode mappingMode = BrushMappingMode.RelativeToBoundingBox;
        public BrushMappingMode MappingMode
        {
            get { return mappingMode; }
            set
            {
                if (mappingMode == value)
                {
                    return;
                }

                mappingMode = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgMappingMode(mappingMode, converter));
            }
        }

        private RenderQueue renderQueue;
        private SvgValueConverter converter;

        public HtmlGradientBrushRenderResource(string tagName, RenderQueue renderQueue, SvgValueConverter converter, SvgDefinitionContainer svgDefinitionContainer) :
            base(tagName, svgDefinitionContainer)
        {
            this.renderQueue = renderQueue;
            this.converter = converter;
        }

        protected override void OnOpacityChanged()
        {
            renderQueue.InvokeAsync(SetGradientStops);
        }

        private void SetGradientStops()
        {
            RenderGradientStop[] gradientStops = GradientStops.ToArray();

            HTMLElement htmlElement = HtmlElement;

            while (htmlElement.childNodes.length > gradientStops.Length)
            {
                htmlElement.removeChild(htmlElement.lastElementChild);
            }

            while (htmlElement.childNodes.length < gradientStops.Length)
            {
                htmlElement.appendChild(SvgDocument.CreateElement("stop"));
            }

            for (int i = 0; i < gradientStops.Length; i++)
            {
                Element stopElement = (Element)htmlElement.childNodes[i];
                stopElement.setAttribute("stop-color", converter.ToColorString(gradientStops[i].Color));
                stopElement.setAttribute("stop-opacity", converter.ToImplicitValueString(Opacity * gradientStops[i].Color.A / 255));
                stopElement.setAttribute("offset", converter.ToImplicitValueString(gradientStops[i].Offset));
            }
        }
    }
}
