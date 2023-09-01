namespace Labyrinthian.Svg
{
    public enum SvgAspectRatio
    {
        [SvgOption("none")]
        None,
        [SvgOption("xMinYMin")]
        XMinYMin,
        [SvgOption("xMidYMin")]
        XMidYMin,
        [SvgOption("xMaxYMin")]
        XMaxYMin,
        [SvgOption("xMinYMid")]
        XMinYMid,
        [SvgOption("xMidYMid")]
        XMidYMid,
        [SvgOption("xMaxYMid")]
        XMaxYMid,
        [SvgOption("xMinYMax")]
        XMinYMax,
        [SvgOption("xMidYMax")]
        XMidYMax,
        [SvgOption("xMaxYMax")]
        XMaxYMax
    }

    public enum SvgPreserveMode
    {
        [SvgOption("meet")]
        Meet,
        [SvgOption("slice")]
        Slice
    }

    public struct SvgPreserveAspectRatio
    {
        public SvgAspectRatio AspectRatio;
        public SvgPreserveMode? Mode;

        public SvgPreserveAspectRatio(SvgAspectRatio aspectRatio, SvgPreserveMode? mode = null)
        {
            AspectRatio = aspectRatio;
            Mode = mode;
        }

        public readonly override string? ToString()
        {
            string? result = AspectRatio.GetSvgOption();
            if (result == null) return null;

            if (AspectRatio != SvgAspectRatio.None && Mode != null)
            {
                result += $" {Mode.GetSvgOption()}";
            }

            return result;
        }
    }
}
