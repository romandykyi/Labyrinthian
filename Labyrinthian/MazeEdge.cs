using System;

namespace Labyrinthian
{
    /// <summary>
    /// Struct that represents an edge of passages graph of a maze.
    /// </summary>
    public readonly struct MazeEdge : IEquatable<MazeEdge>
    {
        public readonly MazeCell Cell1;
        public readonly MazeCell Cell2;

        /// <summary>
        /// Direction from the first cell to the second cell
        /// </summary>
        public int Direction => Cell1.IsMazePart ?
            Array.IndexOf(Cell1.DirectedNeighbors, Cell2) : -Cell1.Index - 1;

        /// <summary>
        /// Inverted edge
        /// </summary>
        public MazeEdge Inverted => new MazeEdge(Cell2, Cell1);

        public MazeEdge(MazeCell cell0, MazeCell cell1) : this()
        {
            Cell1 = cell0;
            Cell2 = cell1;
        }

        /// <summary>
        /// Create an edge where Cell0.index &lt; Cell1.index
        /// </summary>
        public static MazeEdge GetMinMax(MazeCell cell0, MazeCell cell1)
        {
            if (cell0.Index < cell1.Index)
            {
                return new MazeEdge(cell0, cell1);
            }
            else
            {
                return new MazeEdge(cell1, cell0);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is MazeEdge relation && Equals(relation);
        }

        /// <summary>
        /// Two edges are equal if their nodes are equal and have the same order
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if both edges have equal nodes that have the same order;
        /// <see langword="false" /> otherwise
        /// </returns>
        public bool Equals(MazeEdge other)
        {
            return Cell1 == other.Cell1 && Cell2 == other.Cell2;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cell1, Cell2);
        }

        public static bool operator ==(MazeEdge left, MazeEdge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MazeEdge left, MazeEdge right)
        {
            return !(left == right);
        }
    }
}