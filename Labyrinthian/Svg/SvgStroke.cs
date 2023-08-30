using System.IO;
using System.Text;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class that defines five stroke SVG attributes: 
    /// 'stroke', 'stroke-opacity', 'stroke-width', 'stroke-dasharray' and 'stroke-linecap'.
    /// </summary>
    public class SvgStroke
    {
        public enum StrokeLinecap
        {
            Butt, Round, Square
        }

        public readonly float Width;
        public readonly SvgFill Fill;
        public readonly StrokeLinecap Linecap;
        public readonly float[] Dasharray;

        public static readonly SvgStroke None = new SvgStroke(0f, SvgFill.None);

        public SvgStroke(float width, SvgFill fill, 
            StrokeLinecap linecap = StrokeLinecap.Butt, params float[] dasharray)
        {
            Width = width;
            Fill = fill;
            Linecap = linecap;
            Dasharray = dasharray;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"stroke=\"{Fill}\"");
            string? opacity = Fill.Opacity;
            if (opacity != null)
            {
                stringBuilder.Append($" stroke-opacity=\"{opacity}\"");
            }
            if (Width > 0f)
            {
                stringBuilder.Append($" stroke-width=\"{Width.ToInvariantString()}\"");
            }
            switch (Linecap)
            {
                case StrokeLinecap.Square:
                    stringBuilder.Append(" stroke-linecap=\"square\"");
                    break;
                case StrokeLinecap.Round:
                    stringBuilder.Append(" stroke-linecap=\"round\"");
                    break;
            }
            if (Dasharray.Length > 0)
            {
                stringBuilder.Append(" stroke-dasharray=\"");
                for (int i = 0; i < Dasharray.Length - 1; ++i)
                {
                    stringBuilder.Append($"{Dasharray[i].ToInvariantString()},");
                }
                stringBuilder.Append(Dasharray[^1].ToInvariantString());
                stringBuilder.Append('"');
            }

            return stringBuilder.ToString();
        }
    }
}
