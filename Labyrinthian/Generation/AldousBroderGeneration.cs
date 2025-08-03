using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Aldous Broder algorithm.
    /// </summary>
    public sealed class AldousBroderGeneration : MazeGenerator
    {
        private readonly INeighborSelector _neighborSelector;

        /// <summary>
        /// Construct a generator with a default seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="neighborSelector">An optional neighbor selector.</param>
        /// <exception cref="MazeTypeIsNotSupportedException"></exception>
        public AldousBroderGeneration(Maze maze, MazeCell? initialCell = null, INeighborSelector? neighborSelector = null) :
            base(maze, initialCell)
        {
            _neighborSelector = neighborSelector ?? new UnweightedNeighborSelector();
        }

        /// <summary>
        /// Construct a generator with specified seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="seed">Seed for random numbers generator.</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="neighborSelector">An optional neighbor selector.</param>
        public AldousBroderGeneration(Maze maze, int seed, MazeCell? initialCell = null, INeighborSelector? neighborSelector = null) :
            base(maze, seed, initialCell)
        {
            _neighborSelector = neighborSelector ?? new UnweightedNeighborSelector();
        }

        protected override IEnumerable<Maze> Generation()
        {
            _neighborSelector.Init(Maze, Rnd);

            // Pick a random cell as the current cell and mark it as visited
            SelectedCell ??= GetRandomCell();
            VisitedCells[SelectedCell] = true;

            yield return Maze;

            int visitedCells = 1;
            // While there are unvisited cells
            while (visitedCells < Maze.Cells.Length)
            {
                // Pick a random neighbour
                MazeCell neighbor = _neighborSelector.Select(SelectedCell);

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