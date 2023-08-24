using System;

namespace Labyrinthian
{
    /// <summary>
    /// Двовимірний лабіринт, представлений сіткою з квадратів
    /// </summary>
    public sealed class OrthogonalMaze : GridMaze2D
    {
        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
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
            return wall.Direction switch
            {
                0 => (1, 2),
                1 => (3, 0),
                2 => (2, 3),
                3 => (0, 1),
                _ => throw new InvalidWallDirectionException()
            };
        }

        public OrthogonalMaze(int width, int height, Predicate<GridPoint2D> p) : base(width, height, p)
        {
            InitGraph();
            Description = $"Custom Orthogonal maze {Columns}x{Rows}";
        }
        public OrthogonalMaze(int width, int height, int inWidth = 0, int inHeight = 0) :
            this(width, height, RectangularPattern(width, height, inWidth, inHeight))
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
                0 => new float[2] { point.Column, point.Row },
                1 => new float[2] { point.Column + 1f, point.Row },
                2 => new float[2] { point.Column + 1f, point.Row + 1f },
                3 => new float[2] { point.Column, point.Row + 1f },
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];
            return new float[2]
            {
                point.Column + 0.5f,
                point.Row + 0.5f
            };
        }
    }
}