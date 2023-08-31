namespace Labyrinthian.Svg
{
    [SvgElement("rect")]
    public sealed class SvgRect : SvgShape
    {
        [SvgProperty("x")]
        public SvgLength? X { get; set; }
        [SvgProperty("y")]
        public SvgLength? Y { get; set; }
        [SvgProperty("width")]
        public SvgLength? Width { get; set; }
        [SvgProperty("height")]
        public SvgLength? Height { get; set; }
    }
}
