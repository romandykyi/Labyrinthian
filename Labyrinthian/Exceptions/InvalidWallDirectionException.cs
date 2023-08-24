using System;

namespace Labyrinthian
{
    public class InvalidWallDirectionException : Exception
    {
        public InvalidWallDirectionException() : this("Wall direction is out of bounds") { }
        public InvalidWallDirectionException(string message) : base(message) { }
    }
}
