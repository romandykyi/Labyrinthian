namespace Labyrinthian.Svg
{
    [SvgElement("ellipse")]
    public sealed class SvgEllipse : SvgShape
    {
        [SvgProperty("cx")]
        public SvgLength? Cx { get; set; }
        [SvgProperty("cy")]
        public SvgLength? Cy { get; set; }
        [SvgProperty("rx")]
        public SvgLength? Rx { get; set; }
        [SvgProperty("ry")]
        public SvgLength? Ry { get; set; }
    }
}
