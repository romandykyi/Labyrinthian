namespace Labyrinthian.Svg
{
    [SvgElement("polyline")]
    public sealed class SvgPolyline : SvgShape
    {
        [SvgProperty("points", Separator = ' ')]
        public SvgPoint[]? Points { get; set; }
    }
}
