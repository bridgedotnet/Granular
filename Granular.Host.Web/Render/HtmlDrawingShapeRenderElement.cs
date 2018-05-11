using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Granular.Compatibility.Linq;
using Granular.Extensions;
using static Retyped.dom;

namespace Granular.Host.Render
{
    public class HtmlDrawingShapeRenderElement : HtmlRenderElement, IDrawingShapeRenderElement
    {
        private HtmlBrushRenderResource fillRenderResource;
        private Brush fill;
        public Brush Fill
        {
            get { return fill; }
            set
            {
                if (fill == value)
                {
                    return;
                }

                if (fillRenderResource != null && IsLoaded)
                {
                    fillRenderResource.Unload();
                }

                fill = value;
                fillRenderResource = (HtmlBrushRenderResource)(fill?.GetRenderResource(factory));

                if (fillRenderResource != null && IsLoaded)
                {
                    fillRenderResource.Load();
                }

                renderQueue.InvokeAsync(() => HtmlElement.SetSvgFill(fillRenderResource));
            }
        }

        private HtmlBrushRenderResource strokeRenderResource;
        private Brush stroke;
        public Brush Stroke
        {
            get { return stroke; }
            set
            {
                if (stroke == value)
                {
                    return;
                }

                if (strokeRenderResource != null && IsLoaded)
                {
                    strokeRenderResource.Unload();
                }

                stroke = value;
                strokeRenderResource = (HtmlBrushRenderResource)(stroke?.GetRenderResource(factory));

                if (strokeRenderResource != null && IsLoaded)
                {
                    strokeRenderResource.Load();
                }

                renderQueue.InvokeAsync(() => HtmlElement.SetSvgStroke(strokeRenderResource));
            }
        }

        private double strokeThickness;
        public double StrokeThickness
        {
            get { return strokeThickness; }
            set
            {
                if (strokeThickness == value)
                {
                    return;
                }

                strokeThickness = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgStrokeThickness(strokeThickness, converter));
            }
        }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private SvgValueConverter converter;

        public HtmlDrawingShapeRenderElement(HTMLElement htmlElement, IRenderElementFactory factory, RenderQueue renderQueue, SvgValueConverter converter) :
            base(htmlElement)
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.converter = converter;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            if (fillRenderResource != null)
            {
                fillRenderResource.Load();
            }

            if (strokeRenderResource != null)
            {
                strokeRenderResource.Load();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (fillRenderResource != null)
            {
                fillRenderResource.Unload();
            }

            if (strokeRenderResource != null)
            {
                strokeRenderResource.Unload();
            }
        }
    }
}
