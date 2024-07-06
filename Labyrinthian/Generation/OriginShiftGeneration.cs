using System;
using System.Collections.Generic;

namespace Labyrinthian
{
	public class OriginShiftParams
	{
		/// <summary>
		/// Get/set the max algorithm's iterations number.
		/// If the value is negative, there is no iterations limit.
		/// </summary>
		public int MaxIterations { get; set; } = -1;

		/// <summary>
		/// Get/set a flag which determines whether a generation should be stopped when
		/// all cells were visited.
		/// </summary>
		public bool GenerateUntilAllCellsAreVisited { get; set; } = true;

		/// <summary>
		/// Get/set a neighbor selector.
		/// </summary>
		public INeighborSelector NeighborSelector { get; set; } = new HeatMapNeighborSelector();
	}

	/// <summary>
	/// Maze generator that uses Origin Shift algorithm.
	/// Algorithm's author: https://github.com/CaptainLuma.
	/// </summary>
	public class OriginShiftGeneration : MazeGenerator, IDirectedGraphGenerator
	{
		private readonly OriginShiftParams _params;

		public DirectedMaze DirectedMaze { get; private set; }

		/// <summary>
		/// Construct a generator with a default seed.
		/// </summary>
		/// <param name="maze">Maze used for generation.</param>
		/// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
		/// <param name="params">Optional origin shift parameters to use.</param>
		public OriginShiftGeneration(Maze maze, OriginShiftParams? @params = null, MazeCell? initialCell = null) :
			this(maze, Environment.TickCount, @params, initialCell)
		{ }

		/// <summary>
		/// Construct a generator with specified seed.
		/// </summary>
		/// <param name="maze">Maze used for generation.</param>
		/// <param name="seed">Seed for random numbers generator.</param>
		/// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
		/// <param name="params">Optional origin shift parameters to use.</param>
		public OriginShiftGeneration(Maze maze, int seed, OriginShiftParams? @params = null, MazeCell? initialCell = null) :
			base(maze, seed, initialCell, false)
		{
			DirectedMaze = new DirectedMaze(Maze);
			_params = @params ?? new OriginShiftParams();
		}

		private void ConnectToOrigin(MazeCell origin)
		{
			// Use BFS to connect all cells to the origin
			Queue<MazeCell> cells = new Queue<MazeCell>();
			cells.Enqueue(origin);
			var visited = new MarkedCells(Maze, false);
			visited[origin] = true;
			while (cells.Count > 0)
			{
				var currentCell = cells.Dequeue();
				foreach (var neighbor in currentCell.Neighbors)
				{
					if (!visited[neighbor])
					{
						visited[neighbor] = true;
						cells.Enqueue(neighbor);

						DirectedMaze.ConnectCells(neighbor, currentCell);
					}
				}
			}
		}

		protected override IEnumerable<Maze> Generation()
		{
			// Based on https://github.com/CaptainLuma/New-Maze-Generating-Algorithm

			int visitedCount = 1;
			_params.NeighborSelector.Init(Maze, Rnd);

			// Choose the initial cell and mark it as origin
			SelectedCell ??= GetRandomCell();
			VisitedCells[SelectedCell] = true;
			// Make a starting perfect maze
			ConnectToOrigin(SelectedCell);

			yield return Maze;

			for (int i = 0; 
				(_params.MaxIterations < 0 || i < _params.MaxIterations) &&
				(!_params.GenerateUntilAllCellsAreVisited || visitedCount < Maze.Cells.Length);
				i++)
			{
				// Have the origin node, point to a random neighboring node
				var selectedNeighbor = _params.NeighborSelector.Select(SelectedCell);
				DirectedMaze.ConnectCells(SelectedCell, selectedNeighbor);

				// That neigboring node becomes the new origin node
				SelectedCell = selectedNeighbor;

				if (!VisitedCells[SelectedCell])
				{
					visitedCount++;
					VisitedCells[SelectedCell] = true;
				}

				// Have the new origin node point nowhere
				foreach (var neighbor in SelectedCell.Neighbors)
				{
					DirectedMaze.DisconnectCells(SelectedCell, neighbor);
				}

				yield return Maze;
			}

			// Reset state
			SelectedCell = null;
			VisitedCells.SetAll(true);

			yield return Maze;
		}

		public override Maze Generate()
		{
			if (_params.MaxIterations < 0 && !_params.GenerateUntilAllCellsAreVisited)
			{
				throw new InvalidOperationException("Infinite loop detected. Please specify MaxIterations or set GenerateUntilAllCelslAreVisited to true.");
			}

			return base.Generate();
		}

		public override string ToString()
		{
			return "Origin Shift";
		}
	}
}
