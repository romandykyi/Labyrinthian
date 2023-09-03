using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for cells.
    /// </summary>
    public sealed class Cells : IExportModule
    {
        private readonly SvgGroup _group;
        private IEnumerable<MazeCell>? _cells;

        private Cells(SvgGroup group, IEnumerable<MazeCell>? cells = null)
        {
            _group = group;
            _cells = cells;
        }

        private string GetPath(MazeCell cell, MazeSvgExporter exporter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            PathSegment? previousSegment = null;
            foreach (var segment in exporter.Maze.GetCellPath(cell))
            {
                stringBuilder.Append(
                    segment.MoveNext(previousSegment, exporter.CellSize, exporter.Offset));
                previousSegment = segment;
            }
            return stringBuilder.ToString();
        }

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (_cells == null)
                _cells = exporter.Maze.Cells;
            else if (!_cells.Any())
                return;

            svgWriter.StartElement(_group);
            foreach (var cell in _cells)
            {
                SvgPath path = new SvgPath()
                {
                    D = GetPath(cell, exporter)
                };
                svgWriter.StartElement(path);
                svgWriter.EndElement();
            }
            svgWriter.EndElement();
        }

        /// <summary>
        /// Export all maze cells.
        /// </summary>
        /// <param name="group">Group, containing all cells.</param>
        /// <exception cref="ArgumentNullException" />
        public static Cells All(SvgGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            return new Cells(group);
        }

        /// <summary>
        /// Export selected maze cells.
        /// </summary>
        /// <param name="cells">Selected maze cells to be exported.</param>
        /// <param name="group">Group, containing selected cells.</param>
        /// <exception cref="ArgumentNullException" />
        public static Cells Selected(IEnumerable<MazeCell> cells, SvgGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            return new Cells(group, cells);
        }

        /// <summary>
        /// Export selected maze cell.
        /// </summary>
        /// <param name="cell">Selected maze cell to be exported.</param>
        /// <param name="group">Group, containing selected cell.</param>
        /// <exception cref="ArgumentNullException" />
        public static Cells Selected(MazeCell cell, SvgGroup group)
        {
            MazeCell[] cells = new MazeCell[1] { cell };
            return Selected(cells, group);
        }
    }
}
