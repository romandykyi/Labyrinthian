namespace Labyrinthian.Svg
{
    [SvgElement("polygon")]
    public sealed class SvgPolygon : SvgShape
    {
        [SvgProperty("points", Separator = ' ')]
        public SvgPoint[]? Points { get; set; }
    }
}
