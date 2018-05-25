using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Retyped.dom;
using Granular.Extensions;

namespace Granular.Host.Render
{
    public class HtmlGeometryRenderResource : HtmlRenderResource, IGeometryRenderResource
    {
        public string Uri { get; private set; }

        private HtmlTransformRenderResource transformRenderResource;
        private Transform transform;
        public Transform Transform
        {
            get { return transform; }
            set
            {
                if (transform == value)
                {
                    return;
                }

                if (transformRenderResource != null && IsLoaded)
                {
                    transformRenderResource.MatrixChanged -= OnTransformRenderResourceMatrixChanged;
                }

                transform = value;
                transformRenderResource = (HtmlTransformRenderResource)(transform?.GetRenderResource(factory));

                if (transformRenderResource != null && IsLoaded)
                {
                    transformRenderResource.MatrixChanged += OnTransformRenderResourceMatrixChanged;
                }

                renderQueue.InvokeAsync(() => HtmlElement.SetSvgTransform(transformRenderResource?.Matrix, converter));
            }
        }

        public event EventHandler DataChanged;
        private string data;
        public string Data
        {
            get { return data; }
            set
            {
                if (data == value)
                {
                    return;
                }

                data = value;

                if (pathHtmlElement != null)
                {
                    pathHtmlElement.setAttribute("d", data);
                }

                DataChanged.Raise(this);
            }
        }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private SvgDefinitionContainer svgDefinitionContainer;
        private SvgValueConverter converter;

        private HTMLElement pathHtmlElement;

        public HtmlGeometryRenderResource(IRenderElementFactory factory, RenderQueue renderQueue, SvgDefinitionContainer svgDefinitionContainer, SvgValueConverter converter) :
            base(SvgDocument.CreateElement("clipPath"))
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.svgDefinitionContainer = svgDefinitionContainer;
            this.converter = converter;

            this.pathHtmlElement = SvgDocument.CreateElement("path");

            string elementName = $"clipPath{svgDefinitionContainer.GetNextId()}";
            this.Uri = $"url(#{elementName})";
            HtmlElement.setAttribute("id", elementName);
            HtmlElement.appendChild(pathHtmlElement);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            svgDefinitionContainer.Add(this);

            if (transformRenderResource != null)
            {
                transformRenderResource.MatrixChanged += OnTransformRenderResourceMatrixChanged;
            }

            renderQueue.InvokeAsync(() => HtmlElement.SetSvgTransform(transformRenderResource?.Matrix, converter));
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            svgDefinitionContainer.Remove(this);

            if (transformRenderResource != null)
            {
                transformRenderResource.MatrixChanged -= OnTransformRenderResourceMatrixChanged;
            }
        }

        private void OnTransformRenderResourceMatrixChanged(object sender, EventArgs e)
        {
            renderQueue.InvokeAsync(() => HtmlElement.SetSvgTransform(transformRenderResource.Matrix, converter));
        }
    }
}
