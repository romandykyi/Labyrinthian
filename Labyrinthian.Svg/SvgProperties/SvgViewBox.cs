using System.Globalization;

namespace Labyrinthian.Svg
{
    public struct SvgViewBox
    {
        public float MinX;
        public float MinY;
        public float Width;
        public float Height;

        public SvgViewBox(float minX, float minY, float width, float height)
        {
            MinX = minX;
            MinY = minY;
            Width = width;
            Height = height;
        }

        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", MinX, MinY, Width, Height);
        }
    }
}
