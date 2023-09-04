namespace Labyrinthian.Svg
{
    [SvgElement("circle")]
    public sealed class SvgCircle : SvgShape
    {
        [SvgProperty("cx")]
        public SvgLength? Cx { get; set; }
        [SvgProperty("cy")]
        public SvgLength? Cy { get; set; }
        [SvgProperty("r")]
        public SvgLength? R { get; set; }
    }
}
