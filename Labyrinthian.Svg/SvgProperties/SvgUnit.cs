using System;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Unit used for SVG-properties that can be a percentage(e.g. opacity).
    /// </summary>
    public struct SvgUnit : IEquatable<SvgUnit>
    {
        /// <summary>
        /// If <see langword="true" />, then ToString method will
        /// return a percentage instead of value.
        /// </summary>
        public bool IsPercentage;
        /// <summary>
        /// A numeric value(it's never a percentage).
        /// </summary>
        public float Value;

        /// <summary>
        /// Create a value from percentage.
        /// </summary>
        /// <param name="percentage">Percentage.</param>
        /// <returns>
        /// Svg unit that represents <paramref name="percentage"/>.
        /// </returns>
        public static SvgUnit FromPercentage(float percentage)
        {
            return new SvgUnit()
            {
                IsPercentage = true,
                Value = percentage / 100f
            };
        }

        public readonly override string ToString()
        {
            if (IsPercentage)
            {
                float percentage = Value * 100f;
                return $"{percentage.ToInvariantString()}%";
            }
            return Value.ToInvariantString();
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is SvgUnit unit && Equals(unit);
        }

        public readonly bool Equals(SvgUnit other)
        {
            return Value == other.Value;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(SvgUnit left, SvgUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SvgUnit left, SvgUnit right)
        {
            return !(left == right);
        }

        public static implicit operator float(SvgUnit unit)
        {
            return unit.Value;
        }

        public static implicit operator SvgUnit(float f)
        {
            return new SvgUnit()
            {
                IsPercentage = false,
                Value = f
            };
        }
    }
}
