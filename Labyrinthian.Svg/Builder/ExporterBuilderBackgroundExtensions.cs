namespace Labyrinthian.Svg.Builder
{
    public static class ExporterBuilderBackgroundExtensions
    {
        /// <summary>
        /// Adds a background (&lt;rect&gt; with 100% width and height).
        /// </summary>
        /// <param name="builder">Builder to add the background to.</param>
        /// <param name="backgroundFill">Fill used for the background.</param>
        public static MazeSvgExporterBuilder AddBackground(this MazeSvgExporterBuilder builder, SvgFill backgroundFill)
        {
            return builder.AddExportModule(Background.Create(backgroundFill));
        }
    }
}
