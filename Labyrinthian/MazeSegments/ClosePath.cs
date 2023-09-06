using System.Numerics;

namespace Labyrinthian
{
    public sealed class ClosePath : PathSegment
    {
        private readonly Vector2 _previous, _start;

        public override Vector2 StartPoint => _previous;
        public override Vector2 EndPoint => _start;

        public ClosePath(Vector2 previous, Vector2 start)
        {
            _previous = previous;
            _start = start;
        }

        public ClosePath(float previousX, float previousY, float startX, float startY) :
            this(new Vector2(previousX, previousY), new Vector2(startX, startY))
        { }

        public override string MoveToStartSvg(float cellSize, float offset)
        {
            return string.Empty;
        }

        public override string MoveToEndSvg(float cellSize, float offset)
        {
            return $"Z ";
        }
    }
}
