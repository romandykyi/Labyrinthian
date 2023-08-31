namespace Labyrinthian.Svg
{
    [SvgElement("svg")]
    public sealed class SvgRoot : SvgGroup
    {
        [SvgProperty("xmlns")]
        public string Xmlns => "http://www.w3.org/2000/svg";

        [SvgProperty("xlink", LocalName = "xmlns")]
        public string Xlink => "http://www.w3.org/1999/xlink";

        [SvgProperty("version")]
        public string Version => "1.1";

        [SvgProperty("viewBox")]
        public SvgViewBox? ViewBox { get; set; }
    }
}
