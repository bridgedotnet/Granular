using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using Granular.Extensions;

namespace System.Windows.Controls
{
    /// <summary>Converts instances of various types to and from instances of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
    public class DataGridLengthConverter : ITypeConverter
    {
        private static string[] _unitStrings = new string[5]
        {
      "auto",
      "px",
      "sizetocells",
      "sizetoheader",
      "*"
        };
        private static string[] _nonStandardUnitStrings = new string[3]
        {
      "in",
      "cm",
      "pt"
        };
        private static double[] _pixelUnitFactors = new double[3]
        {
      96.0,
      4800.0 / (double) sbyte.MaxValue,
      4.0 / 3.0
        };

        internal static string ConvertToString(DataGridLength length, CultureInfo cultureInfo)
        {
            switch (length.UnitType)
            {
                case DataGridLengthUnitType.Auto:
                case DataGridLengthUnitType.SizeToCells:
                case DataGridLengthUnitType.SizeToHeader:
                    return length.UnitType.ToString();
                case DataGridLengthUnitType.Star:
                    if (!length.Value.IsOne())
                        return Convert.ToString(length.Value, (IFormatProvider)cultureInfo) + "*";
                    return "*";
                default:
                    return Convert.ToString(length.Value, (IFormatProvider)cultureInfo);
            }
        }

        private static DataGridLength ConvertFromString(string s, CultureInfo cultureInfo)
        {
            string lowerInvariant = s.Trim().ToLower();
            for (int index = 0; index < 3; ++index)
            {
                string unitString = DataGridLengthConverter._unitStrings[index];
                if (lowerInvariant == unitString)
                    return new DataGridLength(1.0, (DataGridLengthUnitType)index);
            }
            double num1 = 0.0;
            DataGridLengthUnitType type = DataGridLengthUnitType.Pixel;
            int length1 = lowerInvariant.Length;
            int num2 = 0;
            double num3 = 1.0;
            int length2 = DataGridLengthConverter._unitStrings.Length;
            for (int index = 3; index < length2; ++index)
            {
                string unitString = DataGridLengthConverter._unitStrings[index];
                if (lowerInvariant.EndsWith(unitString))
                {
                    num2 = unitString.Length;
                    type = (DataGridLengthUnitType)index;
                    break;
                }
            }
            if (num2 == 0)
            {
                int length3 = DataGridLengthConverter._nonStandardUnitStrings.Length;
                for (int index = 0; index < length3; ++index)
                {
                    string standardUnitString = DataGridLengthConverter._nonStandardUnitStrings[index];
                    if (lowerInvariant.EndsWith(standardUnitString))
                    {
                        num2 = standardUnitString.Length;
                        num3 = DataGridLengthConverter._pixelUnitFactors[index];
                        break;
                    }
                }
            }
            if (length1 == num2)
            {
                if (type == DataGridLengthUnitType.Star)
                    num1 = 1.0;
            }
            else
                num1 = Convert.ToDouble(lowerInvariant.Substring(0, length1 - num2), (IFormatProvider)cultureInfo) * num3;
            return new DataGridLength(num1, type);
        }

        public object ConvertFrom(XamlNamespaces namespaces, Uri sourceUri, object value)
        {
            if (value != null)
            {
                string s = value as string;
                if (s != null)
                    return ConvertFromString(s, CultureInfo.InvariantCulture);
                double d = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                DataGridLengthUnitType type;
                if (Double.IsNaN(d))
                {
                    d = 1.0;
                    type = DataGridLengthUnitType.Auto;
                }
                else
                    type = DataGridLengthUnitType.Pixel;
                if (!double.IsInfinity(d))
                    return (object)new DataGridLength(d, type);
            }

            throw new Granular.Exception("ConvertTo Failed");
        }
    }
}
