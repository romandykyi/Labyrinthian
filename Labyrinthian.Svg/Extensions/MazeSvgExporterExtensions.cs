using System.IO;
using System.Threading.Tasks;

namespace Labyrinthian.Svg
{
    public static class MazeSvgExporterExtensions
    {
        /// <summary>
        /// Exports a maze to a file.
        /// </summary>
        /// <param name="exporter">The exporter to use.</param>
        /// <param name="filename">Filename to where the maze will be exported.</param>
        /// <param name="root">
        /// Optional <see cref="SvgRoot" /> element.
        /// </param>
        /// <param name="style">
        /// Optional CSS style.
        /// </param>
        public static void ExportToFile(
            this MazeSvgExporter exporter, string filename,
            SvgRoot? root = null, string? style = null
            )
        {
            using StreamWriter streamWriter = new StreamWriter(filename);
            using SvgWriter svgWriter = new SvgWriter(streamWriter);
            exporter.Export(svgWriter, root, style);
        }

        /// <summary>
        /// Exports a maze to a file asynchronously.
        /// </summary>
        /// <param name="exporter">The exporter to use.</param>
        /// <param name="filename">Filename to where the maze will be exported.</param>
        /// <param name="root">
        /// Optional <see cref="SvgRoot" /> element.
        /// </param>
        /// <param name="style">
        /// Optional CSS style.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static async Task ExportToFileAsync(
            this MazeSvgExporter exporter, string filename,
            SvgRoot? root = null, string? style = null
            )
        {
            using StreamWriter streamWriter = new StreamWriter(filename);
            using SvgWriter svgWriter = new SvgWriter(streamWriter);
            await exporter.ExportAsync(svgWriter, root, style);
        }
    }
}
