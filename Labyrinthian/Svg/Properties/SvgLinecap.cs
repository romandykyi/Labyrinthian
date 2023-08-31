using System;

namespace Labyrinthian.Svg
{
    public readonly struct SvgLinecap : IEquatable<SvgLinecap>
    {
        private enum LinecapEnum
        {
            Butt,
            Round,
            Square
        }

        private readonly LinecapEnum _value;

        private SvgLinecap(LinecapEnum value)
        {
            _value = value;
        }

        public static SvgLinecap Butt => new SvgLinecap(LinecapEnum.Butt);
        public static SvgLinecap Round => new SvgLinecap(LinecapEnum.Round);
        public static SvgLinecap Square => new SvgLinecap(LinecapEnum.Square);

        public override string? ToString()
        {
            return _value switch
            {
                LinecapEnum.Butt => "butt",
                LinecapEnum.Square => "square",
                LinecapEnum.Round => "round",
                _ => null
            };
        }

        public override bool Equals(object? obj)
        {
            return obj is SvgLinecap linecap && Equals(linecap);
        }

        public bool Equals(SvgLinecap other)
        {
            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        public static bool operator ==(SvgLinecap left, SvgLinecap right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SvgLinecap left, SvgLinecap right)
        {
            return !(left == right);
        }
    }
}
