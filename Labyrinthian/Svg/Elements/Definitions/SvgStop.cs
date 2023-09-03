namespace Labyrinthian.Svg
{
    [SvgElement("stop")]
    public class SvgStop : SvgElement
    {
        [SvgProperty("offset")]
        public SvgUnit? Offset { get; set; }
        [SvgProperty("stop-color")]
        public SvgColor? StopColor { get; set; }
        [SvgProperty("stop-opacity")]
        public SvgUnit? StopOpacity { get; set; }

        public SvgStop(SvgUnit? offset = null, SvgColor? stopColor = null, SvgUnit? stopOpacity = null)
        {
            Offset = offset;
            StopColor = stopColor;
            StopOpacity = stopOpacity;
        }
    }
}
