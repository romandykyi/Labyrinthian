using System.Numerics;

namespace Labyrinthian
{
    public abstract class PathSegment
    {
        public abstract Vector2 StartPoint { get; }
        public abstract Vector2 EndPoint { get; }
        public virtual Vector2 Center => Vector2.Lerp(StartPoint, EndPoint, 0.5f);

        protected float TransformSize(float size, float cellSize)
        {
            return size * cellSize;
        }

        protected Vector2 TransformPoint(Vector2 point, float cellSize, float offset)
        {
            return new Vector2(point.X * cellSize + offset, point.Y * cellSize + offset);
        }

        public virtual string MoveToStartSvg(float cellSize, float offset)
        {
            Vector2 point = TransformPoint(StartPoint, cellSize, offset);
            return $"M{point.X.ToInvariantString()},{point.Y.ToInvariantString()} ";
        }

        public abstract string MoveToEndSvg(float cellSize, float offset);

        public string MoveNext(PathSegment? from, float cellSize, float offset)
        {
            string result = "";
            if (from == null || !IsContinuous(from, this))
            {
                result += MoveToStartSvg(cellSize, offset);
            }
            result += MoveToEndSvg(cellSize, offset);
            return result;
        }

        public string FromStartToEnd(float cellSize, float offset)
        {
            string start = MoveToStartSvg(cellSize, offset),
                end = MoveToEndSvg(cellSize, offset);

            return $"{start}{end}";
        }

        public static bool IsContinuous(PathSegment first, PathSegment last)
        {
            Vector2 v1 = first.EndPoint, v2 = last.StartPoint;
            return v1.X.ApproximatelyEquals(v2.X) && v1.Y.ApproximatelyEquals(v2.Y);
        }
    }
}
