using System;
using System.Collections.Generic;
using static Retyped.dom;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Granular.Extensions;

namespace Granular.Host
{
    public class TextMeasurementService : ITextMeasurementService
    {
        private HtmlValueConverter converter;
        private HTMLElement htmlElement;

        public TextMeasurementService(HtmlValueConverter converter)
        {
            this.converter = converter;
        }

        public Size Measure(string text, double fontSize, Typeface typeface, double maxWidth)
        {
            if (htmlElement == null)
            {
                htmlElement = document.createElement("div");
                document.body.appendChild(htmlElement);
            }

            htmlElement.SetHtmlStyleProperty("position", "absolute");
            htmlElement.SetHtmlStyleProperty("visibility", "hidden");
            htmlElement.SetHtmlFontSize(fontSize, converter);
            htmlElement.SetHtmlFontFamily(typeface.FontFamily, converter);
            htmlElement.SetHtmlFontStretch(typeface.Stretch, converter);
            htmlElement.SetHtmlFontStyle(typeface.Style, converter);
            htmlElement.SetHtmlFontWeight(typeface.Weight, converter);

            if (maxWidth.IsNaN() || !Double.IsFinite(maxWidth))
            {
                htmlElement.SetHtmlTextWrapping(TextWrapping.NoWrap, converter);
                htmlElement.ClearHtmlStyleProperty("max-width");
            }
            else
            {
                htmlElement.SetHtmlTextWrapping(TextWrapping.Wrap, converter);
                htmlElement.SetHtmlStyleProperty("max-width", converter.ToPixelString(maxWidth));
            }

            htmlElement.innerHTML = converter.ToHtmlContentString(text.DefaultIfNullOrEmpty("A"));

            return new Size(text.IsNullOrEmpty() ? 0 : htmlElement.offsetWidth + 2, htmlElement.offsetHeight);
        }
    }
}
