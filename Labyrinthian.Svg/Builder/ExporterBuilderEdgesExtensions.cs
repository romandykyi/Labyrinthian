using System.Collections.Generic;

namespace Labyrinthian.Svg
{
    public static class ExporterBuilderEdgesExtensions
    {
        /// <summary>
        /// Adds maze edges of the base graph.
        /// </summary>
        /// <param name="builder">Builder to add the cell to.</param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="includeExits">
        /// If <see langword="true"/> then edges that lead to entries/exits will be displayed too.
        /// </param>
        public static MazeSvgExporterBuilder AddBaseGraphEdges(
            this MazeSvgExporterBuilder builder, SvgPath? path = null, bool includeExits = true)
        {
            return builder.AddExportModule(Edges.OfBaseGraph(path, includeExits));
        }

        /// <summary>
        /// Adds maze edges of the passages graph.
        /// </summary>
        /// <param name="builder">Builder to add the cell to.</param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="includeExits">
        /// If <see langword="true"/> then edges that lead to entries/exits will be displayed too.
        /// </param>
        public static MazeSvgExporterBuilder AddPassagesGraphEdges(
            this MazeSvgExporterBuilder builder, SvgPath? path = null, bool includeExits = true)
        {
            return builder.AddExportModule(Edges.OfPassagesGraph(path, includeExits));
        }

        /// <summary>
        /// Adds the specified maze edges.
        /// </summary>
        /// <param name="builder">Builder to add the cell to.</param>
        /// <param name="edges">Specified maze edges.</param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static MazeSvgExporterBuilder AddEdges(
            this MazeSvgExporterBuilder builder, IEnumerable<MazeEdge> edges, SvgPath? path = null)
        {
            return builder.AddExportModule(Edges.Selected(edges, path));
        }

        /// <summary>
        /// Adds edges as separate paths, order of each edge's vertices matters.
        /// </summary>
        /// <param name="builder">Builder to add the cell to.</param>
        /// <param name="directedEdges">Edges to export, order of vertices matters.</param>
        /// <param name="group">
        /// A group to place edges in. If <see langword="null" /> then a default group will be used.
        /// </param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static MazeSvgExporterBuilder AddDirectedEdges(
            this MazeSvgExporterBuilder builder, IEnumerable<MazeEdge> directedEdges,
            SvgGroup? group = null, SvgPath? path = null)
        {
            return builder.AddExportModule(Edges.Directed(directedEdges, group, path));
        }
    }
}
