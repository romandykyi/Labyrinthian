using System;

namespace Labyrinthian
{
	/// <summary>
	/// Neighbors selector for random walk algorithms (Aldous Broder and Origin Shift).
	/// </summary>
	public interface INeighborSelector
	{
		/// <summary>
		/// Initialize the neighbor selector with the given maze and random number generator.
		/// </summary>
		/// <param name="maze">The maze in which the neighbor selection will occur.</param>
		/// <param name="rnd">The random number generator to use for selecting neighbors.</param>
		void Init(Maze maze, Random rnd);

		/// <summary>
		/// Select a neighboring cell based on the current cell provided.
		/// </summary>
		/// <param name="cell">The current cell from which a neighbor is to be selected.</param>
		/// <returns>The selected neighboring cell.</returns>
		MazeCell Select(MazeCell cell);
	}
}
