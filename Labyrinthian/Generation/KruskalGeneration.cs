using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public sealed class KruskalGeneration : MazeGenerator
    {
        public KruskalGeneration(Maze maze) : base(maze, defaultVisited: true) { }
        public KruskalGeneration(Maze maze, int seed) :
            base(maze, seed, defaultVisited: true)
        { }

        protected override IEnumerable<Maze> Generation()
        {
            yield return Maze;

            MazeEdge[] walls = Maze.GetWalls(false).ToArray();
            // Random order of walls
            for (int i = 0; i < walls.Length - 1; ++i)
            {
                int rndIndex = Rnd.Next(i, walls.Length);
                (walls[i], walls[rndIndex]) = (walls[rndIndex], walls[i]);
            }
            DisjointSet<MazeCell> set = new DisjointSet<MazeCell>(Maze.Cells);

            foreach (var wall in walls)
            {
                MazeCell cell0 = wall.Cell1,
                    cell1 = wall.Cell2;
                if (set.Find(cell0) != set.Find(cell1))
                {
                    // Remove the current wall
                    Maze.ConnectCells(cell0, cell1);
                    // Join the sets of the formerly divided cells
                    set.Union(cell0, cell1);

                    yield return Maze;
                }
            }
        }

        public override string ToString()
        {
            return "Kruskal's";
        }
    }
}