using System.Collections.Generic;

namespace Labyrinthian
{
    public sealed class WilsonGeneration : MazeGenerator
    {
        public WilsonGeneration(Maze maze, MazeCell? initialCell = null) :
            base(maze, initialCell)
        { }
        public WilsonGeneration(Maze maze, int seed, MazeCell? initialCell = null) :
            base(maze, seed, initialCell)
        { }

        private void EraseWalk(List<MazeCell> walk, int start = 0)
        {
            for (int i = walk.Count - 1; i > start; --i)
            {
                HighlightedCells[walk[i]] = false;
                walk.RemoveAt(i);
            }
        }

        protected override IEnumerable<Maze> Generation()
        {
            StaticUnorderedList<MazeCell> unvisitedCells = new StaticUnorderedList<MazeCell>(Maze.Cells);

            // Select initial cell
            SelectedCell ??= GetRandomCell();
            // Mark it as visited
            VisitedCells[SelectedCell] = true;
            unvisitedCells.RemoveAt(SelectedCell.Index);

            while (unvisitedCells.Count > 0)
            {
                // New cell chosen arbitrarily
                int rndIndex = Rnd.Next(0, unvisitedCells.Count);
                SelectedCell = unvisitedCells.Pop(rndIndex);

                if (!VisitedCells[SelectedCell]) yield return Maze;
                else continue;

                MazeCell? previousCell = null;
                List<MazeCell> walk = new List<MazeCell>();

                while (!VisitedCells[SelectedCell])
                {
                    // Erase loop
                    if (HighlightedCells[SelectedCell])
                    {
                        int loopStart = walk.IndexOf(SelectedCell);
                        EraseWalk(walk, loopStart);
                    }
                    else
                    {
                        walk.Add(SelectedCell);
                        HighlightedCells[SelectedCell] = true;
                    }

                    var neighbors =
                        SelectedCell.FindNeighbors(neighbor => neighbor != previousCell);

                    if (neighbors.Count != 0)
                    {
                        int rndNeighborIndex = Rnd.Next(0, neighbors.Count);

                        previousCell = SelectedCell;
                        SelectedCell = neighbors[rndNeighborIndex];
                    }
                    else
                    {
                        EraseWalk(walk, 1);
                        SelectedCell = walk[0];
                    }

                    yield return Maze;
                }
                walk.Add(SelectedCell);
                for (int i = 0; i < walk.Count - 1; ++i)
                {
                    MazeCell cell = walk[i];

                    Maze.ConnectCells(cell, walk[i + 1]);
                    VisitedCells[cell] = true;
                    HighlightedCells[cell] = false;
                }
                VisitedCells[walk[^1]] = true;
                HighlightedCells[walk[^1]] = false;
            }

            SelectedCell = null;
            yield return Maze;
        }

        public override string ToString()
        {
            return "Wilson's";
        }
    }
}