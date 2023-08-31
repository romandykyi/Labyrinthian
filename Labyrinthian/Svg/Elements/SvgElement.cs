using System;
using System.Collections.Generic;

namespace Labyrinthian.Svg
{
    public abstract class SvgElement : IEquatable<SvgElement?>
    {
        [SvgProperty("id")]
        public string? Id { get; set; }

        [SvgProperty("style")]
        public string? Style { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SvgElement);
        }

        public bool Equals(SvgElement? other)
        {
            return !(other is null) &&
                   Id != null && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(SvgElement? left, SvgElement? right)
        {
            return EqualityComparer<SvgElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(SvgElement? left, SvgElement? right)
        {
            return !(left == right);
        }
    }
}
