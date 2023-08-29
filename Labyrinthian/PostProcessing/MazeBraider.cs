using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze post processor that removes dead ends.
    /// </summary>
    public class MazeBraider : IMazePostProcessor
    {
        public readonly float braidness;

        /// <summary>
        /// Create a maze braider.
        /// </summary>
        /// <param name="braidness">
        /// Amount of dead ends to be removed(from 0 to 1).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public MazeBraider(float braidness)
        {
            if (braidness < 0f || braidness > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(braidness));
            }
            this.braidness = braidness;
        }

        public IEnumerable<Maze> Process(MazeGenerator caller)
        {
            if (braidness < 0f || braidness > 1f)
                throw new ArgumentOutOfRangeException(nameof(braidness));

            var deadEnds = new List<MazeCell>();
            // Dead ends that cannot be removed, because cell has only one neighbor.
            var completeDeadEnds = new List<MazeCell>();
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

            // Number of dead ends that will be removed.
            int deadEndsToRemove = (int)Math.Round(braidness * deadEnds.Count);
            // Removal algorithm is based on Fisher–Yates shuffle.
            for (int i = 0; i < deadEndsToRemove; ++i)
            {
                int rndIndex = caller.Rnd.Next(0, deadEnds.Count - i);

                caller.SelectedCell = deadEnds[rndIndex];
                caller.HighlightedCells[caller.SelectedCell] = false;

                yield return caller.Maze;

                // Unconnected neighbors of the dead end
                var neighbors = caller.SelectedCell.FindNeighbors(
                    neighbor => !caller.Maze.AreCellsConnected(neighbor, caller.SelectedCell));
                if (neighbors.Count > 0)
                {
                    // Choose random neighbor
                    MazeCell rndNeighbor = neighbors[caller.Rnd.Next(0, neighbors.Count)];
                    caller.Maze.ConnectCells(caller.SelectedCell, rndNeighbor);
                }

                // Place removed dead end to the end
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
