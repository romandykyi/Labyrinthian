using System.Collections.Generic;

namespace Labyrinthian
{
	/// <summary>
	/// A wrapper of the <see cref="Labyrinthian.Maze" /> class which supports directed edges.
	/// </summary>
	/// <remarks>
	/// Note that this class does not track changes to the maze edges made outside of it.
	/// </remarks>
	public class DirectedMaze
	{
		private readonly Dictionary<MazeEdge, bool> _edgesDirections;

		/// <summary>
		/// Get an original maze.
		/// </summary>
		public Maze Maze { get; private set; }

		/// <summary>
		/// Gets a dictionary which defines a direction of each maze edge.
		/// </summary>
		/// <remarks>
		/// If an edge has a value <see langword="false" />, then direction is Cell1 -> Cell2, 
		/// otherwise it's Cell2 -> Cell1.
		/// </remarks>
		public IReadOnlyDictionary<MazeEdge, bool> EdgesDirections => _edgesDirections;

		/// <summary>
		/// Construct a directed maze based on a maze. 
		/// Note that existing edges will be ignored and will not appear in
		/// <see cref="EdgesDirections" /> unless added within this class.
		/// </summary>
		/// <param name="maze">A maze to be based on.</param>
		public DirectedMaze(Maze maze)
		{
			_edgesDirections = new Dictionary<MazeEdge, bool>();
			Maze = maze;
		}

		/// <summary>
		/// Add/update a relation cell1 -> cell2 and carve a passage between the cells in the maze.
		/// </summary>
		/// <param name="cell1">A start cell.</param>
		/// <param name="cell2">An end cell.</param>
		public void ConnectCells(MazeCell cell1, MazeCell cell2)
		{
			// Ensure that cell1.Index < cell2.Index
			var edge = MazeEdge.GetMinMax(cell1, cell2);
			// Select the right direction
			_edgesDirections[edge] = cell1.Index > cell2.Index;

			Maze.ConnectCells(cell1, cell2);
		}

		/// <summary>
		/// Remove a relation cell1 -> cell2 and create a wall between the cells in the maze.
		/// </summary>
		/// <remarks>
		/// If relation cell2 -> cell1 exists instead of cell1 -> cell2, then nothing will change.
		/// </remarks>
		/// <param name="cell1">A start cell.</param>
		/// <param name="cell2">An end cell.</param>
		public void DisconnectCells(MazeCell cell1, MazeCell cell2)
		{
			var edge = MazeEdge.GetMinMax(cell1, cell2);
			if (_edgesDirections.TryGetValue(edge, out bool value) && 
				value == cell1.Index > cell2.Index)
			{
				_edgesDirections.Remove(edge);
				Maze.BlockCells(cell1, cell2);
			}
		}
	}
}
