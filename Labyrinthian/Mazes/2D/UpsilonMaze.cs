using System;

namespace Labyrinthian
{
    /// <summary>
    /// Ћаб≥ринт, €кий складаЇтьс€ з восьмикутник≥в,
    /// м≥ж €кими перпендикул€рно знаход€тьс€ квадрати
    /// </summary>
    public sealed class UpsilonMaze : GridMaze2D
    {
        public const float SquareSize = 0.75f;
        public const float OctagonWidth = SquareSize * 3f;

        private readonly int _reminder;

        private float GetSize(int i, int r)
        {
            int octagons, squares;
            float delta = r % 2 != _reminder ? SquareSize : 0f;

            if (i == 0)
            {
                octagons = squares = 0;
            }
            else if (r % 2 == _reminder)
            {
                squares = i / 2;
                octagons = i - squares;
            }
            else
            {
                octagons = i / 2;
                squares = i - octagons;
            }

            return octagons * OctagonWidth + squares * SquareSize + delta;
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            if ((row + col) % 2 == _reminder)
            {
                return new MazeCell[8]
                    {
                    GetCell(row, col + 1, cell, 1),
                    GetCell(row, col - 1, cell, 0),
                    GetCell(row + 1, col, cell, 3),
                    GetCell(row - 1, col, cell, 2),
                    GetCell(row + 1, col + 1, cell, 5),
                    GetCell(row - 1, col - 1, cell, 4),
                    GetCell(row + 1, col - 1, cell, 7),
                    GetCell(row - 1, col + 1, cell, 6)
                    };
            }
            return new MazeCell[4]
                    {
                    GetCell(row, col + 1, cell, 1),
                    GetCell(row, col - 1, cell, 0),
                    GetCell(row + 1, col, cell, 3),
                    GetCell(row - 1, col, cell, 2)
                    };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            var point = CellsCoordinates[wall.Cell1.Index];

            if ((point.Column + point.Row) % 2 == _reminder)
            {
                return wall.Direction switch
                {
                    0 => (3, 4),
                    1 => (7, 0),
                    2 => (5, 6),
                    3 => (1, 2),
                    4 => (4, 5),
                    5 => (0, 1),
                    6 => (6, 7),
                    7 => (2, 3),
                    _ => throw new InvalidWallDirectionException()
                };
            }

            return wall.Direction switch
            {
                0 => (1, 2),
                1 => (3, 0),
                2 => (2, 3),
                3 => (0, 1),
                _ => throw new InvalidWallDirectionException()
            };
        }

        public UpsilonMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) :
            base(width, height, p)
        {
            _reminder = reminder;
            InitGraph();
            Description = $"Custom Upsilon maze {Columns}x{Rows}";
        }
        public UpsilonMaze(int width, int height, int inWidth = 0, int inHeight = 0, int reminder = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            Description = $"Rectangular Upsilon maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }

        //public override float[] Dimensions => new float[2]
        //{
        //    GetSize(Columns, (Columns + _reminder + 1) % 2),
        //    GetSize(Rows, (Rows + _reminder + 1) % 2),
        //};

        public override int GetCellPointsNumber(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];

            return (point.Row + point.Column) % 2 == _reminder ? 8 : 4;
        }

        public static float[] GetOctagonPoint(int pointIndex, float left = 0f, float top = 0f)
        {
            return pointIndex switch
            {
                0 => new float[2] { left, top + SquareSize },
                1 => new float[2] { left + SquareSize, top },
                2 => new float[2] { left + 2f * SquareSize, top },
                3 => new float[2] { left + OctagonWidth, top + SquareSize },
                4 => new float[2] { left + OctagonWidth, top + 2f * SquareSize },
                5 => new float[2] { left + 2f * SquareSize, top + OctagonWidth },
                6 => new float[2] { left + SquareSize, top + OctagonWidth },
                7 => new float[2] { left, top + 2f * SquareSize },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            float left = GetSize(point.Column, point.Row);
            float top = GetSize(point.Row, point.Column);

            if ((point.Row + point.Column) % 2 == _reminder)
            {
                return GetOctagonPoint(pointIndex, left, top);
            }

            return pointIndex switch
            {
                0 => new float[2] { left, top },
                1 => new float[2] { left + SquareSize, top },
                2 => new float[2] { left + SquareSize, top + SquareSize },
                3 => new float[2] { left, top + SquareSize },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }
    }
}