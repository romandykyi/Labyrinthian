using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when the maze cell is unexpected to be outer
    /// </summary>
    [Serializable]

    public sealed class CellIsOuterException : Exception
    {
        /// <summary>
        /// Constructor that creates an exception with specified message
        /// </summary>
        public CellIsOuterException(string message = "Maze cell is unexpectedly outer.") : base(message) { }

        /// <summary>
        /// Constructor that creates an exception with a message 
        /// <c>"{<paramref name="variableName"/>} is unexpectedly outer. {<paramref name="extraMessage}"/>"</c>
        /// </summary>
        /// <param name="variableName">name of variable that represents outer cell</param>
        /// <param name="extraMessage">extra message for exception</param>
        public CellIsOuterException(string variableName, string extraMessage = "") :
            base($"{variableName} is unexpectedly outer. {extraMessage}") { }
    }
}