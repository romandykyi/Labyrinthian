using System.Numerics;

namespace Labyrinthian
{
    public sealed class Line : PathSegment
    {
        private readonly Vector2 _start, _end;

        public override Vector2 StartPoint => _start;
        public override Vector2 EndPoint => _end;

        public Line(Vector2 start, Vector2 end)
        {
            _start = start;
            _end = end;
        }

        public Line(float x1, float y1, float x2, float y2) :
            this(new Vector2(x1, y1), new Vector2(x2, y2))
        { }

        public override string MoveToEndSvg(float cellSize, float offset)
        {
            Vector2 end = TransformPoint(EndPoint, cellSize, offset);
            if (_start.X.ApproximatelyEquals(_end.X))
            {
                return $"V{end.Y.ToInvariantString()} ";
            }
            if (_start.Y.ApproximatelyEquals(_end.Y))
            {
                return $"H{end.X.ToInvariantString()} ";
            }
            return $"L{end.X.ToInvariantString()},{end.Y.ToInvariantString()} ";
        }
    }
}
