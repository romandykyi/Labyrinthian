using System.Collections.Generic;
using System.Numerics;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional maze base class.
    /// </summary>
    public abstract class Maze2D : Maze
    {
        public sealed override int Dimensions => 2;
        public sealed override float Width2D => Sizes[0];
        public sealed override float Height2D => Sizes[1];

        /// <summary>
        /// Get indices of points of the first node in the edge '<paramref name="wall"/>'
        /// that will be used for drawing a wall.
        /// </summary>
        protected abstract (int, int) GetWallPointsIndices(MazeEdge wall);

        /// <summary>
        /// Make a line wall based on two points of the first node of the edge '<paramref name="wall"/>'.
        /// </summary>
        /// <param name="wall">Wall that needs to be represented as a line.</param>
        /// <param name="point1">First point index</param>
        /// <param name="point2">Second point index</param>
        protected Line MakeLineWall(MazeEdge wall, int point1, int point2)
        {
            float[] p1 = GetCellPoint(wall.Cell1, point1);
            float[] p2 = GetCellPoint(wall.Cell1, point2);

            return new Line(p1[0], p1[1], p2[0], p2[1]);
        }

        public override PathSegment GetWallPosition(MazeEdge wall)
        {
            int point1, point2;
            (point1, point2) = GetWallPointsIndices(wall);
            return MakeLineWall(wall, point1, point2);
        }

        protected override PathSegment GetPath(MazeEdge relation)
        {
            Vector2 from = GetCellCenter2D(relation.Cell1);
            Vector2 to = GetCellCenter2D(relation.Cell2);

            return new Line(from, to);
        }

        /// <summary>
        /// Get path representation of the cell. 
        /// If not overriden from Maze2D, it's all cell points joined together.
        /// </summary>
        /// <param name="cell">Cell whose path representation we need to get.</param>
        /// <returns>
        /// Paths that combined together form a cell shape.
        /// </returns>
        public override IEnumerable<PathSegment> GetCellPath(MazeCell cell)
        {
            int n = GetCellPointsNumber(cell);
            float[] firstPoint, previousPoint;
            firstPoint = previousPoint = GetCellPoint(cell, 0);
            for (int i = 1; i < n; i++)
            {
                float[] currentPoint = GetCellPoint(cell, i);
                yield return new Line(
                    previousPoint[0], previousPoint[1],
                    currentPoint[0], currentPoint[1]);

                previousPoint = currentPoint;
            }

            yield return new Line(
                previousPoint[0], previousPoint[1],
                firstPoint[0], firstPoint[1]);
        }

        public sealed override Vector2 GetCellCenter2D(MazeCell cell)
        {
            float[] point = GetCellCenter(cell);
            return new Vector2(point[0], point[1]);
        }
    }
}
