/**
 * @version 0.4.0.0
 * @author Object.NET, Inc.
 * @copyright Copyright 2008-2018 Object.NET, Inc.
 * @compiler Bridge.NET 17.0.0
 */
Bridge.assembly("Granular.Host", function ($asm, globals) {
    "use strict";

    Bridge.define("Granular.Host.ElementExtensions");

    Bridge.define("Granular.Host.EmbeddedResourceObjectFactory", {
        statics: {
            methods: {
                GetExtension: function (path) {
                    return path.substr(((path.lastIndexOf(String.fromCharCode(46)) + 1) | 0)).toLowerCase();
                }
            }
        },
        fields: {
            converter: null,
            objectUrlCache: null
        },
        ctors: {
            ctor: function (converter) {
                this.$initialize();
                this.converter = converter;
                this.objectUrlCache = Granular.Collections.CacheDictionary$2(System.Uri,System.String).CreateUsingStringKeys(Bridge.fn.cacheBind(this, this.ResolveObjectUrl), $asm.$.Granular.Host.EmbeddedResourceObjectFactory.f1);
            }
        },
        methods: {
            GetObjectUrl: function (uri) {
                return this.objectUrlCache.GetValue(uri);
            },
            ResolveObjectUrl: function (uri) {
                var imageData = System.Windows.EmbeddedResourceLoader.LoadResourceData(uri);
                var mimeType = this.converter.ToMimeTypeString(Granular.Host.EmbeddedResourceObjectFactory.GetExtension(System.UriExtensions.GetLocalPath(uri)));

                return URL.createObjectURL(new Blob([new Uint8Array(imageData)], {type: mimeType}));
            }
        }
    });

    Bridge.ns("Granular.Host.EmbeddedResourceObjectFactory", $asm.$);

    Bridge.apply($asm.$.Granular.Host.EmbeddedResourceObjectFactory, {
        f1: function (uri) {
            return System.UriExtensions.GetAbsoluteUri(uri);
        }
    });

    Bridge.define("Granular.Host.HtmlDefinition", {
        statics: {
            fields: {
                Tags: null
            },
            ctors: {
                init: function () {
                    this.Tags = System.Array.init(["a", "abbr", "acronym", "address", "applet", "area", "article", "aside", "audio", "b", "base", "basefont", "bdi", "bdo", "big", "blockquote", "body", "br", "button", "canvas", "caption", "center", "cite", "code", "col", "colgroup", "datalist", "dd", "del", "details", "dfn", "dialog", "dir", "div", "dl", "dt", "em", "embed", "fieldset", "figcaption", "figure", "font", "footer", "form", "frame", "frameset", "h1", "head", "header", "hr", "html", "i", "iframe", "image", "img", "input", "ins", "kbd", "keygen", "label", "legend", "li", "link", "main", "map", "mark", "menu", "menuitem", "meta", "meter", "nav", "noframes", "noscript", "object", "ol", "optgroup", "option", "output", "p", "param", "pre", "progress", "q", "rp", "rt", "ruby", "s", "samp", "script", "section", "select", "small", "source", "span", "strike", "strong", "style", "sub", "summary", "sup", "table", "tbody", "td", "textarea", "tfoot", "th", "thead", "time", "title", "tr", "track", "tt", "u", "ul", "var", "video", "wbr"], System.String);
                }
            }
        }
    });

    Bridge.define("Granular.Host.HtmlElementExtensions", {
        statics: {
            methods: {
                InsertChild: function (element, index, child) {
                    if (index < element.childElementCount) {
                        element.insertBefore(child, element.children[index]);
                    } else {
                        element.appendChild(child);
                    }
                },
                SetHtmlStyleProperty: function (element, key, value) {
                    element.style.setProperty(key, value);
                },
                ClearHtmlStyleProperty: function (element, key) {
                    element.style.removeProperty(key);
                },
                SetHtmlBackground: function (element, background, targetRect, factory, converter) {
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "background-color");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "background-image");

                    if (Bridge.is(background, System.Windows.Media.SolidColorBrush)) {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "background-color", converter.ToColorString$1(Bridge.cast(background, System.Windows.Media.SolidColorBrush)));
                    } else if (background != null) {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "background-image", Granular.Host.HtmlValueConverterExtensions.ToImageString(converter, background, targetRect, factory));
                    }
                },
                SetHtmlBackgroundLocation: function (element, location, converter) {
                    if (System.Windows.Point.IsNullOrEmpty(location)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "background-position");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "background-position", converter.ToPixelString$1(location));
                    }
                },
                SetHtmlBackgroundSize: function (element, size, converter) {
                    if (System.Windows.Size.IsNullOrEmpty(size)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "background-size");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "background-size", converter.ToPixelString$2(size));
                    }
                },
                SetHtmlBackgroundBounds: function (element, bounds, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlBackgroundLocation(element, bounds.Location, converter);
                    Granular.Host.HtmlElementExtensions.SetHtmlBackgroundSize(element, bounds.Size, converter);
                },
                SetHtmlBorderThickness: function (element, borderThickness, converter) {
                    if (System.Windows.Thickness.op_Equality(borderThickness, System.Windows.Thickness.Zero)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-style");
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-width");
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-image-slice");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-style", "solid");
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-width", converter.ToPixelString$3(borderThickness));
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-image-slice", converter.ToImplicitValueString$2(borderThickness));
                    }
                },
                SetHtmlBorderBrush: function (element, borderBrush, targetSize, factory, converter) {
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-color");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-image-source");

                    if (Bridge.is(borderBrush, System.Windows.Media.SolidColorBrush)) {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-color", converter.ToColorString$1(Bridge.cast(borderBrush, System.Windows.Media.SolidColorBrush)));
                    } else if (borderBrush != null) {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-image-source", Granular.Host.HtmlValueConverterExtensions.ToImageString(converter, borderBrush, new System.Windows.Rect.$ctor3(targetSize), factory));
                    }
                },
                SetHtmlBounds: function (element, bounds, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlLocation(element, bounds.Location, converter);
                    Granular.Host.HtmlElementExtensions.SetHtmlSize(element, bounds.Size, converter);
                },
                SetHtmlLocation: function (element, location, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "position", "absolute");
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "left", converter.ToPixelString(location.X));
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "top", converter.ToPixelString(location.Y));
                },
                SetHtmlSize: function (element, size, converter) {
                    if (Granular.Extensions.DoubleExtensions.IsNaN(size.Width)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "width");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "width", converter.ToPixelString(size.Width));
                    }

                    if (Granular.Extensions.DoubleExtensions.IsNaN(size.Height)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "height");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "height", converter.ToPixelString(size.Height));
                    }
                },
                SetHtmlClipToBounds: function (element, clipToBounds) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "overflow", clipToBounds ? "hidden" : "visible");
                },
                SetHtmlIsHitTestVisible: function (element, isHitTestVisible) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "pointer-events", isHitTestVisible ? "auto" : "none");
                },
                SetHtmlIsVisible: function (element, isVisible) {
                    if (isVisible) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "display");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "display", "none");
                    }
                },
                SetHtmlCornerRadius: function (element, cornerRadius, converter) {
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-radius");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-top-left-radius");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-top-right-radius");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-bottom-left-radius");
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "border-bottom-right-radius");

                    if (System.Windows.CornerRadius.op_Inequality(cornerRadius, System.Windows.CornerRadius.Zero)) {
                        if (cornerRadius.IsUniform) {
                            Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-radius", converter.ToPixelString(cornerRadius.TopLeft));
                        } else {
                            Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-top-left-radius", converter.ToPixelString(cornerRadius.TopLeft));
                            Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-top-right-radius", converter.ToPixelString(cornerRadius.TopRight));
                            Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-bottom-left-radius", converter.ToPixelString(cornerRadius.BottomLeft));
                            Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "border-bottom-right-radius", converter.ToPixelString(cornerRadius.BottomRight));
                        }
                    }
                },
                SetHtmlForeground: function (element, foreground, converter) {
                    if (foreground == null) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "color");
                    } else if (Bridge.is(foreground, System.Windows.Media.SolidColorBrush)) {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "color", converter.ToColorString$1(Bridge.cast(foreground, System.Windows.Media.SolidColorBrush)));
                    } else {
                        throw new Granular.Exception("A \"{0}\" foreground brush is not supported", [Bridge.getType(foreground)]);
                    }
                },
                SetHtmlOpacity: function (element, opacity, converter) {
                    if (opacity === 1.0) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "opacity");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "opacity", converter.ToImplicitValueString(opacity));
                    }
                },
                SetHtmlTransform: function (element, transform, converter) {
                    if (System.Windows.Media.Matrix.op_Equality(transform, System.Windows.Media.Matrix.Identity)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "transform");
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "transform-origin");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "transform", converter.ToTransformString(transform));
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "transform-origin", "0 0");
                    }
                },
                SetHtmlClip: function (element, clip) {
                    if (clip == null) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "clip-path");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "clip-path", clip.Uri);
                    }
                },
                SetHtmlFontFamily: function (element, fontFamily, converter) {
                    if (!System.Linq.Enumerable.from(fontFamily.FamilyNames).any()) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-family");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "font-family", converter.ToFontFamilyNamesString(fontFamily));
                    }
                },
                SetHtmlFontSize: function (element, fontSize, converter) {
                    if (Granular.Extensions.DoubleExtensions.IsNaN(fontSize)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-size");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "font-size", converter.ToPixelString(fontSize));
                    }
                },
                SetHtmlFontStyle: function (element, fontStyle, converter) {
                    if (fontStyle === System.Windows.FontStyle.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-style");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "font-style", converter.ToFontStyleString(fontStyle));
                    }
                },
                SetHtmlFontWeight: function (element, fontWeight, converter) {
                    if (fontWeight === System.Windows.FontWeight.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-weight");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "font-weight", converter.ToFontWeightString(fontWeight));
                    }
                },
                SetHtmlFontStretch: function (element, fontStretch, converter) {
                    if (fontStretch === System.Windows.FontStretch.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-stretch");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "font-stretch", converter.ToFontStretchString(fontStretch));
                    }
                },
                SetHtmlTextAlignment: function (element, textAlignment, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "text-align", converter.ToTextAlignmentString(textAlignment));
                },
                SetHtmlTextTrimming: function (element, textTrimming) {
                    if (textTrimming === System.Windows.TextTrimming.None) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "text-overflow");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "text-overflow", "ellipsis");
                    }
                },
                SetHtmlTextWrapping: function (element, textWrapping, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "white-space", converter.ToWhiteSpaceString(textWrapping));
                },
                SetHtmlHorizontalScrollBarVisibility: function (element, scrollBarVisibility, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "overflow-x", converter.ToOverflowString(scrollBarVisibility));
                },
                SetHtmlVerticalScrollBarVisibility: function (element, scrollBarVisibility, converter) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "overflow-y", converter.ToOverflowString(scrollBarVisibility));
                },
                SetHtmlBackgroundImage: function (element, url, converter, factory) {
                    if (Granular.Extensions.StringExtensions.IsNullOrEmpty(url)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "background-image");
                    } else {
                        Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(element, "background-image", converter.ToUrlString(url));
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Host.HtmlValueConverter", {
        statics: {
            methods: {
                ScaleGradientStops$1: function (gradientStops, startPoint, endPoint, targetSize) {
                    if (startPoint.X === endPoint.X) {
                        return startPoint.Y < endPoint.Y ? Granular.Host.HtmlValueConverter.ScaleGradientStops(gradientStops, startPoint.Y / targetSize.Height, endPoint.Y / targetSize.Height) : Granular.Host.HtmlValueConverter.ScaleGradientStops(gradientStops, 1 - startPoint.Y / targetSize.Height, 1 - endPoint.Y / targetSize.Height);
                    }

                    if (startPoint.Y === endPoint.Y) {
                        return startPoint.X < endPoint.X ? Granular.Host.HtmlValueConverter.ScaleGradientStops(gradientStops, startPoint.X / targetSize.Width, endPoint.X / targetSize.Width) : Granular.Host.HtmlValueConverter.ScaleGradientStops(gradientStops, 1 - startPoint.X / targetSize.Width, 1 - endPoint.X / targetSize.Width);
                    }

                    var direction = System.Windows.Point.op_Subtraction(endPoint, startPoint);
                    var directionLength = System.Windows.PointExtensions.GetLength(direction);

                    var sin = direction.Y / directionLength;
                    var cos = direction.X / directionLength;

                    var generatedImageWidth = Granular.Extensions.DoubleExtensions.Abs(cos) * targetSize.Width + Granular.Extensions.DoubleExtensions.Abs(sin) * targetSize.Height;
                    var generatedImageHeight = Granular.Extensions.DoubleExtensions.Abs(sin) * targetSize.Width + Granular.Extensions.DoubleExtensions.Abs(cos) * targetSize.Height;

                    var matrix = System.Windows.Media.Matrix.op_Multiply(System.Windows.Media.Matrix.op_Multiply(System.Windows.Media.Matrix.op_Multiply(System.Windows.Media.Matrix.TranslationMatrix(-0.5, -0.5), System.Windows.Media.Matrix.ScalingMatrix(generatedImageWidth, generatedImageHeight)), new System.Windows.Media.Matrix(cos, sin, -sin, cos, 0, 0)), System.Windows.Media.Matrix.TranslationMatrix(targetSize.Width / 2, targetSize.Height / 2));

                    var relativeStart = System.Windows.Media.Matrix.op_Multiply$1(startPoint, matrix.Inverse);
                    var relativeEnd = System.Windows.Media.Matrix.op_Multiply$1(endPoint, matrix.Inverse);

                    return Granular.Host.HtmlValueConverter.ScaleGradientStops(gradientStops, relativeStart.X, relativeEnd.X);
                },
                ScaleGradientStops: function (gradientStops, start, end) {
                    return System.Linq.Enumerable.from(gradientStops).select(function (gradientStop) {
                            return new System.Windows.Media.GradientStop.$ctor1(gradientStop.Color, start + gradientStop.Offset * (end - start));
                        }).ToArray(System.Windows.Media.GradientStop);
                },
                GetReflectedGradientStops: function (gradientStops) {
                    return System.Linq.Enumerable.from(gradientStops).select($asm.$.Granular.Host.HtmlValueConverter.f1).concat(System.Linq.Enumerable.from(gradientStops).select($asm.$.Granular.Host.HtmlValueConverter.f2).reverse()).ToArray(System.Windows.Media.GradientStop);
                }
            }
        },
        methods: {
            ToPixelString: function (value) {
                if (Granular.Extensions.DoubleExtensions.IsNaN(value) || !isFinite(value)) {
                    throw new Granular.Exception("Can't convert {0} to pixel string", [Bridge.box(value, System.Double, System.Double.format, System.Double.getHashCode)]);
                }

                return System.String.format("{0}px", [Bridge.box(Bridge.Math.round(value, 2, 6), System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ToPixelString$1: function (point) {
                return System.String.format("{0} {1}", this.ToPixelString(point.X), this.ToPixelString(point.Y));
            },
            ToPixelString$2: function (size) {
                return System.String.format("{0} {1}", this.ToPixelString(size.Width), this.ToPixelString(size.Height));
            },
            ToPixelString$3: function (thickness) {
                return thickness.IsUniform ? this.ToPixelString(thickness.Top) : System.String.format("{0} {1} {2} {3}", this.ToPixelString(thickness.Top), this.ToPixelString(thickness.Right), this.ToPixelString(thickness.Bottom), this.ToPixelString(thickness.Left));
            },
            ToPercentString: function (value) {
                if (Granular.Extensions.DoubleExtensions.IsNaN(value)) {
                    throw new Granular.Exception("Can't convert Double.NaN to percent string");
                }

                return System.String.format("{0}%", [Bridge.box(Bridge.Math.round(value * 100, 2, 6), System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ToPercentString$1: function (point) {
                return System.String.format("{0} {1}", this.ToPercentString(point.X), this.ToPercentString(point.Y));
            },
            ToDegreesString: function (value) {
                if (Granular.Extensions.DoubleExtensions.IsNaN(value)) {
                    throw new Granular.Exception("Can't convert Double.NaN to degrees string");
                }

                return System.String.format("{0}deg", [Bridge.box(Bridge.Math.round(value, 2, 6), System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ToImplicitValueString: function (value) {
                return System.String.format("{0}", [Bridge.box(Bridge.Math.round(value, 2, 6), System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ToImplicitValueString$1: function (point) {
                return System.String.format("{0} {1}", this.ToImplicitValueString(point.X), this.ToImplicitValueString(point.Y));
            },
            ToImplicitValueString$2: function (thickness) {
                return System.String.format("{0} {1} {2} {3}", this.ToImplicitValueString(thickness.Top), this.ToImplicitValueString(thickness.Right), this.ToImplicitValueString(thickness.Bottom), this.ToImplicitValueString(thickness.Left));
            },
            ToColorString: function (color) {
                return color.A === 255 ? System.String.format("#{0:x2}{1:x2}{2:x2}", Bridge.box(color.R, System.Byte), Bridge.box(color.G, System.Byte), Bridge.box(color.B, System.Byte)) : System.String.format("rgba({0}, {1}, {2}, {3})", Bridge.box(color.R, System.Byte), Bridge.box(color.G, System.Byte), Bridge.box(color.B, System.Byte), Bridge.box(Bridge.Math.round(color.A / 255, 2, 6), System.Double, System.Double.format, System.Double.getHashCode));
            },
            ToColorString$1: function (brush) {
                return this.ToColorString(System.Windows.Media.ColorExtensions.ApplyOpacity(brush.Color, brush.Opacity));
            },
            ToUrlString: function (url) {
                return System.String.format("url({0})", [url]);
            },
            ToLinearGradientString: function (brush, targetRect) {
                if (System.Windows.Size.op_Equality(targetRect.Size, System.Windows.Size.Zero)) {
                    return "";
                }

                var gradientStops = brush.GradientStops;

                if (brush.SpreadMethod === System.Windows.Media.GradientSpreadMethod.Reflect) {
                    gradientStops = Granular.Host.HtmlValueConverter.GetReflectedGradientStops(brush.GradientStops);
                }

                var startPoint = brush.StartPoint;
                var endPoint = brush.EndPoint;

                if (brush.MappingMode === System.Windows.Media.BrushMappingMode.Absolute) {
                    startPoint = System.Windows.Point.op_Subtraction(startPoint, targetRect.Location);
                    endPoint = System.Windows.Point.op_Subtraction(endPoint, targetRect.Location);
                } else {
                    startPoint = System.Windows.Point.op_Multiply$2(startPoint, targetRect.Size);
                    endPoint = System.Windows.Point.op_Multiply$2(endPoint, targetRect.Size);
                }

                gradientStops = Granular.Host.HtmlValueConverter.ScaleGradientStops$1(gradientStops, startPoint, endPoint, targetRect.Size);

                var angle = System.Windows.PointExtensions.GetAngle((System.Windows.Point.op_Subtraction(endPoint, startPoint)));
                var gradientType = brush.SpreadMethod === System.Windows.Media.GradientSpreadMethod.Repeat ? "repeating-linear-gradient" : "linear-gradient";
                return System.String.format("{0}({1}, {2})", gradientType, this.ToDegreesString(90 + 180 * (angle / Math.PI)), this.ToColorStopsString(gradientStops));
            },
            ToRadialGradientString: function (brush) {
                var gradientStops = brush.SpreadMethod === System.Windows.Media.GradientSpreadMethod.Reflect ? Granular.Host.HtmlValueConverter.GetReflectedGradientStops(brush.GradientStops) : brush.GradientStops;

                var gradientType = brush.SpreadMethod === System.Windows.Media.GradientSpreadMethod.Repeat ? "repeating-radial-gradient" : "radial-gradient";
                return System.String.format("{0}(ellipse {1} {2} at {3}, {4})", gradientType, this.ToPercentString(brush.RadiusX), this.ToPercentString(brush.RadiusY), this.ToPercentString$1(brush.GradientOrigin), this.ToColorStopsString(gradientStops));
            },
            ToColorStopsString: function (gradientStops) {
                return System.Linq.Enumerable.from(gradientStops).select(Bridge.fn.bind(this, $asm.$.Granular.Host.HtmlValueConverter.f3)).defaultIfEmpty("").aggregate($asm.$.Granular.Host.HtmlValueConverter.f4);
            },
            ToImageString$1: function (brush, targetRect) {
                return this.ToLinearGradientString(brush, targetRect);
            },
            ToImageString$2: function (brush) {
                return this.ToRadialGradientString(brush);
            },
            ToImageString: function (brush, factory) {
                return Bridge.cast(brush.ImageSource.GetRenderResource(factory), Granular.Host.Render.HtmlImageSourceRenderResource).Url;
            },
            ToFontStyleString: function (fontStyle) {
                switch (fontStyle) {
                    case System.Windows.FontStyle.Normal: 
                        return "normal";
                    case System.Windows.FontStyle.Italic: 
                        return "italic";
                    case System.Windows.FontStyle.Oblique: 
                        return "oblique";
                }

                throw new Granular.Exception("Unexpected FontStyle \"{0}\"", [Bridge.box(fontStyle, System.Windows.FontStyle, System.Enum.toStringFn(System.Windows.FontStyle))]);
            },
            ToFontStretchString: function (fontStretch) {
                switch (fontStretch) {
                    case System.Windows.FontStretch.UltraCondensed: 
                        return "ultra-condensed";
                    case System.Windows.FontStretch.ExtraCondensed: 
                        return "extra-condensed";
                    case System.Windows.FontStretch.Condensed: 
                        return "condensed";
                    case System.Windows.FontStretch.SemiCondensed: 
                        return "semi-condensed";
                    case System.Windows.FontStretch.Medium: 
                    case System.Windows.FontStretch.Normal: 
                        return "normal";
                    case System.Windows.FontStretch.SemiExpanded: 
                        return "semi-expanded";
                    case System.Windows.FontStretch.Expanded: 
                        return "expanded";
                    case System.Windows.FontStretch.ExtraExpanded: 
                        return "extra-expanded";
                    case System.Windows.FontStretch.UltraExpanded: 
                        return "ultra-expanded";
                }

                throw new Granular.Exception("Unexpected FontStretch \"{0}\"", [Bridge.box(fontStretch, System.Windows.FontStretch, System.Enum.toStringFn(System.Windows.FontStretch))]);
            },
            ToFontWeightString: function (fontWeight) {
                switch (fontWeight) {
                    case System.Windows.FontWeight.Thin: 
                        return "100";
                    case System.Windows.FontWeight.ExtraLight: 
                    case System.Windows.FontWeight.UltraLight: 
                        return "200";
                    case System.Windows.FontWeight.Light: 
                        return "300";
                    case System.Windows.FontWeight.Normal: 
                    case System.Windows.FontWeight.Regular: 
                        return "400";
                    case System.Windows.FontWeight.Medium: 
                        return "500";
                    case System.Windows.FontWeight.DemiBold: 
                    case System.Windows.FontWeight.SemiBold: 
                        return "600";
                    case System.Windows.FontWeight.Bold: 
                        return "700";
                    case System.Windows.FontWeight.ExtraBold: 
                    case System.Windows.FontWeight.UltraBold: 
                        return "800";
                    case System.Windows.FontWeight.Black: 
                    case System.Windows.FontWeight.Heavy: 
                        return "900";
                    case System.Windows.FontWeight.ExtraBlack: 
                    case System.Windows.FontWeight.UltraBlack: 
                        return "950";
                }

                throw new Granular.Exception("Unexpected FontWeight \"{0}\"", [Bridge.box(fontWeight, System.Windows.FontWeight, System.Enum.toStringFn(System.Windows.FontWeight))]);
            },
            ToTextAlignmentString: function (textAlignment) {
                switch (textAlignment) {
                    case System.Windows.TextAlignment.Left: 
                        return "left";
                    case System.Windows.TextAlignment.Right: 
                        return "right";
                    case System.Windows.TextAlignment.Center: 
                        return "center";
                    case System.Windows.TextAlignment.Justify: 
                        return "justify";
                }

                throw new Granular.Exception("Unexpected TextAlignment \"{0}\"", [Bridge.box(textAlignment, System.Windows.TextAlignment, System.Enum.toStringFn(System.Windows.TextAlignment))]);
            },
            ToOverflowString: function (scrollBarVisibility) {
                switch (scrollBarVisibility) {
                    case System.Windows.Controls.ScrollBarVisibility.Disabled: 
                        return "hidden";
                    case System.Windows.Controls.ScrollBarVisibility.Auto: 
                        return "auto";
                    case System.Windows.Controls.ScrollBarVisibility.Hidden: 
                        return "hidden";
                    case System.Windows.Controls.ScrollBarVisibility.Visible: 
                        return "scroll";
                }

                throw new Granular.Exception("Unexpected ScrollBarVisibility \"{0}\"", [Bridge.box(scrollBarVisibility, System.Windows.Controls.ScrollBarVisibility, System.Enum.toStringFn(System.Windows.Controls.ScrollBarVisibility))]);
            },
            ToHtmlContentString: function (value) {
                return System.String.replaceAll(value, "\n", "<br/>");
            },
            ToWrapString: function (textWrapping) {
                switch (textWrapping) {
                    case System.Windows.TextWrapping.Wrap: 
                        return "soft";
                    case System.Windows.TextWrapping.NoWrap: 
                        return "off";
                }

                throw new Granular.Exception("Unexpected TextWrapping \"{0}\"", [Bridge.box(textWrapping, System.Windows.TextWrapping, System.Enum.toStringFn(System.Windows.TextWrapping))]);
            },
            ToWhiteSpaceString: function (textWrapping) {
                switch (textWrapping) {
                    case System.Windows.TextWrapping.Wrap: 
                        return "pre-wrap";
                    case System.Windows.TextWrapping.NoWrap: 
                        return "pre";
                }

                throw new Granular.Exception("Unexpected TextWrapping \"{0}\"", [Bridge.box(textWrapping, System.Windows.TextWrapping, System.Enum.toStringFn(System.Windows.TextWrapping))]);
            },
            ToFontFamilyNamesString: function (fontFamily) {
                return System.Linq.Enumerable.from(fontFamily.FamilyNames).select($asm.$.Granular.Host.HtmlValueConverter.f5).aggregate($asm.$.Granular.Host.HtmlValueConverter.f4);
            },
            ToBooleanString: function (value) {
                return value ? "true" : "false";
            },
            ToMimeTypeString: function (extension) {
                switch (extension) {
                    case "gif": 
                        return "image/gif";
                    case "jpg": 
                        return "image/jpeg";
                    case "png": 
                        return "image/png";
                    case "svg": 
                        return "image/svg+xml";
                }

                return "";
            },
            ToCursorString: function (cursor, factory) {
                if (cursor == null) {
                    return "default";
                }

                if (cursor.ImageSource != null) {
                    var urlString = this.ToUrlString(Bridge.cast(cursor.ImageSource.GetRenderResource(factory), Granular.Host.Render.HtmlImageSourceRenderResource).Url);

                    return !System.Windows.Point.IsNullOrEmpty(cursor.Hotspot) ? System.String.format("{0} {1}, default", urlString, this.ToImplicitValueString$1(cursor.Hotspot)) : System.String.format("{0}, default", [urlString]);
                }

                switch (cursor.CursorType) {
                    case System.Windows.Input.CursorType.None: 
                        return "none";
                    case System.Windows.Input.CursorType.No: 
                        return "not-allowed";
                    case System.Windows.Input.CursorType.Arrow: 
                        return "default";
                    case System.Windows.Input.CursorType.AppStarting: 
                        return "progress";
                    case System.Windows.Input.CursorType.Cross: 
                        return "crosshair";
                    case System.Windows.Input.CursorType.Help: 
                        return "help";
                    case System.Windows.Input.CursorType.IBeam: 
                        return "text";
                    case System.Windows.Input.CursorType.SizeAll: 
                        return "move";
                    case System.Windows.Input.CursorType.SizeNESW: 
                        return "nesw-resize";
                    case System.Windows.Input.CursorType.SizeNS: 
                        return "ns-resize";
                    case System.Windows.Input.CursorType.SizeNWSE: 
                        return "nwse-resize";
                    case System.Windows.Input.CursorType.SizeWE: 
                        return "ew-resize";
                    case System.Windows.Input.CursorType.Wait: 
                        return "wait";
                    case System.Windows.Input.CursorType.Hand: 
                        return "pointer";
                    case System.Windows.Input.CursorType.UpArrow: 
                    case System.Windows.Input.CursorType.Pen: 
                    case System.Windows.Input.CursorType.ScrollNS: 
                    case System.Windows.Input.CursorType.ScrollWE: 
                    case System.Windows.Input.CursorType.ScrollAll: 
                    case System.Windows.Input.CursorType.ScrollN: 
                    case System.Windows.Input.CursorType.ScrollS: 
                    case System.Windows.Input.CursorType.ScrollW: 
                    case System.Windows.Input.CursorType.ScrollE: 
                    case System.Windows.Input.CursorType.ScrollNW: 
                    case System.Windows.Input.CursorType.ScrollNE: 
                    case System.Windows.Input.CursorType.ScrollSW: 
                    case System.Windows.Input.CursorType.ScrollSE: 
                    case System.Windows.Input.CursorType.ArrowCD: 
                        return "default";
                }

                throw new Granular.Exception("Unexpected CursorType \"{0}\"", [Bridge.box(cursor.CursorType, System.Windows.Input.CursorType, System.Enum.toStringFn(System.Windows.Input.CursorType))]);
            },
            ToTransformString: function (matrix) {
                return System.String.format("matrix({0}, {1}, {2}, {3}, {4}, {5}", Bridge.box(Bridge.Math.round(matrix.M11, 2, 6), System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(Bridge.Math.round(matrix.M12, 2, 6), System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(Bridge.Math.round(matrix.M21, 2, 6), System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(Bridge.Math.round(matrix.M22, 2, 6), System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(Bridge.Math.round(matrix.OffsetX, 2, 6), System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(Bridge.Math.round(matrix.OffsetY, 2, 6), System.Double, System.Double.format, System.Double.getHashCode));
            },
            ConvertBackMouseButton: function (buttonIndex) {
                switch (buttonIndex) {
                    case 0: 
                        return System.Windows.Input.MouseButton.Left;
                    case 1: 
                        return System.Windows.Input.MouseButton.Middle;
                    case 2: 
                        return System.Windows.Input.MouseButton.Right;
                }

                throw new Granular.Exception("Unexpected button index \"{0}\"", [Bridge.box(buttonIndex, System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ConvertBackKey: function (keyCode, location) {
                switch (keyCode) {
                    case 8: 
                        return System.Windows.Input.Key.Back;
                    case 9: 
                        return System.Windows.Input.Key.Tab;
                    case 13: 
                        return System.Windows.Input.Key.Enter;
                    case 16: 
                        return location === 1 ? System.Windows.Input.Key.LeftShift : System.Windows.Input.Key.RightShift;
                    case 17: 
                        return location === 1 ? System.Windows.Input.Key.LeftCtrl : System.Windows.Input.Key.RightCtrl;
                    case 18: 
                        return location === 1 ? System.Windows.Input.Key.LeftAlt : System.Windows.Input.Key.RightAlt;
                    case 19: 
                        return System.Windows.Input.Key.Pause;
                    case 20: 
                        return System.Windows.Input.Key.CapsLock;
                    case 27: 
                        return System.Windows.Input.Key.Escape;
                    case 32: 
                        return System.Windows.Input.Key.Space;
                    case 33: 
                        return System.Windows.Input.Key.PageUp;
                    case 34: 
                        return System.Windows.Input.Key.PageDown;
                    case 35: 
                        return System.Windows.Input.Key.End;
                    case 36: 
                        return System.Windows.Input.Key.Home;
                    case 37: 
                        return System.Windows.Input.Key.Left;
                    case 38: 
                        return System.Windows.Input.Key.Up;
                    case 39: 
                        return System.Windows.Input.Key.Right;
                    case 40: 
                        return System.Windows.Input.Key.Down;
                    case 45: 
                        return System.Windows.Input.Key.Insert;
                    case 46: 
                        return System.Windows.Input.Key.Delete;
                    case 48: 
                        return System.Windows.Input.Key.D0;
                    case 49: 
                        return System.Windows.Input.Key.D1;
                    case 50: 
                        return System.Windows.Input.Key.D2;
                    case 51: 
                        return System.Windows.Input.Key.D3;
                    case 52: 
                        return System.Windows.Input.Key.D4;
                    case 53: 
                        return System.Windows.Input.Key.D5;
                    case 54: 
                        return System.Windows.Input.Key.D6;
                    case 55: 
                        return System.Windows.Input.Key.D7;
                    case 56: 
                        return System.Windows.Input.Key.D8;
                    case 57: 
                        return System.Windows.Input.Key.D9;
                    case 59: 
                        return System.Windows.Input.Key.OemSemicolon;
                    case 65: 
                        return System.Windows.Input.Key.A;
                    case 66: 
                        return System.Windows.Input.Key.B;
                    case 67: 
                        return System.Windows.Input.Key.C;
                    case 68: 
                        return System.Windows.Input.Key.D;
                    case 69: 
                        return System.Windows.Input.Key.E;
                    case 70: 
                        return System.Windows.Input.Key.F;
                    case 71: 
                        return System.Windows.Input.Key.G;
                    case 72: 
                        return System.Windows.Input.Key.H;
                    case 73: 
                        return System.Windows.Input.Key.I;
                    case 74: 
                        return System.Windows.Input.Key.J;
                    case 75: 
                        return System.Windows.Input.Key.K;
                    case 76: 
                        return System.Windows.Input.Key.L;
                    case 77: 
                        return System.Windows.Input.Key.M;
                    case 78: 
                        return System.Windows.Input.Key.N;
                    case 79: 
                        return System.Windows.Input.Key.O;
                    case 80: 
                        return System.Windows.Input.Key.P;
                    case 81: 
                        return System.Windows.Input.Key.Q;
                    case 82: 
                        return System.Windows.Input.Key.R;
                    case 83: 
                        return System.Windows.Input.Key.S;
                    case 84: 
                        return System.Windows.Input.Key.T;
                    case 85: 
                        return System.Windows.Input.Key.U;
                    case 86: 
                        return System.Windows.Input.Key.V;
                    case 87: 
                        return System.Windows.Input.Key.W;
                    case 88: 
                        return System.Windows.Input.Key.X;
                    case 89: 
                        return System.Windows.Input.Key.Y;
                    case 90: 
                        return System.Windows.Input.Key.Z;
                    case 91: 
                        return location === 1 ? System.Windows.Input.Key.LWin : System.Windows.Input.Key.RWin;
                    case 93: 
                        return System.Windows.Input.Key.Apps;
                    case 96: 
                        return System.Windows.Input.Key.NumPad0;
                    case 97: 
                        return System.Windows.Input.Key.NumPad1;
                    case 98: 
                        return System.Windows.Input.Key.NumPad2;
                    case 99: 
                        return System.Windows.Input.Key.NumPad3;
                    case 100: 
                        return System.Windows.Input.Key.NumPad4;
                    case 101: 
                        return System.Windows.Input.Key.NumPad5;
                    case 102: 
                        return System.Windows.Input.Key.NumPad6;
                    case 103: 
                        return System.Windows.Input.Key.NumPad7;
                    case 104: 
                        return System.Windows.Input.Key.NumPad8;
                    case 105: 
                        return System.Windows.Input.Key.NumPad9;
                    case 106: 
                        return System.Windows.Input.Key.Multiply;
                    case 107: 
                        return System.Windows.Input.Key.Add;
                    case 109: 
                        return System.Windows.Input.Key.Subtract;
                    case 110: 
                        return System.Windows.Input.Key.Decimal;
                    case 111: 
                        return System.Windows.Input.Key.Divide;
                    case 112: 
                        return System.Windows.Input.Key.F1;
                    case 113: 
                        return System.Windows.Input.Key.F2;
                    case 114: 
                        return System.Windows.Input.Key.F3;
                    case 115: 
                        return System.Windows.Input.Key.F4;
                    case 116: 
                        return System.Windows.Input.Key.F5;
                    case 117: 
                        return System.Windows.Input.Key.F6;
                    case 118: 
                        return System.Windows.Input.Key.F7;
                    case 119: 
                        return System.Windows.Input.Key.F8;
                    case 120: 
                        return System.Windows.Input.Key.F9;
                    case 121: 
                        return System.Windows.Input.Key.F10;
                    case 122: 
                        return System.Windows.Input.Key.F11;
                    case 123: 
                        return System.Windows.Input.Key.F12;
                    case 144: 
                        return System.Windows.Input.Key.NumLock;
                    case 145: 
                        return System.Windows.Input.Key.Scroll;
                    case 173: 
                        return System.Windows.Input.Key.OemMinus;
                    case 188: 
                        return System.Windows.Input.Key.OemComma;
                    case 190: 
                        return System.Windows.Input.Key.OemPeriod;
                    case 191: 
                        return System.Windows.Input.Key.OemQuestion;
                    case 192: 
                        return System.Windows.Input.Key.OemTilde;
                    case 219: 
                        return System.Windows.Input.Key.OemOpenBrackets;
                    case 220: 
                        return System.Windows.Input.Key.OemPipe;
                    case 221: 
                        return System.Windows.Input.Key.OemCloseBrackets;
                    case 222: 
                        return System.Windows.Input.Key.OemQuotes;
                }

                return System.Windows.Input.Key.None;
            }
        }
    });

    Bridge.ns("Granular.Host.HtmlValueConverter", $asm.$);

    Bridge.apply($asm.$.Granular.Host.HtmlValueConverter, {
        f1: function (gradientStop) {
            return new System.Windows.Media.GradientStop.$ctor1(gradientStop.Color, gradientStop.Offset / 2);
        },
        f2: function (gradientStop) {
            return new System.Windows.Media.GradientStop.$ctor1(gradientStop.Color, 1.0 - gradientStop.Offset / 2);
        },
        f3: function (gradientStop) {
            return System.String.format("{0} {1}", this.ToColorString(gradientStop.Color), this.ToPercentString(gradientStop.Offset));
        },
        f4: function (s1, s2) {
            return System.String.format("{0}, {1}", s1, s2);
        },
        f5: function (familyName) {
            return System.String.format("\"{0}\"", [familyName]);
        }
    });

    Bridge.define("Granular.Host.HtmlValueConverterExtensions", {
        statics: {
            methods: {
                ToImageString: function (converter, brush, targetRect, factory) {
                    if (Bridge.is(brush, System.Windows.Media.LinearGradientBrush)) {
                        return converter.ToImageString$1(Bridge.cast(brush, System.Windows.Media.LinearGradientBrush), targetRect);
                    }

                    if (Bridge.is(brush, System.Windows.Media.RadialGradientBrush)) {
                        return converter.ToImageString$2(Bridge.cast(brush, System.Windows.Media.RadialGradientBrush));
                    }

                    if (Bridge.is(brush, System.Windows.Media.ImageBrush)) {
                        return converter.ToImageString(Bridge.cast(brush, System.Windows.Media.ImageBrush), factory);
                    }

                    throw new Granular.Exception("Unexpected brush type \"{0}\"", [Bridge.getType(brush)]);
                }
            }
        }
    });

    Bridge.define("Granular.Host.ImageElementContainer", {
        props: {
            HtmlElement: null
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                this.HtmlElement = document.createElement("div");
                this.HtmlElement.style.setProperty("visibility", "hidden");
                this.HtmlElement.style.setProperty("overflow", "hidden");
                this.HtmlElement.style.width = "0px";
                this.HtmlElement.style.height = "0px";
            }
        },
        methods: {
            Add: function (element) {
                this.HtmlElement.appendChild(element);
            },
            Remove: function (element) {
                this.HtmlElement.removeChild(element);
            }
        }
    });

    Bridge.define("Granular.Host.PresentationSource", {
        inherits: [System.Windows.IPresentationSource],
        fields: {
            converter: null,
            mouseDownHandled: false,
            mouseMoveHandled: false,
            mouseUpHandled: false,
            keyDownHandled: false,
            keyUpHandled: false
        },
        props: {
            RootElement: null,
            MouseDevice: null,
            KeyboardDevice: null,
            Title: {
                get: function () {
                    return window.document.title;
                },
                set: function (value) {
                    window.document.title = value;
                }
            }
        },
        alias: [
            "addHitTestInvalidated", "System$Windows$IPresentationSource$addHitTestInvalidated",
            "removeHitTestInvalidated", "System$Windows$IPresentationSource$removeHitTestInvalidated",
            "RootElement", "System$Windows$IPresentationSource$RootElement",
            "MouseDevice", "System$Windows$IPresentationSource$MouseDevice",
            "KeyboardDevice", "System$Windows$IPresentationSource$KeyboardDevice",
            "Title", "System$Windows$IPresentationSource$Title",
            "HitTest", "System$Windows$IPresentationSource$HitTest",
            "GetTimestamp", "System$Windows$IPresentationSource$GetTimestamp"
        ],
        ctors: {
            ctor: function (rootElement, htmlRenderElementFactory, converter, imageElementContainer, svgDefinitionContainer) {
                this.$initialize();
                this.RootElement = rootElement;
                this.converter = converter;

                this.RootElement.IsRootElement = true;

                this.MouseDevice = new System.Windows.Input.MouseDevice(this);
                this.KeyboardDevice = new System.Windows.Input.KeyboardDevice(this);

                this.MouseDevice.addCursorChanged(Bridge.fn.bind(this, function (sender, e) {
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(window.document.body, "cursor", converter.ToCursorString(this.MouseDevice.Cursor, htmlRenderElementFactory));
                }));
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(window.document.body, "cursor", converter.ToCursorString(this.MouseDevice.Cursor, htmlRenderElementFactory));

                window.onkeydown = Bridge.fn.cacheBind(this, this.OnKeyDown);
                window.onkeyup = Bridge.fn.cacheBind(this, this.OnKeyUp);
                window.onkeypress = Bridge.fn.cacheBind(this, this.PreventKeyboardHandled);
                window.onmousemove = Bridge.fn.cacheBind(this, this.OnMouseMove);
                window.onmousedown = Bridge.fn.cacheBind(this, this.OnMouseDown);
                window.onmouseup = Bridge.fn.cacheBind(this, this.OnMouseUp);
                window.onscroll = Bridge.fn.cacheBind(this, this.OnScroll);
                window.onfocus = Bridge.fn.bind(this, $asm.$.Granular.Host.PresentationSource.f1);
                window.onblur = Bridge.fn.bind(this, $asm.$.Granular.Host.PresentationSource.f2);
                window.onresize = Bridge.fn.bind(this, $asm.$.Granular.Host.PresentationSource.f3);
                window.onclick = Bridge.fn.cacheBind(this, this.PreventMouseHandled);
                window.oncontextmenu = Bridge.fn.cacheBind(this, this.PreventMouseHandled);
                window.addEventListener("ondblclick", Bridge.fn.cacheBind(this, this.OnDblClick));
                window.addEventListener("wheel", Bridge.fn.cacheBind(this, this.OnMouseWheel));

                this.SetRootElementSize();
                Bridge.cast(this.RootElement, System.Windows.FrameworkElement).Arrange(new System.Windows.Rect.ctor(window.innerWidth, window.innerHeight));

                var renderElement = Bridge.cast(this.RootElement.GetRenderElement(htmlRenderElementFactory), Granular.Host.Render.IHtmlRenderElement);
                renderElement.Granular$Host$Render$IHtmlRenderElement$Load();

                window.document.body.style.overflow = "hidden";
                window.document.body.appendChild(imageElementContainer.HtmlElement);
                window.document.body.appendChild(svgDefinitionContainer.HtmlElement);
                window.document.body.appendChild(renderElement.Granular$Host$Render$IHtmlRenderElement$HtmlElement);

                this.MouseDevice.Activate();
                this.KeyboardDevice.Activate();
            }
        },
        methods: {
            addHitTestInvalidated: function (value) { },
            removeHitTestInvalidated: function (value) { },
            SetRootElementSize: function () {
                Bridge.cast(this.RootElement, System.Windows.FrameworkElement).Width = window.innerWidth;
                Bridge.cast(this.RootElement, System.Windows.FrameworkElement).Height = window.innerHeight;
            },
            OnKeyDown: function (e) {
                var keyboardEvent = Bridge.cast(e, KeyboardEvent);

                var key = this.converter.ConvertBackKey(keyboardEvent.keyCode, keyboardEvent.location);

                this.keyDownHandled = this.ProcessKeyboardEvent(new System.Windows.Input.RawKeyboardEventArgs(key, System.Windows.Input.KeyStates.Down, keyboardEvent.repeat, this.GetTimestamp()));

                if (this.keyDownHandled) {
                    e.preventDefault();
                }

                return e;
            },
            OnKeyUp: function (e) {
                var keyboardEvent = Bridge.cast(e, KeyboardEvent);

                var key = this.converter.ConvertBackKey(keyboardEvent.keyCode, keyboardEvent.location);

                this.keyUpHandled = this.ProcessKeyboardEvent(new System.Windows.Input.RawKeyboardEventArgs(key, System.Windows.Input.KeyStates.None, keyboardEvent.repeat, this.GetTimestamp()));

                if (this.keyDownHandled || this.keyUpHandled) {
                    e.preventDefault();
                }

                return e;
            },
            PreventKeyboardHandled: function (e) {
                if (this.keyDownHandled || this.keyUpHandled) {
                    e.preventDefault();
                }

                return e;
            },
            OnMouseDown: function (e) {
                var mouseEvent = Bridge.cast(e, MouseEvent);

                var position = new System.Windows.Point.$ctor1(mouseEvent.pageX, mouseEvent.pageY);
                var button = this.converter.ConvertBackMouseButton(mouseEvent.button);

                this.mouseDownHandled = this.ProcessMouseEvent(new System.Windows.Input.RawMouseButtonEventArgs(button, System.Windows.Input.MouseButtonState.Pressed, position, this.GetTimestamp()));

                if (this.mouseDownHandled || this.MouseDevice.CaptureTarget != null) {
                    e.preventDefault();
                }

                return e;
            },
            OnMouseUp: function (e) {
                var mouseEvent = Bridge.cast(e, MouseEvent);

                var position = new System.Windows.Point.$ctor1(mouseEvent.pageX, mouseEvent.pageY);
                var button = this.converter.ConvertBackMouseButton(mouseEvent.button);

                this.mouseUpHandled = this.ProcessMouseEvent(new System.Windows.Input.RawMouseButtonEventArgs(button, System.Windows.Input.MouseButtonState.Released, position, this.GetTimestamp()));

                if (this.mouseDownHandled || this.mouseMoveHandled || this.mouseUpHandled || this.MouseDevice.CaptureTarget != null) {
                    e.preventDefault();
                }

                return e;
            },
            OnScroll: function (e) {
                var uiEvent = Bridge.cast(e, MouseEvent);
                var wheelEvent = Bridge.cast(e, WheelEvent);

                var position = new System.Windows.Point.$ctor1(uiEvent.pageX, uiEvent.pageY);
                var delta = (wheelEvent).deltaY > 0 ? -100 : 100;

                if (this.ProcessMouseEvent(new System.Windows.Input.RawMouseWheelEventArgs(delta, position, this.GetTimestamp()))) {
                    e.preventDefault();
                }

                return e;
            },
            OnMouseWheel: function (e) {
                this.OnScroll(e);
            },
            OnMouseMove: function (e) {
                if (!(Bridge.is(e, MouseEvent))) {
                    return e;
                }

                var mouseEvent = Bridge.cast(e, MouseEvent);

                var position = new System.Windows.Point.$ctor1(mouseEvent.pageX, mouseEvent.pageY);

                this.mouseMoveHandled = this.ProcessMouseEvent(new System.Windows.Input.RawMouseEventArgs(position, this.GetTimestamp()));

                if (this.mouseDownHandled || this.mouseMoveHandled || this.MouseDevice.CaptureTarget != null) {
                    e.preventDefault();
                }

                return e;
            },
            OnDblClick: function (e) {
                this.PreventMouseHandled(e);
            },
            PreventMouseHandled: function (e) {
                if (this.mouseDownHandled || this.mouseMoveHandled || this.mouseUpHandled || this.MouseDevice.CaptureTarget != null) {
                    e.preventDefault();
                }

                return e;
            },
            HitTest: function (position) {
                return Bridge.as(this.RootElement.HitTest(position), System.Windows.IInputElement);
            },
            GetTimestamp: function () {
                return 0;
            },
            ProcessKeyboardEvent: function (keyboardEventArgs) {
                return System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(System.Boolean, Bridge.fn.bind(this, function () {
                    return this.KeyboardDevice.ProcessRawEvent(keyboardEventArgs);
                }), System.Windows.Threading.DispatcherPriority.Input);
            },
            ProcessMouseEvent: function (mouseEventArgs) {
                return System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(System.Boolean, Bridge.fn.bind(this, function () {
                    return this.MouseDevice.ProcessRawEvent(mouseEventArgs);
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }
    });

    Bridge.ns("Granular.Host.PresentationSource", $asm.$);

    Bridge.apply($asm.$.Granular.Host.PresentationSource, {
        f1: function (e) {
            this.MouseDevice.Activate();
            return null;
        },
        f2: function (e) {
            this.MouseDevice.Deactivate();
            return null;
        },
        f3: function (e) {
            this.SetRootElementSize();
            return null;
        }
    });

    Bridge.define("Granular.Host.PresentationSourceFactory", {
        inherits: [System.Windows.IPresentationSourceFactory],
        fields: {
            presentationSources: null,
            htmlRenderElementFactory: null,
            htmlValueConverter: null,
            imageElementContainer: null,
            svgDefinitionContainer: null
        },
        alias: [
            "CreatePresentationSource", "System$Windows$IPresentationSourceFactory$CreatePresentationSource",
            "GetPresentationSourceFromElement", "System$Windows$IPresentationSourceFactory$GetPresentationSourceFromElement"
        ],
        ctors: {
            ctor: function (htmlRenderElementFactory, htmlValueConverter, imageElementContainer, svgDefinitionContainer) {
                this.$initialize();
                this.htmlRenderElementFactory = htmlRenderElementFactory;
                this.htmlValueConverter = htmlValueConverter;
                this.imageElementContainer = imageElementContainer;
                this.svgDefinitionContainer = svgDefinitionContainer;

                this.presentationSources = new (System.Collections.Generic.List$1(Granular.Host.PresentationSource)).ctor();
            }
        },
        methods: {
            CreatePresentationSource: function (rootElement) {
                var presentationSource = new Granular.Host.PresentationSource(rootElement, this.htmlRenderElementFactory, this.htmlValueConverter, this.imageElementContainer, this.svgDefinitionContainer);
                this.presentationSources.add(presentationSource);

                return presentationSource;
            },
            GetPresentationSourceFromElement: function (element) {
                while (Bridge.is(element.VisualParent, System.Windows.FrameworkElement)) {
                    element = Bridge.cast(element.VisualParent, System.Windows.FrameworkElement);
                }

                return System.Linq.Enumerable.from(this.presentationSources).firstOrDefault(function (presentationSource) {
                        return Bridge.referenceEquals(presentationSource.RootElement, element);
                    }, null);
            }
        }
    });

    Bridge.define("Granular.Host.Render.IHtmlRenderElement", {
        $kind: "interface"
    });

    Bridge.define("Granular.Host.Render.HtmlRenderResource", {
        fields: {
            referenceCount: 0
        },
        props: {
            HtmlElement: null,
            IsLoaded: {
                get: function () {
                    return this.referenceCount > 0;
                }
            }
        },
        ctors: {
            ctor: function (htmlElement) {
                this.$initialize();
                this.HtmlElement = htmlElement;
            }
        },
        methods: {
            Load: function () {
                this.referenceCount = (this.referenceCount + 1) | 0;

                if (this.referenceCount === 1) {
                    this.OnLoad();
                }
            },
            Unload: function () {
                this.referenceCount = (this.referenceCount - 1) | 0;

                if (this.referenceCount === 0) {
                    this.OnUnload();
                }
            },
            OnLoad: function () { },
            OnUnload: function () { }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlImageSourceRenderResource", {
        inherits: [System.Windows.Media.IImageSourceRenderResource],
        fields: {
            state: 0,
            objectFactory: null,
            container: null,
            image: null
        },
        events: {
            StateChanged: null
        },
        props: {
            State: {
                get: function () {
                    return this.state;
                },
                set: function (value) {
                    if (this.state === value) {
                        return;
                    }

                    this.state = value;
                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.StateChanged, this);
                }
            },
            Size: null,
            Url: null,
            SourceRect: null,
            ImageSize: null
        },
        alias: [
            "addStateChanged", "System$Windows$Media$IImageSourceRenderResource$addStateChanged",
            "removeStateChanged", "System$Windows$Media$IImageSourceRenderResource$removeStateChanged",
            "State", "System$Windows$Media$IImageSourceRenderResource$State",
            "Size", "System$Windows$Media$IImageSourceRenderResource$Size",
            "Initialize", "System$Windows$Media$IImageSourceRenderResource$Initialize"
        ],
        ctors: {
            ctor: function (objectFactory, container) {
                this.$initialize();
                this.objectFactory = objectFactory;
                this.container = container;

                this.SourceRect = System.Windows.Rect.Empty;
                this.ImageSize = System.Windows.Size.Empty;
                this.Size = System.Windows.Size.Empty;
            }
        },
        methods: {
            Initialize: function (uri, sourceRect) {
                this.SourceRect = sourceRect;

                if (Bridge.referenceEquals(System.UriExtensions.GetScheme(uri), "pack")) {
                    this.Url = this.objectFactory.GetObjectUrl(uri);
                } else {
                    this.Url = System.UriExtensions.GetAbsoluteUri(uri);
                    this.State = System.Windows.Media.RenderImageState.DownloadProgress;
                }

                this.image = document.createElement("img");
                this.image.addEventListener("load", Bridge.fn.cacheBind(this, this.OnImageLoad));
                this.image.addEventListener("error", Bridge.fn.cacheBind(this, this.OnImageError));
                this.image.addEventListener("abort", Bridge.fn.cacheBind(this, this.OnImageAbort));

                this.container.Add(this.image);

                this.image.setAttribute("src", this.Url);
            },
            OnImageLoad: function () {
                this.ImageSize = new System.Windows.Size(this.image.clientWidth, this.image.clientHeight);
                this.Size = !System.Windows.RectExtensions.IsNullOrEmpty(this.SourceRect) ? this.SourceRect.Size : this.ImageSize;
                this.State = System.Windows.Media.RenderImageState.DownloadCompleted;
                this.container.Remove(this.image);
            },
            OnImageError: function () {
                this.State = System.Windows.Media.RenderImageState.DownloadFailed;
                this.container.Remove(this.image);
            },
            OnImageAbort: function () {
                this.State = System.Windows.Media.RenderImageState.DownloadFailed;
                this.container.Remove(this.image);
            }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlRenderElementFactory", {
        inherits: [System.Windows.Media.IRenderElementFactory],
        fields: {
            renderQueue: null,
            htmlValueConverter: null,
            imageElementContainer: null,
            embeddedResourceObjectFactory: null,
            svgValueConverter: null,
            svgDefinitionContainer: null
        },
        alias: [
            "CreateVisualRenderElement", "System$Windows$Media$IRenderElementFactory$CreateVisualRenderElement",
            "CreateDrawingRenderElement", "System$Windows$Media$IRenderElementFactory$CreateDrawingRenderElement",
            "CreateDrawingGeometryRenderElement", "System$Windows$Media$IRenderElementFactory$CreateDrawingGeometryRenderElement",
            "CreateTextBoxRenderElement", "System$Windows$Media$IRenderElementFactory$CreateTextBoxRenderElement",
            "CreateTextBlockRenderElement", "System$Windows$Media$IRenderElementFactory$CreateTextBlockRenderElement",
            "CreateBorderRenderElement", "System$Windows$Media$IRenderElementFactory$CreateBorderRenderElement",
            "CreateImageRenderElement", "System$Windows$Media$IRenderElementFactory$CreateImageRenderElement",
            "CreateDrawingContainerRenderElement", "System$Windows$Media$IRenderElementFactory$CreateDrawingContainerRenderElement",
            "CreateDrawingImageRenderElement", "System$Windows$Media$IRenderElementFactory$CreateDrawingImageRenderElement",
            "CreateDrawingTextRenderElement", "System$Windows$Media$IRenderElementFactory$CreateDrawingTextRenderElement",
            "CreateSolidColorBrushRenderResource", "System$Windows$Media$IRenderElementFactory$CreateSolidColorBrushRenderResource",
            "CreateLinearGradientBrushRenderResource", "System$Windows$Media$IRenderElementFactory$CreateLinearGradientBrushRenderResource",
            "CreateRadialGradientBrushRenderResource", "System$Windows$Media$IRenderElementFactory$CreateRadialGradientBrushRenderResource",
            "CreateImageBrushRenderResource", "System$Windows$Media$IRenderElementFactory$CreateImageBrushRenderResource",
            "CreateImageSourceRenderResource", "System$Windows$Media$IRenderElementFactory$CreateImageSourceRenderResource",
            "CreateTransformRenderResource", "System$Windows$Media$IRenderElementFactory$CreateTransformRenderResource",
            "CreateGeometryRenderResource", "System$Windows$Media$IRenderElementFactory$CreateGeometryRenderResource"
        ],
        ctors: {
            ctor: function (renderQueue, htmlValueConverter, imageElementContainer, embeddedResourceObjectFactory, svgValueConverter, svgDefinitionContainer) {
                this.$initialize();
                this.renderQueue = renderQueue;
                this.htmlValueConverter = htmlValueConverter;
                this.imageElementContainer = imageElementContainer;
                this.embeddedResourceObjectFactory = embeddedResourceObjectFactory;
                this.svgValueConverter = svgValueConverter;
                this.svgDefinitionContainer = svgDefinitionContainer;
            }
        },
        methods: {
            CreateVisualRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlVisualRenderElement(owner, this, this.renderQueue, this.htmlValueConverter);
            },
            CreateDrawingRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlDrawingRenderElement(this.renderQueue, this.svgValueConverter);
            },
            CreateDrawingGeometryRenderElement: function () {
                return new Granular.Host.Render.HtmlDrawingGeometryRenderElement(this, this.renderQueue, this.svgValueConverter);
            },
            CreateTextBoxRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlTextBoxRenderElement(this, this.renderQueue, this.htmlValueConverter);
            },
            CreateTextBlockRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlTextBlockRenderElement(this.renderQueue, this.htmlValueConverter);
            },
            CreateBorderRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlBorderRenderElement(this, this.renderQueue, this.htmlValueConverter);
            },
            CreateImageRenderElement: function (owner) {
                return new Granular.Host.Render.HtmlImageRenderElement(this, this.renderQueue, this.htmlValueConverter);
            },
            CreateDrawingContainerRenderElement: function () {
                return new Granular.Host.Render.HtmlDrawingContainerRenderElement(this, this.renderQueue, this.svgValueConverter);
            },
            CreateDrawingImageRenderElement: function () {
                return new Granular.Host.Render.HtmlDrawingImageRenderElement(this, this.renderQueue, this.svgValueConverter);
            },
            CreateDrawingTextRenderElement: function () {
                return new Granular.Host.Render.HtmlDrawingTextRenderElement(this, this.renderQueue, this.svgValueConverter);
            },
            CreateSolidColorBrushRenderResource: function () {
                return new Granular.Host.Render.HtmlSolidColorBrushRenderResource(this.renderQueue, this.svgValueConverter, this.svgDefinitionContainer);
            },
            CreateLinearGradientBrushRenderResource: function () {
                return new Granular.Host.Render.HtmlLinearGradientBrushRenderResource(this.renderQueue, this.svgValueConverter, this.svgDefinitionContainer);
            },
            CreateRadialGradientBrushRenderResource: function () {
                return new Granular.Host.Render.HtmlRadialGradientBrushRenderResource(this.renderQueue, this.svgValueConverter, this.svgDefinitionContainer);
            },
            CreateImageBrushRenderResource: function () {
                throw new System.NotImplementedException.ctor();
            },
            CreateImageSourceRenderResource: function () {
                return new Granular.Host.Render.HtmlImageSourceRenderResource(this.embeddedResourceObjectFactory, this.imageElementContainer);
            },
            CreateTransformRenderResource: function () {
                return new Granular.Host.Render.HtmlTransformRenderResource();
            },
            CreateGeometryRenderResource: function () {
                return new Granular.Host.Render.HtmlGeometryRenderResource(this, this.renderQueue, this.svgDefinitionContainer, this.svgValueConverter);
            }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlTransformRenderResource", {
        inherits: [System.Windows.Media.ITransformRenderResource],
        fields: {
            matrix: null
        },
        events: {
            MatrixChanged: null
        },
        props: {
            Matrix: {
                get: function () {
                    return this.matrix;
                },
                set: function (value) {
                    if (System.Windows.Media.Matrix.op_Equality(this.matrix, value)) {
                        return;
                    }

                    this.matrix = value;
                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.MatrixChanged, this);
                }
            }
        },
        alias: ["Matrix", "System$Windows$Media$ITransformRenderResource$Matrix"]
    });

    Bridge.define("Granular.Host.RenderQueue", {
        fields: {
            actions: null,
            isRenderScheduled: false
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                this.actions = new (System.Collections.Generic.List$1(Function)).ctor();
            }
        },
        methods: {
            InvokeAsync: function (action) {
                this.actions.add(action);
                this.RequestAnimationFrame();
            },
            RequestAnimationFrame: function () {
                if (this.isRenderScheduled) {
                    return;
                }

                this.isRenderScheduled = true;
                System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeAsync(Bridge.fn.bind(this, function () {
                    var $t;
                    this.isRenderScheduled = false;
                    var currentActions = this.actions;
                    this.actions = new (System.Collections.Generic.List$1(Function)).ctor();

                    $t = Bridge.getEnumerator(currentActions);
                    try {
                        while ($t.moveNext()) {
                            var action = $t.Current;
                            action();
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }}), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    });

    Bridge.define("Granular.Host.SvgDefinitionContainer", {
        fields: {
            renderQueue: null,
            definitionsElement: null,
            id: 0
        },
        props: {
            HtmlElement: null
        },
        ctors: {
            ctor: function (renderQueue) {
                this.$initialize();
                this.renderQueue = renderQueue;

                this.HtmlElement = Granular.Host.SvgDocument.CreateElement("svg");
                this.HtmlElement.style.setProperty("overflow", "hidden");
                this.HtmlElement.style.width = "0px";
                this.HtmlElement.style.height = "0px";

                this.definitionsElement = Granular.Host.SvgDocument.CreateElement("defs");
                this.HtmlElement.appendChild(this.definitionsElement);
            }
        },
        methods: {
            GetNextId: function () {
                this.id = (this.id + 1) | 0;
                return this.id;
            },
            Add: function (svgDefinition) {
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, function () {
                    this.definitionsElement.appendChild(svgDefinition.HtmlElement);
                }));
            },
            Remove: function (svgDefinition) {
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, function () {
                    this.definitionsElement.removeChild(svgDefinition.HtmlElement);
                }));
            }
        }
    });

    Bridge.define("Granular.Host.SvgDocument", {
        statics: {
            fields: {
                NamespaceUri: null,
                XlinkNamespaceUri: null
            },
            ctors: {
                init: function () {
                    this.NamespaceUri = "http://www.w3.org/2000/svg";
                    this.XlinkNamespaceUri = "http://www.w3.org/1999/xlink";
                }
            },
            methods: {
                CreateElement: function (qualifiedName) {
                    return document.createElementNS(Granular.Host.SvgDocument.NamespaceUri, qualifiedName);
                }
            }
        }
    });

    Bridge.define("Granular.Host.SvgElementExtensions", {
        statics: {
            methods: {
                SetSvgStartPoint: function (element, startPoint, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgPointAttributes(element, "x1", "y1", startPoint, converter);
                },
                SetSvgEndPoint: function (element, endPoint, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgPointAttributes(element, "x2", "y2", endPoint, converter);
                },
                SetSvgCenter: function (element, center, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgPointAttributes(element, "cx", "cy", center, converter);
                },
                SetSvgGradientOrigin: function (element, gradientOrigin, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgPointAttributes(element, "fx", "fy", gradientOrigin, converter);
                },
                SetSvgSpreadMethod: function (element, spreadMethod, converter) {
                    element.setAttribute("spreadMethod", converter.ToSpreadMethodString(spreadMethod));
                },
                SetSvgMappingMode: function (element, mappingMode, converter) {
                    element.setAttribute("gradientUnits", converter.ToGradientUnitsString(mappingMode));
                },
                SetSvgTransform: function (element, transform, converter) {
                    if (System.Windows.Media.MatrixExtensions.IsNullOrIdentity(transform)) {
                        element.removeAttribute("transform");
                    } else {
                        element.setAttribute("transform", converter.ToMatrixString(transform));
                    }
                },
                SetSvgLocation: function (element, location, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgPointAttributes(element, "x", "y", location, converter);
                },
                SetSvgSize: function (element, size, converter) {
                    if (System.Windows.SizeExtensions.IsNullOrEmpty(size)) {
                        element.removeAttribute("width");
                        element.removeAttribute("height");
                    } else {
                        element.setAttribute("width", converter.ToImplicitValueString(size.Width));
                        element.setAttribute("height", converter.ToImplicitValueString(size.Height));
                    }
                },
                SetSvgBounds: function (element, bounds, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgLocation(element, bounds.Location, converter);
                    Granular.Host.SvgElementExtensions.SetSvgSize(element, bounds.Size, converter);
                },
                SetSvgImageSource: function (element, imageSource, factory, converter) {
                    element.setAttributeNS(Granular.Host.SvgDocument.XlinkNamespaceUri, "href", converter.ToImageUrl(imageSource, factory));
                },
                SetSvgFill: function (element, brush) {
                    Granular.Host.SvgElementExtensions.SetSvgBrush(element, "fill", brush);
                },
                SetSvgStroke: function (element, brush) {
                    Granular.Host.SvgElementExtensions.SetSvgBrush(element, "stroke", brush);
                },
                SetSvgOpacity: function (element, opacity, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgAttribute(element, "opacity", opacity, converter);
                },
                SetSvgStrokeThickness: function (element, strokeThickness, converter) {
                    Granular.Host.SvgElementExtensions.SetSvgAttribute(element, "stroke-width", strokeThickness, converter);
                },
                SetSvgGeometry: function (element, geometry) {
                    if (geometry == null) {
                        element.removeAttribute("d");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "d", geometry.Data);
                    }
                },
                SetSvgClip: function (element, geometry) {
                    if (geometry == null) {
                        element.removeAttribute("clip-path");
                    } else {
                        element.setAttribute("clip-path", geometry.Uri);
                    }
                },
                SetSvgFontFamily: function (element, fontFamily, converter) {
                    if (!Granular.Compatibility.Linq.Enumerable.Any(System.String, fontFamily.FamilyNames)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-family");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "font-family", converter.ToFontFamilyNamesString(fontFamily));
                    }
                },
                SetSvgFontSize: function (element, fontSize, converter) {
                    if (Granular.Extensions.DoubleExtensions.IsNaN(fontSize)) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-size");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "font-size", converter.ToPixelString(fontSize));
                    }
                },
                SetSvgFontStyle: function (element, fontStyle, converter) {
                    if (fontStyle === System.Windows.FontStyle.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-style");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "font-style", converter.ToFontStyleString(fontStyle));
                    }
                },
                SetSvgFontWeight: function (element, fontWeight, converter) {
                    if (fontWeight === System.Windows.FontWeight.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-weight");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "font-weight", converter.ToFontWeightString(fontWeight));
                    }
                },
                SetSvgFontStretch: function (element, fontStretch, converter) {
                    if (fontStretch === System.Windows.FontStretch.Normal) {
                        Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(element, "font-stretch");
                    } else {
                        Granular.Host.SvgElementExtensions.SetSvgAttribute$1(element, "font-stretch", converter.ToFontStretchString(fontStretch));
                    }
                },
                SetSvgAttribute: function (element, attributeName, value, converter) {
                    if (Granular.Extensions.DoubleExtensions.IsNaN(value)) {
                        element.removeAttribute(attributeName);
                    } else {
                        element.setAttribute(attributeName, converter.ToImplicitValueString(value));
                    }
                },
                SetSvgAttribute$1: function (element, attributeName, value) {
                    if (Granular.Extensions.StringExtensions.IsNullOrEmpty(value)) {
                        element.removeAttribute(attributeName);
                    } else {
                        element.setAttribute(attributeName, value);
                    }
                },
                SetSvgPointAttributes: function (element, xAttributeName, yAttributeName, point, converter) {
                    if (System.Windows.PointExtensions.IsNullOrEmpty(point)) {
                        element.removeAttribute(xAttributeName);
                        element.removeAttribute(yAttributeName);
                    } else {
                        element.setAttribute(xAttributeName, converter.ToImplicitValueString(point.X));
                        element.setAttribute(yAttributeName, converter.ToImplicitValueString(point.Y));
                    }
                },
                SetSvgBrush: function (element, attributeName, brush) {
                    if (brush == null) {
                        element.removeAttribute(attributeName);
                    } else {
                        element.setAttribute(attributeName, brush.Uri);
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Host.SvgValueConverter", {
        methods: {
            ToImplicitValueString: function (value) {
                return System.Double.format(Bridge.Math.round(value, 2, 6));
            },
            ToPixelString: function (size) {
                return System.String.format("{0}px", [Bridge.box(size, System.Double, System.Double.format, System.Double.getHashCode)]);
            },
            ToColorString: function (color) {
                return System.String.format("#{0:x2}{1:x2}{2:x2}", Bridge.box(color.R, System.Byte), Bridge.box(color.G, System.Byte), Bridge.box(color.B, System.Byte));
            },
            ToSpreadMethodString: function (spreadMethod) {
                switch (spreadMethod) {
                    case System.Windows.Media.GradientSpreadMethod.Pad: 
                        return "pad";
                    case System.Windows.Media.GradientSpreadMethod.Reflect: 
                        return "reflect";
                    case System.Windows.Media.GradientSpreadMethod.Repeat: 
                        return "repeat";
                }

                throw new Granular.Exception("Unexpected GradientSpreadMethod \"{0}\"", [Bridge.box(spreadMethod, System.Windows.Media.GradientSpreadMethod, System.Enum.toStringFn(System.Windows.Media.GradientSpreadMethod))]);
            },
            ToPathDataString: function (geometry, factory) {
                return Bridge.cast(geometry.GetRenderResource(factory), Granular.Host.Render.HtmlGeometryRenderResource).Data;
            },
            ToImageUrl: function (imageSource, factory) {
                return Bridge.cast(imageSource.GetRenderResource(factory), Granular.Host.Render.HtmlImageSourceRenderResource).Url;
            },
            ToMatrixString: function (matrix) {
                return System.String.format("matrix({0}, {1}, {2}, {3}, {4}, {5})", this.ToImplicitValueString(matrix.M11), this.ToImplicitValueString(matrix.M12), this.ToImplicitValueString(matrix.M21), this.ToImplicitValueString(matrix.M22), this.ToImplicitValueString(matrix.OffsetX), this.ToImplicitValueString(matrix.OffsetY));
            },
            ToGradientUnitsString: function (mappingMode) {
                switch (mappingMode) {
                    case System.Windows.Media.BrushMappingMode.Absolute: 
                        return "userSpaceOnUse";
                    case System.Windows.Media.BrushMappingMode.RelativeToBoundingBox: 
                        return "objectBoundingBox";
                }

                throw new Granular.Exception("Unexpected BrushMappingMode \"{0}\"", [Bridge.box(mappingMode, System.Windows.Media.BrushMappingMode, System.Enum.toStringFn(System.Windows.Media.BrushMappingMode))]);
            },
            ToGradientTransformString: function (radiusX, radiusY) {
                return System.String.format("matrix({0},0,0,{1},{2},{3})", Bridge.box(radiusX * 2, System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(radiusY * 2, System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(0.5 - radiusX, System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(0.5 - radiusY, System.Double, System.Double.format, System.Double.getHashCode));
            },
            ToFontStyleString: function (fontStyle) {
                switch (fontStyle) {
                    case System.Windows.FontStyle.Normal: 
                        return "normal";
                    case System.Windows.FontStyle.Italic: 
                        return "italic";
                    case System.Windows.FontStyle.Oblique: 
                        return "oblique";
                }

                throw new Granular.Exception("Unexpected FontStyle \"{0}\"", [Bridge.box(fontStyle, System.Windows.FontStyle, System.Enum.toStringFn(System.Windows.FontStyle))]);
            },
            ToFontStretchString: function (fontStretch) {
                switch (fontStretch) {
                    case System.Windows.FontStretch.UltraCondensed: 
                        return "ultra-condensed";
                    case System.Windows.FontStretch.ExtraCondensed: 
                        return "extra-condensed";
                    case System.Windows.FontStretch.Condensed: 
                        return "condensed";
                    case System.Windows.FontStretch.SemiCondensed: 
                        return "semi-condensed";
                    case System.Windows.FontStretch.Medium: 
                    case System.Windows.FontStretch.Normal: 
                        return "normal";
                    case System.Windows.FontStretch.SemiExpanded: 
                        return "semi-expanded";
                    case System.Windows.FontStretch.Expanded: 
                        return "expanded";
                    case System.Windows.FontStretch.ExtraExpanded: 
                        return "extra-expanded";
                    case System.Windows.FontStretch.UltraExpanded: 
                        return "ultra-expanded";
                }

                throw new Granular.Exception("Unexpected FontStretch \"{0}\"", [Bridge.box(fontStretch, System.Windows.FontStretch, System.Enum.toStringFn(System.Windows.FontStretch))]);
            },
            ToFontFamilyNamesString: function (fontFamily) {
                return System.Linq.Enumerable.from(fontFamily.FamilyNames).select($asm.$.Granular.Host.SvgValueConverter.f1).aggregate($asm.$.Granular.Host.SvgValueConverter.f2);
            },
            ToFontWeightString: function (fontWeight) {
                switch (fontWeight) {
                    case System.Windows.FontWeight.Thin: 
                        return "100";
                    case System.Windows.FontWeight.ExtraLight: 
                    case System.Windows.FontWeight.UltraLight: 
                        return "200";
                    case System.Windows.FontWeight.Light: 
                        return "300";
                    case System.Windows.FontWeight.Normal: 
                    case System.Windows.FontWeight.Regular: 
                        return "400";
                    case System.Windows.FontWeight.Medium: 
                        return "500";
                    case System.Windows.FontWeight.DemiBold: 
                    case System.Windows.FontWeight.SemiBold: 
                        return "600";
                    case System.Windows.FontWeight.Bold: 
                        return "700";
                    case System.Windows.FontWeight.ExtraBold: 
                    case System.Windows.FontWeight.UltraBold: 
                        return "800";
                    case System.Windows.FontWeight.Black: 
                    case System.Windows.FontWeight.Heavy: 
                        return "900";
                    case System.Windows.FontWeight.ExtraBlack: 
                    case System.Windows.FontWeight.UltraBlack: 
                        return "950";
                }

                throw new Granular.Exception("Unexpected FontWeight \"{0}\"", [Bridge.box(fontWeight, System.Windows.FontWeight, System.Enum.toStringFn(System.Windows.FontWeight))]);
            }
        }
    });

    Bridge.ns("Granular.Host.SvgValueConverter", $asm.$);

    Bridge.apply($asm.$.Granular.Host.SvgValueConverter, {
        f1: function (familyName) {
            return System.String.format("\"{0}\"", [familyName]);
        },
        f2: function (s1, s2) {
            return System.String.format("{0}, {1}", s1, s2);
        }
    });

    Bridge.define("Granular.Host.TaskScheduler", {
        inherits: [System.Windows.Threading.ITaskScheduler],
        alias: ["ScheduleTask", "System$Windows$Threading$ITaskScheduler$ScheduleTask"],
        methods: {
            ScheduleTask: function (timeSpan, action) {
                var token = window.setTimeout(action, timeSpan.getTotalMilliseconds());

                return new Granular.Disposable(function () {
                    window.clearTimeout(token);
                });
            }
        }
    });

    Bridge.define("Granular.Host.TextMeasurementService", {
        inherits: [System.Windows.ITextMeasurementService],
        fields: {
            converter: null,
            htmlElement: null
        },
        alias: ["Measure", "System$Windows$ITextMeasurementService$Measure"],
        ctors: {
            ctor: function (converter) {
                this.$initialize();
                this.converter = converter;
            }
        },
        methods: {
            Measure: function (text, fontSize, typeface, maxWidth) {
                if (this.htmlElement == null) {
                    this.htmlElement = document.createElement("div");
                    document.body.appendChild(this.htmlElement);
                }

                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.htmlElement, "position", "absolute");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.htmlElement, "visibility", "hidden");
                Granular.Host.HtmlElementExtensions.SetHtmlFontSize(this.htmlElement, fontSize, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontFamily(this.htmlElement, typeface.FontFamily, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStretch(this.htmlElement, typeface.Stretch, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStyle(this.htmlElement, typeface.Style, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontWeight(this.htmlElement, typeface.Weight, this.converter);

                if (Granular.Extensions.DoubleExtensions.IsNaN(maxWidth) || !isFinite(maxWidth)) {
                    Granular.Host.HtmlElementExtensions.SetHtmlTextWrapping(this.htmlElement, System.Windows.TextWrapping.NoWrap, this.converter);
                    Granular.Host.HtmlElementExtensions.ClearHtmlStyleProperty(this.htmlElement, "max-width");
                } else {
                    Granular.Host.HtmlElementExtensions.SetHtmlTextWrapping(this.htmlElement, System.Windows.TextWrapping.Wrap, this.converter);
                    Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.htmlElement, "max-width", this.converter.ToPixelString(maxWidth));
                }

                this.htmlElement.innerHTML = this.converter.ToHtmlContentString(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(text, "A"));

                return new System.Windows.Size(Granular.Extensions.StringExtensions.IsNullOrEmpty(text) ? 0 : this.htmlElement.offsetWidth + 2, this.htmlElement.offsetHeight);
            }
        }
    });

    Bridge.define("Granular.Host.WebApplicationHost", {
        inherits: [System.Windows.IApplicationHost],
        props: {
            PresentationSourceFactory: null,
            TaskScheduler: null,
            TextMeasurementService: null
        },
        alias: [
            "PresentationSourceFactory", "System$Windows$IApplicationHost$PresentationSourceFactory",
            "TaskScheduler", "System$Windows$IApplicationHost$TaskScheduler",
            "TextMeasurementService", "System$Windows$IApplicationHost$TextMeasurementService",
            "Run", "System$Windows$IApplicationHost$Run"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                var renderQueue = new Granular.Host.RenderQueue();
                var htmlValueConverter = new Granular.Host.HtmlValueConverter();
                var svgValueConverter = new Granular.Host.SvgValueConverter();
                var svgDefinitionContainer = new Granular.Host.SvgDefinitionContainer(renderQueue);
                var imageElementContainer = new Granular.Host.ImageElementContainer();
                var embeddedResourceObjectFactory = new Granular.Host.EmbeddedResourceObjectFactory(htmlValueConverter);

                var htmlRenderElementFactory = new Granular.Host.Render.HtmlRenderElementFactory(renderQueue, htmlValueConverter, imageElementContainer, embeddedResourceObjectFactory, svgValueConverter, svgDefinitionContainer);

                this.PresentationSourceFactory = new Granular.Host.PresentationSourceFactory(htmlRenderElementFactory, htmlValueConverter, imageElementContainer, svgDefinitionContainer);
                this.TaskScheduler = new Granular.Host.TaskScheduler();
                this.TextMeasurementService = new Granular.Host.TextMeasurementService(htmlValueConverter);
            }
        },
        methods: {
            Run: function (applicationEntryPoint) {
                window.onload = Bridge.fn.combine(window.onload, function (e) {
                    applicationEntryPoint();

                    return e;
                });
            }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlRenderElement", {
        inherits: [Granular.Host.Render.IHtmlRenderElement],
        props: {
            HtmlElement: null,
            IsLoaded: false
        },
        alias: [
            "HtmlElement", "Granular$Host$Render$IHtmlRenderElement$HtmlElement",
            "Load", "Granular$Host$Render$IHtmlRenderElement$Load",
            "Unload", "Granular$Host$Render$IHtmlRenderElement$Unload"
        ],
        ctors: {
            ctor: function () {
                Granular.Host.Render.HtmlRenderElement.$ctor1.call(this, document.createElement("div"));
            },
            $ctor1: function (htmlElement) {
                this.$initialize();
                this.HtmlElement = htmlElement;
            }
        },
        methods: {
            Load: function () {
                if (this.IsLoaded) {
                    return;
                }

                this.IsLoaded = true;
                this.OnLoad();
            },
            Unload: function () {
                if (!this.IsLoaded) {
                    return;
                }

                this.IsLoaded = false;
                this.OnUnload();
            },
            OnLoad: function () { },
            OnUnload: function () { }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlBrushRenderResource", {
        inherits: [Granular.Host.Render.HtmlRenderResource,System.Windows.Media.IBrushRenderResource],
        fields: {
            opacity: 0,
            svgDefinitionContainer: null
        },
        props: {
            Uri: null,
            Opacity: {
                get: function () {
                    return this.opacity;
                },
                set: function (value) {
                    if (this.opacity === value) {
                        return;
                    }

                    this.opacity = value;
                    this.OnOpacityChanged();
                }
            }
        },
        alias: ["Opacity", "System$Windows$Media$IBrushRenderResource$Opacity"],
        ctors: {
            ctor: function (tagName, svgDefinitionContainer) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderResource.ctor.call(this, Granular.Host.SvgDocument.CreateElement(tagName));
                this.svgDefinitionContainer = svgDefinitionContainer;

                var elementName = System.String.format("{0}{1}", tagName, Bridge.box(svgDefinitionContainer.GetNextId(), System.Int32));
                this.Uri = System.String.format("url(#{0})", [elementName]);
                this.HtmlElement.setAttribute("id", elementName);
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlRenderResource.prototype.OnLoad.call(this);

                this.svgDefinitionContainer.Add(this);
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlRenderResource.prototype.OnUnload.call(this);

                this.svgDefinitionContainer.Remove(this);
            },
            OnOpacityChanged: function () { }
        }
    });

    Bridge.define("Granular.Host.Render.IHtmlDeferredRenderElement", {
        inherits: [Granular.Host.Render.IHtmlRenderElement],
        $kind: "interface"
    });

    Bridge.define("Granular.Host.Render.HtmlGeometryRenderResource", {
        inherits: [Granular.Host.Render.HtmlRenderResource,System.Windows.Media.IGeometryRenderResource],
        fields: {
            transformRenderResource: null,
            transform: null,
            data: null,
            factory: null,
            renderQueue: null,
            svgDefinitionContainer: null,
            converter: null,
            pathHtmlElement: null
        },
        events: {
            DataChanged: null
        },
        props: {
            Uri: null,
            Transform: {
                get: function () {
                    return this.transform;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.transform, value)) {
                        return;
                    }

                    if (this.transformRenderResource != null && this.IsLoaded) {
                        this.transformRenderResource.removeMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                    }

                    this.transform = value;
                    this.transformRenderResource = Bridge.cast((this.transform != null ? this.transform.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlTransformRenderResource);

                    if (this.transformRenderResource != null && this.IsLoaded) {
                        this.transformRenderResource.addMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                    }

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlGeometryRenderResource.f1));
                }
            },
            Data: {
                get: function () {
                    return this.data;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.data, value)) {
                        return;
                    }

                    this.data = value;

                    if (this.pathHtmlElement != null) {
                        this.pathHtmlElement.setAttribute("d", this.data);
                    }

                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.DataChanged, this);
                }
            }
        },
        alias: [
            "Transform", "System$Windows$Media$IGeometryRenderResource$Transform",
            "Data", "System$Windows$Media$IGeometryRenderResource$Data"
        ],
        ctors: {
            ctor: function (factory, renderQueue, svgDefinitionContainer, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderResource.ctor.call(this, Granular.Host.SvgDocument.CreateElement("clipPath"));
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.svgDefinitionContainer = svgDefinitionContainer;
                this.converter = converter;

                this.pathHtmlElement = Granular.Host.SvgDocument.CreateElement("path");

                var elementName = System.String.format("clipPath{0}", [Bridge.box(svgDefinitionContainer.GetNextId(), System.Int32)]);
                this.Uri = System.String.format("url(#{0})", [elementName]);
                this.HtmlElement.setAttribute("id", elementName);
                this.HtmlElement.appendChild(this.pathHtmlElement);
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlRenderResource.prototype.OnLoad.call(this);

                this.svgDefinitionContainer.Add(this);

                if (this.transformRenderResource != null) {
                    this.transformRenderResource.addMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                }

                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlGeometryRenderResource.f1));
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlRenderResource.prototype.OnUnload.call(this);

                this.svgDefinitionContainer.Remove(this);

                if (this.transformRenderResource != null) {
                    this.transformRenderResource.removeMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                }
            },
            OnTransformRenderResourceMatrixChanged: function (sender, e) {
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlGeometryRenderResource.f2));
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlGeometryRenderResource", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlGeometryRenderResource, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgTransform(this.HtmlElement, this.transformRenderResource != null ? this.transformRenderResource.Matrix : null, this.converter);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgTransform(this.HtmlElement, this.transformRenderResource.Matrix, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlBorderRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IBorderRenderElement],
        fields: {
            background: null,
            borderThickness: null,
            borderBrush: null,
            bounds: null,
            cornerRadius: null,
            isHitTestVisible: false,
            factory: null,
            renderQueue: null,
            converter: null
        },
        props: {
            Background: {
                get: function () {
                    return this.background;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.background, value)) {
                        return;
                    }

                    if (this.IsLoaded && this.background != null) {
                        this.background.removeChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                    }

                    this.background = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlBorderRenderElement.f1));

                    if (this.IsLoaded && this.background != null) {
                        this.background.addChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                    }
                }
            },
            BorderThickness: {
                get: function () {
                    return this.borderThickness;
                },
                set: function (value) {
                    if (System.Windows.Thickness.op_Equality(this.borderThickness, value)) {
                        return;
                    }

                    this.borderThickness = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlBorderRenderElement.f2));
                }
            },
            BorderBrush: {
                get: function () {
                    return this.borderBrush;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.borderBrush, value)) {
                        return;
                    }

                    if (this.IsLoaded && this.borderBrush != null) {
                        this.borderBrush.removeChanged(Bridge.fn.cacheBind(this, this.OnBorderBrushChanged));
                    }

                    this.borderBrush = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetBorderBrush));

                    if (this.IsLoaded && this.borderBrush != null) {
                        this.borderBrush.addChanged(Bridge.fn.cacheBind(this, this.OnBorderBrushChanged));
                    }
                }
            },
            Bounds: {
                get: function () {
                    return this.bounds;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.bounds, value)) {
                        return;
                    }

                    this.bounds = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlBorderRenderElement.f3));
                }
            },
            CornerRadius: {
                get: function () {
                    return this.cornerRadius;
                },
                set: function (value) {
                    if (System.Windows.CornerRadius.op_Equality(this.cornerRadius, value)) {
                        return;
                    }

                    this.cornerRadius = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetCornerRadius));
                }
            },
            IsHitTestVisible: {
                get: function () {
                    return this.isHitTestVisible;
                },
                set: function (value) {
                    if (this.isHitTestVisible === value) {
                        return;
                    }

                    this.isHitTestVisible = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetIsHitTestVisible));
                }
            }
        },
        alias: [
            "Background", "System$Windows$Media$IBorderRenderElement$Background",
            "BorderThickness", "System$Windows$Media$IBorderRenderElement$BorderThickness",
            "BorderBrush", "System$Windows$Media$IBorderRenderElement$BorderBrush",
            "Bounds", "System$Windows$Media$IBorderRenderElement$Bounds",
            "CornerRadius", "System$Windows$Media$IBorderRenderElement$CornerRadius",
            "IsHitTestVisible", "System$Windows$Media$IBorderRenderElement$IsHitTestVisible"
        ],
        ctors: {
            ctor: function (factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.ctor.call(this);
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.converter = converter;

                this.bounds = System.Windows.Rect.Zero;
                this.borderThickness = System.Windows.Thickness.Zero;
                this.cornerRadius = System.Windows.CornerRadius.Zero;

                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.HtmlElement, "background-clip", "content-box");

                this.SetBackground();
                this.SetBorderBrush();
                this.SetBounds();
                this.SetCornerRadius();
                this.SetIsHitTestVisible();
            }
        },
        methods: {
            OnBackgroundChanged: function (sender, e) {
                this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetBackground));
            },
            OnBorderBrushChanged: function (sender, e) {
                this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetBorderBrush));
            },
            SetBackground: function () {
                Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.HtmlElement, this.background, new System.Windows.Rect.$ctor2(this.BorderThickness.Location, System.Windows.SizeExtensions.Max((System.Windows.Size.op_Subtraction(this.Bounds.Size, this.BorderThickness.Size)), System.Windows.Size.Zero)), this.factory, this.converter);
            },
            SetBorderBrush: function () {
                Granular.Host.HtmlElementExtensions.SetHtmlBorderBrush(this.HtmlElement, this.BorderBrush, this.Bounds.Size, this.factory, this.converter);
            },
            SetBounds: function () {
                Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, new System.Windows.Rect.$ctor2(this.Bounds.Location, System.Windows.SizeExtensions.Max((System.Windows.Size.op_Subtraction(this.Bounds.Size, this.BorderThickness.Size)), System.Windows.Size.Zero)), this.converter);
            },
            SetCornerRadius: function () {
                var borderOutlineCornerRadius = System.Windows.CornerRadius.op_Equality(this.CornerRadius, System.Windows.CornerRadius.Zero) ? System.Windows.CornerRadius.Zero : new System.Windows.CornerRadius.$ctor1(this.CornerRadius.TopLeft + (this.BorderThickness.Top + this.BorderThickness.Left) / 4, this.CornerRadius.TopRight + (this.BorderThickness.Top + this.BorderThickness.Right) / 4, this.CornerRadius.BottomRight + (this.BorderThickness.Bottom + this.BorderThickness.Right) / 4, this.CornerRadius.BottomLeft + (this.BorderThickness.Bottom + this.BorderThickness.Left) / 4);

                Granular.Host.HtmlElementExtensions.SetHtmlCornerRadius(this.HtmlElement, borderOutlineCornerRadius, this.converter);
            },
            SetIsHitTestVisible: function () {
                Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.HtmlElement, this.IsHitTestVisible && this.Background != null);
            },
            OnLoad: function () {
                if (this.Background != null) {
                    this.Background.addChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                }

                if (this.BorderBrush != null) {
                    this.BorderBrush.addChanged(Bridge.fn.cacheBind(this, this.OnBorderBrushChanged));
                }

                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlBorderRenderElement.f4));
            },
            OnUnload: function () {
                if (this.BorderBrush != null) {
                    this.BorderBrush.removeChanged(Bridge.fn.cacheBind(this, this.OnBorderBrushChanged));
                }

                if (this.Background != null) {
                    this.Background.removeChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlBorderRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlBorderRenderElement, {
        f1: function () {
            this.SetBackground();
            this.SetIsHitTestVisible();
        },
        f2: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBorderThickness(this.HtmlElement, this.borderThickness, this.converter);
            this.SetBounds();
            this.SetBackground();
            this.SetCornerRadius();
        },
        f3: function () {
            this.SetBounds();
            this.SetBackground();
            this.SetBorderBrush();
        },
        f4: function () {
            this.SetBackground();
            this.SetBorderBrush();
            this.SetIsHitTestVisible();
        }
    });

    Bridge.define("Granular.Host.Render.HtmlContainerRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IContainerRenderElement],
        fields: {
            children: null,
            renderQueue: null,
            lastDeferredChildIndex: 0,
            deferredChildrenCount: 0
        },
        props: {
            Children: {
                get: function () {
                    return this.children;
                }
            }
        },
        alias: [
            "Children", "System$Windows$Media$IContainerRenderElement$Children",
            "InsertChild", "System$Windows$Media$IContainerRenderElement$InsertChild",
            "RemoveChild", "System$Windows$Media$IContainerRenderElement$RemoveChild"
        ],
        ctors: {
            ctor: function (htmlElement, renderQueue) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.$ctor1.call(this, htmlElement);
                this.renderQueue = renderQueue;

                this.children = new (System.Collections.Generic.List$1(Granular.Host.Render.IHtmlRenderElement)).ctor();

                this.lastDeferredChildIndex = -1;
            }
        },
        methods: {
            OnLoad: function () {
                var $t;
                Granular.Host.Render.HtmlRenderElement.prototype.OnLoad.call(this);

                $t = Bridge.getEnumerator(this.Children, System.Object);
                try {
                    while ($t.moveNext()) {
                        var child = Bridge.cast($t.Current, Granular.Host.Render.IHtmlRenderElement);
                        child.Granular$Host$Render$IHtmlRenderElement$Load();
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }},
            OnUnload: function () {
                var $t;
                Granular.Host.Render.HtmlRenderElement.prototype.OnUnload.call(this);

                $t = Bridge.getEnumerator(this.Children, System.Object);
                try {
                    while ($t.moveNext()) {
                        var child = Bridge.cast($t.Current, Granular.Host.Render.IHtmlRenderElement);
                        child.Granular$Host$Render$IHtmlRenderElement$Unload();
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }},
            InsertChild: function (index, child) {
                var childElement = Bridge.cast(child, Granular.Host.Render.IHtmlRenderElement);

                if (Bridge.is(child, Granular.Host.Render.IHtmlDeferredRenderElement)) {
                    Bridge.cast(child, Granular.Host.Render.IHtmlDeferredRenderElement).Granular$Host$Render$IHtmlDeferredRenderElement$addHtmlElementCreated(Bridge.fn.cacheBind(this, this.OnChildHtmlElementCreated));
                }

                if (this.IsLoaded) {
                    childElement.Granular$Host$Render$IHtmlRenderElement$Load();
                }

                this.children.insert(index, childElement);
                if (childElement.Granular$Host$Render$IHtmlRenderElement$HtmlElement != null) {
                    if (index > this.lastDeferredChildIndex) {
                        index = (index - this.deferredChildrenCount) | 0;
                    } else {
                        this.lastDeferredChildIndex = (this.lastDeferredChildIndex + 1) | 0;
                        index = Granular.Compatibility.Linq.Enumerable.Count$1(Bridge.global.Granular.Host.Render.IHtmlRenderElement, Granular.Compatibility.Linq.Enumerable.Take(Bridge.global.Granular.Host.Render.IHtmlRenderElement, this.children, index), $asm.$.Granular.Host.Render.HtmlContainerRenderElement.f1);
                    }

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, function () {
                        Granular.Host.HtmlElementExtensions.InsertChild(this.HtmlElement, index, childElement.Granular$Host$Render$IHtmlRenderElement$HtmlElement);
                    }));
                } else {
                    this.lastDeferredChildIndex = Granular.Extensions.IntExtensions.Max(this.lastDeferredChildIndex, index);
                    this.deferredChildrenCount = (this.deferredChildrenCount + 1) | 0;
                }
            },
            RemoveChild: function (child) {
                var childElement = Bridge.cast(child, Granular.Host.Render.IHtmlRenderElement);

                if (Bridge.is(child, Granular.Host.Render.IHtmlDeferredRenderElement)) {
                    Bridge.cast(child, Granular.Host.Render.IHtmlDeferredRenderElement).Granular$Host$Render$IHtmlDeferredRenderElement$removeHtmlElementCreated(Bridge.fn.cacheBind(this, this.OnChildHtmlElementCreated));
                }

                if (this.IsLoaded) {
                    childElement.Granular$Host$Render$IHtmlRenderElement$Unload();
                }

                var childIndex = this.children.indexOf(childElement);

                if (childIndex === -1) {
                    return;
                }

                this.children.removeAt(childIndex);

                if (childElement.Granular$Host$Render$IHtmlRenderElement$HtmlElement != null) {
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, function () {
                        this.HtmlElement.removeChild(Bridge.cast(child, Granular.Host.Render.HtmlRenderElement).HtmlElement);
                    }));
                } else {
                    this.deferredChildrenCount = (this.deferredChildrenCount - 1) | 0;
                }
            },
            OnChildHtmlElementCreated: function (sender, e) {
                var childElement = Bridge.cast(sender, Granular.Host.Render.IHtmlRenderElement);
                var index = this.children.indexOf(childElement);

                index = Granular.Compatibility.Linq.Enumerable.Count$1(Bridge.global.Granular.Host.Render.IHtmlRenderElement, Granular.Compatibility.Linq.Enumerable.Take(Bridge.global.Granular.Host.Render.IHtmlRenderElement, this.children, index), $asm.$.Granular.Host.Render.HtmlContainerRenderElement.f1);
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, function () {
                    Granular.Host.HtmlElementExtensions.InsertChild(this.HtmlElement, index, childElement.Granular$Host$Render$IHtmlRenderElement$HtmlElement);
                }));

                this.deferredChildrenCount = (this.deferredChildrenCount - 1) | 0;
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlContainerRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlContainerRenderElement, {
        f1: function (c) {
            return c.Granular$Host$Render$IHtmlRenderElement$HtmlElement != null;
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingShapeRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IDrawingShapeRenderElement],
        fields: {
            fillRenderResource: null,
            fill: null,
            strokeRenderResource: null,
            stroke: null,
            strokeThickness: 0,
            factory: null,
            renderQueue: null,
            converter: null
        },
        props: {
            Fill: {
                get: function () {
                    return this.fill;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.fill, value)) {
                        return;
                    }

                    if (this.fillRenderResource != null && this.IsLoaded) {
                        this.fillRenderResource.Unload();
                    }

                    this.fill = value;
                    this.fillRenderResource = Bridge.cast((this.fill != null ? this.fill.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlBrushRenderResource);

                    if (this.fillRenderResource != null && this.IsLoaded) {
                        this.fillRenderResource.Load();
                    }

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingShapeRenderElement.f1));
                }
            },
            Stroke: {
                get: function () {
                    return this.stroke;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.stroke, value)) {
                        return;
                    }

                    if (this.strokeRenderResource != null && this.IsLoaded) {
                        this.strokeRenderResource.Unload();
                    }

                    this.stroke = value;
                    this.strokeRenderResource = Bridge.cast((this.stroke != null ? this.stroke.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlBrushRenderResource);

                    if (this.strokeRenderResource != null && this.IsLoaded) {
                        this.strokeRenderResource.Load();
                    }

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingShapeRenderElement.f2));
                }
            },
            StrokeThickness: {
                get: function () {
                    return this.strokeThickness;
                },
                set: function (value) {
                    if (this.strokeThickness === value) {
                        return;
                    }

                    this.strokeThickness = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingShapeRenderElement.f3));
                }
            }
        },
        alias: [
            "Fill", "System$Windows$Media$IDrawingShapeRenderElement$Fill",
            "Stroke", "System$Windows$Media$IDrawingShapeRenderElement$Stroke",
            "StrokeThickness", "System$Windows$Media$IDrawingShapeRenderElement$StrokeThickness"
        ],
        ctors: {
            ctor: function (htmlElement, factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.$ctor1.call(this, htmlElement);
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.converter = converter;
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlRenderElement.prototype.OnLoad.call(this);

                if (this.fillRenderResource != null) {
                    this.fillRenderResource.Load();
                }

                if (this.strokeRenderResource != null) {
                    this.strokeRenderResource.Load();
                }
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlRenderElement.prototype.OnUnload.call(this);

                if (this.fillRenderResource != null) {
                    this.fillRenderResource.Unload();
                }

                if (this.strokeRenderResource != null) {
                    this.strokeRenderResource.Unload();
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlDrawingShapeRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlDrawingShapeRenderElement, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgFill(this.HtmlElement, this.fillRenderResource);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgStroke(this.HtmlElement, this.strokeRenderResource);
        },
        f3: function () {
            Granular.Host.SvgElementExtensions.SetSvgStrokeThickness(this.HtmlElement, this.strokeThickness, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingImageRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IDrawingImageRenderElement],
        fields: {
            imageSource: null,
            rectangle: null,
            factory: null,
            renderQueue: null,
            converter: null
        },
        props: {
            ImageSource: {
                get: function () {
                    return this.imageSource;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.imageSource, value)) {
                        return;
                    }

                    this.imageSource = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingImageRenderElement.f1));
                }
            },
            Rectangle: {
                get: function () {
                    return this.rectangle;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.rectangle, value)) {
                        return;
                    }

                    this.rectangle = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingImageRenderElement.f2));
                }
            }
        },
        alias: [
            "ImageSource", "System$Windows$Media$IDrawingImageRenderElement$ImageSource",
            "Rectangle", "System$Windows$Media$IDrawingImageRenderElement$Rectangle"
        ],
        ctors: {
            ctor: function (htmlRenderElementFactory, renderQueue, svgValueConverter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.$ctor1.call(this, Granular.Host.SvgDocument.CreateElement("image"));
                this.factory = htmlRenderElementFactory;
                this.renderQueue = renderQueue;
                this.converter = svgValueConverter;
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlDrawingImageRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlDrawingImageRenderElement, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgImageSource(this.HtmlElement, this.imageSource, this.factory, this.converter);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgBounds(this.HtmlElement, this.rectangle, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingRenderElement", {
        inherits: [Granular.Host.Render.IHtmlDeferredRenderElement,System.Windows.Media.IContainerRenderElement],
        fields: {
            container: null,
            renderQueue: null,
            converter: null,
            isLoaded: false
        },
        events: {
            HtmlElementCreated: null
        },
        props: {
            HtmlElement: {
                get: function () {
                    return this.container != null ? this.container.HtmlElement : null;
                }
            },
            Children: {
                get: function () {
                    return this.container != null ? this.container.Children : System.Array.init(0, null, System.Object);
                }
            }
        },
        alias: [
            "addHtmlElementCreated", "Granular$Host$Render$IHtmlDeferredRenderElement$addHtmlElementCreated",
            "removeHtmlElementCreated", "Granular$Host$Render$IHtmlDeferredRenderElement$removeHtmlElementCreated",
            "HtmlElement", "Granular$Host$Render$IHtmlRenderElement$HtmlElement",
            "Children", "System$Windows$Media$IContainerRenderElement$Children",
            "Load", "Granular$Host$Render$IHtmlRenderElement$Load",
            "Unload", "Granular$Host$Render$IHtmlRenderElement$Unload",
            "InsertChild", "System$Windows$Media$IContainerRenderElement$InsertChild",
            "RemoveChild", "System$Windows$Media$IContainerRenderElement$RemoveChild"
        ],
        ctors: {
            ctor: function (renderQueue, converter) {
                this.$initialize();
                this.renderQueue = renderQueue;
                this.converter = converter;
            }
        },
        methods: {
            Load: function () {
                this.isLoaded = true;

                if (this.container != null) {
                    this.container.Load();
                }
            },
            Unload: function () {
                this.isLoaded = false;

                if (this.container != null) {
                    this.container.Unload();
                }
            },
            InsertChild: function (index, child) {
                if (this.container == null) {
                    var element = Granular.Host.SvgDocument.CreateElement("svg");
                    element.setAttribute("overflow", "visible");

                    this.container = new Granular.Host.Render.HtmlContainerRenderElement(element, this.renderQueue);

                    if (this.isLoaded) {
                        this.container.Load();
                    }

                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.HtmlElementCreated, this);
                }

                this.container.InsertChild(index, child);
            },
            RemoveChild: function (child) {
                if (this.container != null) {
                    this.container.RemoveChild(child);
                }
            }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingTextRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IDrawingTextRenderElement],
        fields: {
            formattedText: null,
            origin: null,
            factory: null,
            renderQueue: null,
            converter: null,
            foregroundRenderResource: null
        },
        props: {
            FormattedText: {
                get: function () {
                    return this.formattedText;
                },
                set: function (value) {
                    var $t;
                    if (Bridge.referenceEquals(this.formattedText, value)) {
                        return;
                    }

                    if (this.foregroundRenderResource != null && this.IsLoaded) {
                        this.foregroundRenderResource.Unload();
                    }

                    this.formattedText = value;
                    this.foregroundRenderResource = Bridge.cast((this.formattedText != null && ($t = this.formattedText.Foreground) != null ? $t.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlBrushRenderResource);

                    if (this.foregroundRenderResource != null && this.IsLoaded) {
                        this.foregroundRenderResource.Load();
                    }

                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetFormattedText));
                }
            },
            Origin: {
                get: function () {
                    return this.origin;
                },
                set: function (value) {
                    if (System.Windows.Point.op_Equality(this.origin, value)) {
                        return;
                    }

                    this.origin = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingTextRenderElement.f1));
                }
            }
        },
        alias: [
            "FormattedText", "System$Windows$Media$IDrawingTextRenderElement$FormattedText",
            "Origin", "System$Windows$Media$IDrawingTextRenderElement$Origin"
        ],
        ctors: {
            ctor: function (factory, renderQueue, svgValueConverter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.$ctor1.call(this, Granular.Host.SvgDocument.CreateElement("text"));
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.converter = svgValueConverter;

                this.HtmlElement.setAttribute("dy", "1em");
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlRenderElement.prototype.OnLoad.call(this);

                if (this.foregroundRenderResource != null) {
                    this.foregroundRenderResource.Load();
                }
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlRenderElement.prototype.OnUnload.call(this);

                if (this.foregroundRenderResource != null) {
                    this.foregroundRenderResource.Unload();
                }
            },
            SetFormattedText: function () {
                Granular.Host.SvgElementExtensions.SetSvgFill(this.HtmlElement, this.foregroundRenderResource);
                Granular.Host.SvgElementExtensions.SetSvgFontFamily(this.HtmlElement, this.FormattedText.Typeface.FontFamily, this.converter);
                Granular.Host.SvgElementExtensions.SetSvgFontStretch(this.HtmlElement, this.FormattedText.Typeface.Stretch, this.converter);
                Granular.Host.SvgElementExtensions.SetSvgFontStyle(this.HtmlElement, this.FormattedText.Typeface.Style, this.converter);
                Granular.Host.SvgElementExtensions.SetSvgFontWeight(this.HtmlElement, this.FormattedText.Typeface.Weight, this.converter);
                Granular.Host.SvgElementExtensions.SetSvgFontSize(this.HtmlElement, this.FormattedText.Size, this.converter);
                this.HtmlElement.textContent = this.FormattedText.Text;
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlDrawingTextRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlDrawingTextRenderElement, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgLocation(this.HtmlElement, this.origin, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlGradientBrushRenderResource", {
        inherits: [Granular.Host.Render.HtmlBrushRenderResource,System.Windows.Media.IGradientBrushRenderResource],
        fields: {
            gradientStops: null,
            spreadMethod: 0,
            mappingMode: 0,
            renderQueue: null,
            converter: null
        },
        props: {
            GradientStops: {
                get: function () {
                    return this.gradientStops;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.gradientStops, value)) {
                        return;
                    }

                    this.gradientStops = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetGradientStops));
                }
            },
            SpreadMethod: {
                get: function () {
                    return this.spreadMethod;
                },
                set: function (value) {
                    if (this.spreadMethod === value) {
                        return;
                    }

                    this.spreadMethod = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlGradientBrushRenderResource.f1));
                }
            },
            MappingMode: {
                get: function () {
                    return this.mappingMode;
                },
                set: function (value) {
                    if (this.mappingMode === value) {
                        return;
                    }

                    this.mappingMode = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlGradientBrushRenderResource.f2));
                }
            }
        },
        alias: [
            "GradientStops", "System$Windows$Media$IGradientBrushRenderResource$GradientStops",
            "SpreadMethod", "System$Windows$Media$IGradientBrushRenderResource$SpreadMethod",
            "MappingMode", "System$Windows$Media$IGradientBrushRenderResource$MappingMode"
        ],
        ctors: {
            init: function () {
                this.mappingMode = System.Windows.Media.BrushMappingMode.RelativeToBoundingBox;
            },
            ctor: function (tagName, renderQueue, converter, svgDefinitionContainer) {
                this.$initialize();
                Granular.Host.Render.HtmlBrushRenderResource.ctor.call(this, tagName, svgDefinitionContainer);
                this.renderQueue = renderQueue;
                this.converter = converter;
            }
        },
        methods: {
            OnOpacityChanged: function () {
                this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetGradientStops));
            },
            SetGradientStops: function () {
                var gradientStops = Granular.Compatibility.Linq.Enumerable.ToArray(Bridge.global.System.Windows.Media.RenderGradientStop, this.GradientStops);

                var htmlElement = this.HtmlElement;

                while (htmlElement.childNodes.length > gradientStops.length) {
                    htmlElement.removeChild(htmlElement.lastElementChild);
                }

                while (htmlElement.childNodes.length < gradientStops.length) {
                    htmlElement.appendChild(Granular.Host.SvgDocument.CreateElement("stop"));
                }

                for (var i = 0; i < gradientStops.length; i = (i + 1) | 0) {
                    var stopElement = Bridge.cast(htmlElement.childNodes[i], Element);
                    stopElement.setAttribute("stop-color", this.converter.ToColorString(gradientStops[System.Array.index(i, gradientStops)].Color));
                    stopElement.setAttribute("stop-opacity", this.converter.ToImplicitValueString(this.Opacity * gradientStops[System.Array.index(i, gradientStops)].Color.A / 255));
                    stopElement.setAttribute("offset", this.converter.ToImplicitValueString(gradientStops[System.Array.index(i, gradientStops)].Offset));
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlGradientBrushRenderResource", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlGradientBrushRenderResource, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgSpreadMethod(this.HtmlElement, this.spreadMethod, this.converter);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgMappingMode(this.HtmlElement, this.mappingMode, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlImageRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.IImageRenderElement],
        fields: {
            bounds: null,
            renderResource: null,
            source: null,
            factory: null,
            renderQueue: null,
            converter: null
        },
        props: {
            Bounds: {
                get: function () {
                    return this.bounds;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.bounds, value)) {
                        return;
                    }

                    this.bounds = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlImageRenderElement.f1));
                }
            },
            Source: {
                get: function () {
                    return this.source;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.source, value)) {
                        return;
                    }

                    this.source = value;
                    this.renderResource = this.source != null ? Bridge.cast(this.source.GetRenderResource(this.factory), Granular.Host.Render.HtmlImageSourceRenderResource) : null;

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlImageRenderElement.f2));
                }
            }
        },
        alias: [
            "Bounds", "System$Windows$Media$IImageRenderElement$Bounds",
            "Source", "System$Windows$Media$IImageRenderElement$Source"
        ],
        ctors: {
            ctor: function (factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.ctor.call(this);
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.converter = converter;

                this.bounds = System.Windows.Rect.Zero;

                Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.Bounds, converter);
            }
        },
        methods: {
            SetSourceRect: function () {
                if (this.renderResource == null || System.Windows.RectExtensions.IsNullOrEmpty(this.Bounds) || this.Bounds.Size.Width === 0 || this.Bounds.Size.Height === 0) {
                    return;
                }

                if (!System.Windows.RectExtensions.IsNullOrEmpty(this.renderResource.SourceRect)) {
                    var widthFactor = this.Bounds.Size.Width / this.renderResource.SourceRect.Width;
                    var heightFactor = this.Bounds.Size.Height / this.renderResource.SourceRect.Height;

                    var location = new System.Windows.Point.$ctor1(-this.renderResource.SourceRect.Left * widthFactor, -this.renderResource.SourceRect.Top * heightFactor);
                    var size = new System.Windows.Size(this.renderResource.ImageSize.Width * widthFactor, this.renderResource.ImageSize.Height * heightFactor);

                    Granular.Host.HtmlElementExtensions.SetHtmlBackgroundBounds(this.HtmlElement, new System.Windows.Rect.$ctor2(location, size), this.converter);
                } else {
                    Granular.Host.HtmlElementExtensions.SetHtmlBackgroundBounds(this.HtmlElement, new System.Windows.Rect.$ctor3(this.Bounds.Size), this.converter);
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlImageRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlImageRenderElement, {
        f1: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.bounds, this.converter);
            this.SetSourceRect();
        },
        f2: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBackgroundImage(this.HtmlElement, this.renderResource != null ? this.renderResource.Url : null, this.converter, this.factory);
            this.SetSourceRect();
        }
    });

    Bridge.define("Granular.Host.Render.HtmlSolidColorBrushRenderResource", {
        inherits: [Granular.Host.Render.HtmlBrushRenderResource,System.Windows.Media.ISolidColorBrushRenderResource],
        fields: {
            color: null,
            renderQueue: null,
            converter: null,
            stopElement: null
        },
        props: {
            Color: {
                get: function () {
                    return this.color;
                },
                set: function (value) {
                    if (System.Windows.Media.Color.op_Equality(this.color, value)) {
                        return;
                    }

                    this.color = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetStop));
                }
            }
        },
        alias: ["Color", "System$Windows$Media$ISolidColorBrushRenderResource$Color"],
        ctors: {
            ctor: function (renderQueue, converter, svgDefinitionContainer) {
                this.$initialize();
                Granular.Host.Render.HtmlBrushRenderResource.ctor.call(this, "linearGradient", svgDefinitionContainer);
                this.renderQueue = renderQueue;
                this.converter = converter;

                this.stopElement = Granular.Host.SvgDocument.CreateElement("stop");
                this.HtmlElement.appendChild(this.stopElement);
            }
        },
        methods: {
            OnOpacityChanged: function () {
                this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetStop));
            },
            SetStop: function () {
                this.stopElement.setAttribute("stop-color", this.converter.ToColorString(this.Color));
                this.stopElement.setAttribute("stop-opacity", this.converter.ToImplicitValueString(this.Opacity * this.Color.A / 255));
            }
        }
    });

    Bridge.define("Granular.Host.Render.HtmlTextBlockRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.ITextBlockRenderElement],
        fields: {
            text: null,
            bounds: null,
            foreground: null,
            fontFamily: null,
            fontSize: 0,
            fontStyle: 0,
            fontWeight: 0,
            fontStretch: 0,
            textAlignment: 0,
            textTrimming: 0,
            textWrapping: 0,
            renderQueue: null,
            converter: null
        },
        props: {
            Text: {
                get: function () {
                    return this.text;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.text, value)) {
                        return;
                    }

                    this.text = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f1));
                }
            },
            Bounds: {
                get: function () {
                    return this.bounds;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.bounds, value)) {
                        return;
                    }

                    this.bounds = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f2));
                }
            },
            Foreground: {
                get: function () {
                    return this.foreground;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.foreground, value)) {
                        return;
                    }

                    if (this.IsLoaded && this.foreground != null) {
                        this.foreground.removeChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                    }

                    this.foreground = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f3));

                    if (this.IsLoaded && this.foreground != null) {
                        this.foreground.addChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                    }
                }
            },
            FontFamily: {
                get: function () {
                    return this.fontFamily;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.fontFamily, value)) {
                        return;
                    }

                    this.fontFamily = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f4));
                }
            },
            FontSize: {
                get: function () {
                    return this.fontSize;
                },
                set: function (value) {
                    if (this.fontSize === value) {
                        return;
                    }

                    this.fontSize = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f5));
                }
            },
            FontStyle: {
                get: function () {
                    return this.fontStyle;
                },
                set: function (value) {
                    if (this.fontStyle === value) {
                        return;
                    }

                    this.fontStyle = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f6));
                }
            },
            FontWeight: {
                get: function () {
                    return this.fontWeight;
                },
                set: function (value) {
                    if (this.fontWeight === value) {
                        return;
                    }

                    this.fontWeight = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f7));
                }
            },
            FontStretch: {
                get: function () {
                    return this.fontStretch;
                },
                set: function (value) {
                    if (this.fontStretch === value) {
                        return;
                    }

                    this.fontStretch = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f8));
                }
            },
            TextAlignment: {
                get: function () {
                    return this.textAlignment;
                },
                set: function (value) {
                    if (this.textAlignment === value) {
                        return;
                    }

                    this.textAlignment = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f9));
                }
            },
            TextTrimming: {
                get: function () {
                    return this.textTrimming;
                },
                set: function (value) {
                    if (this.textTrimming === value) {
                        return;
                    }

                    this.textTrimming = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f10));
                }
            },
            TextWrapping: {
                get: function () {
                    return this.textWrapping;
                },
                set: function (value) {
                    if (this.textWrapping === value) {
                        return;
                    }

                    this.textWrapping = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f11));
                }
            }
        },
        alias: [
            "Text", "System$Windows$Media$ITextBlockRenderElement$Text",
            "Bounds", "System$Windows$Media$ITextBlockRenderElement$Bounds",
            "Foreground", "System$Windows$Media$ITextBlockRenderElement$Foreground",
            "FontFamily", "System$Windows$Media$ITextBlockRenderElement$FontFamily",
            "FontSize", "System$Windows$Media$ITextBlockRenderElement$FontSize",
            "FontStyle", "System$Windows$Media$ITextBlockRenderElement$FontStyle",
            "FontWeight", "System$Windows$Media$ITextBlockRenderElement$FontWeight",
            "FontStretch", "System$Windows$Media$ITextBlockRenderElement$FontStretch",
            "TextAlignment", "System$Windows$Media$ITextBlockRenderElement$TextAlignment",
            "TextTrimming", "System$Windows$Media$ITextBlockRenderElement$TextTrimming",
            "TextWrapping", "System$Windows$Media$ITextBlockRenderElement$TextWrapping"
        ],
        ctors: {
            ctor: function (renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.ctor.call(this);
                this.renderQueue = renderQueue;
                this.converter = converter;

                this.bounds = System.Windows.Rect.Zero;
                this.fontFamily = System.Windows.Media.FontFamily.Default;
                this.fontSize = Number.NaN;

                Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.Bounds, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlForeground(this.HtmlElement, this.Foreground, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontFamily(this.HtmlElement, this.FontFamily, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontSize(this.HtmlElement, this.FontSize, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStyle(this.HtmlElement, this.FontStyle, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontWeight(this.HtmlElement, this.FontWeight, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStretch(this.HtmlElement, this.FontStretch, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.HtmlElement, false);
                Granular.Host.HtmlElementExtensions.SetHtmlTextAlignment(this.HtmlElement, this.TextAlignment, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlTextTrimming(this.HtmlElement, this.TextTrimming);
                Granular.Host.HtmlElementExtensions.SetHtmlTextWrapping(this.HtmlElement, this.TextWrapping, converter);
            }
        },
        methods: {
            OnForegroundChanged: function (sender, e) {
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f3));
            },
            OnLoad: function () {
                if (this.Foreground != null) {
                    this.Foreground.addChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                }

                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBlockRenderElement.f3));
            },
            OnUnload: function () {
                if (this.Foreground != null) {
                    this.Foreground.removeChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlTextBlockRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlTextBlockRenderElement, {
        f1: function () {
            this.HtmlElement.textContent = this.text;
        },
        f2: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.bounds, this.converter);
        },
        f3: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlForeground(this.HtmlElement, this.Foreground, this.converter);
        },
        f4: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontFamily(this.HtmlElement, this.fontFamily, this.converter);
        },
        f5: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontSize(this.HtmlElement, this.fontSize, this.converter);
        },
        f6: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontStyle(this.HtmlElement, this.fontStyle, this.converter);
        },
        f7: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontWeight(this.HtmlElement, this.fontWeight, this.converter);
        },
        f8: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontStretch(this.HtmlElement, this.fontStretch, this.converter);
        },
        f9: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTextAlignment(this.HtmlElement, this.textAlignment, this.converter);
        },
        f10: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTextTrimming(this.HtmlElement, this.textTrimming);
        },
        f11: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTextWrapping(this.HtmlElement, this.textWrapping, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlTextBoxRenderElement", {
        inherits: [Granular.Host.Render.HtmlRenderElement,System.Windows.Media.ITextBoxRenderElement],
        fields: {
            contentElement: null,
            text: null,
            maxLength: 0,
            caretIndex: 0,
            selectionStart: 0,
            selectionLength: 0,
            isPassword: false,
            isReadOnly: false,
            spellCheck: false,
            bounds: null,
            foreground: null,
            fontFamily: null,
            fontSize: 0,
            fontStyle: 0,
            fontWeight: 0,
            fontStretch: 0,
            textAlignment: 0,
            textTrimming: 0,
            textWrapping: 0,
            acceptsReturn: false,
            horizontalScrollBarVisibility: 0,
            verticalScrollBarVisibility: 0,
            isHitTestVisible: false,
            factory: null,
            renderQueue: null,
            converter: null,
            isFocused: false
        },
        events: {
            TextChanged: null,
            CaretIndexChanged: null,
            SelectionStartChanged: null,
            SelectionLengthChanged: null
        },
        props: {
            ContentElement: {
                get: function () {
                    return this.contentElement;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.contentElement, value)) {
                        return;
                    }

                    if (this.contentElement != null) {
                        this.HtmlElement.removeChild(this.contentElement.HtmlElement);
                    }

                    this.contentElement = value;

                    if (this.contentElement != null) {
                        this.HtmlElement.appendChild(this.contentElement.HtmlElement);
                    }
                }
            },
            Text: {
                get: function () {
                    return this.text;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.text, value)) {
                        return;
                    }

                    this.text = value;

                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f1));

                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.TextChanged, this);
                }
            },
            MaxLength: {
                get: function () {
                    return this.maxLength;
                },
                set: function (value) {
                    if (this.maxLength === value) {
                        return;
                    }

                    this.maxLength = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementMaxLength));
                }
            },
            CaretIndex: {
                get: function () {
                    return this.caretIndex;
                },
                set: function (value) {
                    if (this.caretIndex === value) {
                        return;
                    }

                    this.caretIndex = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementCaretIndex));
                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.CaretIndexChanged, this);
                }
            },
            SelectionStart: {
                get: function () {
                    return this.selectionStart;
                },
                set: function (value) {
                    if (this.selectionStart === value) {
                        return;
                    }

                    this.selectionStart = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementSelectionStart));
                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.SelectionStartChanged, this);
                }
            },
            SelectionLength: {
                get: function () {
                    return this.selectionLength;
                },
                set: function (value) {
                    if (this.selectionLength === value) {
                        return;
                    }

                    this.selectionLength = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementSelectionLength));
                    Granular.Extensions.EventHandlerExtensions.Raise$2(this.SelectionLengthChanged, this);
                }
            },
            IsPassword: {
                get: function () {
                    return this.isPassword;
                },
                set: function (value) {
                    if (this.isPassword === value) {
                        return;
                    }

                    this.isPassword = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElement));
                }
            },
            IsReadOnly: {
                get: function () {
                    return this.isReadOnly;
                },
                set: function (value) {
                    if (this.isReadOnly === value) {
                        return;
                    }

                    this.isReadOnly = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementIsReadOnly));
                }
            },
            SpellCheck: {
                get: function () {
                    return this.spellCheck;
                },
                set: function (value) {
                    if (this.spellCheck === value) {
                        return;
                    }

                    this.spellCheck = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementSpellCheck));
                }
            },
            Bounds: {
                get: function () {
                    return this.bounds;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.bounds, value)) {
                        return;
                    }

                    this.bounds = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f2));
                }
            },
            Foreground: {
                get: function () {
                    return this.foreground;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.foreground, value)) {
                        return;
                    }

                    if (this.IsLoaded && this.foreground != null) {
                        this.foreground.removeChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                    }

                    this.foreground = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f3));

                    if (this.IsLoaded && this.foreground != null) {
                        this.foreground.addChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                    }
                }
            },
            FontFamily: {
                get: function () {
                    return this.fontFamily;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.fontFamily, value)) {
                        return;
                    }

                    this.fontFamily = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f4));
                }
            },
            FontSize: {
                get: function () {
                    return this.fontSize;
                },
                set: function (value) {
                    if (this.fontSize === value) {
                        return;
                    }

                    this.fontSize = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f5));
                }
            },
            FontStyle: {
                get: function () {
                    return this.fontStyle;
                },
                set: function (value) {
                    if (this.fontStyle === value) {
                        return;
                    }

                    this.fontStyle = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f6));
                }
            },
            FontWeight: {
                get: function () {
                    return this.fontWeight;
                },
                set: function (value) {
                    if (this.fontWeight === value) {
                        return;
                    }

                    this.fontWeight = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f7));
                }
            },
            FontStretch: {
                get: function () {
                    return this.fontStretch;
                },
                set: function (value) {
                    if (this.fontStretch === value) {
                        return;
                    }

                    this.fontStretch = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f8));
                }
            },
            TextAlignment: {
                get: function () {
                    return this.textAlignment;
                },
                set: function (value) {
                    if (this.textAlignment === value) {
                        return;
                    }

                    this.textAlignment = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f9));
                }
            },
            TextTrimming: {
                get: function () {
                    return this.textTrimming;
                },
                set: function (value) {
                    if (this.textTrimming === value) {
                        return;
                    }

                    this.textTrimming = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f10));
                }
            },
            TextWrapping: {
                get: function () {
                    return this.textWrapping;
                },
                set: function (value) {
                    if (this.textWrapping === value) {
                        return;
                    }

                    this.textWrapping = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElementTextWrapping));
                }
            },
            AcceptsReturn: {
                get: function () {
                    return this.acceptsReturn;
                },
                set: function (value) {
                    if (this.acceptsReturn === value) {
                        return;
                    }

                    this.acceptsReturn = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.cacheBind(this, this.SetContentElement));
                }
            },
            HorizontalScrollBarVisibility: {
                get: function () {
                    return this.horizontalScrollBarVisibility;
                },
                set: function (value) {
                    if (this.horizontalScrollBarVisibility === value) {
                        return;
                    }

                    this.horizontalScrollBarVisibility = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f11));
                }
            },
            VerticalScrollBarVisibility: {
                get: function () {
                    return this.verticalScrollBarVisibility;
                },
                set: function (value) {
                    if (this.verticalScrollBarVisibility === value) {
                        return;
                    }

                    this.verticalScrollBarVisibility = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f12));
                }
            },
            IsHitTestVisible: {
                get: function () {
                    return this.isHitTestVisible;
                },
                set: function (value) {
                    if (this.isHitTestVisible === value) {
                        return;
                    }

                    this.isHitTestVisible = value;
                    this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f13));
                }
            },
            AcceptsTab: false
        },
        alias: [
            "addTextChanged", "System$Windows$Media$ITextBoxRenderElement$addTextChanged",
            "removeTextChanged", "System$Windows$Media$ITextBoxRenderElement$removeTextChanged",
            "Text", "System$Windows$Media$ITextBoxRenderElement$Text",
            "MaxLength", "System$Windows$Media$ITextBoxRenderElement$MaxLength",
            "addCaretIndexChanged", "System$Windows$Media$ITextBoxRenderElement$addCaretIndexChanged",
            "removeCaretIndexChanged", "System$Windows$Media$ITextBoxRenderElement$removeCaretIndexChanged",
            "CaretIndex", "System$Windows$Media$ITextBoxRenderElement$CaretIndex",
            "addSelectionStartChanged", "System$Windows$Media$ITextBoxRenderElement$addSelectionStartChanged",
            "removeSelectionStartChanged", "System$Windows$Media$ITextBoxRenderElement$removeSelectionStartChanged",
            "SelectionStart", "System$Windows$Media$ITextBoxRenderElement$SelectionStart",
            "addSelectionLengthChanged", "System$Windows$Media$ITextBoxRenderElement$addSelectionLengthChanged",
            "removeSelectionLengthChanged", "System$Windows$Media$ITextBoxRenderElement$removeSelectionLengthChanged",
            "SelectionLength", "System$Windows$Media$ITextBoxRenderElement$SelectionLength",
            "IsPassword", "System$Windows$Media$ITextBoxRenderElement$IsPassword",
            "IsReadOnly", "System$Windows$Media$ITextBoxRenderElement$IsReadOnly",
            "SpellCheck", "System$Windows$Media$ITextBoxRenderElement$SpellCheck",
            "Bounds", "System$Windows$Media$ITextBoxRenderElement$Bounds",
            "Foreground", "System$Windows$Media$ITextBoxRenderElement$Foreground",
            "FontFamily", "System$Windows$Media$ITextBoxRenderElement$FontFamily",
            "FontSize", "System$Windows$Media$ITextBoxRenderElement$FontSize",
            "FontStyle", "System$Windows$Media$ITextBoxRenderElement$FontStyle",
            "FontWeight", "System$Windows$Media$ITextBoxRenderElement$FontWeight",
            "FontStretch", "System$Windows$Media$ITextBoxRenderElement$FontStretch",
            "TextAlignment", "System$Windows$Media$ITextBoxRenderElement$TextAlignment",
            "TextWrapping", "System$Windows$Media$ITextBoxRenderElement$TextWrapping",
            "AcceptsReturn", "System$Windows$Media$ITextBoxRenderElement$AcceptsReturn",
            "HorizontalScrollBarVisibility", "System$Windows$Media$ITextBoxRenderElement$HorizontalScrollBarVisibility",
            "VerticalScrollBarVisibility", "System$Windows$Media$ITextBoxRenderElement$VerticalScrollBarVisibility",
            "IsHitTestVisible", "System$Windows$Media$ITextBoxRenderElement$IsHitTestVisible",
            "AcceptsTab", "System$Windows$Media$ITextBoxRenderElement$AcceptsTab",
            "Focus", "System$Windows$Media$ITextBoxRenderElement$Focus",
            "ClearFocus", "System$Windows$Media$ITextBoxRenderElement$ClearFocus",
            "ProcessKeyEvent", "System$Windows$Media$ITextBoxRenderElement$ProcessKeyEvent"
        ],
        ctors: {
            ctor: function (factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlRenderElement.ctor.call(this);
                this.factory = factory;
                this.renderQueue = renderQueue;
                this.converter = converter;

                this.bounds = System.Windows.Rect.Empty;
                this.fontFamily = System.Windows.Media.FontFamily.Default;

                var styleElement = document.createElement("style");
                styleElement.textContent = "::-ms-clear { width: 0px; height: 0px; }";

                this.HtmlElement.appendChild(styleElement);

                this.SetContentElement();
            }
        },
        methods: {
            SetContentElement: function () {
                var htmlElement;

                if (this.IsPassword || !this.AcceptsReturn) {
                    htmlElement = document.createElement("input");
                    htmlElement.setAttribute("type", this.IsPassword ? "password" : "text");
                } else {
                    htmlElement = document.createElement("textArea");
                }

                this.ContentElement = new Granular.Host.Render.HtmlRenderElement.$ctor1(htmlElement);

                this.SetContentElementText();
                this.SetContentElementMaxLength();
                this.SetContentElementSelectionStart();
                this.SetContentElementSelectionLength();
                this.SetContentElementIsReadOnly();
                this.SetContentElementSpellCheck();
                this.SetContentElementTextWrapping();

                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "resize", "none");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "margin", "0px");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "padding", "0px");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "border", "0px solid transparent");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "outline", "1px solid transparent");
                Granular.Host.HtmlElementExtensions.SetHtmlStyleProperty(this.ContentElement.HtmlElement, "cursor", "inherit");
                Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.ContentElement.HtmlElement, System.Windows.Media.Brushes.Transparent, System.Windows.Rect.Zero, this.factory, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlLocation(this.ContentElement.HtmlElement, System.Windows.Point.Zero, this.converter);

                Granular.Host.HtmlElementExtensions.SetHtmlSize(this.ContentElement.HtmlElement, this.Bounds.Size, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlForeground(this.ContentElement.HtmlElement, this.Foreground, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontFamily(this.ContentElement.HtmlElement, this.FontFamily, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontSize(this.ContentElement.HtmlElement, this.FontSize, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStyle(this.ContentElement.HtmlElement, this.FontStyle, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontWeight(this.ContentElement.HtmlElement, this.FontWeight, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlFontStretch(this.ContentElement.HtmlElement, this.FontStretch, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.ContentElement.HtmlElement, this.IsHitTestVisible);
                Granular.Host.HtmlElementExtensions.SetHtmlTextAlignment(this.ContentElement.HtmlElement, this.TextAlignment, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlTextTrimming(this.ContentElement.HtmlElement, this.TextTrimming);
                Granular.Host.HtmlElementExtensions.SetHtmlHorizontalScrollBarVisibility(this.ContentElement.HtmlElement, this.HorizontalScrollBarVisibility, this.converter);
                Granular.Host.HtmlElementExtensions.SetHtmlVerticalScrollBarVisibility(this.ContentElement.HtmlElement, this.VerticalScrollBarVisibility, this.converter);

                this.ContentElement.HtmlElement.oninput = Bridge.fn.combine(this.ContentElement.HtmlElement.oninput, Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f14));
                this.ContentElement.HtmlElement.onkeydown = Bridge.fn.combine(this.ContentElement.HtmlElement.onkeydown, Bridge.fn.cacheBind(this, this.OnContentElementKeyDown));
                this.ContentElement.HtmlElement.onselect = Bridge.fn.combine(this.ContentElement.HtmlElement.onselect, Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f15));
                this.ContentElement.HtmlElement.onkeyup = Bridge.fn.combine(this.ContentElement.HtmlElement.onkeyup, Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f15));
                this.ContentElement.HtmlElement.onmouseup = Bridge.fn.combine(this.ContentElement.HtmlElement.onmouseup, Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f15));
            },
            Focus: function () {
                this.isFocused = true;
                this.ContentElement.HtmlElement.focus();
            },
            ClearFocus: function () {
                this.isFocused = false;
                this.ContentElement.HtmlElement.blur();
            },
            OnForegroundChanged: function (sender, e) {
                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f3));
            },
            OnContentElementKeyDown: function (e) {
                if (!this.IsReadOnly && this.AcceptsTab && Bridge.cast(e, KeyboardEvent).keyCode === 9) {
                    var selectionStart = this.SelectionStart;

                    var contentElementText = this.ContentElement.HtmlElement.value;
                    this.Text = System.String.format("{0}\t{1}", contentElementText.substr(0, this.SelectionStart), contentElementText.substr(((this.SelectionStart + this.SelectionLength) | 0)));

                    this.ContentElement.HtmlElement.selectionStart = ((selectionStart + 1) | 0);
                    this.ContentElement.HtmlElement.selectionEnd = ((selectionStart + 1) | 0);
                    this.GetContentElementSelection();

                    e.preventDefault();
                }

                return e;
            },
            GetContentElementSelection: function () {
                var selectionStart = this.ContentElement.HtmlElement.selectionStart;
                var selectionEnd = this.ContentElement.HtmlElement.selectionEnd;

                if (this.SelectionStart !== selectionStart || this.SelectionLength !== ((selectionEnd - selectionStart) | 0)) {
                    var changeIndex = ((this.SelectionStart + this.SelectionLength) | 0) !== selectionEnd ? selectionEnd : selectionStart;

                    this.SelectionStart = selectionStart;
                    this.SelectionLength = (selectionEnd - selectionStart) | 0;
                    this.CaretIndex = changeIndex;
                }

                return null;
            },
            SetContentElementCaretIndex: function () {
                var $t;
                if (this.isFocused && this.CaretIndex !== this.SelectionStart && this.CaretIndex !== ((this.SelectionStart + this.SelectionLength) | 0)) {
                    this.ContentElement.HtmlElement.focus();
                    ($t = this.CaretIndex, this.ContentElement.HtmlElement.setSelectionRange($t, $t));
                }
            },
            SetContentElementSelectionStart: function () {
                if (this.ContentElement.HtmlElement.selectionStart !== this.SelectionStart) {
                    this.ContentElement.HtmlElement.selectionStart = this.SelectionStart;
                }
            },
            SetContentElementSelectionLength: function () {
                if (this.ContentElement.HtmlElement.selectionEnd !== ((this.SelectionStart + this.SelectionLength) | 0)) {
                    this.ContentElement.HtmlElement.selectionEnd = ((this.SelectionStart + this.SelectionLength) | 0);
                }
            },
            SetContentElementText: function () {
                if (!Bridge.referenceEquals(this.ContentElement.HtmlElement.value, Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(this.Text))) {
                    this.ContentElement.HtmlElement.value = this.Text;
                }
            },
            SetContentElementMaxLength: function () {
                if (this.maxLength > 0) {
                    this.ContentElement.HtmlElement.setAttribute("maxLength", Bridge.toString(this.maxLength));
                } else {
                    this.ContentElement.HtmlElement.removeAttribute("maxLength");
                }
            },
            SetContentElementIsReadOnly: function () {
                if (this.IsReadOnly) {
                    this.ContentElement.HtmlElement.setAttribute("readonly", "");
                } else {
                    this.ContentElement.HtmlElement.removeAttribute("readonly");
                }
            },
            SetContentElementSpellCheck: function () {
                this.ContentElement.HtmlElement.setAttribute("spellcheck", this.converter.ToBooleanString(this.SpellCheck));
            },
            SetContentElementTextWrapping: function () {
                this.ContentElement.HtmlElement.setAttribute("wrap", this.converter.ToWrapString(this.TextWrapping));
            },
            ProcessKeyEvent: function (e) {
                e.ForceHostHandling = true;
                e.Handled = true;
            },
            OnLoad: function () {
                if (this.Foreground != null) {
                    this.Foreground.addChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                }

                this.renderQueue.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlTextBoxRenderElement.f3));
            },
            OnUnload: function () {
                if (this.Foreground != null) {
                    this.Foreground.removeChanged(Bridge.fn.cacheBind(this, this.OnForegroundChanged));
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlTextBoxRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlTextBoxRenderElement, {
        f1: function () {
            this.SetContentElementText();
            this.GetContentElementSelection();
        },
        f2: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.bounds, this.converter);
            Granular.Host.HtmlElementExtensions.SetHtmlSize(this.ContentElement.HtmlElement, this.bounds.Size, this.converter);
        },
        f3: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlForeground(this.ContentElement.HtmlElement, this.Foreground, this.converter);
        },
        f4: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontFamily(this.ContentElement.HtmlElement, this.fontFamily, this.converter);
        },
        f5: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontSize(this.ContentElement.HtmlElement, this.fontSize, this.converter);
        },
        f6: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontStyle(this.ContentElement.HtmlElement, this.fontStyle, this.converter);
        },
        f7: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontWeight(this.ContentElement.HtmlElement, this.fontWeight, this.converter);
        },
        f8: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlFontStretch(this.ContentElement.HtmlElement, this.fontStretch, this.converter);
        },
        f9: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTextAlignment(this.ContentElement.HtmlElement, this.textAlignment, this.converter);
        },
        f10: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTextTrimming(this.ContentElement.HtmlElement, this.textTrimming);
        },
        f11: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlHorizontalScrollBarVisibility(this.ContentElement.HtmlElement, this.horizontalScrollBarVisibility, this.converter);
        },
        f12: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlVerticalScrollBarVisibility(this.ContentElement.HtmlElement, this.verticalScrollBarVisibility, this.converter);
        },
        f13: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.ContentElement.HtmlElement, this.IsHitTestVisible);
        },
        f14: function (e) {
            this.Text = this.ContentElement.HtmlElement.value;
            return e;
        },
        f15: function (e) {
            return this.GetContentElementSelection();
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingContainerRenderElement", {
        inherits: [Granular.Host.Render.HtmlContainerRenderElement,System.Windows.Media.IDrawingContainerRenderElement],
        fields: {
            clipRenderResource: null,
            clip: null,
            opacity: 0,
            transformRenderResource: null,
            transform: null,
            factory: null,
            renderQueue$1: null,
            converter: null
        },
        props: {
            Clip: {
                get: function () {
                    return this.clip;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.clip, value)) {
                        return;
                    }

                    if (this.clipRenderResource != null && this.IsLoaded) {
                        this.clipRenderResource.Unload();
                    }

                    this.clip = value;
                    this.clipRenderResource = Bridge.cast((this.clip != null ? this.clip.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlGeometryRenderResource);

                    if (this.clipRenderResource != null && this.IsLoaded) {
                        this.clipRenderResource.Load();
                    }

                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement.f1));
                }
            },
            Opacity: {
                get: function () {
                    return this.opacity;
                },
                set: function (value) {
                    if (this.opacity === value) {
                        return;
                    }

                    this.opacity = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement.f2));
                }
            },
            Transform: {
                get: function () {
                    return this.transform;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.transform, value)) {
                        return;
                    }

                    if (this.transformRenderResource != null && this.IsLoaded) {
                        this.transformRenderResource.removeMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                    }

                    this.transform = value;
                    this.transformRenderResource = Bridge.cast((this.transform != null ? this.transform.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlTransformRenderResource);

                    if (this.transformRenderResource != null && this.IsLoaded) {
                        this.transformRenderResource.addMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                    }

                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement.f3));
                }
            }
        },
        alias: [
            "Clip", "System$Windows$Media$IDrawingContainerRenderElement$Clip",
            "Opacity", "System$Windows$Media$IDrawingContainerRenderElement$Opacity",
            "Transform", "System$Windows$Media$IDrawingContainerRenderElement$Transform"
        ],
        ctors: {
            ctor: function (factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlContainerRenderElement.ctor.call(this, Granular.Host.SvgDocument.CreateElement("g"), renderQueue);
                this.factory = factory;
                this.renderQueue$1 = renderQueue;
                this.converter = converter;
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlContainerRenderElement.prototype.OnLoad.call(this);

                if (this.clipRenderResource != null) {
                    this.clipRenderResource.Load();
                }

                if (this.transformRenderResource != null) {
                    this.transformRenderResource.addMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                }

                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement.f3));
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlContainerRenderElement.prototype.OnUnload.call(this);

                if (this.clipRenderResource != null) {
                    this.clipRenderResource.Unload();
                }

                if (this.transformRenderResource != null) {
                    this.transformRenderResource.removeMatrixChanged(Bridge.fn.cacheBind(this, this.OnTransformRenderResourceMatrixChanged));
                }
            },
            OnTransformRenderResourceMatrixChanged: function (sender, e) {
                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement.f4));
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlDrawingContainerRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlDrawingContainerRenderElement, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgClip(this.HtmlElement, this.clipRenderResource);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgOpacity(this.HtmlElement, this.opacity, this.converter);
        },
        f3: function () {
            Granular.Host.SvgElementExtensions.SetSvgTransform(this.HtmlElement, this.transformRenderResource != null ? this.transformRenderResource.Matrix : null, this.converter);
        },
        f4: function () {
            Granular.Host.SvgElementExtensions.SetSvgTransform(this.HtmlElement, this.transformRenderResource.Matrix, this.converter);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlDrawingGeometryRenderElement", {
        inherits: [Granular.Host.Render.HtmlDrawingShapeRenderElement,System.Windows.Media.IDrawingGeometryRenderElement],
        fields: {
            geometryRenderResource: null,
            geometry: null,
            factory$1: null,
            renderQueue$1: null,
            converter$1: null
        },
        props: {
            Geometry: {
                get: function () {
                    return this.geometry;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.geometry, value)) {
                        return;
                    }

                    if (this.geometryRenderResource != null && this.IsLoaded) {
                        this.geometryRenderResource.removeDataChanged(Bridge.fn.cacheBind(this, this.OnGeometryDataChanged));
                        this.geometryRenderResource.Unload();
                    }

                    this.geometry = value;
                    this.geometryRenderResource = Bridge.cast((this.geometry != null ? this.geometry.GetRenderResource(this.factory$1) : null), Granular.Host.Render.HtmlGeometryRenderResource);

                    if (this.geometryRenderResource != null && this.IsLoaded) {
                        this.geometryRenderResource.addDataChanged(Bridge.fn.cacheBind(this, this.OnGeometryDataChanged));
                        this.geometryRenderResource.Load();
                    }

                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingGeometryRenderElement.f1));
                }
            }
        },
        alias: ["Geometry", "System$Windows$Media$IDrawingGeometryRenderElement$Geometry"],
        ctors: {
            ctor: function (factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlDrawingShapeRenderElement.ctor.call(this, Granular.Host.SvgDocument.CreateElement("path"), factory, renderQueue, converter);
                this.factory$1 = factory;
                this.renderQueue$1 = renderQueue;
                this.converter$1 = converter;
            }
        },
        methods: {
            OnLoad: function () {
                Granular.Host.Render.HtmlDrawingShapeRenderElement.prototype.OnLoad.call(this);

                if (this.geometryRenderResource != null) {
                    this.geometryRenderResource.addDataChanged(Bridge.fn.cacheBind(this, this.OnGeometryDataChanged));
                    this.geometryRenderResource.Load();
                }

                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingGeometryRenderElement.f1));
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlDrawingShapeRenderElement.prototype.OnUnload.call(this);

                if (this.geometryRenderResource != null) {
                    this.geometryRenderResource.removeDataChanged(Bridge.fn.cacheBind(this, this.OnGeometryDataChanged));
                    this.geometryRenderResource.Unload();
                }
            },
            OnGeometryDataChanged: function (sender, e) {
                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlDrawingGeometryRenderElement.f1));
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlDrawingGeometryRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlDrawingGeometryRenderElement, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgGeometry(this.HtmlElement, this.geometryRenderResource);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlLinearGradientBrushRenderResource", {
        inherits: [Granular.Host.Render.HtmlGradientBrushRenderResource,System.Windows.Media.ILinearGradientBrushRenderResource],
        fields: {
            startPoint: null,
            endPoint: null,
            renderQueue$1: null,
            converter$1: null
        },
        props: {
            StartPoint: {
                get: function () {
                    return this.startPoint;
                },
                set: function (value) {
                    if (System.Windows.Point.op_Equality(this.startPoint, value)) {
                        return;
                    }

                    this.startPoint = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlLinearGradientBrushRenderResource.f1));
                }
            },
            EndPoint: {
                get: function () {
                    return this.endPoint;
                },
                set: function (value) {
                    if (System.Windows.Point.op_Equality(this.endPoint, value)) {
                        return;
                    }

                    this.endPoint = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlLinearGradientBrushRenderResource.f2));
                }
            }
        },
        alias: [
            "StartPoint", "System$Windows$Media$ILinearGradientBrushRenderResource$StartPoint",
            "EndPoint", "System$Windows$Media$ILinearGradientBrushRenderResource$EndPoint"
        ],
        ctors: {
            ctor: function (renderQueue, converter, svgDefinitionContainer) {
                this.$initialize();
                Granular.Host.Render.HtmlGradientBrushRenderResource.ctor.call(this, "linearGradient", renderQueue, converter, svgDefinitionContainer);
                this.renderQueue$1 = renderQueue;
                this.converter$1 = converter;
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlLinearGradientBrushRenderResource", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlLinearGradientBrushRenderResource, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgStartPoint(this.HtmlElement, this.startPoint, this.converter$1);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgEndPoint(this.HtmlElement, this.endPoint, this.converter$1);
        }
    });

    Bridge.define("Granular.Host.Render.HtmlRadialGradientBrushRenderResource", {
        inherits: [Granular.Host.Render.HtmlGradientBrushRenderResource,System.Windows.Media.IRadialGradientBrushRenderResource],
        fields: {
            center: null,
            gradientOrigin: null,
            radiusX: 0,
            radiusY: 0,
            renderQueue$1: null,
            converter$1: null
        },
        props: {
            Center: {
                get: function () {
                    return this.center;
                },
                set: function (value) {
                    if (System.Windows.Point.op_Equality(this.center, value)) {
                        return;
                    }

                    this.center = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlRadialGradientBrushRenderResource.f1));
                }
            },
            GradientOrigin: {
                get: function () {
                    return this.gradientOrigin;
                },
                set: function (value) {
                    if (System.Windows.Point.op_Equality(this.gradientOrigin, value)) {
                        return;
                    }

                    this.gradientOrigin = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlRadialGradientBrushRenderResource.f2));
                }
            },
            RadiusX: {
                get: function () {
                    return this.radiusX;
                },
                set: function (value) {
                    if (this.radiusX === value) {
                        return;
                    }

                    this.radiusX = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlRadialGradientBrushRenderResource.f3));
                }
            },
            RadiusY: {
                get: function () {
                    return this.radiusY;
                },
                set: function (value) {
                    if (this.radiusY === value) {
                        return;
                    }

                    this.radiusY = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlRadialGradientBrushRenderResource.f3));
                }
            }
        },
        alias: [
            "Center", "System$Windows$Media$IRadialGradientBrushRenderResource$Center",
            "GradientOrigin", "System$Windows$Media$IRadialGradientBrushRenderResource$GradientOrigin",
            "RadiusX", "System$Windows$Media$IRadialGradientBrushRenderResource$RadiusX",
            "RadiusY", "System$Windows$Media$IRadialGradientBrushRenderResource$RadiusY"
        ],
        ctors: {
            ctor: function (renderQueue, converter, svgDefinitionContainer) {
                this.$initialize();
                Granular.Host.Render.HtmlGradientBrushRenderResource.ctor.call(this, "radialGradient", renderQueue, converter, svgDefinitionContainer);
                this.renderQueue$1 = renderQueue;
                this.converter$1 = converter;
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlRadialGradientBrushRenderResource", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlRadialGradientBrushRenderResource, {
        f1: function () {
            Granular.Host.SvgElementExtensions.SetSvgCenter(this.HtmlElement, this.center, this.converter$1);
        },
        f2: function () {
            Granular.Host.SvgElementExtensions.SetSvgGradientOrigin(this.HtmlElement, this.gradientOrigin, this.converter$1);
        },
        f3: function () {
            this.HtmlElement.setAttribute("gradientTransform", this.converter$1.ToGradientTransformString(this.RadiusX, this.RadiusY));
        }
    });

    Bridge.define("Granular.Host.Render.HtmlVisualRenderElement", {
        inherits: [Granular.Host.Render.HtmlContainerRenderElement,System.Windows.Media.IVisualRenderElement],
        statics: {
            fields: {
                ElementTagNameCache: null
            },
            ctors: {
                init: function () {
                    this.ElementTagNameCache = Granular.Collections.CacheDictionary$2(Function,System.String).CreateUsingStringKeys(Granular.Host.Render.HtmlVisualRenderElement.ResolveElementTagName, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f1);
                }
            },
            methods: {
                GetElementTagName: function (target) {
                    return Granular.Host.Render.HtmlVisualRenderElement.ElementTagNameCache.GetValue(Bridge.getType(target));
                },
                ResolveElementTagName: function (type) {
                    var typeName = System.String.replaceAll(Bridge.Reflection.getTypeName(type), String.fromCharCode(36), String.fromCharCode(95));
                    return System.Linq.Enumerable.from(Granular.Host.HtmlDefinition.Tags).contains(typeName.toLowerCase()) ? System.String.format("{0}_", [typeName]) : typeName;
                },
                GetElementId: function (target) {
                    var nameAttribute = Bridge.as(System.Linq.Enumerable.from(Bridge.Reflection.getAttributes(Bridge.getType(target), System.Windows.Markup.RuntimeNamePropertyAttribute, true)).firstOrDefault(null, null), System.Windows.Markup.RuntimeNamePropertyAttribute);
                    return nameAttribute != null ? Bridge.cast(Bridge.Reflection.midel(Bridge.Reflection.getMembers(Bridge.getType(target), 16, 284, nameAttribute.Name).g, Bridge.unbox(target))(), System.String) : "";
                },
                CreateHtmlElement: function (owner) {
                    var htmlElement = document.createElement(Granular.Host.Render.HtmlVisualRenderElement.GetElementTagName(owner));

                    var htmlElementId = Granular.Host.Render.HtmlVisualRenderElement.GetElementId(owner);

                    if (!Granular.Extensions.StringExtensions.IsNullOrEmpty(htmlElementId)) {
                        htmlElement.id = htmlElementId;
                    }

                    return htmlElement;
                }
            }
        },
        fields: {
            background: null,
            bounds: null,
            clipRenderResource: null,
            clip: null,
            clipToBounds: false,
            isHitTestVisible: false,
            isVisible: false,
            opacity: 0,
            transform: null,
            factory: null,
            renderQueue$1: null,
            converter: null
        },
        props: {
            Background: {
                get: function () {
                    return this.background;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.background, value)) {
                        return;
                    }

                    if (this.IsLoaded && this.background != null) {
                        this.background.removeChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                    }

                    this.background = value;

                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f2));

                    if (this.IsLoaded && this.background != null) {
                        this.background.addChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                    }
                }
            },
            Bounds: {
                get: function () {
                    return this.bounds;
                },
                set: function (value) {
                    if (System.Windows.Rect.op_Equality(this.bounds, value)) {
                        return;
                    }

                    this.bounds = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f3));
                }
            },
            Clip: {
                get: function () {
                    return this.clip;
                },
                set: function (value) {
                    if (Bridge.referenceEquals(this.clip, value)) {
                        return;
                    }

                    if (this.clipRenderResource != null && this.IsLoaded) {
                        this.clipRenderResource.Unload();
                    }

                    this.clip = value;
                    this.clipRenderResource = Bridge.cast((this.clip != null ? this.clip.GetRenderResource(this.factory) : null), Granular.Host.Render.HtmlGeometryRenderResource);

                    if (this.clipRenderResource != null && this.IsLoaded) {
                        this.clipRenderResource.Load();
                    }

                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f4));
                }
            },
            ClipToBounds: {
                get: function () {
                    return this.clipToBounds;
                },
                set: function (value) {
                    if (this.clipToBounds === value) {
                        return;
                    }

                    this.clipToBounds = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f5));
                }
            },
            IsHitTestVisible: {
                get: function () {
                    return this.isHitTestVisible;
                },
                set: function (value) {
                    if (this.isHitTestVisible === value) {
                        return;
                    }

                    this.isHitTestVisible = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f6));
                }
            },
            IsVisible: {
                get: function () {
                    return this.isVisible;
                },
                set: function (value) {
                    if (this.isVisible === value) {
                        return;
                    }

                    this.isVisible = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f7));
                }
            },
            Opacity: {
                get: function () {
                    return this.opacity;
                },
                set: function (value) {
                    if (this.opacity === value) {
                        return;
                    }

                    this.opacity = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f8));
                }
            },
            Transform: {
                get: function () {
                    return this.transform;
                },
                set: function (value) {
                    if (System.Windows.Media.Matrix.op_Equality(this.transform, value)) {
                        return;
                    }

                    this.transform = value;
                    this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f9));
                }
            }
        },
        alias: [
            "Background", "System$Windows$Media$IVisualRenderElement$Background",
            "Bounds", "System$Windows$Media$IVisualRenderElement$Bounds",
            "Clip", "System$Windows$Media$IVisualRenderElement$Clip",
            "ClipToBounds", "System$Windows$Media$IVisualRenderElement$ClipToBounds",
            "IsHitTestVisible", "System$Windows$Media$IVisualRenderElement$IsHitTestVisible",
            "IsVisible", "System$Windows$Media$IVisualRenderElement$IsVisible",
            "Opacity", "System$Windows$Media$IVisualRenderElement$Opacity",
            "Transform", "System$Windows$Media$IVisualRenderElement$Transform"
        ],
        ctors: {
            ctor: function (owner, factory, renderQueue, converter) {
                this.$initialize();
                Granular.Host.Render.HtmlContainerRenderElement.ctor.call(this, Granular.Host.Render.HtmlVisualRenderElement.CreateHtmlElement(owner), renderQueue);
                this.factory = factory;
                this.renderQueue$1 = renderQueue;
                this.converter = converter;

                this.bounds = System.Windows.Rect.Zero;
                this.isVisible = true;
                this.opacity = 1;
                this.transform = System.Windows.Media.Matrix.Identity;

                Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.Bounds, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlClipToBounds(this.HtmlElement, this.ClipToBounds);
                Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.HtmlElement, this.IsHitTestVisible && this.Background != null);
                Granular.Host.HtmlElementExtensions.SetHtmlIsVisible(this.HtmlElement, this.IsVisible);
                Granular.Host.HtmlElementExtensions.SetHtmlOpacity(this.HtmlElement, this.Opacity, converter);
                Granular.Host.HtmlElementExtensions.SetHtmlTransform(this.HtmlElement, this.Transform, converter);
            }
        },
        methods: {
            OnBackgroundChanged: function (sender, e) {
                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f10));
            },
            OnLoad: function () {
                Granular.Host.Render.HtmlContainerRenderElement.prototype.OnLoad.call(this);

                if (this.Background != null) {
                    this.Background.addChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                }

                if (this.clipRenderResource != null) {
                    this.clipRenderResource.Load();
                }

                this.renderQueue$1.InvokeAsync(Bridge.fn.bind(this, $asm.$.Granular.Host.Render.HtmlVisualRenderElement.f11));
            },
            OnUnload: function () {
                Granular.Host.Render.HtmlContainerRenderElement.prototype.OnUnload.call(this);

                if (this.Background != null) {
                    this.Background.removeChanged(Bridge.fn.cacheBind(this, this.OnBackgroundChanged));
                }

                if (this.clipRenderResource != null) {
                    this.clipRenderResource.Unload();
                }
            }
        }
    });

    Bridge.ns("Granular.Host.Render.HtmlVisualRenderElement", $asm.$);

    Bridge.apply($asm.$.Granular.Host.Render.HtmlVisualRenderElement, {
        f1: function (type) {
            return Bridge.Reflection.getTypeFullName(type);
        },
        f2: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.HtmlElement, this.background, new System.Windows.Rect.$ctor3(this.Bounds.Size), this.factory, this.converter);
            Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.HtmlElement, this.IsHitTestVisible && this.background != null);
        },
        f3: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBounds(this.HtmlElement, this.bounds, this.converter);
            Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.HtmlElement, this.background, new System.Windows.Rect.$ctor3(this.Bounds.Size), this.factory, this.converter);
        },
        f4: function () {
            Granular.Host.SvgElementExtensions.SetSvgClip(this.HtmlElement, this.clipRenderResource);
        },
        f5: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlClipToBounds(this.HtmlElement, this.clipToBounds);
        },
        f6: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlIsHitTestVisible(this.HtmlElement, this.isHitTestVisible && this.Background != null);
        },
        f7: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlIsVisible(this.HtmlElement, this.isVisible);
        },
        f8: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlOpacity(this.HtmlElement, this.opacity, this.converter);
        },
        f9: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlTransform(this.HtmlElement, this.transform, this.converter);
        },
        f10: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.HtmlElement, this.background, new System.Windows.Rect.$ctor3(this.Bounds.Size), this.factory, this.converter);
        },
        f11: function () {
            Granular.Host.HtmlElementExtensions.SetHtmlBackground(this.HtmlElement, this.Background, new System.Windows.Rect.$ctor3(this.Bounds.Size), this.factory, this.converter);
        }
    });
});
