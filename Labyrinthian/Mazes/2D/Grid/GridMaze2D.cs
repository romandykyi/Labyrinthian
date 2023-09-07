using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional maze that can be represented with a single grid.
    /// </summary>
    public abstract class GridMaze2D : Maze2D
    {
        /// <summary>
        /// Grid of cells(<see langword="null"/> when there's a gap).
        /// </summary>
        protected readonly MazeCell?[,] CellsGrid;
        /// <summary>
        /// Coordinates of each cell on the grid.
        /// </summary>
        protected readonly GridPoint2D[] CellsCoordinates;

        /// <summary>
        /// Number of rows.
        /// </summary>
        public readonly int Rows;
        /// <summary>
        /// Number of columns.
        /// </summary>
        public readonly int Columns;

        /// <summary>
        /// Create a grid-based maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="p">Predicate that's used to determine whether we should include the cell.</param>
        /// <exception cref="ArgumentOutOfRangeException" />
        protected GridMaze2D(int width, int height, Predicate<GridPoint2D> p) : base()
        {
            if (width <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            List<MazeCell> cells = new List<MazeCell>();
            List<GridPoint2D> cellsCoordinates = new List<GridPoint2D>();

            int index = 0;
            CellsGrid = new MazeCell[height, width];
            // Initialize each cell on the grid
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    GridPoint2D point = new GridPoint2D(i, j);
                    if (p(point))
                    {
                        MazeCell cell = new MazeCell(index);
                        cells.Add(cell);
                        cellsCoordinates.Add(point);
                        CellsGrid[i, j] = cell;
                        ++index;
                    }
                    else
                    {
                        CellsGrid[i, j] = null;
                    }
                }
            }
            Columns = width;
            Rows = height;
            Cells = cells.ToArray();
            CellsCoordinates = cellsCoordinates.ToArray();
        }

        protected sealed override MazeCell?[] GetDirectedNeighbors(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];
            return GetDirectedNeighbors(cell, point.Row, point.Column);
        }

        /// <summary>
        /// Get directed neighbors of the cell.
        /// </summary>
        /// <param name="cell">Cell, whose neighbors will be returned.</param>
        /// <param name="row">Row of the cell.</param>
        /// <param name="column">Column of the cell.</param>
        /// <returns></returns>
        protected abstract MazeCell?[] GetDirectedNeighbors(MazeCell cell, int row, int column);

        /// <summary>
        /// Get a cell or create a new outer cell and return it.
        /// </summary>
        /// <param name="row">Row of the cell.</param>
        /// <param name="column">Column of the cell.</param>
        /// <param name="neighbor">Neighbor of the cell.</param>
        /// <param name="direction">Direction to the neighbor.</param>
        /// <returns>
        /// Cell that's already exists with given coordinates or a new outer cell.
        /// </returns>
        protected MazeCell GetCell(int row, int column, MazeCell neighbor, int direction)
        {
            MazeCell? cell = null;
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                cell = CellsGrid[row, column];
            }
            return cell ?? MazeCell.CreateOuterCell(neighbor, direction);
        }

        /// <summary>
        /// Access a cell from the grid.
        /// </summary>
        public MazeCell? this[int row, int column] => CellsGrid[row, column];
    }
}
