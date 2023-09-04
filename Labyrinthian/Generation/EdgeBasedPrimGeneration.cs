using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Edge-based Prim's algorithm.
    /// </summary>
    public sealed class EdgeBasedPrimGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public EdgeBasedPrimGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell)
        { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public EdgeBasedPrimGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell)
        { }

        protected override IEnumerable<Maze> Generation()
        {
            yield return Maze;

            List<MazeEdge> walls = new List<MazeEdge>();

            // Pick a cell, mark it as part of the maze
            SelectedCell ??= GetRandomCell();
            VisitedCells[SelectedCell] = true;

            // Add the walls of the cell to the wall list
            walls.AddRange(Maze.GetWallsOfCell(SelectedCell));

            yield return Maze;

            // While there are walls in the list
            while (walls.Count > 0)
            {
                // Pick a random wall from the list.
                int currentWallIndex = Rnd.Next(0, walls.Count);
                MazeEdge currentWall = walls[currentWallIndex];
                MazeCell cell0 = currentWall.Cell1, cell1 = currentWall.Cell2;

                // If only one of the cells that the wall divides is visited
                if (!VisitedCells[cell1])
                {
                    // Make the wall a passage
                    Maze.ConnectCells(cell0, cell1);

                    // Mark the unvisited cell as part of the maze
                    VisitedCells[cell1] = true;
                    SelectedCell = cell1;

                    // Add the neighboring walls of the cell to the wall list
                    walls.AddRange(Maze.GetWallsOfCell(cell1));

                    yield return Maze;
                }

                // Remove the wall from the list
                walls.RemoveAt(currentWallIndex);
            }
            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Edge-based Prim's";
        }
    }
}