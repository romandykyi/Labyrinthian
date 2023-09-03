using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for maze graph edges.
    /// </summary>
    public sealed class Edges : IExportModule
    {
        private readonly bool _baseGraphEdges;
        private readonly SvgPath _path;
        private IEnumerable<MazeEdge>? _edges;

        private static SvgPath DefaultPath => new SvgPath()
        {
            Fill = SvgFill.None,
            Stroke = SvgColor.Black,
            StrokeWidth = 2f,
        };

        private Edges(SvgPath? path, bool baseGraphEdges)
        {
            _path = path ?? DefaultPath;
            _baseGraphEdges = baseGraphEdges;
        }

        private Edges(SvgPath? path, IEnumerable<MazeEdge> edges)
        {
            _path = path ?? DefaultPath;
            _edges = edges;
        }

        private static IEnumerable<MazeEdge> BaseGraphEdges(Maze maze)
        {
            return maze.GetBaseGraphEdges();
        }

        private static IEnumerable<MazeEdge> PassagesGraphEdges(Maze maze)
        {
            return maze.FindGraphEdgesDFS(
                edge => maze.AreCellsConnected(edge.Cell1, edge.Cell2), true);
        }

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            _edges ??= _baseGraphEdges ?
                BaseGraphEdges(exporter.Maze) :
                PassagesGraphEdges(exporter.Maze);

            if (!_edges.Any()) return;

            StringBuilder pathBuilder = new StringBuilder();
            PathSegment? previousSegment = null;
            foreach (var edge in _edges)
            {
                PathSegment currentSegment = exporter.Maze.GetPathBetweenCells(edge);
                pathBuilder.Append(
                    currentSegment.MoveNext(previousSegment, exporter.CellSize, exporter.Offset)
                    );
                previousSegment = currentSegment;
            }

            _path.D = pathBuilder.ToString();
            svgWriter.StartElement(_path);
            svgWriter.EndElement();
        }

        /// <summary>
        /// Export maze edges of base graph.
        /// </summary>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static Edges OfBaseGraph(SvgPath? path = null)
        {
            return new Edges(path, true);
        }

        /// <summary>
        /// Export maze edges of passages graph.
        /// </summary>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static Edges OfPassagesGraph(SvgPath? path = null)
        {
            return new Edges(path, false);
        }

        /// <summary>
        /// Export selected maze edges.
        /// </summary>
        /// <param name="edges">Selected maze edges.</param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static Edges Selected(IEnumerable<MazeEdge> edges, SvgPath? path = null)
        {
            return new Edges(path, edges);
        }
    }
}
