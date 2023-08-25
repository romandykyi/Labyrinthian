using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// ���������� �������, ���� ����� ����������� �� ��������� ����
    /// </summary>
    public abstract class GridMaze2D : Maze2D
    {
        /// <summary>
        /// ѳ��� �������(null �� ����� ��������)
        /// </summary>
        protected readonly MazeCell?[,] CellsGrid;
        /// <summary>
        /// ���������� ����� �������
        /// </summary>
        protected readonly GridPoint2D[] CellsCoordinates;

        public readonly int Rows, Columns;

        protected GridMaze2D(int width, int height, Predicate<GridPoint2D> p) : base()
        {
            List<MazeCell> cells = new List<MazeCell>();
            List<GridPoint2D> cellsCoordinates = new List<GridPoint2D>();

            int index = 0;
            CellsGrid = new MazeCell[height, width];
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
        /// �������� ����� �������
        /// </summary>
        protected abstract MazeCell?[] GetDirectedNeighbors(MazeCell cell, int row, int col);

        /// <summary>
        /// �������� ������� � ����� ��� �������� �� ���� ������ ��������
        /// </summary>
        /// <param name="row">�����</param>
        /// <param name="column">��������</param>
        /// <param name="neighbor">����</param>
        /// <param name="direction">�������� �� �����</param>
        protected MazeCell GetCell(int row, int column, MazeCell neighbor, int direction)
        {
            MazeCell? cell = null;
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                cell = CellsGrid[row, column];
            }
            return cell ?? MazeCell.CreateOuterCell(neighbor, direction);
        }

        public MazeCell? this[int row, int column] => CellsGrid[row, column];

        /// <summary>
        /// �������� �������� � ������ ������������ � ����������� �������
        /// </summary>
        /// <param name="width">������� ��������� ������������</param>
        /// <param name="height">������ ��������� ������������</param>
        /// <param name="inWidth">������� ����������� ������������</param>
        /// <param name="inHeight">������ ���������� ������������</param>
        public static Predicate<GridPoint2D> RectangularPattern(int width, int height, int inWidth, int inHeight)
        {
            int m0 = (height - inHeight) / 2;
            int m1 = m0 + inHeight;
            int n0 = (width - inWidth) / 2;
            int n1 = n0 + inWidth;

            return p => p.Row < m0 || p.Row >= m1 ||
                p.Column < n0 || p.Column >= n1;
        }

        /// <summary>
        /// �������� �������� � ������ ���������� � ��������� �������
        /// </summary>
        /// <param name="sideLength">������� ��������� ����������</param>
        /// <param name="inSideLength">������� ����������� ����������</param>
        public static Predicate<GridPoint2D> TriangularPattern(int sideLength, int inSideLength)
        {
            int k = sideLength - 1;
            int r = inSideLength - 1;

            int mid = (sideLength - inSideLength) / 2;
            mid += mid % 2;
            int maxI = mid + inSideLength;

            int q = sideLength - inSideLength;

            return p =>
                Math.Abs(p.Column - k) <= p.Row &&
                (Math.Abs(p.Column - r - q) > p.Row - mid || p.Row >= maxI);
        }

        /// <summary>
        /// �������� �������� � ������ ������������ � ����������� �������
        /// </summary>
        /// <param name="sideLength">������� ��������� ������������</param>
        /// <param name="inSideLength">������� ����������� ������������</param>
        public static Predicate<GridPoint2D> HexagonalPattern(int sideLength, int inSideLength)
        {
            int n = 2 * sideLength - 1;
            int m = 2 * inSideLength - 1;

            int q = (n - m) / 2;

            return p =>
            {
                int k = Math.Abs(sideLength - 1 - p.Column);
                int inI = p.Row - q;
                int inJ = p.Column - q;
                int r = Math.Abs(inSideLength - 1 - inJ);

                return
                    p.Row >= k / 2 && p.Row < n - k / 2 - k % 2 &&
                    (inI < 0 || inI >= m || inJ < 0 || inJ >= m ||
                    inI < r / 2 || inI >= m - r / 2 - r % 2);
            };
        }

        /// <summary>
        /// �������� �������� � ������ ������������ � ����������� ������� �� �������� ����
        /// </summary>
        /// <param name="sideLength">������� ��������� ������������</param>
        /// <param name="inSideLength">������� ����������� ������������</param>
        public static Predicate<GridPoint2D> DeltaHexagonalPattern(int sideLength, int inSideLength)
        {
            int r = (sideLength + 1) % 2;
            Predicate<GridPoint2D> pattern = HexagonalPattern(sideLength, inSideLength);

            return (p) =>
            {
                int row = p.Row, col = p.Column;

                col /= 3;
                int d = (col + r) % 2;
                if (d == 1 && row == 0) return false;
                else row = (row - d) / 2;

                return pattern(new GridPoint2D(row, col));
            };
        }
    }
}