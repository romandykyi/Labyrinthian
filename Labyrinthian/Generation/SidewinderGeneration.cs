using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Sidewinder algorithm.
    /// Can be used only for <see cref="OrthogonalMaze"/>.
    /// </summary>
    public sealed class SidewinderGeneration : MazeGenerator
    {
        private readonly float m_horizontalCarveProbability = 0.5f;

        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public SidewinderGeneration(Maze maze) : this(maze, Environment.TickCount) { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public SidewinderGeneration(Maze maze, int seed, float horizontalCarveProbability = 0.5f) : base(maze, seed)
        {
            m_horizontalCarveProbability = horizontalCarveProbability;
        }

        protected override bool IsSuitableFor(Maze maze) => IsSuitableForSidewinder(maze);

        protected override IEnumerable<Maze> Generation()
        {
            GridMaze2D gridMaze2D = (GridMaze2D)Maze;

            int horizontal = OrthogonalMaze.East;
            int vertical = Maze is OrthogonalMaze ? OrthogonalMaze.North : ThetaMaze.North;
            List<MazeCell> cells = new List<MazeCell>();

            int column = 0;
            foreach (var cell in Maze.Cells)
            {
                SelectedCell = cell;
                VisitedCells[cell] = true;
                if (!cell.DirectedNeighbors[vertical].IsNotNullAndMazePart())
                {
                    if (cell.DirectedNeighbors[horizontal].IsNotNullAndMazePart())
                    {
                        Maze.ConnectCells(cell, cell.DirectedNeighbors[horizontal]!);
                    }
                    yield return Maze;
                    continue;
                }

                HighlightedCells[cell] = true;
                if (cells.Count > 0) Maze.ConnectCells(cell, cells[^1]);
                cells.Add(cell);

                column++;
                if (column == gridMaze2D.Columns ||
                    (float)Rnd.NextDouble() < m_horizontalCarveProbability)
                {
                    MazeCell rndCell = cells[Rnd.Next(0, cells.Count)];
                    Maze.ConnectCells(rndCell, rndCell.DirectedNeighbors[vertical]!);
                    foreach (var activeCell in cells)
                    {
                        HighlightedCells[activeCell] = false;
                    }
                    cells.Clear();
                }
                column %= gridMaze2D.Columns;

                yield return Maze;
            }
            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Sidewinder";
        }
    }
}