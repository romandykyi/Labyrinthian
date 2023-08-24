using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    public class MazeBraider : IMazePostProcessor
    {
        public readonly float braidness;

        public MazeBraider(float braidness)
        {
            this.braidness = braidness;
        }

        public IEnumerable<Maze> Process(MazeGenerator caller)
        {
            if (braidness < 0f || braidness > 1f)
                throw new ArgumentOutOfRangeException(nameof(braidness));

            List<MazeCell> deadEnds = new List<MazeCell>();
            List<MazeCell> completeDeadEnds = new List<MazeCell>();
            foreach (MazeCell deadEnd in caller.Maze.FindDeadEnds())
            {
                if (deadEnd.Neighbors.Length == 1)
                {
                    caller.VisitedCells[deadEnd] = false;
                    completeDeadEnds.Add(deadEnd);
                }
                else
                {
                    caller.HighlightedCells[deadEnd] = true;
                    deadEnds.Add(deadEnd);
                }
            }
            yield return caller.Maze;

            int deadEndsToRemove = (int)Math.Round(braidness * deadEnds.Count);
            for (int i = 0; i < deadEndsToRemove; ++i)
            {
                int rndIndex = caller.Rnd.Next(0, deadEnds.Count - i);

                caller.SelectedCell = deadEnds[rndIndex];
                caller.HighlightedCells[caller.SelectedCell] = false;

                yield return caller.Maze;

                var neighbors = caller.SelectedCell.FindNeighbors(
                    neighbor => !caller.Maze.AreCellsConnected(neighbor, caller.SelectedCell));
                if (neighbors.Count > 0)
                {
                    MazeCell rndNeighbor = neighbors[caller.Rnd.Next(0, neighbors.Count)];
                    caller.Maze.ConnectCells(caller.SelectedCell, rndNeighbor);
                }

                (deadEnds[rndIndex], deadEnds[^(i + 1)]) =
                    (deadEnds[^(i + 1)], deadEnds[rndIndex]);

                yield return caller.Maze;
            }

            for (int i = 0; i < deadEnds.Count - deadEndsToRemove; ++i)
            {
                caller.HighlightedCells[deadEnds[i]] = false;
            }
            foreach (MazeCell completeDeadEnd in completeDeadEnds)
            {
                caller.VisitedCells[completeDeadEnd] = true;
            }
            caller.SelectedCell = null;
            yield return caller.Maze;
        }
    }
}
