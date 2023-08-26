using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that is thrown when the generator doesn't support the maze.
    /// </summary>
    [Serializable]
    public class MazeTypeIsNotSupportedException : Exception
    {
        /// <summary>
        /// Constructor that will generate message based on <paramref name="generator"/> and <paramref name="maze"/>.
        /// </summary>
        /// <param name="generator">Generator that has thrown this exception(cannot be <see langword="null"/>).</param>
        /// <param name="maze">Maze, which <paramref name="generator"/> doesn't support.</param>
        public MazeTypeIsNotSupportedException(MazeGenerator generator, Maze maze) : this(
            $"\"{generator}\" does not support \"{maze}\""
            ) { }

        /// <summary>
        /// Constructor with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public MazeTypeIsNotSupportedException(string message) : base(message) { }
    }
}
