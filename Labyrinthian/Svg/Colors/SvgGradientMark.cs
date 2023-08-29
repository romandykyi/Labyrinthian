using System.Text;

namespace Labyrinthian
{
    /// <summary>
    /// SVG &lt;stop&gt; tag. Used for gradients.
    /// </summary>
    public readonly struct SvgGradientMark
    {
        public readonly float Offset;
        public readonly SvgColor Color;

        public SvgGradientMark(float offset, SvgColor color) : this()
        {
            Offset = offset;
            Color = color;
        }

        public override string ToString()
        {
            StringBuilder style = new StringBuilder();
            style.Append($"stop-color:{Color}");
            if (!Color.IsOpaque)
            {
                style.Append($";stop-opacity:{Color.Opacity.ToInvariantString()}");
            }

            return $"<stop offset=\"{Offset.ToInvariantString()}\" style=\"{style}\"/>";
        }
    }
}
