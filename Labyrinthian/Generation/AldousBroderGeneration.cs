using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Aldous Broder algorithm.
    /// </summary>
    public sealed class AldousBroderGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public AldousBroderGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell)
        { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public AldousBroderGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell)
        { }

        protected override IEnumerable<Maze> Generation()
        {
            // Pick a random cell as the current cell and mark it as visited
            SelectedCell ??= GetRandomCell();
            VisitedCells[SelectedCell] = true;

            yield return Maze;

            int visitedCells = 1;
            // While there are unvisited cells
            while (visitedCells < Maze.Cells.Length)
            {
                // Pick a random neighbour
                int rndIndex = Rnd.Next(0, SelectedCell.Neighbors.Length);
                MazeCell neighbor = SelectedCell.Neighbors[rndIndex];

                // If the chosen neighbour has not been visited
                if (!VisitedCells[neighbor])
                {
                    // Remove the wall between the current cell and the chosen neighbour
                    Maze.ConnectCells(SelectedCell, neighbor);
                    // Mark the chosen neighbour as visited
                    VisitedCells[neighbor] = true;

                    visitedCells++;
                }

                // Make the chosen neighbour the current cell
                SelectedCell = neighbor;

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Aldous Broder";
        }
    }
}