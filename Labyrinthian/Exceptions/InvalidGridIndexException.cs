using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when an index of the grid of the <see cref="MultiGridMaze2D"/> is invalid.
    /// </summary>
    [Serializable]
    public class InvalidGridIndexException : Exception
    {
        /// <summary>
        /// Constructor with default message.
        /// </summary>
        public InvalidGridIndexException() : this("Grid index is invalid") { }
        
        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public InvalidGridIndexException(string message) : base(message) { }
    }
}
