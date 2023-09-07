namespace Labyrinthian.Svg
{
    [SvgElement("svg")]
    public sealed class SvgRoot : SvgViewBoxElement
    {
        [SvgProperty("xmlns")]
        public string Xmlns => "http://www.w3.org/2000/svg";

        [SvgProperty("xlink", Prefix = "xmlns")]
        public string Xlink => "http://www.w3.org/1999/xlink";

        [SvgProperty("version")]
        public string Version => "1.1";
    }
}
