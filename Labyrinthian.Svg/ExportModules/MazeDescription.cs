using System.Threading.Tasks;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for maze description.
    /// </summary>
    public sealed class MazeDescription : IExportModule
    {
        private MazeDescription() { }

        /// <summary>
        /// Default description about maze.
        /// </summary>
        public static MazeDescription Default => new MazeDescription();

        public async Task ExportAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            await svgWriter.WriteStringElementAsync("desc", $"{exporter.Maze.Description}; Generator: Labyrinthian");
        }
    }
}
