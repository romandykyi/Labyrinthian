using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses randomized Depth-first search algorithm.
    /// </summary>
    public sealed class DFSGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public DFSGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell) { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public DFSGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell) { }

        protected override IEnumerable<Maze> Generation()
        {
            Stack<MazeCell> dfsStack = new Stack<MazeCell>(1);

            // Choose the initial cell
            MazeCell startCell = SelectedCell ?? GetRandomCell();
            // Mark it as visited
            VisitedCells[startCell] = true;
            // Push it to the stack
            dfsStack.Push(startCell);

            // While the stack != empty
            while (dfsStack.Count > 0)
            {
                // Pop a cell from the stack and make it a current cell
                SelectedCell = dfsStack.Pop();
                yield return Maze;

                // If the current cell has any neighbours which have not been visited
                List<MazeCell> neighbors =
                    SelectedCell.FindNeighbors(neighbor => !VisitedCells[neighbor]);
                if (neighbors.Count > 0)
                {
                    // Push the current cell to the stack
                    dfsStack.Push(SelectedCell);

                    // Choose one of the unvisited neighbours
                    int neighborIndex = Rnd.Next(0, neighbors.Count);
                    MazeCell neighbor = neighbors[neighborIndex];

                    // Remove the wall between the current cell and the chosen cell
                    Maze.ConnectCells(SelectedCell, neighbor);

                    // Mark the chosen cell as visited and push it to the stack
                    VisitedCells[neighbor] = HighlightedCells[neighbor] = true;
                    dfsStack.Push(neighbor);
                }
                else
                {
                    HighlightedCells[SelectedCell] = false;
                }
            }
            SelectedCell = null;
            HighlightedCells.SetAll(false);

            yield return Maze;
        }

        public override string ToString()
        {
            return "Randomized Depth-first search(DFS)";
        }
    }
}