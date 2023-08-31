using System;

namespace Labyrinthian.Svg
{
    public readonly struct SvgGradientUnits : IEquatable<SvgGradientUnits>
    {
        private readonly bool _userSpaceOnUse;

        public static SvgGradientUnits UserSpaceOnUse => new SvgGradientUnits(true);
        public static SvgGradientUnits ObjectBoundingBox => new SvgGradientUnits(false);

        private SvgGradientUnits(bool userSpaceOnUse)
        {
            _userSpaceOnUse = userSpaceOnUse;
        }

        public override string ToString()
        {
            return _userSpaceOnUse ? "userSpaceOnUse" : "objectBoundingBox";
        }

        public override bool Equals(object? obj)
        {
            return obj is SvgGradientUnits units && Equals(units);
        }

        public bool Equals(SvgGradientUnits other)
        {
            return _userSpaceOnUse == other._userSpaceOnUse;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userSpaceOnUse);
        }

        public static bool operator ==(SvgGradientUnits left, SvgGradientUnits right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SvgGradientUnits left, SvgGradientUnits right)
        {
            return !(left == right);
        }
    }
}
