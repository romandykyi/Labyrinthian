using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Labyrinthian
{
    /// <summary>
    /// Двовимірний лабіринт
    /// </summary>
    public abstract class Maze2D : Maze
    {
        public override int Dimensions => 2;

        /// <summary>
        /// Отримати номери точок стіни
        /// </summary>
        protected abstract (int, int) GetWallPointsIndices(MazeEdge wall);

        /// <summary>
        /// Створити лінійну стіну на основі двох точок
        /// </summary>
        protected Line MakeLineWall(MazeEdge wall, int point1, int point2)
        {
            float[] p1 = GetCellPoint(wall.Cell0, point1);
            float[] p2 = GetCellPoint(wall.Cell0, point2);

            return new Line(p1[0], p1[1], p2[0], p2[1]);
        }

        // Конвертація клітинки в полігон
        public override string CellToSvg(MazeCell cell, float cellSize, float offset)
        {
            StringBuilder line = new StringBuilder();
            line.Append("<polygon points=\"");
            int n = GetCellPointsNumber(cell);
            for (int i = 0; i < n; ++i)
            {
                float[] point = GetCellPoint(cell, i);
                point[0] = point[0] * cellSize + offset;
                point[1] = point[1] * cellSize + offset;
                string x = point[0].ToString(CultureInfo.InvariantCulture);
                string y = point[1].ToString(CultureInfo.InvariantCulture);
                line.Append($"{x},{y}");
                if (i < n - 1) line.Append(" ");
            }
            line.Append("\"/>");

            return line.ToString();
        }

        public override PathSegment GetWallPosition(MazeEdge wall)
        {
            int point1, point2;
            (point1, point2) = GetWallPointsIndices(wall);
            return MakeLineWall(wall, point1, point2);
        }

        protected override PathSegment GetPath(MazeEdge relation)
        {
            Vector2 from = PositionTo2DPoint(GetCellCenter(relation.Cell0));
            Vector2 to = PositionTo2DPoint(GetCellCenter(relation.Cell1));

            return new Line(from, to);
        }

        public override Vector2 PositionTo2DPoint(float[] position) => new Vector2(position[0], position[1]);
    }
}