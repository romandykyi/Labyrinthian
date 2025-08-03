using System;
using System.Collections.Generic;

namespace Labyrinthian.Svg.Builder
{
    public static class ExporterBuilderNodesExtensions
    {
        /// <summary>
        /// Adds selected the maze cell as a nodes.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="cells">
        /// Selected nodes.
        /// </param>
        /// <param name="nodeShape">
        /// Optional shape of the node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// Should have ID.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain nodes.
        /// </param>
        /// <exception cref="ArgumentNullException" />
        public static MazeSvgExporterBuilder AddNodes(this MazeSvgExporterBuilder builder, IEnumerable<MazeCell> cells, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return builder.AddExportModule(Nodes.Selected(cells, nodeShape, nodesGroup));
        }

        /// <summary>
        /// Adds a selected node.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="cell">
        /// Selected node.
        /// </param>
        /// <param name="nodeShape">
        /// Optional shape of the node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// Should have ID.
        /// </param>
        /// <exception cref="ArgumentNullException" />
        public static MazeSvgExporterBuilder AddNode(this MazeSvgExporterBuilder builder, MazeCell cell, SvgShape? nodeShape = null)
        {
            return builder.AddExportModule(Nodes.Selected(cell, nodeShape));
        }

        /// <summary>
        /// Adds all nodes.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="nodeShape">
        /// Optional shape of the node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain nodes.
        /// </param>
        public static MazeSvgExporterBuilder AddAllNodes(this MazeSvgExporterBuilder builder, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return builder.AddExportModule(Nodes.All(nodeShape, nodesGroup));
        }
    }
}
