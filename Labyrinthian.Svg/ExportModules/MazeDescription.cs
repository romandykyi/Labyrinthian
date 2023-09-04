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

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            svgWriter.WriteStringElement("desc", $"{exporter.Maze.Description}; Generator: Labyrinthian");
        }
    }
}
