using System;

namespace Labyrinthian
{
    /// <summary>
    /// Struct which represents a value in a heat map.
    /// </summary>
    public readonly struct HeatMapValue
    {
        /// <summary>
        /// Heat map temperature (value), should not be negative.
        /// </summary>
        public readonly double Temperature;
        /// <summary>
        /// Number of visits to the cell.
        /// </summary>
        public readonly int VisitsCount;

        public HeatMapValue(double temperature, int visitsCount)
        {
            Temperature = temperature;
            VisitsCount = visitsCount;
        }
    }

    /// <summary>
    /// A weighted random neighbor selector which uses heat maps for cells.
    /// </summary>
    public class HeatMapNeighborSelector : INeighborSelector
    {
        private bool _initialized = false;

        private Random _rnd = null!;
        /// <summary>
        /// A heatmap for each maze cell, negative numbers may cause undefined behaviour.
        /// </summary>
        private HeatMapValue[] _heatmap = Array.Empty<HeatMapValue>();

        private readonly Func<HeatMapValue, double> _decayFunction;

        public HeatMapNeighborSelector() : this(HeatMapDecayFunctions.Multiplicative())
        {
        }

        public HeatMapNeighborSelector(Func<HeatMapValue, double> decayFunction)
        {
            _decayFunction = decayFunction;
        }

        public virtual void Init(Maze maze, Random rnd)
        {
            _initialized = true;
            _rnd = rnd;
            _heatmap = new HeatMapValue[maze.Cells.Length];

            for (int i = 0; i < _heatmap.Length; i++)
            {
                _heatmap[i] = new HeatMapValue(1.0, 0);
            }
        }

        public MazeCell Select(MazeCell cell)
        {
            if (!_initialized)
                throw new InvalidOperationException("Neighbor selector was not initialized.");

            // Apply a decay function
            var previousValue = _heatmap[cell.Index];
            _heatmap[cell.Index] = new HeatMapValue(
                temperature: _decayFunction(previousValue),
                visitsCount: previousValue.VisitsCount + 1
                );

            double[] neighborsHeatmap = new double[cell.Neighbors.Length];
            double heatmapSum = 0.0;
            for (int i = 0; i < neighborsHeatmap.Length; i++)
            {
                double heatValue = _heatmap[cell.Neighbors[i].Index].Temperature;
                neighborsHeatmap[i] = heatValue;
                heatmapSum += heatValue;
            }

            double rndNumber = (double)(1.0 - _rnd.NextDouble()); // From 0 (exclusive) to 1 (inclusive)

            // Cumulative Distribution Function
            double cdf = 0.0;
            for (int i = 0; i < neighborsHeatmap.Length; i++)
            {
                cdf += neighborsHeatmap[i] / heatmapSum;
                if (rndNumber <= cdf)
                {
                    return cell.Neighbors[i];
                }
            }

            // Return random neighbor if failed
            int rndIndex = _rnd.Next(cell.Neighbors.Length);
            return cell.Neighbors[rndIndex];
        }
    }
}
