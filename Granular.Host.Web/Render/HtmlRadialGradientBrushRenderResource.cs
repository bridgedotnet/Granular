using System;
using System.Windows;
using System.Windows.Media;

namespace Granular.Host.Render
{
    public class HtmlRadialGradientBrushRenderResource : HtmlGradientBrushRenderResource, IRadialGradientBrushRenderResource
    {
        private Point center;
        public Point Center
        {
            get { return center; }
            set
            {
                if (center == value)
                {
                    return;
                }

                center = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgCenter(center, converter));
            }
        }

        private Point gradientOrigin;
        public Point GradientOrigin
        {
            get { return gradientOrigin; }
            set
            {
                if (gradientOrigin == value)
                {
                    return;
                }

                gradientOrigin = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgGradientOrigin(gradientOrigin, converter));
            }
        }

        private double radiusX;
        public double RadiusX
        {
            get { return radiusX; }
            set
            {
                if (radiusX == value)
                {
                    return;
                }

                radiusX = value;
                renderQueue.InvokeAsync(() => HtmlElement.setAttribute("gradientTransform", converter.ToGradientTransformString(RadiusX, RadiusY)));
            }
        }

        private double radiusY;
        public double RadiusY
        {
            get { return radiusY; }
            set
            {
                if (radiusY == value)
                {
                    return;
                }

                radiusY = value;
                renderQueue.InvokeAsync(() => HtmlElement.setAttribute("gradientTransform", converter.ToGradientTransformString(RadiusX, RadiusY)));
            }
        }

        private RenderQueue renderQueue;
        private SvgValueConverter converter;

        public HtmlRadialGradientBrushRenderResource(RenderQueue renderQueue, SvgValueConverter converter, SvgDefinitionContainer svgDefinitionContainer) :
            base("radialGradient", renderQueue, converter, svgDefinitionContainer)
        {
            this.renderQueue = renderQueue;
            this.converter = converter;
        }
    }
}
