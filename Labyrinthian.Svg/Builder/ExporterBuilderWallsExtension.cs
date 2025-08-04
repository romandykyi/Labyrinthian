namespace Labyrinthian.Svg
{
    public static class ExporterBuilderWallsExtension
    {
        /// <summary>
        /// Adds walls as a singe <see cref="SvgPath"/> element.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="path">
        /// Optional template path('stroke-width' will be set to <paramref name="wallsWidth"/>).
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is recommended.
        /// </param>
        /// <param name="wallsWidth">Walls width in pixels.</param>
        public static MazeSvgExporterBuilder AddWallsAsSinglePath(this MazeSvgExporterBuilder builder, SvgPath? path = null, float wallsWidth = 2f)
        {
            return builder.AddExportModule(Walls.AsOnePath(path, wallsWidth));
        }

        /// <summary>
        /// Adds walls using separate <see cref="SvgPath" /> elements that 
        /// will be placed in a <see cref="SvgGroup" />.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="group">
        /// Optional params of group which will contain paths.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is recommended.
        /// </param>
        /// <param name="path">
        /// Optional template path('stroke-width' will be set to <paramref name="wallsWidth"/>).
        /// It's recommended to set all path properties inside <paramref name="group"/>.
        /// </param>
        /// <param name="wallsWidth">Walls width in pixels.</param>
        public static MazeSvgExporterBuilder AddWallsAsSeparatePaths(this MazeSvgExporterBuilder builder, SvgGroup? group = null, float wallsWidth = 2f, SvgPath? path = null)
        {
            return builder.AddExportModule(Walls.AsSeparatePaths(group, wallsWidth, path));
        }
    }
}
