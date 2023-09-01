namespace Labyrinthian.Svg
{
    [SvgElement("linearGradient")]
    public sealed class SvgLinearGradient : SvgGradient
    {
        [SvgProperty("x1")]
        public SvgLength X1 { get; set; }
        [SvgProperty("y1")]
        public SvgLength Y1 { get; set; }
        [SvgProperty("x2")]
        public SvgLength X2 { get; set; }
        [SvgProperty("y2")]
        public SvgLength Y2 { get; set; }
    }
}
