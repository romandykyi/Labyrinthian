using System;

namespace Labyrinthian.Svg
{
    public enum SvgMarkerOrient
    {
        [SvgOption("auto")]
        Auto,
        [SvgOption("auto-start-reverse")]
        AutoStartReverse
    }

    public enum SvgMarkerUnits
    {
        [SvgOption("userSpaceOnUse")]
        UserSpaceOnUse,
        [SvgOption("strokeWidth")]
        StrokeWidth
    }

    [SvgElement("marker")]
    public sealed class SvgMarker : SvgViewBoxGroup, INeedsDefinition
    {
        [SvgProperty("refX")]
        public SvgLength? RefX { get; set; }
        [SvgProperty("refY")]
        public SvgLength? RefY { get; set; }

        [SvgProperty("markerWidth")]
        public SvgLength? MarkerWidth { get; set; }
        [SvgProperty("markerHeight")]
        public SvgLength? MarkerHeight { get; set; }

        [SvgProperty("markerUnits")]
        public SvgMarkerUnits? MarkerUnits { get; set; }
        [SvgProperty("orient")]
        public SvgMixedEnum<SvgMarkerOrient, float>? Orient { get; set; }

        public SvgElement? Definition => this;

        public override string ToString()
        {
            SvgMixedEnum<SvgMarkerUnits, float> p;
            if (Id == null)
            {
                throw new InvalidOperationException("SVG-marker should have ID.");
            }
            return $"url(#{Id})";
        }
    }
}
