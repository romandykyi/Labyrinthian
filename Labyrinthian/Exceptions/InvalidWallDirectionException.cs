using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when a wall direction is invalid.
    /// </summary>
    [Serializable]
    public class InvalidWallDirectionException : Exception
    {
        /// <summary>
        /// Constructor with default message.
        /// </summary>
        public InvalidWallDirectionException() : this("Wall direction is out of bounds") { }
        
        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public InvalidWallDirectionException(string message) : base(message) { }
    }
}
