namespace Labyrinthian.Svg
{
    [SvgElement("radialGradient")]
    public sealed class SvgRadialGradient : SvgGradient
    {
        [SvgProperty("cx")]
        public SvgLength? Cx { get; set; }
        [SvgProperty("cy")]
        public SvgLength? Cy { get; set; }
        [SvgProperty("r")]
        public SvgLength? R { get; set; }
        [SvgProperty("fr")]
        public SvgLength? Fr { get; set; }
        [SvgProperty("fx")]
        public SvgLength? Fx { get; set; }
        [SvgProperty("fy")]
        public SvgLength? Fy { get; set; }
    }
}
