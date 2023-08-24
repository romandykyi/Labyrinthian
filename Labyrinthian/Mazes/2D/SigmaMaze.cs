using System;

namespace Labyrinthian
{
    /// <summary>
    /// Двовимірний лабіринт, представлений сіткою з правильних шестикутників
    /// </summary>
    public sealed class SigmaMaze : GridMaze2D
    {
        private readonly int _reminder = 0;
        private static readonly float WidthToHeight = MathF.Sqrt(3f) / 2f;

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            int d = (col + _reminder) % 2;
            return new MazeCell[6]
            {
                GetCell(row + 1, col, cell, 1),
                GetCell(row - 1, col, cell, 0),
                GetCell(row - d + 1, col - 1, cell, 3),
                GetCell(row - d, col + 1, cell, 2),
                GetCell(row - d + 1, col + 1, cell, 5),
                GetCell(row - d, col - 1, cell, 4)
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            return wall.Direction switch
            {
                0 => (3, 4),
                1 => (0, 1),
                2 => (4, 5),
                3 => (1, 2),
                4 => (2, 3),
                5 => (5, 0),
                _ => throw new InvalidWallDirectionException()
            };
        }

        public SigmaMaze(int width, int height, Predicate<GridPoint2D> p, int reminder = 0) : base(width, height, p)
        {
            _reminder = reminder;
            InitGraph();
            Description = $"Custom Sigma maze {Columns}x{Rows}";
        }
        public SigmaMaze(int width, int height, int inWidth, int inHeight, int reminder = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight), reminder)
        {
            Description = $"Rectangular Sigma maze {Columns}x{Rows}";
            if (inWidth * inHeight > 0)
            {
                Description += $" with inner rectangular room {inWidth}x{inHeight}";
            }
        }
        public SigmaMaze(int sideLength, int inSideLength = 0) :
            this(2 * sideLength - 1, 2 * sideLength - 1, HexagonalPattern(sideLength, inSideLength), sideLength % 2)
        {
            Description = $"Hexagonal Sigma maze(size {sideLength})";
            if (inSideLength > 0)
            {
                Description += $" with inner hexagonal room(size {inSideLength})";
            }
        }

        //public override float[] Dimensions => new float[2]
        //{
        //    Columns * 0.75f + 0.25f,
        //    Rows * WidthToHeight + WidthToHeight / 2f
        //};

        public override int GetCellPointsNumber(MazeCell cell) => 6;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            float x = point.Column * 0.75f;
            float yOffset = point.Column % 2 == _reminder ? 0.5f : 0f;

            return pointIndex switch
            {
                0 => new float[2] { x + 0.25f, (point.Row + yOffset) * WidthToHeight },
                1 => new float[2] { x + 0.75f, (point.Row + yOffset) * WidthToHeight },
                2 => new float[2] { x + 1f, (point.Row + yOffset + 0.5f) * WidthToHeight },
                3 => new float[2] { x + 0.75f, (point.Row + yOffset + 1f) * WidthToHeight },
                4 => new float[2] { x + 0.25f, (point.Row + yOffset + 1f) * WidthToHeight },
                5 => new float[2] { x, (point.Row + yOffset + 0.5f) * WidthToHeight },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];
            float yOffset = point.Column % 2 == _reminder ? 1f : 0.5f;
            return new float[2]
            {
                point.Column * 0.75f + 0.5f,
                (point.Row + yOffset) * WidthToHeight
            };
        }
    }
}