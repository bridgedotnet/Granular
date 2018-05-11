using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Granular.Collections;
using Granular.Extensions;
using Granular.Host.Render;
using static Retyped.dom;

namespace Granular.Host
{
    public static class HtmlElementExtensions
    {
        public static void InsertChild(this HTMLElement element, int index, HTMLElement child)
        {
            if (index < element.childElementCount)
            {
                element.insertBefore(child, element.children[index]);
            }
            else
            {
                element.appendChild(child);
            }
        }

        public static void SetHtmlStyleProperty(this HTMLElement element, string key, string value)
        {
            element.style.setProperty(key, value);
        }

        public static void ClearHtmlStyleProperty(this HTMLElement element, string key)
        {
            element.style.removeProperty(key);
        }

        public static void SetHtmlBackground(this HTMLElement element, Brush background, Rect targetRect, IRenderElementFactory factory, HtmlValueConverter converter)
        {
            element.ClearHtmlStyleProperty("background-color");
            element.ClearHtmlStyleProperty("background-image");

            if (background is SolidColorBrush)
            {
                element.SetHtmlStyleProperty("background-color", converter.ToColorString((SolidColorBrush)background));
            }
            else if (background != null)
            {
                element.SetHtmlStyleProperty("background-image", converter.ToImageString(background, targetRect, factory));
            }
        }

        public static void SetHtmlBackgroundLocation(this HTMLElement element, Point location, HtmlValueConverter converter)
        {
            if (Point.IsNullOrEmpty(location))
            {
                element.ClearHtmlStyleProperty("background-position");
            }
            else
            {
                element.SetHtmlStyleProperty("background-position", converter.ToPixelString(location));
            }
        }

        public static void SetHtmlBackgroundSize(this HTMLElement element, Size size, HtmlValueConverter converter)
        {
            if (Size.IsNullOrEmpty(size))
            {
                element.ClearHtmlStyleProperty("background-size");
            }
            else
            {
                element.SetHtmlStyleProperty("background-size", converter.ToPixelString(size));
            }
        }

        public static void SetHtmlBackgroundBounds(this HTMLElement element, Rect bounds, HtmlValueConverter converter)
        {
            element.SetHtmlBackgroundLocation(bounds.Location, converter);
            element.SetHtmlBackgroundSize(bounds.Size, converter);
        }

        public static void SetHtmlBorderThickness(this HTMLElement element, Thickness borderThickness, HtmlValueConverter converter)
        {
            if (borderThickness == Thickness.Zero)
            {
                element.ClearHtmlStyleProperty("border-style");
                element.ClearHtmlStyleProperty("border-width");
                element.ClearHtmlStyleProperty("border-image-slice");
            }
            else
            {
                element.SetHtmlStyleProperty("border-style", "solid");
                element.SetHtmlStyleProperty("border-width", converter.ToPixelString(borderThickness));
                element.SetHtmlStyleProperty("border-image-slice", converter.ToImplicitValueString(borderThickness));
            }
        }

        public static void SetHtmlBorderBrush(this HTMLElement element, Brush borderBrush, Size targetSize, IRenderElementFactory factory, HtmlValueConverter converter)
        {
            element.ClearHtmlStyleProperty("border-color");
            element.ClearHtmlStyleProperty("border-image-source");

            if (borderBrush is SolidColorBrush)
            {
                element.SetHtmlStyleProperty("border-color", converter.ToColorString((SolidColorBrush)borderBrush));
            }
            else if (borderBrush != null)
            {
                element.SetHtmlStyleProperty("border-image-source", converter.ToImageString(borderBrush, new Rect(targetSize), factory));
            }
        }

        public static void SetHtmlBounds(this HTMLElement element, Rect bounds, HtmlValueConverter converter)
        {
            element.SetHtmlLocation(bounds.Location, converter);
            element.SetHtmlSize(bounds.Size, converter);
        }

