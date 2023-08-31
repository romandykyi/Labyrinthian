using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when path in the maze cannot be found.
    /// </summary>
    [Serializable]
    public sealed class PathNotFoundException : Exception
    {
        /// <summary>
        /// Constructor with default message.
        /// </summary>
        public PathNotFoundException() :
            this("Couldn't find the path between two cells. Make sure that both cells belong to the same maze and that the maze has no isolated cells.")
        { }

        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public PathNotFoundException(string message) : base(message) { }
    }
}