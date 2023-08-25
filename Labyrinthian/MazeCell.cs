using System;
using System.Linq;

namespace Labyrinthian
{
    /// <summary>
    /// Representation of a maze cell used by <see cref="Maze"/>. Cell can be either a part of the maze or an outer cell
    /// </summary>
    /// <remarks>
    /// <para>
    /// A part of the maze is a cell that is exists inside the maze. Between these cells can exist passages or walls.
    /// </para>
    /// <para>
    /// An outer cell is a cell that is used only for represantaion of entries/exits. 
    /// When the cell is outer then its <see cref="Index"/> is negative, <see cref="DirectedNeighbors"/> is null
    /// and it has only one neighbor. Direction to this neighbor can be calculated using this formula:
    /// <br />
    /// <code>-<see cref="Index"/> - 1</code>
    /// </para>
    /// </remarks>
    public sealed class MazeCell : IEquatable<MazeCell>
    {
        /// <summary>
        /// If positive, it's an index of the cell in <see cref="Maze.Cells"/>;
        /// if negative then this cell is outer.
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// All neighbors from <see cref="DirectedNeighbors"/> that are parts of the maze and not null
        /// </summary>
        public MazeCell[] Neighbors { get; private set; } = null!;
        /// <summary>
        /// An array, whose indices mean directions(e.g. for <see cref="OrthogonalMaze"/> it's 0-right, 1-left, 2-down, 3-up).
        /// <see langword="null"/> in the array means, that the direction doesn't exist for this cell.
        /// The array can contain outer cells.
        /// </summary>
        /// <remarks>
        /// This array is <see langword="null"/> when the cell is outer.
        /// </remarks>
        public MazeCell?[] DirectedNeighbors { get; private set; } = null!;

        /// <summary>
        /// <see langword="true"/> if this cell is a maze part;
        /// <see langword="false"/> if this cell is outer.
        /// </summary>
        public bool IsMazePart => Index >= 0;

        /// <summary>
        /// Create a maze cell.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use <see cref="SetNeighbors(MazeCell?[])"/> after to set its neighbors.
        /// </para>
        /// <para>
        /// For creating outer cells use <see cref="CreateOuterCell(MazeCell, int)"/>.
        /// </para>
        /// </remarks>
        /// <param name="index">index of the cell</param>
        public MazeCell(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Create an outer cell
        /// </summary>
        /// <param name="neighbor">single neighbor of the cell</param>
        /// <param name="direction">direction from this cell to the neighbor</param>
        public static MazeCell CreateOuterCell(MazeCell neighbor, int direction)
        {
            return new MazeCell(-direction - 1)
            {
                Neighbors = new MazeCell[1] { neighbor },
                DirectedNeighbors = null! // DirectedNeighbors shouldn't be used for cells that are not parts of maze
            };
        }

        /// <summary>
        /// Set directed neighbors to the cell.
        /// </summary>
        /// <exception cref="CellIsOuterException"/>
        public void SetNeighbors(params MazeCell?[] directedNeighbors)
        {
            if (!IsMazePart) throw new CellIsOuterException();

            DirectedNeighbors = directedNeighbors;

            Neighbors = (from neighbor in directedNeighbors
                         where neighbor != null && neighbor.Index >= 0
                         select neighbor).ToArray();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MazeCell);
        }

        /// <summary>
        /// Compare cells by reference
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true, if both cells are equal by reference</returns>
        public bool Equals(MazeCell? other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index);
        }

        public static bool operator ==(MazeCell? left, MazeCell? right)
        {
            if (left is null)
            {
                // Both cells are null
                if (right is null) return true;

                // Only left cell is null
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(MazeCell? left, MazeCell? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Check whether two cells are neighbors(order of the arguments doesn't mater).
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if two cells are neighbors;
        /// <see langword="false"/> if two cells are not neighbors
        /// </returns>
        public static bool AreNeighbors(MazeCell cell1, MazeCell cell2)
        {
            if (cell1.IsMazePart) return cell1.DirectedNeighbors.Contains(cell2);
            return cell1.Neighbors[0] == cell2;
        }
    }
}
