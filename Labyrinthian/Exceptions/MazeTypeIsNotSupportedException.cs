using System;

namespace Labyrinthian
{
    public class MazeTypeIsNotSupportedException : Exception
    {
        public MazeTypeIsNotSupportedException(MazeGenerator generator, Maze maze) : this(
            $"\"{generator}\" does not support \"{maze}\""
            ) { }
        public MazeTypeIsNotSupportedException(string message) : base(message) { }
    }
}