        public static void SetHtmlLocation(this HTMLElement element, Point location, HtmlValueConverter converter)
        {
            element.SetHtmlStyleProperty("position", "absolute");
            element.SetHtmlStyleProperty("left", converter.ToPixelString(location.X));
            element.SetHtmlStyleProperty("top", converter.ToPixelString(location.Y));
        }

        public static void SetHtmlSize(this HTMLElement element, Size size, HtmlValueConverter converter)
        {
            if (size.Width.IsNaN())
            {
                element.ClearHtmlStyleProperty("width");
            }
            else
            {
                element.SetHtmlStyleProperty("width", converter.ToPixelString(size.Width));
            }

            if (size.Height.IsNaN())
            {
                element.ClearHtmlStyleProperty("height");
            }
            else
            {
                element.SetHtmlStyleProperty("height", converter.ToPixelString(size.Height));
            }
        }

        public static void SetHtmlClipToBounds(this HTMLElement element, bool clipToBounds)
        {
            element.SetHtmlStyleProperty("overflow", clipToBounds ? "hidden" : "visible");
        }

        public static void SetHtmlIsHitTestVisible(this HTMLElement element, bool isHitTestVisible)
        {
            element.SetHtmlStyleProperty("pointer-events", isHitTestVisible ? "auto" : "none");
        }

        public static void SetHtmlIsVisible(this HTMLElement element, bool isVisible)
        {
            if (isVisible)
            {
                element.ClearHtmlStyleProperty("display");
            }
            else
            {
                element.SetHtmlStyleProperty("display", "none");
            }
        }

        public static void SetHtmlCornerRadius(this HTMLElement element, CornerRadius cornerRadius, HtmlValueConverter converter)
        {
            element.ClearHtmlStyleProperty("border-radius");
            element.ClearHtmlStyleProperty("border-top-left-radius");
            element.ClearHtmlStyleProperty("border-top-right-radius");
            element.ClearHtmlStyleProperty("border-bottom-left-radius");
            element.ClearHtmlStyleProperty("border-bottom-right-radius");

            if (cornerRadius != CornerRadius.Zero)
            {
                if (cornerRadius.IsUniform)
                {
                    element.SetHtmlStyleProperty("border-radius", converter.ToPixelString(cornerRadius.TopLeft));
                }
                else
                {
                    element.SetHtmlStyleProperty("border-top-left-radius", converter.ToPixelString(cornerRadius.TopLeft));
                    element.SetHtmlStyleProperty("border-top-right-radius", converter.ToPixelString(cornerRadius.TopRight));
                    element.SetHtmlStyleProperty("border-bottom-left-radius", converter.ToPixelString(cornerRadius.BottomLeft));
                    element.SetHtmlStyleProperty("border-bottom-right-radius", converter.ToPixelString(cornerRadius.BottomRight));
                }
            }
        }

        public static void SetHtmlForeground(this HTMLElement element, Brush foreground, HtmlValueConverter converter)
        {
            if (foreground == null)
            {
                element.ClearHtmlStyleProperty("color");
            }
            else if (foreground is SolidColorBrush)
            {
                element.SetHtmlStyleProperty("color", converter.ToColorString((SolidColorBrush)foreground));
            }
            else
            {
                throw new Granular.Exception("A \"{0}\" foreground brush is not supported", foreground.GetType());
            }
        }

        public static void SetHtmlOpacity(this HTMLElement element, double opacity, HtmlValueConverter converter)
        {
            if (opacity == 1.0)
            {
                element.ClearHtmlStyleProperty("opacity");
            }
            else
            {
                element.SetHtmlStyleProperty("opacity", converter.ToImplicitValueString(opacity));
            }
        }

        public static void SetHtmlTransform(this HTMLElement element, Matrix transform, HtmlValueConverter converter)
        {
            if (transform == Matrix.Identity)
            {
                element.ClearHtmlStyleProperty("transform");
                element.ClearHtmlStyleProperty("transform-origin");
            }
            else
            {
                element.SetHtmlStyleProperty("transform", converter.ToTransformString(transform));
                element.SetHtmlStyleProperty("transform-origin", "0 0");
            }
        }

