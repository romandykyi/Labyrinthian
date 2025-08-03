using System;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional Maze that consists of Hexagons.
    /// </summary>
    public sealed class SigmaMaze : GridMaze2D
    {
        public const int South = 0;
        public const int North = 1;
        public const int SouthWest = 2;
        public const int NorthEast = 3;
        public const int SouthEast = 4;
        public const int NorthWest = 5;

        private const float WidthToHeight = 0.86602540378443864676372317075294f;

        private readonly int _reminder = 0;

        /// <summary>
        /// Create a custom sigma maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="p">Predicate that's used to determine whether we should include the cell.</param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first hexagon on the grid will be a bit lower.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public SigmaMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) : base(width, height, p)
        {
            if (reminder != 0 && reminder != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(reminder));
            }

            _reminder = reminder;
            InitGraph();
            Description = $"Custom Sigma maze {Columns}x{Rows}";
        }

        /// <summary>
        /// Create a custom sigma maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="inWidth">
        /// Number of the rows of the inner hexagon. Should be positive and not greater than
        /// <c><paramref name="width"/> - 2</c>
        /// </param>
        /// <param name="inHeight">
        /// Number of the columns of the inner hexagon. Should be positive and not greater than
        /// <c><paramref name="height"/> - 2</c>
        /// </param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first hexagon on the grid will be a bit lower.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public SigmaMaze(int width, int height, int inWidth, int inHeight, int reminder = 0) :
            this(width, height,
                Grid2DPatterns.RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            Description = $"Rectangular Sigma maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }

        /// <summary>
        /// Create a hexagonal sigma maze with hexagonal inner room.
        /// </summary>
        /// <param name="sideLength">Side length of the hexagon. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner hexagon. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 1</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public SigmaMaze(int sideLength, int inSideLength = 0) :
            this(2 * sideLength - 1, 2 * sideLength - 1,
                Grid2DPatterns.HexagonalPattern(sideLength, inSideLength), sideLength % 2)
        {
            Description = $"Hexagonal Sigma maze(size {sideLength})";
            if (inSideLength > 0)
            {
                Description += $" with inner hexagonal room(size {inSideLength})";
            }
        }

        public override int GetCellPointsNumber(MazeCell cell) => 6;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            float x = point.Column * 0.75f;
            float yOffset = point.Column % 2 == _reminder ? 0.5f : 0f;

            return pointIndex switch
            {
                // Top-left point
                0 => new float[2] { x + 0.25f, (point.Row + yOffset) * WidthToHeight },
                // Top-right point
                1 => new float[2] { x + 0.75f, (point.Row + yOffset) * WidthToHeight },
                // Middle-right point
                2 => new float[2] { x + 1f, (point.Row + yOffset + 0.5f) * WidthToHeight },
                // Bottom-right point
                3 => new float[2] { x + 0.75f, (point.Row + yOffset + 1f) * WidthToHeight },
                // Bottom-left point
                4 => new float[2] { x + 0.25f, (point.Row + yOffset + 1f) * WidthToHeight },
                // Middle-left point
                5 => new float[2] { x, (point.Row + yOffset + 0.5f) * WidthToHeight },

                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            // Optimized way of finding a center of the hexagon
            var point = CellsCoordinates[cell.Index];
            float yOffset = point.Column % 2 == _reminder ? 1f : 0.5f;
            return new float[2]
            {
                point.Column * 0.75f + 0.5f,
                (point.Row + yOffset) * WidthToHeight
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            return wall.Direction switch
            {
                // Bottom wall
                0 => (3, 4),
                // Top wall
                1 => (0, 1),
                // Bottom-left wall
                2 => (4, 5),
                // Top-right wall
                3 => (1, 2),
                // Bottom-right wall
                4 => (2, 3),
                // Top-left wall
                5 => (5, 0),
                _ => throw new InvalidWallDirectionException()
            };
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            int d = (col + _reminder) % 2;
            return new MazeCell[6]
            {
                // Bottom neighbor
                GetCell(row + 1, col, cell, North),
                // Top neighbor
                GetCell(row - 1, col, cell, South),
                // Bottom-left neighbor
                GetCell(row - d + 1, col - 1, cell, NorthEast),
                // Top-right neighbor
                GetCell(row - d, col + 1, cell, SouthWest),
                // Bottom-right neighbor
                GetCell(row - d + 1, col + 1, cell, NorthWest),
                // Top-left neighbor
                GetCell(row - d, col - 1, cell, SouthEast)
            };
        }
    }
}
