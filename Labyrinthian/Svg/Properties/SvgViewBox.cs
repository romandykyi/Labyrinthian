using System.Globalization;

namespace Labyrinthian.Svg
{
    public struct SvgViewBox
    {
        public float MinX;
        public float MinY;
        public float Width;
        public float Height;

        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}", MinX, MinY, Width, Height);
        }
    }
}
