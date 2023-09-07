namespace Labyrinthian.Svg
{
    public abstract class SvgShape : SvgPresentationElement
    {
        [SvgProperty("pathLength")]
        public float? PathLength { get; set; }
        [SvgProperty("marker-start")]
        public SvgMarker? MarkerStart { get; set; }
        [SvgProperty("marker-mid")]
        public SvgMarker? MarkerMid { get; set; }
        [SvgProperty("marker-end")]
        public SvgMarker? MarkerEnd { get; set; }
    }
}
