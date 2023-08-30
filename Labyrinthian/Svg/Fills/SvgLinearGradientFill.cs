using System;
using System.Text;
using System.Numerics;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// SVG linear gradient fill.
    /// </summary>
    public sealed class SvgLinearGradientFill : SvgFill
    {
        private static uint s_freeId = 100;

        private string StrId => $"gr{_id}"; 

        private readonly uint _id;

        public readonly SvgGradientMark[] Marks = null!;
        public readonly Vector2 From, To;
        
        private SvgLinearGradientFill(Vector2 from, Vector2 to)
        {
            _id = s_freeId++;
            From = from;
            To = to;
        }

        /// <summary>
        /// Create a gradient from marks.
        /// </summary>
        /// <param name="from">First point of the gradient.</param>
        /// <param name="to">Last point of the gradient.</param>
        /// <param name="marks">Gradient marks(length should be at least 2).</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SvgLinearGradientFill(Vector2 from, Vector2 to, params SvgGradientMark[] marks) : this(from, to)
        {
            if (marks == null) throw new ArgumentNullException(nameof(marks));
            if (marks.Length < 2)
            {
                throw new ArgumentException("Gradient should consist of at least 2 marks", nameof(marks));
            }

            Marks = marks;
        }

        /// <summary>
        /// Create a gradient from colors that will be spreaded linearly.
        /// </summary>
        /// <param name="from">First point of the gradient.</param>
        /// <param name="to">Last point of the gradient.</param>
        /// <param name="colors">Gradient colors(length should be at least 2).</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SvgLinearGradientFill(Vector2 from, Vector2 to, params SvgColor[] colors) : this(from, to)
        {
            if (colors.Length < 2)
            {
                throw new ArgumentException("Gradient should consist of at least 2 colors", nameof(colors));
            }

            Marks = new SvgGradientMark[colors.Length];
            float n = Marks.Length - 1;
            for (int i = 0; i < Marks.Length; ++i)
            {
                Marks[i] = new SvgGradientMark(i / n, colors[i]);
            }
        }

        public override string ToString() => $"url(#{StrId})";

        public override string Definition
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"<linearGradient ");
                builder.Append($"x1=\"{From.X.ToInvariantString()}\" y1=\"{From.Y.ToInvariantString()}\" ");
                builder.Append($"x2=\"{To.X.ToInvariantString()}\" y2=\"{To.Y.ToInvariantString()}\" ");
                builder.Append($"gradientUnits=\"userSpaceOnUse\" id=\"{StrId}\">");
                foreach (var mark in Marks) builder.Append(mark);
                builder.Append("</linearGradient>");

                return builder.ToString();
            }
        }
    }
}
