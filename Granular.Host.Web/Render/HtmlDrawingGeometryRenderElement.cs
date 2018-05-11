using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Granular.Compatibility.Linq;
using Granular.Extensions;
using static Retyped.dom;

namespace Granular.Host.Render
{
    public class HtmlDrawingGeometryRenderElement : HtmlDrawingShapeRenderElement, IDrawingGeometryRenderElement
    {
        private HtmlGeometryRenderResource geometryRenderResource;
        private Geometry geometry;
        public Geometry Geometry
        {
            get { return geometry; }
            set
            {
                if (geometry == value)
                {
                    return;
                }

                if (geometryRenderResource != null && IsLoaded)
                {
                    geometryRenderResource.DataChanged -= OnGeometryDataChanged;
                    geometryRenderResource.Unload();
                }

                geometry = value;
                geometryRenderResource = (HtmlGeometryRenderResource)(geometry?.GetRenderResource(factory));

                if (geometryRenderResource != null && IsLoaded)
                {
                    geometryRenderResource.DataChanged += OnGeometryDataChanged;
                    geometryRenderResource.Load();
                }

                renderQueue.InvokeAsync(() => HtmlElement.SetSvgGeometry(geometryRenderResource));
            }
        }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private SvgValueConverter converter;

        public HtmlDrawingGeometryRenderElement(IRenderElementFactory factory, RenderQueue renderQueue, SvgValueConverter converter) :
            base(SvgDocument.CreateElement("path"), factory, renderQueue, converter)
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.converter = converter;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            if (geometryRenderResource != null)
            {
                geometryRenderResource.DataChanged += OnGeometryDataChanged;
                geometryRenderResource.Load();
            }

            renderQueue.InvokeAsync(() => HtmlElement.SetSvgGeometry(geometryRenderResource));
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (geometryRenderResource != null)
            {
                geometryRenderResource.DataChanged -= OnGeometryDataChanged;
                geometryRenderResource.Unload();
            }
        }

        private void OnGeometryDataChanged(object sender, EventArgs e)
        {
            renderQueue.InvokeAsync(() => HtmlElement.SetSvgGeometry(geometryRenderResource));
        }
    }
}
