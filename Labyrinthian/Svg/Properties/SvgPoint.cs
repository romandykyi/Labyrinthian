using System.Numerics;

namespace Labyrinthian.Svg
{
    public struct SvgPoint
    {
        public float X; 
        public float Y;

        public SvgPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public readonly override string ToString()
        {
            return $"{X.ToInvariantString()},{Y.ToInvariantString()}";
        }

        public static implicit operator Vector2(SvgPoint p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static implicit operator SvgPoint(Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}
