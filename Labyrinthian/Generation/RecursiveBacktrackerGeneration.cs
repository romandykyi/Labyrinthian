using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    public sealed class RecursiveBacktrackerGeneration : MazeGeneratorWithCustomSelection
    {
        public RecursiveBacktrackerGeneration(Maze maze, MazeCellSelection? selection = null, MazeCell? initialCell = null) :
            base(maze, Environment.TickCount, selection, initialCell)
        { }
        public RecursiveBacktrackerGeneration(Maze maze, int seed, MazeCellSelection? selection = null,
            MazeCell? initialCell = null) : base(maze, seed, selection, initialCell) { }

        protected override IEnumerable<Maze> Generation()
        {
            List<MazeCell> cells = new List<MazeCell>();

            // Choose the initial cell
            SelectedCell ??= GetRandomCell();
            // Mark it as visited
            VisitedCells[SelectedCell] = true;
            // Add to list
            cells.Add(SelectedCell);

            yield return Maze;

            while (cells.Count > 0)
            {
                // Check if the current cell has any neighbours which have not been visited
                List<MazeCell> neighbors =
                    SelectedCell.FindNeighbors(neighbor => !VisitedCells[neighbor]);
                if (neighbors.Count > 0)
                {
                    // Choose one of the unvisited neighbours
                    int neighborIndex = Rnd.Next(0, neighbors.Count);
                    MazeCell neighbor = neighbors[neighborIndex];

                    // Remove the wall between the current cell and the chosen cell
                    Maze.ConnectCells(SelectedCell, neighbor);

                    // Mark the chosen cell as visited
                    VisitedCells[neighbor] = HighlightedCells[neighbor] = true;
                    // Add chosen cell to list
                    cells.Add(neighbor);
                    // Make the chosen neighbour the current cell
                    SelectedCell = neighbor;
                }
                // Look for an unvisited cell that is adjacent to a visited cell
                else
                {
                    int index = Selection(Rnd, cells.Count);

                    SelectedCell = cells[index];
                    cells.RemoveAt(index);

                    HighlightedCells[SelectedCell] = false;
                }

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Recursive backtracker";
        }
    }
}