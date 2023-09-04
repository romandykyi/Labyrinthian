using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses randomized Prim's algorithm.
    /// </summary>
    public sealed class PrimGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public PrimGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell)
        { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public PrimGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell)
        { }

        protected override IEnumerable<Maze> Generation()
        {
            yield return Maze;
            /*
             * each cell is one of three types: 
             * (1) "In": The cell is part of the Maze and has been carved into already, 
             * (2) "Frontier": The cell != part of the Maze and has not been carved into yet,
             * but is next to a cell that's already "in"
             * (3) "Out": The cell that != visited
            */
            List<MazeCell> frontierCells = new List<MazeCell>();

            // Pick a cell, mark it as "In"
            SelectedCell ??= GetRandomCell();
            VisitedCells[SelectedCell] = true;

            // Mark all neighbors of chosen cell as "frontier"
            foreach (MazeCell cell in SelectedCell.Neighbors)
            {
                HighlightedCells[cell] = true;
                frontierCells.Add(cell);
            }

            yield return Maze;

            // While there are "frontier" cells left 
            while (frontierCells.Count > 0)
            {
                // Pick a "frontier" cell at random
                int rndIndex = Rnd.Next(0, frontierCells.Count);
                SelectedCell = frontierCells[rndIndex];
                frontierCells.RemoveAt(rndIndex);

                // Change the "frontier" cell to "in"
                HighlightedCells[SelectedCell] = false;
                VisitedCells[SelectedCell] = true;

                List<MazeCell> inNeighbors = new List<MazeCell>();

                // Update any of its neighbors that are "out" to "frontier"
                foreach (MazeCell cell in SelectedCell.Neighbors)
                {
                    if (VisitedCells[cell])
                    {
                        inNeighbors.Add(cell);
                    }
                    else if (!HighlightedCells[cell])
                    {
                        frontierCells.Add(cell);
                        HighlightedCells[cell] = true;
                    }
                }

                // Carve into it from one of its neighbor cells that are "in"
                int rndNeighbor = Rnd.Next(0, inNeighbors.Count);
                Maze.ConnectCells(inNeighbors[rndNeighbor], SelectedCell);

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Prim's";
        }
    }
}