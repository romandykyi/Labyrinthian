using System;

namespace Labyrinthian
{
    /// <summary>
    /// An unweighted random neighbor selector for random walk 
    /// algorithms (Aldous Broder and Origin Shift).
    /// </summary>
    public class UnweightedNeighborSelector : INeighborSelector
    {
        private Random? _rnd;

        public void Init(Maze maze, Random rnd)
        {
            _rnd = rnd;
        }

        public MazeCell Select(MazeCell cell)
        {
            if (_rnd == null)
                throw new InvalidOperationException("Neighbor selector was not initialized.");

            int rndIndex = _rnd.Next(cell.Neighbors.Length);
            return cell.Neighbors[rndIndex];
        }
    }
}
