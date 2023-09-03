namespace Labyrinthian.Svg
{
    public enum SvgLinejoin
    {
        [SvgOption("arcs")]
        Arcs,
        [SvgOption("bevel")]
        Bevel,
        [SvgOption("miter")]
        Miter,
        [SvgOption("miter-clip")]
        MiterClip,
        [SvgOption("round")]
        Round
    }
}
