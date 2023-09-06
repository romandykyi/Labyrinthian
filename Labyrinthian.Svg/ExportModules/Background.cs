using System.Threading.Tasks;

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

        public async Task ExportAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            SvgRect rect = new SvgRect()
            {
                X = 0,
                Y = 0,
                Fill = _fill,
                Width = new SvgLength(100f, SvgLengthUnit.Percentage),
                Height = new SvgLength(100f, SvgLengthUnit.Percentage)
            };
            await svgWriter.StartElementAsync(rect);
            await svgWriter.EndElementAsync();
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
