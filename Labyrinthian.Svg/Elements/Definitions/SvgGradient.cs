namespace Labyrinthian.Svg
{
    public enum SvgGradientUnits
    {
        [SvgOption("userSpaceOnUse")]
        UserSpaceOnUse,
        [SvgOption("objectBoundingBox")]
        ObjectBoundingBox
    }

    public enum SvgSpreadMethod
    {
        [SvgOption("pad")]
        Pad,
        [SvgOption("reflect")]
        Reflect,
        [SvgOption("repeat")]
        Repeat
    }

    public abstract class SvgGradient : SvgElement
    {
        [SvgChildren]
        public SvgStop[]? Stops { get; set; }

        [SvgProperty("href")]
        public string? Href { get; set; }

        [SvgProperty("gradientUnits")]
        public SvgGradientUnits? GradientUnits { get; set; }

        [SvgProperty("spreadMethod")]
        public SvgSpreadMethod? SpreadMethod { get; set; }

        [SvgProperty("gradientTransform")]
        public string? GradientTransform { get; set; }

        public static implicit operator SvgFill(SvgGradient gradient)
        {
            return new SvgGradientFill(gradient);
        }
    }
}
