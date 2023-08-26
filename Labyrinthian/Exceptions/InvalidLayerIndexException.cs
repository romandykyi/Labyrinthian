using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when a layer of the <see cref="MultiGridMaze2D"/> is invalid.
    /// </summary>
    [Serializable]
    public class InvalidLayerIndexException : Exception
    {
        /// <summary>
        /// Constructor with default message.
        /// </summary>
        public InvalidLayerIndexException() : this("Layer index is invalid") { }
        
        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public InvalidLayerIndexException(string message) : base(message) { }
    }
}
