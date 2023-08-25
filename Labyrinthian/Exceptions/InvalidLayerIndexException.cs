using System;

namespace Labyrinthian
{
    [Serializable]
    public class InvalidLayerIndexException : Exception
    {
        public InvalidLayerIndexException() : this("Layer index is invalid") { }
        public InvalidLayerIndexException(string message) : base(message) { }
    }
}
