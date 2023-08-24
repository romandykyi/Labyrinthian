using System;

namespace Labyrinthian
{
    public class InvalidLayerIndexException : Exception
    {
        public InvalidLayerIndexException() : this("Layer index is invalid") { }
        public InvalidLayerIndexException(string message) : base(message) { }
    }
}
