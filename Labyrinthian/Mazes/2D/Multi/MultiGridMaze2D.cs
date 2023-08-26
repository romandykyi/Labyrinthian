using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Maze that can be represented with multiple grids that intersect each others.
    /// </summary>
    public abstract class MultiGridMaze2D : Maze2D
    {
        /// <summary>
        /// Grids of cells(<see langword="null"/> when there's a gap).
        /// </summary>
        protected readonly MazeCell?[][,] CellsGrids;
        /// <summary>
        /// Coordinates of each cell.
        /// </summary>
        protected readonly MultiGridPoint2D[] CellsCoordinates;

        /// <summary>
        /// Main grid rows number.
        /// </summary>
        public readonly int BaseRows;
        /// <summary>
        /// Main grid columns number.
        /// </summary>
        public readonly int BaseColumns;
        /// <summary>
        /// Number of grids.
        /// </summary>
        public abstract int GridsNumber { get; }

        /// <summary>
        /// Get sizes of the grid.
        /// </summary>
        /// <param name="grid">Index of the grid.</param>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        /// <exception cref="InvalidGridIndexException" />
        protected abstract void GetGridSizes(int grid, out int rows, out int columns);

        /// <summary>
        /// Create a maze.
        /// </summary>
        /// <param name="baseWidth">Width(columns) of the main grid.</param>
        /// <param name="baseHeight">Height(rows) of the main grid.</param>
        /// <param name="predicate">Predicate, used to determine whether we need to include the cell or not.</param>
        public MultiGridMaze2D(int baseWidth, int baseHeight, Predicate<MultiGridPoint2D> predicate) : base()
        {
            BaseRows = baseHeight;
            BaseColumns = baseWidth;

            List<MazeCell> cells = new List<MazeCell>();
            List<MultiGridPoint2D> cellsCoordinates = new List<MultiGridPoint2D>();

            int index = 0;
            CellsGrids = new MazeCell[GridsNumber][,];
            // Initialize cells for each grid
            for (int k = 0; k < GridsNumber; ++k)
            {
                GetGridSizes(k, out int rows, out int columns);
                CellsGrids[k] = new MazeCell[rows, columns];
                for (int i = 0; i < rows; ++i)
                {
                    for (int j = 0; j < columns; ++j)
                    {
                        MultiGridPoint2D point = new MultiGridPoint2D(k, i, j);
                        if (predicate(point))
                        {
                            MazeCell cell = new MazeCell(index);
                            cells.Add(cell);
                            cellsCoordinates.Add(point);
                            CellsGrids[k][i, j] = cell;
                            ++index;
                        }
                        else
                        {
                            CellsGrids[k][i, j] = null;
                        }
                    }
                }
            }
            Cells = cells.ToArray();
            CellsCoordinates = cellsCoordinates.ToArray();
        }

        /// <summary>
        /// Access a cell from the grid.
        /// </summary>
        public MazeCell? this[int grid, int row, int column] => CellsGrids[grid][row, column];

        /// <summary>
        /// Get directed neighbors of the cell.
        /// </summary>
        /// <param name="cell">Cell, whose neighbors will be returned.</param>
        /// <param name="grid">Grid of the cell.</param>
        /// <param name="row">Row of the cell.</param>
        /// <param name="column">Column of the cell.</param>
        /// <returns></returns>
        protected abstract MazeCell[] GetDirectedNeighbors(MazeCell cell, int grid, int row, int column);

        protected sealed override MazeCell[] GetDirectedNeighbors(MazeCell cell)
        {
            MultiGridPoint2D point = CellsCoordinates[cell.Index];
            return GetDirectedNeighbors(cell, point.Grid, point.Row, point.Column);
        }

        /// <summary>
        /// Get a cell or create a new outer cell and return it.
        /// </summary>
        /// <param name="grid">Index of the grid where cell will be created.</param>
        /// <param name="row">Row of the cell.</param>
        /// <param name="column">Column of the cell.</param>
        /// <param name="neighbor">Neighbor of the cell.</param>
        /// <param name="direction">Direction to the neighbor.</param>
        /// <returns>
        /// Cell that's already exists with given coordinates or a new outer cell.
        /// </returns>
        protected MazeCell GetCell(int grid, int row, int column, MazeCell neighbor, int direction)
        {
            MazeCell? cell = null;
            int rows = CellsGrids[grid].GetLength(0);
            int columns = CellsGrids[grid].GetLength(1);
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                cell = CellsGrids[grid][row, column];
            }
            return cell ?? MazeCell.CreateOuterCell(neighbor, direction);
        }
    }
}
