using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    public sealed class SidewinderGeneration : MazeGenerator
    {
        private readonly float m_horizontalCarveProbability = 0.5f;

        public SidewinderGeneration(Maze maze) : this(maze, Environment.TickCount) { }
        public SidewinderGeneration(Maze maze, int seed, float horizontalCarveProbability = 0.5f) : base(maze, seed)
        {
            m_horizontalCarveProbability = horizontalCarveProbability;
        }

        protected override bool IsSuitableFor(Maze maze) => IsMazeDefaultOrthogonal(maze);

        protected override IEnumerable<Maze> Generation()
        {
            List<MazeCell> cells = new List<MazeCell>();

            foreach (var cell in Maze.Cells)
            {
                SelectedCell = cell;
                VisitedCells[cell] = true;
                if (!cell.DirectedNeighbors[3].IsNotNullAndMazePart())
                {
                    if (cell.DirectedNeighbors[0].IsNotNullAndMazePart())
                    {
                        Maze.ConnectCells(cell, cell.DirectedNeighbors[0]!);
                    }
                    yield return Maze;
                    continue;
                }

                HighlightedCells[cell] = true;
                if (cells.Count > 0) Maze.ConnectCells(cell, cells[^1]);
                cells.Add(cell);

                if (!cell.DirectedNeighbors[0].IsNotNullAndMazePart() ||
                    (float)Rnd.NextDouble() < m_horizontalCarveProbability)
                {
                    MazeCell rndCell = cells[Rnd.Next(0, cells.Count)];
                    Maze.ConnectCells(rndCell, rndCell.DirectedNeighbors[3]!);
                    foreach (var activeCell in cells)
                    {
                        HighlightedCells[activeCell] = false;
                    }
                    cells.Clear();
                }

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