using System;
using System.Collections.Generic;
using System.Numerics;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional Theta(circular) maze.
    /// </summary>
    /// <remarks>
    /// Rows here are circles and columns are segments.
    /// </remarks>
    public sealed class ThetaMaze : GridMaze2D
    {
        public const int East = 0;
        public const int West = 1;
        public const int North = 2;
        public const int South = 3;

        private const int DefaultSegmentsNumber = 20;

        /// <summary>
        /// Inner radius of maze. 
        /// This value is -1 if maze is custom.
        /// </summary>
        public readonly int InRadius = -1;

        private float[] PolarToCartesian(float circle, float segment)
        {
            float offset = Rows + 1f;
            float angle = (segment / Columns) * 2f * MathF.PI;
            return new float[2]
            {
                MathF.Cos(angle) * circle + offset,
                MathF.Sin(angle) * circle + offset
            };
        }

        /// <summary>
        /// Create a custom Theta(circular) maze.
        /// </summary>
        /// <param name="radius">Radius of the maze(number of circles). Should be greater than 1.</param>
        /// <param name="p">
        /// Predicate that's used to determine whether we should include the cell.
        /// </param>
        /// <param name="segments">Number of cells in one circle.</param>
        public ThetaMaze(int radius, Predicate<GridPoint2D> p, int segments = DefaultSegmentsNumber) :
            base(segments, radius, p)
        {
            InitGraph();
            Description = $"Custom Theta(circular) maze with radius {radius}";
        }

        /// <summary>
        /// Create a Theta(circular) maze with inner circular room.
        /// </summary>
        /// <param name="radius">Radius of the maze(number of circles). Should be greater than 1.</param>
        /// <param name="inRadius">
        /// Radius of the inner circle. Should be positive and not greater than 
        /// <c><paramref name="radius"/> - 1</c>
        /// </param>
        /// <param name="segments">Number of cells in one circle.</param>
        public ThetaMaze(int radius, int inRadius = 0, int segments = DefaultSegmentsNumber) :
            this(radius, Grid2DPatterns.CirclePredicate(radius, inRadius), segments)
        {
            InRadius = inRadius;
            Description = "Theta(circular) maze with ";
            Description += inRadius == 0 ?
                $"radius {radius}" :
                $"base radius {inRadius} and inner radius {inRadius}";
        }

        public override int GetCellPointsNumber(MazeCell cell) => 4;

        public override float[] GetCellPoint(MazeCell cell, int pointIndex)
        {
            var point = CellsCoordinates[cell.Index];

            return pointIndex switch
            {
                0 => PolarToCartesian(point.Row + 2f, point.Column), // Top-left corner
                1 => PolarToCartesian(point.Row + 2f, point.Column + 1f), // Top-right corner
                2 => PolarToCartesian(point.Row + 1f, point.Column + 1f), // Bottom-right corner
                3 => PolarToCartesian(point.Row + 1f, point.Column), // Bottom-left corner
                _ => throw new ArgumentOutOfRangeException(nameof(pointIndex))
            };
        }

        public override float[] GetCellCenter(MazeCell cell)
        {
            // Optimized way of finding a center of the segment
            var point = CellsCoordinates[cell.Index];
            return PolarToCartesian(point.Row + 1.5f, point.Column + 0.5f);
        }

        protected override (int, int) GetWallPointsIndices(MazeEdge wall)
        {
            return wall.Direction switch
            {
                0 => (1, 2), // Right wall
                1 => (3, 0), // Left wall
                2 => (0, 1), // Up wall
                3 => (2, 3), // Bottom wall
                _ => throw new InvalidWallDirectionException()
            };
        }

        protected override MazeCell[] GetDirectedNeighbors(MazeCell cell, int row, int col)
        {
            int right = col + 1;
            if (right >= Columns) right = 0;
            int left = col - 1;
            if (left < 0) left = Columns - 1;

            return new MazeCell[4]
            {
                GetCell(row, right, cell, West), // Right
                GetCell(row, left, cell, East), // Left
                GetCell(row + 1, col, cell, South), // Up
                GetCell(row - 1, col, cell, North)  // Bottom
            };
        }

        public override PathSegment GetWallPosition(MazeEdge wall)
        {
            // Arc wall
            var circlePoint = CellsCoordinates[wall.Cell1.Index];

            int point1, point2;
            (point1, point2) = GetWallPointsIndices(wall);
            float[] p1 = GetCellPoint(wall.Cell1, point1);
            float[] p2 = GetCellPoint(wall.Cell1, point2);

            switch (wall.Direction)
            {
                case East:
                case West:
                    return new Line(p1[0], p1[1], p2[0], p2[1]);
                case North:
                    return new Arc(p1[0], p1[1], p2[0], p2[1], circlePoint.Row + 2f, true);
                case South:
                    return new Arc(p1[0], p1[1], p2[0], p2[1], circlePoint.Row + 1f, false);

                default:
                    throw new InvalidWallDirectionException();
            }
        }

        protected override PathSegment GetPath(MazeEdge relation)
        {
            int direction = relation.Direction;
            // Basic line
            if (direction != East && direction != West)
            {
                return base.GetPath(relation);
            }

            // Arc
            Vector2 from = GetCellCenter2D(relation.Cell1);
            Vector2 to = GetCellCenter2D(relation.Cell2);

            var point1 = CellsCoordinates[relation.Cell1.Index];
            var point2 = CellsCoordinates[relation.Cell2.Index];

            bool clockwise;
            // Edge cases
            if (point1.Column == 0) clockwise = point2.Column == 1;
            else if (point2.Column == 0) clockwise = point1.Column != 1;
            else clockwise = point1.Column < point2.Column;

            return new Arc(from, to, point2.Row + 0.5f, clockwise);
        }

        public override IEnumerable<PathSegment> GetCellPath(MazeCell cell)
        {
            var point = CellsCoordinates[cell.Index];

            float[] p0 = GetCellPoint(cell, 0);
            float[] p1 = GetCellPoint(cell, 1);
            float[] p2 = GetCellPoint(cell, 2);
            float[] p3 = GetCellPoint(cell, 3);

            yield return new Arc(p0[0], p0[1], p1[0], p1[1], point.Row + 2f, true);
            yield return new Line(p1[0], p1[1], p2[0], p2[1]);
            yield return new Arc(p2[0], p2[1], p3[0], p3[1], point.Row + 2f, false);
            yield return new ClosePath(p3[0], p3[1], p0[0], p0[1]);
        }
    }
}
