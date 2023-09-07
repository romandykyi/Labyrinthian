namespace Labyrinthian.Svg
{
    public abstract class SvgViewBoxElement : SvgPresentationElement
    {
        [SvgProperty("viewBox")]
        public SvgViewBox? ViewBox { get; set; }
        [SvgProperty("preserveAspectRatio")]
        public SvgPreserveAspectRatio? PreserveAspectRatio { get; set; }
    }
}
