using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for maze graph edges.
    /// </summary>
    public sealed class Edges : IExportModule
    {
        private readonly bool _baseGraphEdges;
        private readonly bool _includeExits;
        private readonly bool _areEdgesDirected;
        private readonly SvgPath _path;
        private readonly SvgGroup? _group;
        private IEnumerable<MazeEdge>? _edges;

        private static SvgPath DefaultPath => new SvgPath()
        {
            Fill = SvgFill.None,
            Stroke = SvgColor.Black,
            StrokeWidth = 2f,
        };

        private Edges(SvgPath? path, bool baseGraphEdges, bool includeExits = true)
        {
            _path = path ?? DefaultPath;
            _baseGraphEdges = baseGraphEdges;
            _includeExits = includeExits;
        }

        private Edges(SvgPath? path, IEnumerable<MazeEdge> edges)
        {
            _path = path ?? DefaultPath;
            _edges = edges;
            _includeExits = false;
        }

        private Edges(SvgPath? path, SvgGroup? group, IEnumerable<MazeEdge> directedEdges)
        {
            _path = path ?? new SvgPath();
            _group = group ?? new SvgGroup()
            {
                Fill = SvgFill.None,
                Stroke = SvgColor.Black,
                StrokeWidth = 2f
            };
            _edges = directedEdges;
            _areEdgesDirected = true;
        }

        private static IEnumerable<MazeEdge> BaseGraphEdges(Maze maze)
        {
            return maze.GetBaseGraphEdges();
        }

        private static IEnumerable<MazeEdge> PassagesGraphEdges(Maze maze)
        {
            return maze.FindGraphEdgesDFS(
                 edge => maze.AreCellsConnected(edge.Cell1, edge.Cell2));
        }

        private async Task ExportDirectedEdgesAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (_edges == null || !_edges.Any()) return;

            await svgWriter.StartElementAsync(_group!);
            foreach (var edge in _edges)
            {
                PathSegment currentSegment = exporter.Maze.GetPathBetweenCells(edge);
                _path.D = currentSegment.MoveNext(null, exporter.CellSize, exporter.Offset);

                await svgWriter.StartElementAsync(_path);
                await svgWriter.EndElementAsync();
            }
            await svgWriter.EndElementAsync();
        }

        private async Task ExportUndirectedEdgesAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            _edges ??= _baseGraphEdges ?
                BaseGraphEdges(exporter.Maze) :
                PassagesGraphEdges(exporter.Maze);

            if (!_edges.Any()) return;

            StringBuilder pathBuilder = new StringBuilder();
            PathSegment? previousSegment = null;

            void ExportEdge(MazeEdge edge)
            {
                PathSegment currentSegment = exporter.Maze.GetPathBetweenCells(edge);
                pathBuilder.Append(
                    currentSegment.MoveNext(previousSegment, exporter.CellSize, exporter.Offset)
                    );
                previousSegment = currentSegment;
            }

            foreach (var edge in _edges)
            {
                ExportEdge(edge);
            }
            if (_includeExits)
            {
                foreach (var path in exporter.Maze.Paths)
                {
                    ExportEdge(path.Entry);
                    ExportEdge(path.Exit);
                }
            }

            _path.D = pathBuilder.ToString();
            await svgWriter.StartElementAsync(_path);
            await svgWriter.EndElementAsync();
        }

        public Task ExportAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            return _areEdgesDirected ?
                ExportDirectedEdgesAsync(exporter, svgWriter) :
                ExportUndirectedEdgesAsync(exporter, svgWriter);
        }

        /// <summary>
        /// Export maze edges of base graph.
        /// </summary>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="includeExits">
        /// If <see langword="true"/> then edges that lead to entries/exits will be displayed too.
        /// </param>
        public static Edges OfBaseGraph(SvgPath? path = null, bool includeExits = true)
        {
            return new Edges(path, true, includeExits);
        }

        /// <summary>
        /// Export maze edges of passages graph.
        /// </summary>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="includeExits">
        /// If <see langword="true"/> then edges that lead to entries/exits will be displayed too.
        /// </param>
        public static Edges OfPassagesGraph(SvgPath? path = null, bool includeExits = true)
        {
            return new Edges(path, false, includeExits);
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

        /// <summary>
        /// Export edges as separate paths, order of each edge's vertices matters.
        /// </summary>
        /// <param name="directedEdges">Edges to export, order of vertices matters.</param>
        /// <param name="group">
        /// A group to place edges in. If <see langword="null" /> then a default group will be used.
        /// </param>
        /// <param name="path">
        /// Optional path used for edges.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        public static Edges Directed(IEnumerable<MazeEdge> directedEdges,
            SvgGroup? group = null, SvgPath? path = null)
        {
            return new Edges(path, group, directedEdges);
        }
    }
}
