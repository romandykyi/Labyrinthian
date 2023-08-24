using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Labyrinthian
{
    public sealed class MarkedCells
    {
        private readonly Maze _maze;
        private readonly bool[] _marks;

        public event Action<MazeCell, bool>? CellChanged;

        public MarkedCells(Maze maze, bool value = false)
        {
            _maze = maze;
            _marks = Enumerable.Repeat(value, maze.Cells.Length).ToArray();
        }

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

        public void SetAll(bool value)
        {
            foreach (MazeCell cell in _maze.Cells)
                this[cell] = value;
        }
    }
}