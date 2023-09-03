using System;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// SVG gradient fill.
    /// </summary>
    public sealed class SvgGradientFill : SvgFill, INeedsDefinition
    {
        public SvgGradient Gradient { get; set; }
        public SvgElement? Definition => Gradient;

        public SvgGradientFill(SvgGradient gradient)
        {
            Gradient = gradient;
        }

        public override string ToString()
        {
            if (Gradient.Id == null)
            {
                throw new InvalidOperationException("Gradient id can't be null.");
            }
            return $"url(#{Gradient.Id})";
        }
    }
}
