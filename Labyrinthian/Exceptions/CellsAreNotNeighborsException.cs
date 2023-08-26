using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when two cells are unexpectedly not neighbors.
    /// </summary>
    [Serializable]
    public sealed class CellsAreNotNeighborsException : Exception
    {
        /// <summary>
        /// Constructor that creates an exception with message that
        /// contains indices of two specified cells.
        /// </summary>
        /// <param name="cell1">First cell. Cannot be <see langword="null"/>.</param>
        /// <param name="cell2">Second cell. Cannot be <see langword="null"/>.</param>
        /// <param name="extraMessage">An extra message(optional).</param>
        public CellsAreNotNeighborsException(MazeCell cell1, MazeCell cell2, string extraMessage = "") :
            base($"Two cells(first cell index = {cell1.Index}, second cell index = {cell2.Index}) are unexpectedly not neighbors. {extraMessage}") { }
    }
}