namespace Labyrinthian.Svg
{
    public static class ExporterBuilderSolutionsExtensions
    {
        /// <summary>
        /// Adds all solutions as paths.
        /// </summary>
        /// <param name="builder">Builder to add the solutions to.</param>
        /// <param name="group">
        /// Optional params for the group in which all solutions will be placed.
        /// Setting <see cref="SvgPresentationElement.Fill"/> to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="pathCreator">
        /// Creator for each path.
        /// </param>
        /// <param name="intersectOuterCells">
        /// If <see langword="true"/> then solutions lines will intersect outer cells.
        /// </param>
        public static MazeSvgExporterBuilder AddSolutions(
            this MazeSvgExporterBuilder builder,
            SvgGroup? group = null, Solutions.PathCreator? pathCreator = null,
            bool intersectOuterCells = true)
        {
            return builder.AddExportModule(Solutions.All(group, pathCreator, intersectOuterCells));
        }
    }
}
