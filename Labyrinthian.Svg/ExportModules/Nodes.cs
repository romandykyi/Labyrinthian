using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for maze graph nodes.
    /// </summary>
    public sealed class Nodes : IExportModule
    {
        private readonly SvgShape _nodeShape;
        private readonly SvgGroup? _nodesGroup;
        private IEnumerable<MazeCell>? _cells;

        private Nodes(SvgShape? svgShape, SvgGroup? svgGroup, IEnumerable<MazeCell>? cells = null)
        {
            _nodeShape = svgShape ?? new SvgCircle()
            {
                R = 4f,
                Fill = SvgColor.White,
                Stroke = SvgColor.Black,
                StrokeWidth = 1f,
                Id = "node"
            };
            _nodesGroup = svgGroup;
            _cells = cells;
        }

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (_cells == null)
                _cells = exporter.Maze.Cells;
            else if (!_cells.Any())
                return;

            if (_nodesGroup != null) svgWriter.StartElement(_nodesGroup);
            foreach (var cell in _cells)
            {
                Vector2 cellCenter = exporter.Maze.GetCellCenter2D(cell);
                cellCenter *= exporter.CellSize;
                cellCenter.X += exporter.Offset;
                cellCenter.Y += exporter.Offset;

                SvgUse use = new SvgUse()
                {
                    UsedElement = _nodeShape,
                    X = cellCenter.X,
                    Y = cellCenter.Y
                };

                svgWriter.StartElement(use);
                svgWriter.EndElement();
            }
            if (_nodesGroup != null) svgWriter.EndElement();
        }

        /// <summary>
        /// Export selected nodes.
        /// </summary>
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
        public static Nodes Selected(IEnumerable<MazeCell> cells, SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }
            return new Nodes(nodeShape, nodesGroup, cells);
        }

        /// <summary>
        /// Export selected node.
        /// </summary>
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
        public static Nodes Selected(MazeCell cell, SvgShape? nodeShape = null)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }
            return new Nodes(nodeShape, null, new MazeCell[1] { cell });
        }

        /// <summary>
        /// Export all nodes.
        /// </summary>
        /// <param name="nodeShape">
        /// Optional shape of the node.
        /// Will be written in '&lt;defs&gt;' and then used with '&lt;use&gt;'.
        /// If <see langword="null"/> then '&lt;circle&gt;' will be used.
        /// </param>
        /// <param name="nodesGroup">
        /// Optional group which will contain nodes.
        /// </param>
        public static Nodes All(SvgShape? nodeShape = null, SvgGroup? nodesGroup = null)
        {
            return new Nodes(nodeShape, nodesGroup ?? new SvgGroup());
        }
    }
}
