using System;

namespace Labyrinthian
{
    /// <summary>
    /// Struct to represent a point that each cell of <see cref="GridMaze2D"/> has.
    /// </summary>
    public readonly struct GridPoint2D : IEquatable<GridPoint2D>
    {
        public readonly int Row;
        public readonly int Column;

        public GridPoint2D(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPoint2D point && Equals(point);
        }

        public bool Equals(GridPoint2D point)
        {
            return Row == point.Row && Column == point.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public override string ToString()
        {
            return $"(row={Row}; col={Column})";
        }

        public static bool operator ==(GridPoint2D left, GridPoint2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridPoint2D left, GridPoint2D right)
        {
            return !(left == right);
        }
    }
}
