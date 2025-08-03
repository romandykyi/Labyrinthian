namespace Labyrinthian.Svg.Builder
{
    public static class ExporterBuilderMetadataExtensions
    {
        /// <summary>
        /// Adds a metadata export module to the builder.
        /// </summary>
        /// <param name="builder">Builder to add the metadata to.</param>
        public static MazeSvgExporterBuilder IncludeMetadata(this MazeSvgExporterBuilder builder)
        {
            return builder.AddExportModule(MazeDescription.Default);
        }
    }
}
