using System.Globalization;
using Granular.Extensions;

namespace System.Windows.Controls
{
    /// <summary>Represents the lengths of elements within the <see cref="T:System.Windows.Controls.DataGrid" /> control. </summary>
    [System.Windows.Markup.TypeConverter(typeof(DataGridLengthConverter))]
    public struct DataGridLength : IEquatable<DataGridLength>
    {
        private static readonly DataGridLength _auto = new DataGridLength(1.0, DataGridLengthUnitType.Auto, 0.0, 0.0);
        private static readonly DataGridLength _sizeToCells = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells, 0.0, 0.0);
        private static readonly DataGridLength _sizeToHeader = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader, 0.0, 0.0);
        private double _unitValue;
        private DataGridLengthUnitType _unitType;
        private double _desiredValue;
        private double _displayValue;
        private const double AutoValue = 1.0;

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with an absolute value in pixels.</summary>
        /// <param name="pixels">The absolute pixel value (96 pixels-per-inch) to initialize the length to.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="pixels" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.</exception>
        public DataGridLength(double pixels)
        {
            this = new DataGridLength(pixels, DataGridLengthUnitType.Pixel);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with a specified value and unit.</summary>
        /// <param name="value">The requested size of the element.</param>
        /// <param name="type">The type that is used to determine how the size of the element is calculated.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.-or-
        /// <paramref name="type" /> is not <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />, or <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />.</exception>
        public DataGridLength(double value, DataGridLengthUnitType type)
        {
            this = new DataGridLength(value, type, type == DataGridLengthUnitType.Pixel ? value : double.NaN, type == DataGridLengthUnitType.Pixel ? value : double.NaN);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with the specified value, unit, desired value, and display value.</summary>
        /// <param name="value">The requested size of the element.</param>
        /// <param name="type">The type that is used to determine how the size of the element is calculated.</param>
        /// <param name="desiredValue">The calculated size needed for the element.</param>
        /// <param name="displayValue">The allocated size for the element.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.-or-
        /// <paramref name="type" /> is not <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />, or <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />.-or-
        /// <paramref name="desiredValue" /> is <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />.-or-
        /// <paramref name="displayValue" /> is <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />.</exception>
        public DataGridLength(double value, DataGridLengthUnitType type, double desiredValue, double displayValue)
        {
            if (value.IsNaN() || double.IsInfinity(value))
                throw new ArgumentException("DataGridLength_Infinity", nameof(value));
            if (type != DataGridLengthUnitType.Auto && type != DataGridLengthUnitType.Pixel && (type != DataGridLengthUnitType.Star && type != DataGridLengthUnitType.SizeToCells) && type != DataGridLengthUnitType.SizeToHeader)
                throw new ArgumentException("DataGridLength_InvalidType", nameof(type));
            if (double.IsInfinity(desiredValue))
                throw new ArgumentException("DataGridLength_Infinity", nameof(desiredValue));
            if (double.IsInfinity(displayValue))
                throw new ArgumentException("DataGridLength_Infinity", nameof(displayValue));
            this._unitValue = type == DataGridLengthUnitType.Auto ? 1.0 : value;
            this._unitType = type;
            this._desiredValue = desiredValue;
            this._displayValue = displayValue;
        }

        /// <summary>Compares two <see cref="T:System.Windows.Controls.DataGridLength" /> structures for equality.</summary>
        /// <param name="gl1">The first <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
        /// <param name="gl2">The second <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="T:System.Windows.Controls.DataGridLength" /> instances have the same value or sizing mode; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(DataGridLength gl1, DataGridLength gl2)
        {
            if (gl1.UnitType != gl2.UnitType || gl1.Value != gl2.Value || gl1.DesiredValue != gl2.DesiredValue && (!gl1.DesiredValue.IsNaN() || !gl2.DesiredValue.IsNaN()))
                return false;
            if (gl1.DisplayValue == gl2.DisplayValue)
                return true;
            if (gl1.DisplayValue.IsNaN())
                return gl2.DisplayValue.IsNaN();
            return false;
        }

        /// <summary>Compares two <see cref="T:System.Windows.Controls.DataGridLength" /> structures to determine whether they are not equal.</summary>
        /// <param name="gl1">The first <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
        /// <param name="gl2">The second <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="T:System.Windows.Controls.DataGridLength" /> instances do not have the same value or sizing mode; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(DataGridLength gl1, DataGridLength gl2)
        {
            if (gl1.UnitType != gl2.UnitType || gl1.Value != gl2.Value || gl1.DesiredValue != gl2.DesiredValue && (!gl1.DesiredValue.IsNaN() || !gl2.DesiredValue.IsNaN()))
                return true;
            if (gl1.DisplayValue == gl2.DisplayValue)
                return false;
            if (gl1.DisplayValue.IsNaN())
                return !gl2.DisplayValue.IsNaN();
            return true;
        }

        /// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns>
        /// <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Controls.DataGridLength" /> with the same value or sizing mode as the current <see cref="T:System.Windows.Controls.DataGridLength" />; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DataGridLength)
                return this == (DataGridLength)obj;
            return false;
        }

        /// <summary>Determines whether the specified <see cref="T:System.Windows.Controls.DataGridLength" /> is equal to the current <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
        /// <param name="other">The <see cref="T:System.Windows.Controls.DataGridLength" /> to compare to the current instance.</param>
        /// <returns>
        /// <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Controls.DataGridLength" /> with the same value or sizing mode as the current <see cref="T:System.Windows.Controls.DataGridLength" />; otherwise, <see langword="false" />.</returns>
        public bool Equals(DataGridLength other)
        {
            return this == other;
        }

        /// <summary>Gets a hash code for the <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Windows.Controls.DataGridLength" />.</returns>
        public override int GetHashCode()
        {
            return (int)((int)this._unitValue + this._unitType + (int)this._desiredValue + (int)this._displayValue);
        }

        /// <summary>Gets a value that indicates whether this instance sizes elements based on a fixed pixel value.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />; otherwise, <see langword="false" />.</returns>
        public bool IsAbsolute
        {
            get
            {
                return this._unitType == DataGridLengthUnitType.Pixel;
            }
        }

        /// <summary>Gets a value that indicates whether this instance automatically sizes elements based on both the content of cells and the column headers.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />; otherwise, <see langword="false" />.</returns>
        public bool IsAuto
        {
            get
            {
                return this._unitType == DataGridLengthUnitType.Auto;
            }
        }

        /// <summary>Gets a value that indicates whether this instance automatically sizes elements based on a weighted proportion of available space.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />; otherwise, <see langword="false" />.</returns>
        public bool IsStar
        {
            get
            {
                return this._unitType == DataGridLengthUnitType.Star;
            }
        }

        /// <summary>Gets a value that indicates whether this instance automatically sizes elements based on the content of the cells.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />; otherwise, <see langword="false" />.</returns>
        public bool IsSizeToCells
        {
            get
            {
                return this._unitType == DataGridLengthUnitType.SizeToCells;
            }
        }

        /// <summary>Gets a value that indicates whether this instance automatically sizes elements based on the header.</summary>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />; otherwise, <see langword="false" />.</returns>
        public bool IsSizeToHeader
        {
            get
            {
                return this._unitType == DataGridLengthUnitType.SizeToHeader;
            }
        }

        /// <summary>Gets the absolute value of the <see cref="T:System.Windows.Controls.DataGridLength" /> in pixels.</summary>
        /// <returns>The absolute value of the <see cref="T:System.Windows.Controls.DataGridLength" /> in pixels, or 1.0 if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />.</returns>
        public double Value
        {
            get
            {
                if (this._unitType != DataGridLengthUnitType.Auto)
                    return this._unitValue;
                return 1.0;
            }
        }

        /// <summary>Gets the type that is used to determine how the size of the element is calculated.</summary>
        /// <returns>A type that represents how size is determined.</returns>
        public DataGridLengthUnitType UnitType
        {
            get
            {
                return this._unitType;
            }
        }

        /// <summary>Gets the calculated pixel value needed for the element.</summary>
        /// <returns>The number of pixels calculated for the size of the element.</returns>
        public double DesiredValue
        {
            get
            {
                return this._desiredValue;
            }
        }

        /// <summary>Gets the pixel value allocated for the size of the element.</summary>
        /// <returns>The number of pixels allocated for the element.</returns>
        public double DisplayValue
        {
            get
            {
                return this._displayValue;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represent the current object.</returns>
        public override string ToString()
        {
            return DataGridLengthConverter.ConvertToString(this, CultureInfo.InvariantCulture);
        }

        /// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the standard automatic sizing mode.</summary>
        /// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the standard automatic sizing mode.</returns>
        public static DataGridLength Auto
        {
            get
            {
                return DataGridLength._auto;
            }
        }

        /// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the cell-based automatic sizing mode.</summary>
        /// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the cell-based automatic sizing mode.</returns>
        public static DataGridLength SizeToCells
        {
            get
            {
                return DataGridLength._sizeToCells;
            }
        }

        /// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the header-based automatic sizing mode.</summary>
        /// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the header-based automatic sizing mode.</returns>
        public static DataGridLength SizeToHeader
        {
            get
            {
                return DataGridLength._sizeToHeader;
            }
        }

        /// <summary>Converts a <see cref="T:System.Double" /> to an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
        /// <param name="value">The absolute pixel value (96 pixels-per-inch) to initialize the length to.</param>
        /// <returns>An object that represents the specified length.</returns>
        public static implicit operator DataGridLength(double value)
        {
            return new DataGridLength(value);
        }
    }
}
