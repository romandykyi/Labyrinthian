using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    /// <summary>
    /// Class for step by step visualization of maze generation process.
    /// </summary>
    public abstract class MazeGenerator
    {
        private MazeCell? _selectedCell;

        /// <summary>
        /// Random numbers generator used for generation.
        /// </summary>
        public readonly Random Rnd;
        /// <summary>
        /// Maze that is currently generating.
        /// </summary>
        public readonly Maze Maze;

        /// <summary>
        /// Markers for highlighted cells. Used for visualization.
        /// </summary>
        public readonly MarkedCells HighlightedCells;
        /// <summary>
        /// Markers for visited cells. Used for visualization.
        /// </summary>
        public readonly MarkedCells VisitedCells;

        /// <summary>
        /// Cell that is marked as current.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Can be <see langword="null"/> if no cell is marked as current.
        /// </para>
        /// <para>
        /// This value isn't intended to be changed outside of <see cref="MazeGenerator"/> or <see cref="IMazePostProcessor"/>.
        /// </para>
        /// </remarks>
        public MazeCell? SelectedCell
        {
            get => _selectedCell;
            set
            {
                MazeCell? lastSelected = _selectedCell;
                _selectedCell = value;

                if (lastSelected != null)
                    CellStateChanged?.Invoke(lastSelected);

                if (value != null)
                    CellStateChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Post processors that will be used and visualized right after maze generation.
        /// </summary>
        public readonly List<IMazePostProcessor> PostProcessors;

        /// <summary>
        /// <para>
        /// Event that's raised when state of the cell is changed.
        /// </para>
        /// <para>
        /// Changings made to one of these properties will raise this event:
        /// <list type="bullet">
        /// <item><see cref="HighlightedCells"/></item>
        /// <item><see cref="VisitedCells"/></item>
        /// <item><see cref="SelectedCell"/></item>
        /// </list>
        /// </para>
        /// </summary>
        public event Action<MazeCell>? CellStateChanged;

        /// <summary>
        /// Construct a generator with a default seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="defaultVisited">Default 'visited' state for all cells(false by default).</param>
        /// <exception cref="MazeTypeIsNotSupportedException"></exception>
        protected MazeGenerator(Maze maze, MazeCell? initialCell = null, bool defaultVisited = false) :
            this(maze, Environment.TickCount, initialCell, defaultVisited)
        { }

        /// <summary>
        /// Construct a generator with specified seed.
        /// </summary>
        /// <param name="maze">Maze used for generation.</param>
        /// <param name="seed">Seed for random numbers generator.</param>
        /// <param name="initialCell">Cell that will be current at the start of generation(optional).</param>
        /// <param name="defaultVisited">Default 'visited' state for all cells(false by default).</param>
        /// <exception cref="MazeTypeIsNotSupportedException"></exception>
        protected MazeGenerator(Maze maze, int seed,
            MazeCell? initialCell = null, bool defaultVisited = false)
        {
            if (!IsSuitableFor(maze))
                throw new MazeTypeIsNotSupportedException(this, maze);

            Maze = maze;
            Rnd = new Random(seed);

            SelectedCell = initialCell;

            HighlightedCells = new MarkedCells(Maze);
            VisitedCells = new MarkedCells(Maze, defaultVisited);
            PostProcessors = new List<IMazePostProcessor>();

            VisitedCells.MarkChanged += mark => CellStateChanged?.Invoke(mark.Cell);
            HighlightedCells.MarkChanged += mark => CellStateChanged?.Invoke(mark.Cell);

            maze.Description += $" generated with {ToString()} algorithm(seed: {seed})";
        }

        /// <summary>
        /// Get random cell of the maze.
        /// </summary>
        /// <returns>Random cell of the maze.</returns>
        protected MazeCell GetRandomCell()
        {
            int rndIndex = Rnd.Next(0, Maze.Cells.Length);
            return Maze.Cells[rndIndex];
        }

        /// <summary>
        /// Apply all post processors.
        /// </summary>
        /// <returns>Step by step maze post processing.</returns>
        protected IEnumerable<Maze> ApplyPostProcessorsStepByStep()
        {
            if (PostProcessors.Count == 0)
            {
                yield return Maze;
                yield break;
            }

            foreach (var processor in PostProcessors)
            {
                foreach (var maze in processor.Process(this))
                    yield return maze;
            }
        }

        /// <summary>
        /// Check whether the maze is supported for this generation algorithm.
        /// This method is called inside the constructor.
        /// Override it if your maze generation algorithm can be used only for
        /// some specific maze types.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if maze is supported;
        /// <see langword="false"/> otherwise if maze is not supported
        /// </returns>
        protected virtual bool IsSuitableFor(Maze maze) => true;

        /// <summary>
        /// Step by step generation of maze.
        /// </summary>
        /// <returns>Each step of the generation process.</returns>
        protected abstract IEnumerable<Maze> Generation();

        /// <summary>
        /// Step by step generation of maze. 
        /// All post processors from <see cref="PostProcessors" /> will be applied.
        /// </summary>
        /// <returns>Each step of the generation process.</returns>
        public IEnumerable<Maze> GenerateStepByStep()
        {
            // Generate maze
            foreach (var maze in Generation()) yield return maze;
            // Apply post processors
            ApplyPostProcessorsStepByStep();
        }

        /// <summary>
        /// Generate maze. 
        /// All post processors from <see cref="PostProcessors" /> will be applied.
        /// </summary>
        /// <remarks>
        /// By default it just returns the last element of <see cref="GenerateStepByStep"/>.
        /// You can override this method to optimize generation process without step by step
        /// visualisation.
        /// </remarks>
        /// <example>
        /// Overriding method:
        /// <code>
        /// public override Maze Generate()
        /// {
        ///     // [Maze generation process here]
        ///     
        ///     return ApplyPostProcessorsStepByStep().Last();\
        /// }
        /// </code>
        /// </example>
        /// <returns>Generated maze.</returns>
        public virtual Maze Generate()
        {
            GenerateStepByStep().Last();
            return ApplyPostProcessorsStepByStep().Last();
        }

        /// <summary>
        /// Check whether maze is classic orthogonal.
        /// </summary>
        /// <param name="maze">Maze to be checked.</param>
        /// <returns>
        /// <see langword="true"/> if maze is orthogonal without 'holes' in it;
        /// <see langword="false"/> otherwise if maze is not orthogonal or
        /// has 'holes' in it.
        /// </returns>
        protected static bool IsMazeDefaultOrthogonal(Maze maze)
        {
            if (!(maze is OrthogonalMaze orthogonalMaze)) return false;

            return orthogonalMaze.Cells.Length == orthogonalMaze.Columns * orthogonalMaze.Rows;
        }

        /// <summary>
        /// Get user-friendly algorithm's name.
        /// </summary>
        /// <returns>User-friendly algorithm's name</returns>
        public abstract override string ToString();
    }
}
