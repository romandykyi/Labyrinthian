using System;

namespace Labyrinthian
{
    /// <summary>
    /// Struct to represent a point that each cell of <see cref="MultiGridMaze2D"/> has.
    /// </summary>
    public readonly struct MultiGridPoint2D : IEquatable<MultiGridPoint2D>
    {
        public readonly int Row;
        public readonly int Column;
        public readonly int Grid;

        public MultiGridPoint2D(int grid, int row, int column)
        {
            Row = row;
            Column = column;
            Grid = grid;
        }

        public override bool Equals(object obj)
        {
            return obj is MultiGridPoint2D point && Equals(point);
        }

        public bool Equals(MultiGridPoint2D point)
        {
            return Row == point.Row && Column == point.Column && Grid == point.Grid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column, Grid);
        }

        public override string ToString()
        {
            return $"(row={Row}; col={Column}; grid={Grid})";
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
