namespace Labyrinthian.Svg
{
    public interface IExportModule
    {
        /// <summary>
        /// Export a part of maze into SVG.
        /// </summary>
        /// <param name="exporter">
        /// Exporter, that calls this method.
        /// </param>
        /// <param name="svgWriter">
        /// Writer for exporting.
        /// </param>
        void Export(MazeSvgExporter exporter, SvgWriter svgWriter);
    }
}
