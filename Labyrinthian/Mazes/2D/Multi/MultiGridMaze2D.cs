using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Двовимірний лабіринт, який можна представити за допомогою різних сіток які перетинаються
    /// </summary>
    public abstract class MultiGridMaze2D : Maze2D
    {
        /// <summary>
        /// Сітки клітинок(null на місцях пропуску)
        /// </summary>
        protected readonly MazeCell?[][,] CellsGrids;
        /// <summary>
        /// Координати кожної клітинки
        /// </summary>
        protected readonly MultiGridPoint2D[] CellsCoordinates;

        public readonly int BaseRows;
        public readonly int BaseColumns;
        public abstract int Layers { get; }

        protected abstract void GetLayerSizes(int layer, out int rows, out int columns);

        public MultiGridMaze2D(int baseWidth, int baseHeight, Predicate<MultiGridPoint2D> predicate)
        {
            BaseRows = baseHeight;
            BaseColumns = baseWidth;

            List<MazeCell> cells = new List<MazeCell>();
            List<MultiGridPoint2D> cellsCoordinates = new List<MultiGridPoint2D>();

            int index = 0;
            CellsGrids = new MazeCell[Layers][,];
            for (int k = 0; k < Layers; ++k)
            {
                GetLayerSizes(k, out int rows, out int columns);
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

        public MazeCell? this[int layer, int row, int column] => CellsGrids[layer][row, column];

        protected abstract MazeCell[] GetDirectedNeighbors(MazeCell cell, int layer, int row, int column);

        protected sealed override MazeCell[] GetDirectedNeighbors(MazeCell cell)
        {
            MultiGridPoint2D point = CellsCoordinates[cell.Index];
            return GetDirectedNeighbors(cell, point.Layer, point.Row, point.Column);
        }

        /// <summary>
        /// Отримати клітинку в межах або створити її поза межами лабіринту
        /// </summary>
        /// <param name="layer">шар</param>
        /// <param name="row">рядок</param>
        /// <param name="column">стовпчик</param>
        /// <param name="neighbor">сусід</param>
        /// <param name="direction">напрямок до сусіда</param>
        protected MazeCell GetCell(int layer, int row, int column, MazeCell neighbor, int direction)
        {
            MazeCell? cell = null;
            int rows = CellsGrids[layer].GetLength(0);
            int columns = CellsGrids[layer].GetLength(1);
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                cell = CellsGrids[layer][row, column];
            }
            return cell ?? MazeCell.CreateEdgeCell(neighbor, direction);
        }
    }
}
