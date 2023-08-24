using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    /// <summary>
    /// Генератор лабіринтів
    /// </summary>
    public abstract class MazeGenerator
    {
        private MazeCell? _selectedCell;

        public readonly Random Rnd;
        public readonly Maze Maze;

        public readonly MarkedCells HighlightedCells, VisitedCells;
        public MazeCell? SelectedCell
        {
            get => _selectedCell;
            set
            {
                MazeCell? lastSelected = _selectedCell;
                _selectedCell = value;

                if (lastSelected != null)
                    CellStateChanged?.Invoke(lastSelected);

                if (value != null)
                    CellStateChanged?.Invoke(value);
            }
        }

        public readonly List<IMazePostProcessor> PostProcessors;

        public event Action<MazeCell>? CellStateChanged;

        protected MazeGenerator(Maze maze, MazeCell? initialCell = null, bool defaultVisited = false) :
            this(maze, Environment.TickCount, initialCell, defaultVisited)
        { }

        protected MazeGenerator(Maze maze, int seed,
            MazeCell? initialCell = null, bool defaultVisited = false)
        {
            if (!IsSuitableFor(maze))
                throw new MazeTypeIsNotSupportedException(this, maze);

            Maze = maze;
            Rnd = new Random(seed);

            SelectedCell = initialCell;

            HighlightedCells = new MarkedCells(Maze);
            VisitedCells = new MarkedCells(Maze, defaultVisited);
            PostProcessors = new List<IMazePostProcessor>();

            VisitedCells.CellChanged += (cell, _) => CellStateChanged?.Invoke(cell);
            HighlightedCells.CellChanged += (cell, _) => CellStateChanged?.Invoke(cell);

            maze.Description += $" generated with {ToString()} algorithm(seed: {seed})";
        }

        protected MazeCell GetRandomCell()
        {
            int rndIndex = Rnd.Next(0, Maze.Cells.Length);
            return Maze.Cells[rndIndex];
        }

        public virtual bool IsSuitableFor(Maze maze) => true;

        protected abstract IEnumerable<Maze> Generation();

        public IEnumerable<Maze> GenerateStepByStep()
        {
            foreach (var maze in Generation()) yield return maze;
            foreach (var processor in PostProcessors)
            {
                foreach (var maze in processor.Process(this))
                    yield return maze;
            }
        }

        public virtual Maze Generate() => Generation().Last();

        public static bool IsMazeDefaultOrthogonal(Maze maze)
        {
            if (!(maze is OrthogonalMaze orthogonalMaze)) return false;

            return orthogonalMaze.Cells.Length == orthogonalMaze.Columns * orthogonalMaze.Rows;
        }
    }
}