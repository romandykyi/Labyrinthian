using System.Text;
using System.Threading.Tasks;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for maze walls.
    /// </summary>
    public sealed class Walls : IExportModule
    {
        private readonly SvgPath _path;
        private readonly SvgGroup? _group;
        private readonly bool _separatePaths;

        /// <summary>
        /// 'stroke-width' of walls in pixels.
        /// </summary>
        internal readonly float WallsWidth;

        private Walls(SvgPath path, float wallsWidth, bool separatePaths, SvgGroup? group = null)
        {
            _path = path;
            WallsWidth = wallsWidth;
            _separatePaths = separatePaths;
            _group = group;
        }

        private async Task ExportAsSeparatePaths(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            await svgWriter.StartElementAsync(_group!);
            foreach (var wall in exporter.Maze.GetWalls())
            {
                _path.D = exporter.Maze.GetWallPosition(wall).
                    FromStartToEnd(exporter.CellSize, exporter.Offset);

                await svgWriter.StartElementAsync(_path);
                await svgWriter.EndElementAsync();
            }
            await svgWriter.EndElementAsync();
        }

        private async Task ExportAsOnePath(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            PathSegment? previous = null;
            StringBuilder pathD = new StringBuilder();
            foreach (var wall in exporter.Maze.GetWalls())
            {
                PathSegment segment = exporter.Maze.GetWallPosition(wall);
                string line = segment.MoveNext(previous, exporter.CellSize, exporter.Offset);
                pathD.Append(line);

                previous = segment;
            }
            _path.D = pathD.ToString();
            await svgWriter.StartElementAsync(_path);
            await svgWriter.EndElementAsync();
        }

        public async Task ExportAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (_separatePaths)
            {
                await ExportAsSeparatePaths(exporter, svgWriter);
            }
            else
            {
                await ExportAsOnePath(exporter, svgWriter);
            }
        }

        /// <summary>
        /// When calling this method, walls will be exported using
        /// one &lt;path&gt; element.
        /// </summary>
        /// <param name="path">
        /// Optional template path('stroke-width' will be set to <paramref name="wallsWidth"/>).
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is recommended.
        /// </param>
        /// <param name="wallsWidth">Walls width in pixels.</param>
        public static Walls AsOnePath(SvgPath? path = null, float wallsWidth = 2f)
        {
            path ??= new SvgPath()
            {
                Fill = SvgFill.None,
                Stroke = SvgColor.Black,
                StrokeWidth = 2f,
                StrokeLinecap = SvgLinecap.Square
            };
            return new Walls(path, wallsWidth, false);
        }

        /// <summary>
        /// When calling this method, walls will be exported using
        /// many &lt;path&gt; elements.
        /// </summary>
        /// <param name="group">
        /// Optional params of group which will contain paths.
        /// Setting 'Fill' to <see cref="SvgFill.None"/> is recommended.
        /// </param>
        /// <param name="path">
        /// Optional template path('stroke-width' will be set to <paramref name="wallsWidth"/>).
        /// It's recommended to set all path properties inside <paramref name="group"/>.
        /// </param>
        /// <param name="wallsWidth">Walls width in pixels.</param>
        public static Walls AsSeparatePaths(SvgGroup? group = null, float wallsWidth = 2f, SvgPath? path = null)
        {
            path ??= new SvgPath();
            group ??= new SvgGroup()
            {
                Fill = SvgFill.None,
                Stroke = SvgColor.Black,
                StrokeWidth = 2f,
                StrokeLinecap = SvgLinecap.Square
            };
            return new Walls(path, wallsWidth, true, group);
        }
    }
}
