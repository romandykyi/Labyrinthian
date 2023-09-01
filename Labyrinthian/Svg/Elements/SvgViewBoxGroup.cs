namespace Labyrinthian.Svg
{
    public abstract class SvgViewBoxGroup : SvgGroup
    {
        [SvgProperty("viewBox")]
        public SvgViewBox? ViewBox { get; set; }
        [SvgProperty("preserveAspectRatio")]
        public SvgPreserveAspectRatio? PreserveAspectRatio { get; set; }
    }
}
