using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Hunt and Kill algorithm.
    /// </summary>
    public sealed class HuntAndKillGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public HuntAndKillGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell)
        { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public HuntAndKillGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell)
        { }

        // Implementation may be incorrect, but it works
        protected override IEnumerable<Maze> Generation()
        {
            int currentIndex = 0, visitedCells = 1;
            // Choose the initial cell
            SelectedCell ??= GetRandomCell();
            // Mark it as visited
            VisitedCells[SelectedCell] = true;

            yield return Maze;

            while (visitedCells < Maze.Cells.Length)
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
                    VisitedCells[neighbor] = true;
                    visitedCells++;
                    // Make the chosen neighbour the current cell
                    SelectedCell = neighbor;
                }
                // Scan grid for an unvisited cell that is adjacent to a visited cell
                else
                {
                    // Hunt mode
                    while (true)
                    {
                        SelectedCell = Maze.Cells[currentIndex++];
                        currentIndex %= Maze.Cells.Length;
                        if (!VisitedCells[SelectedCell] &&
                            SelectedCell.TryFindNeighbor(c => VisitedCells[c], out MazeCell neighbor))
                        {
                            VisitedCells[SelectedCell] = true;
                            visitedCells++;
                            Maze.ConnectCells(SelectedCell, neighbor);
                            break;
                        }

                        yield return Maze;
                    }
                }

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Hunt and Kill";
        }
    }
}