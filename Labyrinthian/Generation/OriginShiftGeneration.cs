using System;
using System.Collections.Generic;

namespace Labyrinthian
{
	/// <summary>
	/// Maze generator that uses Origin Shift algorithm.
	/// Algorithm's author: https://github.com/CaptainLuma.
	/// </summary>
	public class OriginShiftGeneration : MazeGenerator, IDirectedGraphGenerator
	{
		/// <summary>
		/// Gets the max algorithm's iterations number.
		/// If the value is negative, generator generates a maze forever.
		/// </summary>
		public int MaxIterations { get; private set; }

		public DirectedMaze DirectedMaze { get; private set; }

		/// <summary>
		/// Construct a generator with a default seed.
		/// </summary>
		/// <param name="maze">Maze used for generation.</param>
		/// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
		/// <param name="maxIterations">
		/// Max algorithm's iterations number. If negative, maze doesn't stop generating,
		/// if <see langword="null"/> then a default number of iterations is used based on the number of maze cells.
		/// </param>
		public OriginShiftGeneration(Maze maze, int? maxIterations = null, MazeCell? initialCell = null) :
			this(maze, Environment.TickCount, maxIterations, initialCell)
		{ }

		/// <summary>
		/// Construct a generator with specified seed.
		/// </summary>
		/// <param name="maze">Maze used for generation.</param>
		/// <param name="seed">Seed for random numbers generator.</param>
		/// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
		/// <param name="maxIterations">
		/// Max algorithm's iterations number. If negative, maze doesn't stop generating,
		/// if <see langword="null"/> then a default number of iterations is used based on the number of maze cells.
		/// </param>
		public OriginShiftGeneration(Maze maze, int seed, int? maxIterations = null, MazeCell? initialCell = null) :
			base(maze, seed, initialCell, true)
		{
			DirectedMaze = new DirectedMaze(Maze);
			MaxIterations = maxIterations ?? Maze.Cells.Length * 10;
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

			// Choose the initial cell and mark it as origin
			SelectedCell ??= GetRandomCell();
			// Make a starting perfect maze
			ConnectToOrigin(SelectedCell);

			yield return Maze;

			for (int i = 0; i < MaxIterations || MaxIterations < 0; i++)
			{
				// Have the origin node, point to a random neighboring node
				int neighborIndex = Rnd.Next(0, SelectedCell.Neighbors.Length);
				var selectedNeighbor = SelectedCell.Neighbors[neighborIndex];
				DirectedMaze.ConnectCells(SelectedCell, selectedNeighbor);

				// That neigboring node becomes the new origin node
				SelectedCell = selectedNeighbor;

				// Have the new origin node point nowhere
				foreach (var neighbor in SelectedCell.Neighbors)
				{
					DirectedMaze.DisconnectCells(SelectedCell, neighbor);
				}

				yield return Maze;
			}
		}

		public override Maze Generate()
		{
			if (MaxIterations < 0)
			{
				throw new InvalidOperationException("To prevent an infinite loop, Generate cannot be called when MaxIterations is set to a negative number.");
			}

			return base.Generate();
		}

		public override string ToString()
		{
			return "Origin Shift";
		}
	}
}
