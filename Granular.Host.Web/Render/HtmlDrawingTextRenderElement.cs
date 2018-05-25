using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Granular.Host.Render
{
    public class HtmlDrawingTextRenderElement : HtmlRenderElement, IDrawingTextRenderElement
    {
        private FormattedText formattedText;
        public FormattedText FormattedText
        {
            get { return formattedText; }
            set
            {
                if (formattedText == value)
                {
                    return;
                }

                if (foregroundRenderResource != null && IsLoaded)
                {
                    foregroundRenderResource.Unload();
                }

                formattedText = value;
                foregroundRenderResource = (HtmlBrushRenderResource)(formattedText?.Foreground?.GetRenderResource(factory));

                if (foregroundRenderResource != null && IsLoaded)
                {
                    foregroundRenderResource.Load();
                }

                renderQueue.InvokeAsync(SetFormattedText);
            }
        }

        private Point origin;
        public Point Origin
        {
            get { return origin; }
            set
            {
                if (origin == value)
                {
                    return;
                }

                origin = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetSvgLocation(origin, converter));
            }
        }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private SvgValueConverter converter;
        private HtmlBrushRenderResource foregroundRenderResource;

        public HtmlDrawingTextRenderElement(IRenderElementFactory factory, RenderQueue renderQueue, SvgValueConverter svgValueConverter) :
            base(SvgDocument.CreateElement("text"))
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.converter = svgValueConverter;

            HtmlElement.setAttribute("dy", "1em");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            if (foregroundRenderResource != null)
            {
                foregroundRenderResource.Load();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (foregroundRenderResource != null)
            {
                foregroundRenderResource.Unload();
            }
        }

        private void SetFormattedText()
        {
            HtmlElement.SetSvgFill(foregroundRenderResource);
            HtmlElement.SetSvgFontFamily(FormattedText.Typeface.FontFamily, converter);
            HtmlElement.SetSvgFontStretch(FormattedText.Typeface.Stretch, converter);
            HtmlElement.SetSvgFontStyle(FormattedText.Typeface.Style, converter);
            HtmlElement.SetSvgFontWeight(FormattedText.Typeface.Weight, converter);
            HtmlElement.SetSvgFontSize(FormattedText.Size, converter);
            //HtmlElement.SetSvgFlowDirection(FormattedText.FlowDirection, converter);
            //HtmlElement.SetSvgLineHeight(FormattedText.LineHeight, converter;
            //HtmlElement.SetSvgLineCount(FormattedText.MaxLineCount, converter);
            //HtmlElement.SetSvgMaxHeight(FormattedText.MaxTextHeight, converter);
            //HtmlElement.SetSvgMaxWidth(FormattedText.MaxTextWidth, converter);
            //HtmlElement.SetSvgTextAlignment(FormattedText.TextAlignment, converter);
            //HtmlElement.SetSvgTextTrimming(FormattedText.Trimming);
            HtmlElement.textContent = FormattedText.Text;
        }
    }
}
