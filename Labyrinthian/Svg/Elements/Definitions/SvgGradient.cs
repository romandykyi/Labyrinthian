namespace Labyrinthian.Svg
{
    public abstract class SvgGradient : SvgElement
    {
        [SvgChildren]
        public SvgStop[]? Stops { get; set; }

        [SvgProperty("href")]
        public string? Href { get; set; }

        [SvgProperty("gradientUnits")]
        public SvgGradientUnits? GradientUnits { get; set; }

        [SvgProperty("spreadMethod")]
        public SvgGradientUnits? SpreadMethod { get; set; }

        [SvgProperty("gradientTransform")]
        public string? GradientTransform { get; set; }
    }
}
