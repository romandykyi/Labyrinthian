using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    public sealed class GrowingTreeGeneration : MazeGeneratorWithCustomSelection
    {
        public GrowingTreeGeneration(Maze maze, MazeCellSelection? selection = null, MazeCell? initialCell = null) :
            base(maze, Environment.TickCount, selection, initialCell) { }
        public GrowingTreeGeneration(Maze maze, int seed, MazeCellSelection? selection = null,
            MazeCell? initialCell = null) : base(maze, seed, selection, initialCell) { }

        protected override IEnumerable<Maze> Generation()
        {
            yield return Maze;

            List<MazeCell> cells = new List<MazeCell>();

            // Choose the initial cell
            SelectedCell ??= GetRandomCell();
            // Mark it as visited
            HighlightedCells[SelectedCell] = VisitedCells[SelectedCell] = true;
            // Add to the list
            cells.Add(SelectedCell);

            while (cells.Count > 0)
            {
                int cellIndex = Selection(Rnd, cells.Count);
                SelectedCell = cells[cellIndex];

                // Find unvisited neighbors of current cell
                List<MazeCell> unvisitedNeighbors = 
                    SelectedCell.FindNeighbors(neighbor => !VisitedCells[neighbor]);

                // If there are no unvisited neighbors, 
                if (unvisitedNeighbors.Count == 0)
                {
                    // Remove the cell from the list
                    cells.RemoveAt(cellIndex);
                    HighlightedCells[SelectedCell] = false;
                }
                else
                {
                    // Choose a random neighbor
                    int rndNeighborIndex = Rnd.Next(0, unvisitedNeighbors.Count);
                    MazeCell neighbor = unvisitedNeighbors[rndNeighborIndex];
                    // Carve a passage to the neighbor
                    Maze.ConnectCells(SelectedCell, neighbor);
                    // Add the neighbor to the list
                    cells.Add(neighbor);
                    // Mark the neighbor as visited
                    VisitedCells[neighbor] = HighlightedCells[neighbor] = true;
                }

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Growing Tree";
        }
    }
}