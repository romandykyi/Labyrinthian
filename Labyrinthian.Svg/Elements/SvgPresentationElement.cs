﻿namespace Labyrinthian.Svg
{
    public abstract class SvgPresentationElement : SvgElement
    {
        [SvgProperty("opacity")]
        public SvgUnit? Opacity { get; set; }

        [SvgProperty("fill")]
        public SvgFill? Fill { get; set; }

        [SvgProperty("fill-opacity")]
        public SvgUnit? FillOpacity { get; set; }

        [SvgProperty("stroke")]
        public SvgFill? Stroke { get; set; }

        [SvgProperty("stroke-width")]
        public SvgLength? StrokeWidth { get; set; }

        [SvgProperty("stroke-opacity")]
        public SvgUnit? StrokeOpacity { get; set; }

        [SvgProperty("stroke-dashoffset")]
        public SvgLength[]? StrokeDashoffset { get; set; }

        [SvgProperty("stroke-dasharray", Separator = ',')]
        public SvgLength[]? StrokeDasharray { get; set; }

        [SvgProperty("stroke-linecap")]
        public SvgLinecap? StrokeLinecap { get; set; }
        [SvgProperty("stroke-linejoin")]
        public SvgLinejoin? StrokeLinejoin { get; set; }
        [SvgProperty("stroke-miterlimit")]
        public float? StrokeMiterlimit { get; set; }

        [SvgProperty("transform")]
        public string? Transform { get; set; }
    }
}
