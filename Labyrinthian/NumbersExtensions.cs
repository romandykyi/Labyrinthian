using System;
using System.Globalization;

namespace Labyrinthian
{
    public static class FloatExtensions
    {
        private const float DefaultEpsilon = 1e-8f;

        public static string ToInvariantString(this float value) => value.ToString(CultureInfo.InvariantCulture);

        public static bool ApproximatelyEquals(this float a, float b, float epsilon = DefaultEpsilon)
        {
            return Math.Abs(b - a) <= epsilon;
        }
    }
    public static class DoubleExtensions
    {
        public static string ToInvariantString(this double value) => value.ToString(CultureInfo.InvariantCulture);
    }
    public static class DecimalExtensions
    {
        public static string ToInvariantString(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
