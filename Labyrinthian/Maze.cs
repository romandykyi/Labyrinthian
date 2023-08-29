using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Labyrinthian
{
    /// <summary>
    /// Class representating a graph-based maze. 
    /// It consists of two undirected graphs: <b>base graph</b> and <b>passages graph</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each <b><b>node</b></b> of the <b>base graph</b> represents a maze cell or a special cell with a negative index,
    /// that is not the maze part and used to draw exterior walls.
    /// <br/>
    /// Each <b>edge</b> of the <b>base graph</b> represents a possible wall/passage, if one of the nodes
    /// of the <b>edge</b> is not the maze part then this <b>edge</b> is an exterior wall.
    /// </para>
    /// <para>
    /// Each <b>node</b> of the <b>passages graph</b> represents a maze cell.
    /// <br/>
    /// Each <b>edge</b> of the <b>passages graph</b> represent passage between two maze cells.
    /// </para>
    /// <para>
    /// <b>Base graph</b> is created inside the constructor and <b>passages graph</b> should be initialized
    /// inside the <see cref="MazeGenerator"/> class
    /// </para>
    /// </remarks>
    public abstract class Maze
    {
        // Pre-calculated sizes
        private float[]? _sizes;
        // Set that contains all passages in the maze.
        private readonly HashSet<MazeEdge> Connections;

        /// <summary>
        /// Cells that maze contains
        /// </summary>
        public MazeCell[] Cells { get; protected set; } = null!;

        /// <summary>
        /// Each solution of the maze.
        /// </summary>
        public readonly List<MazePath> Paths;

        /// <summary>
        /// Description of the maze. Contains type of the maze and its dimensions 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sizes of the maze in each dimension
        /// </summary>
        public virtual float[] Sizes => _sizes ?? CalculateSizes();
        /// <summary>
        /// Number of the maze dimensions
        /// </summary>
        public abstract int Dimensions { get; }

        /// <summary>
        /// Delegete of an event that is raised when passage is carved or wall is added
        /// </summary>
        /// <param name="owner">Maze that called the event</param>
        /// <param name="edge">Edge that was changed</param>
        /// <param name="isConnected">True if passage is carved, false if wall is added</param>
        public delegate void EdgeChangedEvent(Maze owner, MazeEdge edge, bool isConnected);

        /// <summary>
        /// Event that is raised when passage is carved or wall is added
        /// </summary>
        public event EdgeChangedEvent? EdgeChanged;

        private float[] CalculateSizes()
        {
            _sizes = Enumerable.Repeat(0f, Dimensions).ToArray();
            foreach (var cell in Cells)
            {
                for (int i = 0; i < GetCellPointsNumber(cell); i++)
                {
                    float[] point = GetCellPoint(cell, i);
                    for (int dim = 0; dim < Dimensions; dim++)
                    {
                        if (point[dim] > _sizes[dim])
                            _sizes[dim] = point[dim];
                    }
                }
            }
            return _sizes;
        }

        /// <summary>
        /// Basic initialization without touching the Cells array
        /// </summary>
        protected Maze()
        {
            Description = string.Empty;
            Connections = new HashSet<MazeEdge>();
            Paths = new List<MazePath>();
        }

        /// <summary>
        /// Initialization of <see cref="Cells"/>
        /// </summary>
        /// <param name="size">A size of <see cref="Cells"/></param>
        protected Maze(int size) : this()
        {
            Cells = new MazeCell[size];
            for (int i = 0; i < Cells.Length; ++i)
            {
                Cells[i] = new MazeCell(i);
            }
        }

        private bool ChangeEdge(MazeCell cell1, MazeCell cell2, Predicate<MazeEdge> change)
        {
            // Null checks just in case
            if (cell1 is null)
                throw new ArgumentNullException(nameof(cell1), "cells cannot be null");
            if (cell2 is null)
                throw new ArgumentNullException(nameof(cell2), "cells cannot be null");

            if (!cell1.IsMazePart)
                throw new CellIsOuterException(nameof(cell1), "You cannot connect cells which are not the parts of maze");
            if (!cell2.IsMazePart)
                throw new CellIsOuterException(nameof(cell2), "You cannot connect cells which are not the parts of maze");
            if (!MazeCell.AreNeighbors(cell1, cell2))
                throw new CellsAreNotNeighborsException(cell1, cell2, "You can connect only neighboring cells");

            MazeEdge relation = MazeEdge.GetMinMax(cell1, cell2);
            bool result = change(relation);
            if (result) EdgeChanged?.Invoke(this, relation, true);

            return result;
        }

        /// <summary>
        /// Carve a passage between <paramref name="cell1"/> and <paramref name="cell2"/>
        /// (order of the arguments doesn't matter).
        /// </summary>
        /// <returns><see langword="true"/> when passage is carved, or <see langword="false"/> when passage was already carved</returns>
        /// <exception cref="CellIsOuterException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        /// <exception cref="ArgumentNullException" />
        public bool ConnectCells(MazeCell cell1, MazeCell cell2)
        {
            return ChangeEdge(cell1, cell2, Connections.Add);
        }

        /// <summary>
        /// Create a wall between <paramref name="cell1"/> and <paramref name="cell2"/>
        /// (order of the arguments doesn't matter).
        /// </summary>
        /// <returns><see langword="true"/> when wall is created, or <see langword="false"/> when wall was already present</returns>
        /// <exception cref="CellIsOuterException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        /// <exception cref="ArgumentNullException" />
        public bool BlockCells(MazeCell cell1, MazeCell cell2)
        {
            return ChangeEdge(cell1, cell2, Connections.Remove);
        }

        /// <summary>
        /// Check if there is a passage between <paramref name="cell1"/> and <paramref name="cell2"/>
        /// (order of the arguments doesn't matter).
        /// </summary>
        /// <returns>
        /// True when a passages between cell1 and cell2 exists. 
        /// False when a passage between cell1 and cell2 doesn't exist,
        /// at least one of the cells isn't the part of the maze,
        /// cell1 and cell2 aren't neighbors
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public bool AreCellsConnected(MazeCell cell1, MazeCell cell2)
        {
            if (cell1 is null)
                throw new ArgumentNullException(nameof(cell1), "cells cannot be null");
            if (cell2 is null)
                throw new ArgumentNullException(nameof(cell2), "cells cannot be null");

            return cell1.IsMazePart && cell2.IsMazePart &&
                Connections.Contains(MazeEdge.GetMinMax(cell1, cell2));
        }

        /// <summary>
        /// Init the base graph of the maze. This method uses an abstract method GetDirectedNeighbors(cell) to set
        /// all neighbors of Cells. It's intended to be called inside the constructor
        /// of Maze overrides after initializing Cells array.
        /// </summary>
        protected void InitGraph()
        {
            foreach (var cell in Cells) cell.SetNeighbors(GetDirectedNeighbors(cell));
        }

        /// <summary>
        /// This method should return all directed neighbors of the cell. It's used inside the method InitGraph to
        /// set all neighbors of Cells.
        /// </summary>
        /// <returns>
        /// All directed neighbors of the cell.
        /// </returns>
        protected abstract MazeCell?[] GetDirectedNeighbors(MazeCell cell);

        /// <summary>
        /// Get the number of points needed to represent the cell
        /// </summary>
        public abstract int GetCellPointsNumber(MazeCell cell);

        /// <summary>
        /// Get a point of the cell
        /// </summary>
        /// <param name="cell">cell which point we need to get</param>
        /// <param name="pointIndex">index of the point</param>
        /// <returns>point of the cell</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public abstract float[] GetCellPoint(MazeCell cell, int pointIndex);

        /// <summary>
        /// Get a center of the cell.
        /// </summary>
        /// <returns>
        /// If isn't overrided, returns a center of a polygon created by points of the cell.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        public virtual float[] GetCellCenter(MazeCell cell)
        {
            if (cell is null) throw new ArgumentNullException(nameof(cell));

            int n = GetCellPointsNumber(cell);
            float[] center = new float[Dimensions];
            // Sum of all points coordinates
            for (int i = 0; i < n; ++i)
            {
                float[] point = GetCellPoint(cell, i);
                for (int j = 0; j < center.Length; ++j)
                {
                    center[j] += point[j];
                }
            }
            // Average
            for (int i = 0; i < center.Length; ++i)
            {
                center[i] /= n;
            }

            return center;
        }

        /// <summary>
        /// Get a path that represents a wall.
        /// </summary>
        /// <param name="wall">edge that represents a wall</param>
        public abstract PathSegment GetWallPosition(MazeEdge wall);

        /// <summary>
        /// Get a path between two neighboring cells.
        /// </summary>
        /// <param name="edge">An edge that represents two neighboring cells</param>
        /// <returns>A path between two neighboring cells.</returns>
        protected abstract PathSegment GetPath(MazeEdge edge);

        /// <summary>
        /// Get a path between two neighboring cells.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        public PathSegment GetPathBetweenCells(MazeCell from, MazeCell to)
        {
            if (from is null)
                throw new ArgumentNullException(nameof(from), "cells cannot be null");
            if (to is null)
                throw new ArgumentNullException(nameof(to), "cells cannot be null");

            return GetPathBetweenCells(new MazeEdge(from, to));
        }

        /// <summary>
        /// Get a path that represents two neighboring cells.
        /// </summary>
        /// <param name="relation"></param>
        /// <returns>
        /// <see cref="Line"/> if isn't overrided
        /// </returns>
        /// <exception cref="CellsAreNotNeighborsException"></exception>
        public virtual PathSegment GetPathBetweenCells(MazeEdge relation)
        {
            if (!MazeCell.AreNeighbors(relation.Cell1, relation.Cell2))
                throw new CellsAreNotNeighborsException(relation.Cell1, relation.Cell2, "This method designed for getting path only between two neighboring cells");

            float[] start, end;
            if (!relation.Cell1.IsMazePart)
            {
                start = GetWallPosition(relation.Inverted).Center;
                end = GetCellCenter(relation.Cell2);
            }
            else if (!relation.Cell2.IsMazePart)
            {
                start = GetCellCenter(relation.Cell1);
                end = GetWallPosition(relation).Center;
            }
            else
            {
                return GetPath(relation);
            }

            Vector2 startV = PositionTo2DPoint(start),
                endV = PositionTo2DPoint(end);

            return new Line(startV, endV);
        }

        /// <summary>
        /// Transform a maze point into 2D plane.
        /// </summary>
        /// <param name="position">A maze point.</param>
        public abstract Vector2 PositionTo2DPoint(float[] position);

        /// <summary>
        /// Convert a cell into svg-string.
        /// </summary>
        /// <param name="cell">A cell that will be converted into svg-string.</param>
        /// <param name="cellSize">The size of cell.</param>
        /// <param name="offset">Offset from top left corner.</param>
        /// <returns>
        /// A svg-string that represents the cell. It returns &lt;polygon&gt; if isn't overrided.
        /// </returns>
        public virtual string CellToSvgString(MazeCell cell, float cellSize, float offset)
        {
            StringBuilder line = new StringBuilder();
            line.Append("<polygon points=\"");
            int n = GetCellPointsNumber(cell);
            for (int i = 0; i < n; ++i)
            {
                float[] point = GetCellPoint(cell, i);
                point[0] = point[0] * cellSize + offset;
                point[1] = point[1] * cellSize + offset;
                string x = point[0].ToInvariantString();
                string y = point[1].ToInvariantString();
                line.Append($"{x},{y}");
                if (i < n - 1) line.Append(" ");
            }
            line.Append("\"/>");

            return line.ToString();
        }

        /// <summary>
        /// Get maze description.
        /// </summary>
        /// <returns><see cref="Description" /></returns>
        public sealed override string ToString()
        {
            return Description;
        }
    }
}
