using System;

namespace Granular.Extensions
{
    public static class DoubleExtensions
    {
        internal const double DBL_EPSILON = 2.2204460492503131e-016; /* smallest such that 1.0+DBL_EPSILON != 1.0 */
        private const double Epsilon = 1e-10;

        public static bool IsClose(this double @this, double value)
        {
            // |a-b|/(|a|+|b|+1) < Epsilon
            return (@this == value) || @this.IsNaN() && value.IsNaN() || Math.Abs(@this - value) < Epsilon * (Math.Abs(@this) + Math.Abs(value) + 1);
        }

        public static bool IsNaN(this double @this)
        {
            return Double.IsNaN(@this);
        }

        public static double DefaultIfNaN(this double @this, double defaultValue)
        {
            return Double.IsNaN(@this) ? defaultValue : @this;
        }

        public static double Min(this double @this, double value)
        {
            return @this < value ? @this : value;
        }

        public static double Max(this double @this, double value)
        {
            return @this > value ? @this : value;
        }

        public static double Bounds(this double @this, double minimum, double maximum)
        {
            if (minimum > maximum)
            {
                throw new Granular.Exception("Invalid bounds (minimum: {0}, maximum: {1})", minimum, maximum);
            }

            return @this.Max(minimum).Min(maximum);
        }

        public static double Abs(this double @this)
        {
            return Math.Abs(@this);
        }

        /// <summary>
        /// IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
        /// but this is faster.
        /// </summary>
        /// <returns>
        /// bool - the result of the AreClose comparision.
        /// </returns>
        /// <param name="value"> The double to compare to 1. </param>
        public static bool IsOne(this double value)
        {
            return Math.Abs(value - 1.0) < 10.0 * DBL_EPSILON;
        }
    }
}
