namespace Labyrinthian.Generation
{
	/// <summary>
	/// An interface for generators which use directed graphs.
	/// </summary>
	public interface IDirectedGraphGenerator
	{
		/// <summary>
		/// Get a directed maze.
		/// </summary>
		public DirectedMaze DirectedMaze { get; }
	}
}
