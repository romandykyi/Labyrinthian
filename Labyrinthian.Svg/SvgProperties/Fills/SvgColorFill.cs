namespace Labyrinthian.Svg
{
    /// <summary>
    /// Solid color fill.
    /// </summary>
    public sealed class SvgColorFill : SvgFill
    {
        public SvgColor Color { get; set; }

        public SvgColorFill(SvgColor color)
        {
            Color = color;
        }

        public override string ToString() => Color.ToString();
    }
}
