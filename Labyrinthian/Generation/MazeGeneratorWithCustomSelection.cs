using System;

namespace Labyrinthian
{
    /// <summary>
    /// Function that's used for selecting a cell from a list.
    /// </summary>
    /// <param name="rnd">
    /// Random numbers generator used for selecting a cell(cannot be <see langword="null"/>).
    /// </param>
    /// <param name="count">Size of the list</param>
    /// <returns>Index of the selected cell in the list.</returns>
    /// <example>
    /// Select the newest cell:
    /// <code>MazeCellSelection newestCell = (rnd, count) => count - 1;</code>
    /// </example>
    /// <example>
    /// Select the oldest cell:
    /// <code>MazeCellSelection newestCell = (rnd, count) => 0;</code>
    /// </example>
    /// <example>
    /// Select a random cell:
    /// <code>MazeCellSelection newestCell = (rnd, count) => rnd.Next(count);</code>
    /// </example>
    public delegate int MazeCellSelection(Random rnd, int count);

    /// <summary>
    /// <para>
    /// Maze generator with custom selection.
    /// </para>
    /// <para>
    /// Algorithms that inherit this class:
    /// <list type="bullet">
    /// <item><see cref="RecursiveBacktrackerGeneration"/></item>
    /// <item><see cref="GrowingTreeGeneration"/></item>
    /// </list>
    /// </para>
    /// </summary>
    public abstract class MazeGeneratorWithCustomSelection : MazeGenerator
    {
        /// <summary>
        /// Random selection of a cell.
        /// </summary>
        public readonly static MazeCellSelection RandomSelection = (rnd, n) => rnd.Next(n);

        /// <summary>
        /// Cell selection used inside the algorithm.
        /// </summary>
        protected readonly MazeCellSelection Selection;

        /// <summary>
        /// Constructor with specified seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="seed">Seed for random numbers generator.</param>
        /// <param name="selection">Selection used for generation(optional).</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="defaultVisited">Default visited state for all cells.</param>
        /// <exception cref="MazeTypeIsNotSupportedException"></exception>
        protected MazeGeneratorWithCustomSelection(
            Maze maze, int seed, MazeCellSelection? selection = null, MazeCell? initialCell = null,
            bool defaultVisited = false)
            : base(maze, seed, initialCell, defaultVisited)
        {
            Selection = selection ?? RandomSelection;
        }

        /// <summary>
        /// Constructor with specified seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="selection">Selection used for generation(optional).</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="defaultVisited">Default visited state for all cells.</param>
        /// <exception cref="MazeTypeIsNotSupportedException"></exception>
        protected MazeGeneratorWithCustomSelection(Maze maze, MazeCellSelection? selection = null,
            MazeCell? initialCell = null, bool defaultVisited = false) :
            this(maze, Environment.TickCount, selection, initialCell, defaultVisited)
        { }
    }
}
