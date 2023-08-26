using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when the maze cell is unexpected to be outer.
    /// </summary>
    [Serializable]

    public sealed class CellIsOuterException : Exception
    {
        /// <summary>
        /// Constructor with default message.
        /// </summary>
        public CellIsOuterException() : this("Maze cell is unexpectedly outer.") { }

        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        public CellIsOuterException(string message) : base(message) { }

        /// <summary>
        /// Constructor which will generate a message based on <paramref name="variableName"/>.
        /// </summary>
        /// <param name="variableName">Name of variable that represents outer cell.</param>
        /// <param name="extraMessage">An extra message for exception(optional).</param>
        public CellIsOuterException(string variableName, string extraMessage = "") :
            base($"{variableName} is unexpectedly outer. {extraMessage}") { }
    }
}