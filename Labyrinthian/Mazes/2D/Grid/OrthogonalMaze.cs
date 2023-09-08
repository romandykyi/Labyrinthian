using System;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional Maze that consists of Squares.
    /// </summary>
    public sealed class OrthogonalMaze : GridMaze2D
    {
        public const int East = 0;
        public const int West = 1;
        public const int South = 2;
        public const int North = 3;

        /// <summary>
        /// Create a custom orthogonal maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="p">Predicate that's used to determine whether we should include the cell.</param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public OrthogonalMaze(int width, int height, Predicate<GridPoint2D> p) : base(width, height, p)
        {
            InitGraph();
            Description = $"Custom Orthogonal maze {Columns}x{Rows}";
        }

        /// <summary>
        /// Create an orthogonal maze with inner rectangular room.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="inWidth">
        /// Number of columns of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="width"/> - 2</c>
        /// </param>
        /// <param name="inHeight">
        /// Number of rows of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="height"/> - 2</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public OrthogonalMaze(int width, int height, int inWidth = 0, int inHeight = 0) :
            this(width, height, Grid2DPatterns.RectangularPattern(width, height, inWidth, inHeight))
        {
            Description = $"Orthogonal maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }

        public override int GetCellPointsNumber(MazeCell cell) => 4;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            return pointIndex switch
            {
                0 => new float[2] { point.Column, point.Row }, // Top-left corner
                1 => new float[2] { point.Column + 1f, point.Row }, // Top-right corner
                2 => new float[2] { point.Column + 1f, point.Row + 1f }, // Bottom-right corner
                3 => new float[2] { point.Column, point.Row + 1f }, // Bottom-left corner
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            // Optimized way of finding a center of the square
            var point = CellsCoordinates[cell.Index];
            return new float[2]
            {
                point.Column + 0.5f,
                point.Row + 0.5f
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            return wall.Direction switch
            {
                0 => (1, 2), // Right wall
                1 => (3, 0), // Left wall
                2 => (2, 3), // Down wall
                3 => (0, 1), // Up wall
                _ => throw new InvalidWallDirectionException()
            };
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            return new MazeCell[4]
            {
                GetCell(row, col + 1, cell, West), // Right
                GetCell(row, col - 1, cell, East), // Left
                GetCell(row + 1, col, cell, North), // Down
                GetCell(row - 1, col, cell, South)  // Up
            };
        }
    }
}
