using System;
using System.Text;

namespace Labyrinthian
{
    public readonly struct SvgStroke
    {
        public enum StrokeLinecap
        {
            Butt, Round, Square
        }

        public readonly float Width;
        public readonly SvgFill Fill;
        public readonly StrokeLinecap Linecap;

        public static readonly SvgStroke None = new SvgStroke(-1f);

        public SvgStroke(float width, StrokeLinecap linecap = StrokeLinecap.Butt) :
            this(width, new SvgColorFill("#000000"), linecap) { }

        public SvgStroke(float width, SvgFill fill, StrokeLinecap linecap = StrokeLinecap.Butt) : this()
        {
            Width = width;
            Fill = fill;
            Linecap = linecap;
        }

        public override string ToString()
        {
            if (Width <= 0f) return "stroke=\"none\"";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"stroke=\"{Fill}\"");
            string? opacity = Fill.Opacity;
            if (opacity != null)
            {
                stringBuilder.Append($" stroke-opacity=\"{opacity}\"");
            }
            stringBuilder.Append($" stroke-width=\"{Width.ToInvariantString()}\"");
            switch (Linecap)
            {
                case StrokeLinecap.Square:
                    stringBuilder.Append(" stroke-linecap=\"square\"");
                    break;
                case StrokeLinecap.Round:
                    stringBuilder.Append(" stroke-linecap=\"round\"");
                    break;
            }

            return stringBuilder.ToString();
        }
    }
}