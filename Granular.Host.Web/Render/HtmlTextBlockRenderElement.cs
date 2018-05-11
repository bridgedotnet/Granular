using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Granular.Host.Render
{
    public class HtmlTextBlockRenderElement : HtmlRenderElement, ITextBlockRenderElement
    {
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                {
                    return;
                }

                text = value;
                renderQueue.InvokeAsync(() => HtmlElement.textContent = text);
            }
        }

        private Rect bounds;
        public Rect Bounds
        {
            get { return bounds; }
            set
            {
                if (bounds == value)
                {
                    return;
                }

                bounds = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlBounds(bounds, converter));
            }
        }

        private Brush foreground;
        public Brush Foreground
        {
            get { return foreground; }
            set
            {
                if (foreground == value)
                {
                    return;
                }

                if (IsLoaded && foreground != null)
                {
                    foreground.Changed -= OnForegroundChanged;
                }

                foreground = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlForeground(Foreground, converter));

                if (IsLoaded && foreground != null)
                {
                    foreground.Changed += OnForegroundChanged;
                }
            }
        }

        private FontFamily fontFamily;
        public FontFamily FontFamily
        {
            get { return fontFamily; }
            set
            {
                if (fontFamily == value)
                {
                    return;
                }

                fontFamily = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlFontFamily(fontFamily, converter));
            }
        }

        private double fontSize;
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize == value)
                {
                    return;
                }

                fontSize = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlFontSize(fontSize, converter));
            }
        }

        private FontStyle fontStyle;
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set
            {
                if (fontStyle == value)
                {
                    return;
                }

                fontStyle = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlFontStyle(fontStyle, converter));
            }
        }

        private FontWeight fontWeight;
        public FontWeight FontWeight
        {
            get { return fontWeight; }
            set
            {
                if (fontWeight == value)
                {
                    return;
                }

                fontWeight = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlFontWeight(fontWeight, converter));
            }
        }

        private FontStretch fontStretch;
        public FontStretch FontStretch
        {
            get { return fontStretch; }
            set
            {
                if (fontStretch == value)
                {
                    return;
                }

                fontStretch = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlFontStretch(fontStretch, converter));
            }
        }

        private TextAlignment textAlignment;
        public TextAlignment TextAlignment
        {
            get { return textAlignment; }
            set
            {
                if (textAlignment == value)
                {
                    return;
                }

                textAlignment = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlTextAlignment(textAlignment, converter));
            }
        }

        private TextTrimming textTrimming;
        public TextTrimming TextTrimming
        {
            get { return textTrimming; }
            set
            {
                if (textTrimming == value)
                {
                    return;
                }

                textTrimming = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlTextTrimming(textTrimming));
            }
        }

        private TextWrapping textWrapping;
        public TextWrapping TextWrapping
        {
            get { return textWrapping; }
            set
            {
                if (textWrapping == value)
                {
                    return;
                }

                textWrapping = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlTextWrapping(textWrapping, converter));
            }
        }

        private RenderQueue renderQueue;
        private HtmlValueConverter converter;

        public HtmlTextBlockRenderElement(RenderQueue renderQueue, HtmlValueConverter converter)
        {
            this.renderQueue = renderQueue;
            this.converter = converter;

            bounds = Rect.Zero;
            fontFamily = FontFamily.Default;
            fontSize = Double.NaN;

            HtmlElement.SetHtmlBounds(Bounds, converter);
            HtmlElement.SetHtmlForeground(Foreground, converter);
            HtmlElement.SetHtmlFontFamily(FontFamily, converter);
            HtmlElement.SetHtmlFontSize(FontSize, converter);
            HtmlElement.SetHtmlFontStyle(FontStyle, converter);
            HtmlElement.SetHtmlFontWeight(FontWeight, converter);
            HtmlElement.SetHtmlFontStretch(FontStretch, converter);
            HtmlElement.SetHtmlIsHitTestVisible(false);
            HtmlElement.SetHtmlTextAlignment(TextAlignment, converter);
            HtmlElement.SetHtmlTextTrimming(TextTrimming);
            HtmlElement.SetHtmlTextWrapping(TextWrapping, converter);
        }

        private void OnForegroundChanged(object sender, EventArgs e)
        {
            renderQueue.InvokeAsync(() => HtmlElement.SetHtmlForeground(Foreground, converter));
        }

        protected override void OnLoad()
        {
            if (Foreground != null)
            {
                Foreground.Changed += OnForegroundChanged;
            }

            renderQueue.InvokeAsync(() => HtmlElement.SetHtmlForeground(Foreground, converter));
        }

        protected override void OnUnload()
        {
            if (Foreground != null)
            {
                Foreground.Changed -= OnForegroundChanged;
            }
        }
    }
}
