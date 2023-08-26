using System;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional Maze that consists of Triangles.
    /// </summary>
    public sealed class DeltaMaze : GridMaze2D
    {
        // Const that's equal to sqrt(3) / 2
        private const float WidthToHeight = 0.86602540378443864676372317075294f;

        // If 0 then the first triangle on the grid will be poining up
        private readonly int _reminder = 0;

        protected override MazeCell?[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            MazeCell? down = null, up = null;
            // Up-pointing triangle
            if ((row + col) % 2 == _reminder)
            {
                down = GetCell(row + 1, col, cell, 3);
            }
            // Down-pointing triangle
            else
            {
                up = GetCell(row - 1, col, cell, 2);
            }

            return new MazeCell?[4]
            {
                GetCell(row, col + 1, cell, 1), // right
                GetCell(row, col - 1, cell, 0), // left
                down, up
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            var point = CellsCoordinates[wall.Cell1.Index];

            // Up-pointing triangle
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
            // Down-pointing triangle
            return wall.Direction switch
            {
                0 => (1, 2),
                1 => (2, 0),
                3 => (0, 1),
                _ => throw new InvalidWallDirectionException()
            };
        }

        /// <summary>
        /// Create a custom delta maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="p">Predicate that's used to determine whether we should include the cell.</param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first triangle on the grid will be pointing up,
        /// otherwise it will be pointing down.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public DeltaMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) : base(width, height, p)
        {
            if (reminder != 0 && reminder != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(reminder));
            }

            _reminder = reminder;
            InitGraph();
            Description = $"Custom Delta maze {Columns}x{Rows}";
        }

        /// <summary>
        /// Create a delta maze with inner room.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="inWidth">
        /// Number of the rows of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="width"/> - 2</c>
        /// </param>
        /// <param name="inHeight">
        /// Number of the columns of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="height"/> - 2</c>
        /// </param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first triangle on the grid will be pointing up,
        /// otherwise it will be pointing down.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public DeltaMaze(int width, int height, int inWidth, int inHeight, int reminder = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            Description = $"Rectangular Delta maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }

        /// <summary>
        /// Create a triangular delta maze with triangular inner room.
        /// </summary>
        /// <param name="sideLength">Side length of the triangle. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner triangle. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 3</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public DeltaMaze(int sideLength, int inSideLength = 0) :
            this(2 * sideLength - 1, sideLength, TriangularPattern(sideLength, inSideLength), (sideLength + 1) % 2)
        {
            Description = $"Triangular maze(size {sideLength})";
            if (inSideLength > 0)
            {
                Description += $" with inner triangular room(size {inSideLength})";
            }
        }

        /// <summary>
        /// Create a hexagonal delta maze with hexagonal inner room.
        /// </summary>
        /// <param name="sideLength">Side length of the hexagon. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner hexagon. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 1</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
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

        public override int GetCellPointsNumber(MazeCell cell) => 3;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            // Up-pointing triangle
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
            // Down-pointing triangle
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
            // Optimized way of finding a center of the triangle
            var point = CellsCoordinates[cell.Index];
            int a = (point.Row + point.Column + 1 - _reminder) % 2 + 1;
            int b = (point.Row + point.Column + _reminder) % 2 + 1;
            return new float[2]
            {
                0.5f * point.Column + 0.5f,
                WidthToHeight * ((point.Row + 1f) * a + point.Row * b) / 3f
            };
        }
    }
}
