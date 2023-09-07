using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze generator that uses Binary tree algorithm.
    /// Can be used only for <see cref="OrthogonalMaze"/> or <see cref="ThetaMaze"/>.
    /// </summary>
    public sealed class BinaryTreeGeneration : MazeGenerator
    {
        /// <inheritdoc cref="MazeGenerator(Maze, MazeCell?, bool)" />
        public BinaryTreeGeneration(Maze maze) : base(maze) { }
        /// <inheritdoc cref="MazeGenerator(Maze, int, MazeCell?, bool)" />
        public BinaryTreeGeneration(Maze maze, int seed) : base(maze, seed) { }

        protected override bool IsSuitableFor(Maze maze) => 
            maze is OrthogonalMaze || maze is ThetaMaze;

        protected override IEnumerable<Maze> Generation()
        {
            foreach (MazeCell cell in Maze.Cells)
            {
                List<MazeCell> mainNeighbors = new List<MazeCell>();

                SelectedCell = cell;
                VisitedCells[SelectedCell] = true;
                for (int i = 0; i < cell.DirectedNeighbors.Length; i += 2)
                {
                    MazeCell? neighbor = cell.DirectedNeighbors[i];
                    if (neighbor != null && neighbor.IsMazePart)
                        mainNeighbors.Add(neighbor);
                }

                if (mainNeighbors.Count > 0)
                {
                    int rndIndex = Rnd.Next(0, mainNeighbors.Count);
                    Maze.ConnectCells(SelectedCell, mainNeighbors[rndIndex]);
                }

                yield return Maze;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Binary Tree";
        }
    }
}