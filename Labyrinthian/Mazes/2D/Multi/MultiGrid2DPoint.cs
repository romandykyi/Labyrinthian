using System;

namespace Labyrinthian
{
    public readonly struct MultiGridPoint2D : IEquatable<MultiGridPoint2D>
    {
        public readonly int Row;
        public readonly int Column;
        public readonly int Layer;

        public MultiGridPoint2D(int layer, int row, int column)
        {
            Row = row;
            Column = column;
            Layer = layer;
        }

        public override bool Equals(object obj)
        {
            return obj is MultiGridPoint2D point && Equals(point);
        }

        public bool Equals(MultiGridPoint2D point)
        {
            return Row == point.Row && Column == point.Column && Layer == point.Layer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column, Layer);
        }

        public override string ToString()
        {
            return $"(row={Row}; col={Column}; layer={Layer})";
        }

        public static bool operator ==(MultiGridPoint2D left, MultiGridPoint2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MultiGridPoint2D left, MultiGridPoint2D right)
        {
            return !(left == right);
        }
    }
}
