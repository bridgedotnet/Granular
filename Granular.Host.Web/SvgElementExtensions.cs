using System;
using System.Windows;
using System.Windows.Media;
using static Retyped.dom;
using Granular.Host.Render;
using Granular.Extensions;
using Granular.Compatibility.Linq;

namespace Granular.Host
{
    public static class SvgElementExtensions
    {
        public static void SetSvgStartPoint(this HTMLElement element, Point startPoint, SvgValueConverter converter)
        {
            element.SetSvgPointAttributes("x1", "y1", startPoint, converter);
        }

        public static void SetSvgEndPoint(this HTMLElement element, Point endPoint, SvgValueConverter converter)
        {
            element.SetSvgPointAttributes("x2", "y2", endPoint, converter);
        }

        public static void SetSvgCenter(this HTMLElement element, Point center, SvgValueConverter converter)
        {
            element.SetSvgPointAttributes("cx", "cy", center, converter);
        }

        public static void SetSvgGradientOrigin(this HTMLElement element, Point gradientOrigin, SvgValueConverter converter)
        {
            element.SetSvgPointAttributes("fx", "fy", gradientOrigin, converter);
        }

        public static void SetSvgSpreadMethod(this HTMLElement element, GradientSpreadMethod spreadMethod, SvgValueConverter converter)
        {
            element.setAttribute("spreadMethod", converter.ToSpreadMethodString(spreadMethod));
        }

        public static void SetSvgMappingMode(this HTMLElement element, BrushMappingMode mappingMode, SvgValueConverter converter)
        {
            element.setAttribute("gradientUnits", converter.ToGradientUnitsString(mappingMode));
        }

        public static void SetSvgTransform(this HTMLElement element, Matrix transform, SvgValueConverter converter)
        {
            if (transform.IsNullOrIdentity())
            {
                element.removeAttribute("transform");
            }
            else
            {
                element.setAttribute("transform", converter.ToMatrixString(transform));
            }
        }

        public static void SetSvgLocation(this HTMLElement element, Point location, SvgValueConverter converter)
        {
            element.SetSvgPointAttributes("x", "y", location, converter);
        }

        public static void SetSvgSize(this HTMLElement element, Size size, SvgValueConverter converter)
        {
            if (size.IsNullOrEmpty())
            {
                element.removeAttribute("width");
                element.removeAttribute("height");
            }
            else
            {
                element.setAttribute("width", converter.ToImplicitValueString(size.Width));
                element.setAttribute("height", converter.ToImplicitValueString(size.Height));
            }
        }

        public static void SetSvgBounds(this HTMLElement element, Rect bounds, SvgValueConverter converter)
        {
            element.SetSvgLocation(bounds.Location, converter);
            element.SetSvgSize(bounds.Size, converter);
        }

        public static void SetSvgImageSource(this HTMLElement element, ImageSource imageSource, IRenderElementFactory factory, SvgValueConverter converter)
        {
            element.setAttributeNS(SvgDocument.XlinkNamespaceUri, "href", converter.ToImageUrl(imageSource, factory));
        }

        public static void SetSvgFill(this HTMLElement element, HtmlBrushRenderResource brush)
        {
            element.SetSvgBrush("fill", brush);
        }

        public static void SetSvgStroke(this HTMLElement element, HtmlBrushRenderResource brush)
        {
            element.SetSvgBrush("stroke", brush);
        }

        public static void SetSvgOpacity(this HTMLElement element, double opacity, SvgValueConverter converter)
        {
            element.SetSvgAttribute("opacity", opacity, converter);
        }

        public static void SetSvgStrokeThickness(this HTMLElement element, double strokeThickness, SvgValueConverter converter)
        {
            element.SetSvgAttribute("stroke-width", strokeThickness, converter);
        }

        public static void SetSvgGeometry(this HTMLElement element, HtmlGeometryRenderResource geometry)
        {
            if (geometry == null)
            {
                element.removeAttribute("d");
            }
            else
            {
                element.SetSvgAttribute("d", geometry.Data);
            }
        }

        public static void SetSvgClip(this HTMLElement element, HtmlGeometryRenderResource geometry)
        {
            if (geometry == null)
            {
                element.removeAttribute("clip-path");
            }
            else
            {
                element.setAttribute("clip-path", geometry.Uri);
            }
        }

        public static void SetSvgFontFamily(this HTMLElement element, FontFamily fontFamily, SvgValueConverter converter)
        {
            if (!fontFamily.FamilyNames.Any())
            {
                element.ClearHtmlStyleProperty("font-family");
            }
            else
            {
                element.SetSvgAttribute("font-family", converter.ToFontFamilyNamesString(fontFamily));
            }
        }

        public static void SetSvgFontSize(this HTMLElement element, double fontSize, SvgValueConverter converter)
        {
            if (fontSize.IsNaN())
            {
                element.ClearHtmlStyleProperty("font-size");
            }
            else
            {
                element.SetSvgAttribute("font-size", converter.ToPixelString(fontSize));
            }
        }

        public static void SetSvgFontStyle(this HTMLElement element, System.Windows.FontStyle fontStyle, SvgValueConverter converter)
        {
            if (fontStyle == System.Windows.FontStyle.Normal)
            {
                element.ClearHtmlStyleProperty("font-style");
            }
            else
            {
                element.SetSvgAttribute("font-style", converter.ToFontStyleString(fontStyle));
            }
        }

        public static void SetSvgFontWeight(this HTMLElement element, FontWeight fontWeight, SvgValueConverter converter)
        {
            if (fontWeight == FontWeight.Normal)
            {
                element.ClearHtmlStyleProperty("font-weight");
            }
            else
            {
                element.SetSvgAttribute("font-weight", converter.ToFontWeightString(fontWeight));
            }
        }

        public static void SetSvgFontStretch(this HTMLElement element, System.Windows.FontStretch fontStretch, SvgValueConverter converter)
        {
            if (fontStretch == System.Windows.FontStretch.Normal)
            {
                element.ClearHtmlStyleProperty("font-stretch");
            }
            else
            {
                element.SetSvgAttribute("font-stretch", converter.ToFontStretchString(fontStretch));
            }
        }

        private static void SetSvgAttribute(this HTMLElement element, string attributeName, double value, SvgValueConverter converter)
        {
            if (value.IsNaN())
            {
                element.removeAttribute(attributeName);
            }
            else
            {
                element.setAttribute(attributeName, converter.ToImplicitValueString(value));
            }
        }

        private static void SetSvgAttribute(this HTMLElement element, string attributeName, string value)
        {
            if (value.IsNullOrEmpty())
            {
                element.removeAttribute(attributeName);
            }
            else
            {
                element.setAttribute(attributeName, value);
            }
        }

        private static void SetSvgPointAttributes(this HTMLElement element, string xAttributeName, string yAttributeName, Point point, SvgValueConverter converter)
        {
            if (point.IsNullOrEmpty())
            {
                element.removeAttribute(xAttributeName);
                element.removeAttribute(yAttributeName);
            }
            else
            {
                element.setAttribute(xAttributeName, converter.ToImplicitValueString(point.X));
                element.setAttribute(yAttributeName, converter.ToImplicitValueString(point.Y));
            }
        }

        private static void SetSvgBrush(this HTMLElement element, string attributeName, HtmlBrushRenderResource brush)
        {
            if (brush == null)
            {
                element.removeAttribute(attributeName);
            }
            else
            {
                element.setAttribute(attributeName, brush.Uri);
            }
        }
    }
}
