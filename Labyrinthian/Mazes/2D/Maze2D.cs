using System.Numerics;

namespace Labyrinthian
{
    /// <summary>
    /// 2-dimensional maze base class
    /// </summary>
    public abstract class Maze2D : Maze
    {
        public sealed override int Dimensions => 2;

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
            Vector2 from = PositionTo2DPoint(GetCellCenter(relation.Cell1));
            Vector2 to = PositionTo2DPoint(GetCellCenter(relation.Cell2));

            return new Line(from, to);
        }

        public override Vector2 PositionTo2DPoint(float[] position) => new Vector2(position[0], position[1]);
    }
}
