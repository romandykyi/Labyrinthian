namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for background.
    /// </summary>
    public sealed class Background : IExportModule
    {
        private readonly SvgFill _fill;

        private Background(SvgFill fill)
        {
            _fill = fill;
        }

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            SvgRect rect = new SvgRect()
            {
                X = 0,
                Y = 0,
                Fill = _fill,
                Width = new SvgLength(100f, SvgLengthUnit.Percentage),
                Height = new SvgLength(100f, SvgLengthUnit.Percentage)
            };
            svgWriter.StartElement(rect);
            svgWriter.EndElement();
        }

        /// <summary>
        /// Add a background(&lt;rect&gt; with 100% width and height).
        /// </summary>
        /// <param name="fill">
        /// Fill used for a background.
        /// </param>
        public static Background Create(SvgFill fill) => new Background(fill);
    }
}
