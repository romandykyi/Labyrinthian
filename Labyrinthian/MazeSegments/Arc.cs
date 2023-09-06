using System.Numerics;

namespace Labyrinthian
{
    public sealed class Arc : PathSegment
    {
        private readonly float _rX, _rY;
        private readonly Vector2 _start, _end;

        public override Vector2 StartPoint => _start;
        public override Vector2 EndPoint => _end;

        public float Rx => _rX;
        public float Ry => _rY;

        public Arc(Vector2 start, Vector2 end, float rX, float rY)
        {
            _start = start;
            _end = end;
            _rX = rX;
            _rY = rY;
        }

        public Arc(Vector2 start, Vector2 end, float r) : this(start, end, r, r)
        { }

        public Arc(float x1, float y1, float x2, float y2, float rX, float rY) :
            this(new Vector2(x1, y1), new Vector2(x2, y2), rX, rY)
        { }

        public Arc(float x1, float y1, float x2, float y2, float r) :
            this(x1, y1, x2, y2, r, r)
        { }

        public override string MoveToEndSvg(float cellSize, float offset)
        {
            float rX = TransformSize(_rX, cellSize);
            float rY = TransformSize(_rY, cellSize);
            Vector2 end = TransformPoint(EndPoint, cellSize, offset);

            return $"A{rX.ToInvariantString()},{rY.ToInvariantString()} 0 0 1 {end.X.ToInvariantString()} {end.Y.ToInvariantString()} ";
        }
    }
}
