using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian.Svg
{
    public static class ExporterBuilderGeneratorStateExtensions
    {
        private static IEnumerable<MazeCell> UnvisitedCells(MazeGenerator generator)
        {
            return generator.Maze.Cells.Where(c => !generator.VisitedCells[c] && !generator.HighlightedCells[c]);
        }

        private static IEnumerable<MazeCell> HighlightedCells(MazeGenerator generator)
        {
            return generator.Maze.Cells.Where(c => generator.HighlightedCells[c]);
        }

        private static IEnumerable<MazeCell> SelectedCell(MazeGenerator generator)
        {
            if (generator.SelectedCell != null) yield return generator.SelectedCell;
        }

        /// <summary>
        /// Adds all unvisited cells that are not highlighted as <see cref="SvgPath"/> 
        /// elements inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="group">Group, that will contain the unvisited cells.</param>
        public static MazeSvgExporterBuilder AddUnvisitedCells(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgGroup group)
        {
            return builder.AddCells(UnvisitedCells(generator), group);
        }

        /// <summary>
        /// Adds all highlighted cells as <see cref="SvgPath"/> elements inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="group">Group, that will contain the highlighted cells.</param>
        public static MazeSvgExporterBuilder AddHighlightedCells(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgGroup group)
        {
            return builder.AddCells(HighlightedCells(generator), group);
        }

        /// <summary>
        /// Adds the selected cell (if exists) as <see cref="SvgPath"/> elements inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="group">Group, that will contain the selected cell.</param>
        public static MazeSvgExporterBuilder AddSelectedCell(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgGroup group)
        {
            return builder.AddCells(SelectedCell(generator), group);
        }

        /// <summary>
        /// Adds all unvisited maze cells that are not highlighted as nodes.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="nodeShape">
        /// Optional shape of a node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// Should have ID.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain the nodes.
        /// </param>
        public static MazeSvgExporterBuilder AddUnvisitedNodes(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return builder.AddNodes(UnvisitedCells(generator), nodeShape, nodesGroup);
        }

        /// <summary>
        /// Adds all highlighted maze cells as nodes.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="nodeShape">
        /// Optional shape of a node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// Should have ID.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain the nodes.
        /// </param>
        public static MazeSvgExporterBuilder AddHighlightedNodes(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return builder.AddNodes(HighlightedCells(generator), nodeShape, nodesGroup);
        }

        /// <summary>
        /// Adds a selected maze cell (if exists) as a node.
        /// </summary>
        /// <param name="builder">Builder to use.</param>
        /// <param name="generator">The maze generator which provides the cells' state.</param>
        /// <param name="nodeShape">
        /// Optional shape of the node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// Should have ID.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain the node.
        /// </param>
        public static MazeSvgExporterBuilder AddSelectedNode(this MazeSvgExporterBuilder builder, MazeGenerator generator, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return builder.AddNodes(SelectedCell(generator), nodeShape, nodesGroup);
        }
    }
}
