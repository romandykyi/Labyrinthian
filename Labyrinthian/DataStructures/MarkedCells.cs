using System;
using System.Linq;

namespace Labyrinthian
{
    /// <summary>
    /// Data structure that represents a Boolean array and accepts MazeCell as index.
    /// Can be used in DFS/BFS algorithms to store the visited/unvisited state of each maze cell.
    /// </summary>
    public sealed class MarkedCells
    {
        private readonly Maze _maze;
        private readonly bool[] _marks;

        /// <summary>
        /// Event that is raised when the state of a cell changes.
        /// </summary>
        public event Action<MazeCell, bool>? CellChanged;

        /// <summary>
        /// Create a MarkedCells data structure.
        /// </summary>
        /// <param name="maze">Maze, which cells will be stored</param>
        /// <param name="value">Initial value of each cell</param>
        public MarkedCells(Maze maze, bool value = false)
        {
            _maze = maze;
            _marks = Enumerable.Repeat(value, maze.Cells.Length).ToArray();
        }

        /// <summary>
        /// Get the state of a cell. This method does not handle exceptions
        /// when the cell is not part of the maze or is from another maze.
        /// </summary>
        /// <param name="cell">The cell whose state will be returned.</param>
        /// <returns>true/false state of cell</returns>
        public bool this[MazeCell cell]
        {
            get => _marks[cell.Index];
            set
            {
                if (_marks[cell.Index] != value)
                {
                    _marks[cell.Index] = value;
                    CellChanged?.Invoke(cell, value);
                }
            }
        }

        /// <summary>
        /// Set the state of each cell to the specified value.
        /// </summary>
        public void SetAll(bool value)
        {
            foreach (MazeCell cell in _maze.Cells)
                this[cell] = value;
        }
    }
}
