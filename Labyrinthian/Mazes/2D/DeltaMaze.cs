using System;

namespace Labyrinthian
{
    /// <summary>
    /// Двовимірний лабіринт, представлений сіткою з правильних трикутників
    /// </summary>
    public sealed class DeltaMaze : GridMaze2D
    {
        private static readonly float WidthToHeight = MathF.Sqrt(3f) / 2f;

        private readonly int _reminder = 0;

        protected override MazeCell?[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            MazeCell? bottom = null, up = null;
            if ((row + col) % 2 == _reminder)
            {
                bottom = GetCell(row + 1, col, cell, 3);
            }
            else
            {
                up = GetCell(row - 1, col, cell, 2);
            }

            return new MazeCell?[4]
            {
                GetCell(row, col + 1, cell, 1),
                GetCell(row, col - 1, cell, 0),
                bottom, up
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            var point = CellsCoordinates[wall.Cell1.Index];

            if ((point.Column + point.Row) % 2 == _reminder)
            {
                return wall.Direction switch
                {
                    0 => (0, 1),
                    1 => (2, 0),
                    2 => (1, 2),
                    _ => throw new InvalidWallDirectionException()
                };
            }
            return wall.Direction switch
            {
                0 => (1, 2),
                1 => (2, 0),
                3 => (0, 1),
                _ => throw new InvalidWallDirectionException()
            };
        }

        public DeltaMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) : base(width, height, p)
        {
            _reminder = reminder;
            InitGraph();
            Description = $"Custom Delta maze {Columns}x{Rows}";
        }
        public DeltaMaze(int width, int height, int inWidth, int inHeight, int reminder = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            Description = $"Rectangular Delta maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }
        public DeltaMaze(int sideLength, int inSideLength = 0) :
            this(2 * sideLength - 1, sideLength, TriangularPattern(sideLength, inSideLength), (sideLength + 1) % 2)
        {
            Description = $"Triangular maze(size {sideLength})";
            if (inSideLength > 0)
            {
                Description += $" with inner triangular room(size {inSideLength})";
            }
        }

        public static DeltaMaze CreateHexagonal(int sideLength, int inSideLength = 0)
        {
            int size = 2 * sideLength - 1;

            DeltaMaze maze = new DeltaMaze(3 * size, 2 * size,
                DeltaHexagonalPattern(sideLength, inSideLength),
                (sideLength + 1) % 2)
            {
                Description = $"Hexagonal Delta maze(size {sideLength})"
            };
            if (inSideLength > 0)
            {
                maze.Description += $" with inner hexagonal room(size {inSideLength})";
            }

            return maze;
        }

        //public override float[] Dimensions => new float[2]
        //{
        //    (Columns + 1) / 2f,
        //    Rows * WidthToHeight
        //};

        public override int GetCellPointsNumber(MazeCell cell) => 3;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            if ((point.Row + point.Column) % 2 == _reminder)
            {
                return pointIndex switch
                {
                    0 => new float[2] { point.Column / 2f + 0.5f, point.Row * WidthToHeight },
                    1 => new float[2] { point.Column / 2f + 1f, (point.Row + 1f) * WidthToHeight },
                    2 => new float[2] { point.Column / 2f, (point.Row + 1f) * WidthToHeight },
                    _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
                };
            }
            return pointIndex switch
            {
                0 => new float[2] { point.Column / 2f, point.Row * WidthToHeight },
                1 => new float[2] { point.Column / 2f + 1f, point.Row * WidthToHeight },
                2 => new float[2] { point.Column / 2f + 0.5f, (point.Row + 1f) * WidthToHeight },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];
            int a = (point.Row + point.Column + 1 - _reminder) % 2 + 1;
            int b = (point.Row + point.Column + _reminder) % 2 + 1;
            return new float[2]
            {
                0.5f * point.Column + 0.5f,
                WidthToHeight * ((point.Row + 1f) * a + point.Row * b) / 3f
            };
            //return new float[2]
            //{
            //    0.5f * point.Column + 0.5f,
            //    WidthToHeight * (point.Row + 0.5f),
            //};
        }
    }
}