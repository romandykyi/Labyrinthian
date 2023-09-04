namespace Labyrinthian.Svg
{
    [SvgElement("path")]
    public sealed class SvgPath : SvgShape
    {
        [SvgProperty("d")]
        public string? D { get; set; }
    }
}
