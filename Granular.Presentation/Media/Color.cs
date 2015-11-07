using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Xaml;
using Granular.Extensions;

namespace System.Windows.Media
{
    [TypeConverter(typeof(ColorTypeConverter))]
    public sealed class Color
    {
        public byte A { get; private set; }
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        private Color(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public override bool Equals(object obj)
        {
            Color other = obj as Color;

            return Object.ReferenceEquals(this, other) || !Object.ReferenceEquals(other, null) &&
                this.A == other.A && this.R == other.R &&
                this.G == other.G && this.B == other.B;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
        }

        public static bool operator ==(Color color1, Color color2)
        {
            return Object.ReferenceEquals(color1, null) ? Object.ReferenceEquals(color2, null) : color1.Equals(color2);
        }

        public static bool operator !=(Color color1, Color color2)
        {
            return !(color1 == color2);
        }

        public static Color operator +(Color color1, Color color2)
        {
            return new Color(
                (byte)(color1.A + color2.A).Bounds(0, 255),
                (byte)(color1.R + color2.R).Bounds(0, 255),
                (byte)(color1.G + color2.G).Bounds(0, 255),
                (byte)(color1.B + color2.B).Bounds(0, 255));
        }

        public static Color operator -(Color color1, Color color2)
        {
            return new Color(
                (byte)(color1.A - color2.A).Bounds(0, 255),
                (byte)(color1.R - color2.R).Bounds(0, 255),
                (byte)(color1.G - color2.G).Bounds(0, 255),
                (byte)(color1.B - color2.B).Bounds(0, 255));
        }

        public static Color operator *(Color color, double scalar)
        {
            return new Color(
                (byte)(scalar * color.A).Bounds(0, 255),
                (byte)(scalar * color.R).Bounds(0, 255),
                (byte)(scalar * color.G).Bounds(0, 255),
                (byte)(scalar * color.B).Bounds(0, 255));
        }

        public static Color operator *(double scalar, Color color)
        {
            return color * scalar;
        }

        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color(255, r, g, b);
        }

        public static Color FromUInt32(uint argb)
        {
            return new Color(
                (byte)((argb & 0xff000000) >> 24),
                (byte)((argb & 0x00ff0000) >> 16),
                (byte)((argb & 0x0000ff00) >> 8),
                (byte)(argb & 0x000000ff));
        }

        public bool IsClose(Color color)
        {
            return Math.Abs(this.A - color.A) <= 1 &&
                Math.Abs(this.R - color.R) <= 1 &&
                Math.Abs(this.G - color.G) <= 1 &&
                Math.Abs(this.B - color.B) <= 1;
        }
    }

    public static class ColorExtensions
    {
        public static Color ApplyOpacity(this Color color, double opacity)
        {
            return opacity == 1 ? color : Color.FromArgb((byte)(opacity * color.A), color.R, color.G, color.B);
        }
    }

    public class ColorTypeConverter : ITypeConverter
    {
        public object ConvertFrom(XamlNamespaces namespaces, object value)
        {
            if (value is string)
            {
                string stringValue = ((string)value).Trim();

                if (stringValue.StartsWith("#") && stringValue.Length == 9)
                {
                    return Color.FromUInt32(Granular.Compatibility.Convert.ToUInt32(stringValue.Substring(1), 16));
                }

                if (stringValue.StartsWith("#") && stringValue.Length == 7)
                {
                    return Color.FromUInt32(0xff000000 | Granular.Compatibility.Convert.ToUInt32(stringValue.Substring(1), 16));
                }

                if (stringValue.StartsWith("#") && stringValue.Length == 4)
                {
                    return Color.FromUInt32(0xff000000 | Granular.Compatibility.Convert.ToUInt32(String.Format("{0}{0}{1}{1}{2}{2}", stringValue[1].ToString(), stringValue[2].ToString(), stringValue[3].ToString()), 16));
                }

                PropertyInfo propertyInfo = typeof(Colors).GetProperty(stringValue, BindingFlags.Static | BindingFlags.Public);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(null, new object[0]);
                }
            }

            throw new Granular.Exception("Can't convert \"{0}\" to Color", value);
        }
    }
}
