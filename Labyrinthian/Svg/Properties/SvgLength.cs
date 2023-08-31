using System;

namespace Labyrinthian.Svg
{
    public enum SvgLengthUnit
    {
        None = 0,
        Centimeter,
        Millimeter,
        Inch,
        Em,
        Ex,
        Point,
        Pica,
        Pixel,
        Percentage
    }

    public struct SvgLength : IEquatable<SvgLength>
    {
        public float Value;
        public SvgLengthUnit Unit;

        public SvgLength(float value, SvgLengthUnit unit = SvgLengthUnit.None)
        {
            Value = value;
            Unit = unit;
        }

        public readonly override string ToString()
        {
            string result = Value.ToInvariantString();
            switch (Unit)
            {
                case SvgLengthUnit.Centimeter:
                    result += "cm";
                    break;
                case SvgLengthUnit.Millimeter:
                    result += "mm";
                    break;
                case SvgLengthUnit.Em:
                    result += "em";
                    break;
                case SvgLengthUnit.Ex:
                    result += "ex";
                    break;
                case SvgLengthUnit.Point:
                    result += "pt";
                    break;
                case SvgLengthUnit.Pica:
                    result += "pc";
                    break;
                case SvgLengthUnit.Pixel:
                    result += "px";
                    break;
                case SvgLengthUnit.Inch:
                    result += "in";
                    break;
                case SvgLengthUnit.Percentage:
                    result += "%";
                    break;
            }
            return result;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is SvgLength size && Equals(size);
        }

        public readonly bool Equals(SvgLength other)
        {
            return Value == other.Value &&
                   Unit == other.Unit;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        public static bool operator ==(SvgLength left, SvgLength right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SvgLength left, SvgLength right)
        {
            return !(left == right);
        }

        public static implicit operator SvgLength(float f)
        {
            return new SvgLength(f);
        }
    }
}