        public static void SetHtmlClip(this HTMLElement element, HtmlGeometryRenderResource clip)
        {
            if (clip == null)
            {
                element.ClearHtmlStyleProperty("clip-path");
            }
            else
            {
                element.SetHtmlStyleProperty("clip-path", clip.Uri);
            }
        }

        public static void SetHtmlFontFamily(this HTMLElement element, FontFamily fontFamily, HtmlValueConverter converter)
        {
            if (!fontFamily.FamilyNames.Any())
            {
                element.ClearHtmlStyleProperty("font-family");
            }
            else
            {
                element.SetHtmlStyleProperty("font-family", converter.ToFontFamilyNamesString(fontFamily));
            }
        }

        public static void SetHtmlFontSize(this HTMLElement element, double fontSize, HtmlValueConverter converter)
        {
            if (fontSize.IsNaN())
            {
                element.ClearHtmlStyleProperty("font-size");
            }
            else
            {
                element.SetHtmlStyleProperty("font-size", converter.ToPixelString(fontSize));
            }
        }

        public static void SetHtmlFontStyle(this HTMLElement element, System.Windows.FontStyle fontStyle, HtmlValueConverter converter)
        {
            if (fontStyle == System.Windows.FontStyle.Normal)
            {
                element.ClearHtmlStyleProperty("font-style");
            }
            else
            {
                element.SetHtmlStyleProperty("font-style", converter.ToFontStyleString(fontStyle));
            }
        }

        public static void SetHtmlFontWeight(this HTMLElement element, FontWeight fontWeight, HtmlValueConverter converter)
        {
            if (fontWeight == FontWeight.Normal)
            {
                element.ClearHtmlStyleProperty("font-weight");
            }
            else
            {
                element.SetHtmlStyleProperty("font-weight", converter.ToFontWeightString(fontWeight));
            }
        }

        public static void SetHtmlFontStretch(this HTMLElement element, System.Windows.FontStretch fontStretch, HtmlValueConverter converter)
        {
            if (fontStretch == System.Windows.FontStretch.Normal)
            {
                element.ClearHtmlStyleProperty("font-stretch");
            }
            else
            {
                element.SetHtmlStyleProperty("font-stretch", converter.ToFontStretchString(fontStretch));
            }
        }

        public static void SetHtmlTextAlignment(this HTMLElement element, TextAlignment textAlignment, HtmlValueConverter converter)
        {
            element.SetHtmlStyleProperty("text-align", converter.ToTextAlignmentString(textAlignment));
        }

        public static void SetHtmlTextTrimming(this HTMLElement element, TextTrimming textTrimming)
        {
            if (textTrimming == TextTrimming.None)
            {
                element.ClearHtmlStyleProperty("text-overflow");
            }
            else
            {
                element.SetHtmlStyleProperty("text-overflow", "ellipsis");
            }
        }

        public static void SetHtmlTextWrapping(this HTMLElement element, TextWrapping textWrapping, HtmlValueConverter converter)
        {
            element.SetHtmlStyleProperty("white-space", converter.ToWhiteSpaceString(textWrapping));
        }

        public static void SetHtmlHorizontalScrollBarVisibility(this HTMLElement element, ScrollBarVisibility scrollBarVisibility, HtmlValueConverter converter)
        {
            element.SetHtmlStyleProperty("overflow-x", converter.ToOverflowString(scrollBarVisibility));
        }

        public static void SetHtmlVerticalScrollBarVisibility(this HTMLElement element, ScrollBarVisibility scrollBarVisibility, HtmlValueConverter converter)
        {
            element.SetHtmlStyleProperty("overflow-y", converter.ToOverflowString(scrollBarVisibility));
        }

        public static void SetHtmlBackgroundImage(this HTMLElement element, string url, HtmlValueConverter converter, IRenderElementFactory factory)
        {
            if (url.IsNullOrEmpty())
            {
                element.ClearHtmlStyleProperty("background-image");
            }
            else
            {
                element.SetHtmlStyleProperty("background-image", converter.ToUrlString(url));
            }
        }
    }
}
