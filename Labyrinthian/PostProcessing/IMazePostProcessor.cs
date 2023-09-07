using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Interface for maze post processors.
    /// </summary>
    public interface IMazePostProcessor
    {
        /// <summary>
        /// Process maze after it's generated.
        /// </summary>
        /// <param name="caller">Maze generator, that calls this method.</param>
        /// <returns>
        /// Step-by-step maze post processing.
        /// </returns>
        IEnumerable<Maze> Process(MazeGenerator caller);
    }
}
