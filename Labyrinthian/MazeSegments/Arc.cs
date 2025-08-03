using System.Numerics;

namespace Labyrinthian
{
    public sealed class Arc : PathSegment
    {
        private readonly bool _clockwise = true;
        private readonly float _rX, _rY;
        private readonly Vector2 _start, _end;

        public override Vector2 StartPoint => _start;
        public override Vector2 EndPoint => _end;

        public float Rx => _rX;
        public float Ry => _rY;
        public bool Clockwise => _clockwise;

        public Arc(Vector2 start, Vector2 end, float rX, float rY, bool clockwise = true)
        {
            _start = start;
            _end = end;
            _rX = rX;
            _rY = rY;
            _clockwise = clockwise;
        }

        public Arc(Vector2 start, Vector2 end, float r, bool clockwise = true) :
            this(start, end, r, r, clockwise)
        { }

        public Arc(float x1, float y1, float x2, float y2, float rX, float rY, bool clockwise = true) :
            this(new Vector2(x1, y1), new Vector2(x2, y2), rX, rY, clockwise)
        { }

        public Arc(float x1, float y1, float x2, float y2, float r, bool clockwise = true) :
            this(x1, y1, x2, y2, r, r, clockwise)
        { }

        public override string MoveToEndSvg(float cellSize, float offset)
        {
            float rX = TransformSize(_rX, cellSize);
            float rY = TransformSize(_rY, cellSize);
            Vector2 end = TransformPoint(EndPoint, cellSize, offset);

            return $"A{rX.ToInvariantString()},{rY.ToInvariantString()} 0 0 {(_clockwise ? '1' : '0')} {end.X.ToInvariantString()} {end.Y.ToInvariantString()} ";
        }
    }
}
