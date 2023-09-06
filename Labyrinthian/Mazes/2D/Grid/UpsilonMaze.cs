using System;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional maze that consists of both octagonal and square cells.
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

        /// <summary>
        /// Create a custom upsilon maze.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="p">Predicate that's used to determine whether we should include the cell.</param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first cell will be an octagon,
        /// otherwise it will be a square.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public UpsilonMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) :
            base(width, height, p)
        {
            _reminder = reminder;
            InitGraph();
            Description = $"Custom Upsilon maze {Columns}x{Rows}";
        }

        /// <summary>
        /// Create an upsilon maze with inner room.
        /// </summary>
        /// <param name="width">Number of columns. Should be greater than 1.</param>
        /// <param name="height">Number of rows. Should be greater than 1.</param>
        /// <param name="inWidth">
        /// Number of the rows of the inner room. Should be positive and not greater than
        /// <c><paramref name="width"/> - 2</c>
        /// </param>
        /// <param name="inHeight">
        /// Number of the columns of the inner room. Should be positive and not greater than
        /// <c><paramref name="height"/> - 2</c>
        /// </param>
        /// <param name="reminder">
        /// 0 or 1 value.
        /// If 0 then first cell will be an octagon,
        /// otherwise it will be a square.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public UpsilonMaze(int width, int height, int inWidth = 0, int inHeight = 0, int reminder = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            if (reminder != 0 && reminder != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(reminder));
            }

            Description = $"Rectangular Upsilon maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }

        public override int GetCellPointsNumber(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];

            return (point.Row + point.Column) % 2 == _reminder ? 8 : 4;
        }

        /// <summary>
        /// Get a point on the octagon.
        /// </summary>
        /// <param name="pointIndex">Index of a point.</param>
        /// <param name="left">Left offset.</param>
        /// <param name="top">Top offset.</param>
        /// <returns>Float array with 2 elements - x and y coordinates of the octagon point.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

            // Octagon
            if ((point.Row + point.Column) % 2 == _reminder)
            {
                return GetOctagonPoint(pointIndex, left, top);
            }

            // Square
            return pointIndex switch
            {
                0 => new float[2] { left, top },
                1 => new float[2] { left + SquareSize, top },
                2 => new float[2] { left + SquareSize, top + SquareSize },
                3 => new float[2] { left, top + SquareSize },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            var point = CellsCoordinates[wall.Cell1.Index];

            if ((point.Column + point.Row) % 2 == _reminder)
            {
                // Octagon
                return wall.Direction switch
                {
                    // Right wall
                    0 => (3, 4),
                    // Left wall
                    1 => (7, 0),
                    // Bottom wall
                    2 => (5, 6),
                    // Top wall
                    3 => (1, 2),
                    // Bottom-right wall
                    4 => (4, 5),
                    // Top-left wall
                    5 => (0, 1),
                    // Botom-left wall
                    6 => (6, 7),
                    // Top-right wall
                    7 => (2, 3),

                    _ => throw new InvalidWallDirectionException()
                };
            }

            // Square
            return wall.Direction switch
            {
                // Right wall
                0 => (1, 2),
                // Left wall
                1 => (3, 0),
                // Bottom wall
                2 => (2, 3),
                // Top wall
                3 => (0, 1),

                _ => throw new InvalidWallDirectionException()
            };
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            if ((row + col) % 2 == _reminder)
            {
                // Octagon
                return new MazeCell[8]
                    {
                    // Right neighbor
                    GetCell(row, col + 1, cell, 1),
                    // Left neighbor
                    GetCell(row, col - 1, cell, 0),
                    // Bottom neighbor
                    GetCell(row + 1, col, cell, 3),
                    // Top neighbor
                    GetCell(row - 1, col, cell, 2),
                    // Bottom-right neighbor
                    GetCell(row + 1, col + 1, cell, 5),
                    // Top-left neighbor
                    GetCell(row - 1, col - 1, cell, 4),
                    // Botom-left neighbor
                    GetCell(row + 1, col - 1, cell, 7),
                    // Top-right neighbor
                    GetCell(row - 1, col + 1, cell, 6)
                    };
            }
            // Square
            return new MazeCell[4]
                    {
                    // Left neighbor
                    GetCell(row, col + 1, cell, 1),
                    // Right neighbor
                    GetCell(row, col - 1, cell, 0),
                    // Bottom neighbor
                    GetCell(row + 1, col, cell, 3),
                    // Top neighbor
                    GetCell(row - 1, col, cell, 2)
                    };
        }
    }
}
