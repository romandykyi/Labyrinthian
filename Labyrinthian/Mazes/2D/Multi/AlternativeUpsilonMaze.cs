using System;
using static Labyrinthian.UpsilonMaze;

namespace Labyrinthian
{
    /// <summary>
    /// <para>
    /// Maze that consists of both octagonal and square cells. 
    /// 0-grid is used for octagons and 1-grid is used for squares.
    /// </para>
    /// <para>
    /// It's similiar to <see cref="UpsilonMaze"/>, but square cells are placed
    /// diagonally. Also, width and height here include only octagonal cells.
    /// </para>
    /// </summary>
    public class AlternativeUpsilonMaze : MultiGridMaze2D
    {
        public override int GridsNumber => 2;

        public AlternativeUpsilonMaze(int baseWidth, int baseHeight) : this(baseWidth, baseHeight, _ => true)
        {
            Description = $"Alternative Upsilon maze {baseWidth}x{baseHeight}";
        }

        public AlternativeUpsilonMaze(int baseWidth, int baseHeight, Predicate<MultiGridPoint2D> predicate) :
            base(baseWidth, baseHeight, predicate)
        {
            InitGraph();
            Description = $"Custom Alternative Upsilon maze {baseWidth}x{baseHeight}";
        }

        protected override void GetGridSizes(int grid, out int rows, out int columns)
        {
            switch (grid)
            {
                // Octagons grid
                case 0:
                    rows = BaseRows;
                    columns = BaseColumns;
                    break;
                // Squares grid
                case 1:
                    rows = BaseRows - 1;
                    columns = BaseColumns - 1;
                    break;
                default:
                    throw new InvalidGridIndexException();
            }
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int grid, int row, int column)
        {
            return grid switch
            {
                // Octagon neighbors
                0 => new MazeCell[8]
                    {
                    GetCell(1, row, column, cell, 1),
                    GetCell(1, row - 1, column - 1, cell, 0),
                    GetCell(1, row, column - 1, cell, 3),
                    GetCell(1, row - 1, column, cell, 2),
                    GetCell(0, row, column + 1, cell, 5),
                    GetCell(0, row, column - 1, cell, 4),
                    GetCell(0, row + 1, column, cell, 7),
                    GetCell(0, row - 1, column, cell, 6),
                    },
                // Square neighbors
                1 => new MazeCell[4]
                {
                    GetCell(0, row + 1, column + 1, cell, 1),
                    GetCell(0, row, column, cell, 0),
                    GetCell(0, row + 1, column, cell, 3),
                    GetCell(0, row , column + 1, cell, 2)
                },
                _ => throw new InvalidGridIndexException()
            };
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            var point = CellsCoordinates[wall.Cell1.Index];
            return point.Grid switch
            {
                // Octagon walls
                0 => wall.Direction switch
                {
                    0 => (4, 5),
                    1 => (0, 1),
                    2 => (6, 7),
                    3 => (2, 3),
                    4 => (3, 4),
                    5 => (7, 0),
                    6 => (5, 6),
                    7 => (1, 2),
                    _ => throw new InvalidWallDirectionException()
                },
                // Square walls
                1 => wall.Direction switch
                {
                    0 => (1, 2),
                    1 => (3, 0),
                    2 => (2, 3),
                    3 => (0, 1),
                    _ => throw new InvalidWallDirectionException()
                },
                _ => throw new InvalidGridIndexException(),
            };
        }

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];
            switch (point.Grid)
            {
                // Octagon point
                case 0:
                    return GetOctagonPoint(pointIndex, point.Column * OctagonWidth, point.Row * OctagonWidth);
                // Square point
                case 1:
                    float left = OctagonWidth * (point.Column + 1);
                    float top = OctagonWidth * (point.Row + 1);
                    return pointIndex switch
                    {
                        0 => new float[2] { left, top - SquareSize },
                        1 => new float[2] { left + SquareSize, top },
                        2 => new float[2] { left, top + SquareSize },
                        3 => new float[2] { left - SquareSize, top },
                        _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
                    };
                default:
                    throw new InvalidGridIndexException();
            }
        }

        public override int GetCellPointsNumber(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];
            return point.Grid switch
            {
                // Octagon
                0 => 8,
                // Square
                1 => 4,
                _ => throw new InvalidGridIndexException()
            };
        }
    }
}
