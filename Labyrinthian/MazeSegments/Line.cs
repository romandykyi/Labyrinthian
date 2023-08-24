using System.Numerics;

namespace Labyrinthian
{
    public sealed class Line : PathSegment
    {
        public Vector2 From { get; set; }
        public Vector2 To { get; set; }

        public Line(Vector2 from, Vector2 to)
        {
            From = from;
            To = to;
        }

        public Line(float x0, float y0, float x1, float y1)
        {
            From = new Vector2(x0, y0);
            To = new Vector2(x1, y1);
        }

        public override Vector2 StartPoint => From;
        public override Vector2 EndPoint => To;

        public override float[] Center => new float[2]
        {
            (From.X + To.X) / 2f,
            (From.Y + To.Y) / 2f
        };

        protected override string MoveToEndSvg(Vector2 v)
        {
            if (From.X.ApproximatelyEquals(To.X))
            {
                return $"V {v.Y.ToInvariantString()} ";
            }
            if (From.Y.ApproximatelyEquals(To.Y))
            {
                return $"H {v.X.ToInvariantString()} ";
            }
            return $"L {v.X.ToInvariantString()} {v.Y.ToInvariantString()} ";
        }
    }
}