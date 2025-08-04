using System.Collections.Generic;

namespace Labyrinthian.Svg
{
    public static class ExporterBuilderCellsExtensions
    {
        /// <summary>
        /// Adds all maze cells as <see cref="SvgPath"/> elements inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">Builder to add the cells to.</param>
        /// <param name="group">Group, that will contain all cells.</param>
        public static MazeSvgExporterBuilder AddAllCells(this MazeSvgExporterBuilder builder, SvgGroup group)
        {
            return builder.AddExportModule(Cells.All(group));
        }

        /// <summary>
        /// Adds the maze cell as <see cref="SvgPath"/> element
        /// inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">Builder to add the cell to.</param>
        /// <param name="cell">The maze cell to be exported.</param>
        /// <param name="group">Group, that will contain the selected cells.</param>
        public static MazeSvgExporterBuilder AddCell(this MazeSvgExporterBuilder builder, MazeCell cell, SvgGroup group)
        {
            return builder.AddExportModule(Cells.Selected(cell, group));
        }

        /// <summary>
        /// Adds the maze cells as <see cref="SvgPath"/> elements inside the <paramref name="group"/>.
        /// </summary>
        /// <param name="builder">Builder to add the cells to.</param>
        /// <param name="cells">Selected maze cells to be exported.</param>
        /// <param name="group">Group, that will contain the selected cells.</param>
        public static MazeSvgExporterBuilder AddCells(this MazeSvgExporterBuilder builder, IEnumerable<MazeCell> cells, SvgGroup group)
        {
            return builder.AddExportModule(Cells.Selected(cells, group));
        }
    }
}
