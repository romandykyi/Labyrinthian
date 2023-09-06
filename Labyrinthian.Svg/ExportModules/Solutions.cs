using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Export module for solutions.
    /// </summary>
    public sealed class Solutions : IExportModule
    {
        /// <summary>
        /// Delegate for creating a path for a maze solution.
        /// </summary>
        /// <param name="solutionIndex">
        /// Index of the created solution.
        /// </param>
        /// <returns>
        /// Path that represents a solution('d' property will be overwritten).
        /// </returns>
        public delegate SvgPath PathCreator(int solutionIndex);

        private readonly SvgGroup _group;
        private readonly PathCreator _pathCreator;
        private readonly bool _intersectOuterCells;

        private Solutions(SvgGroup? group, PathCreator? pathCreator, bool intersectOuterCells)
        {
            if (group == null)
            {
                _group = new SvgGroup()
                {
                    Fill = SvgFill.None,
                    Stroke = SvgColor.Blue,
                    StrokeWidth = 2f
                };
            }
            else
            {
                _group = group;
            }
            _pathCreator = pathCreator ?? (_ => new SvgPath());
            _intersectOuterCells = intersectOuterCells;
        }

        private string GetSvgPath(MazeSvgExporter exporter, MazePath path)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var segments = path.GetSegments(_intersectOuterCells);
            // Move to the start at the beginning(we shouldn't use MoveNext here)
            stringBuilder.Append(segments.First().MoveToStartSvg(exporter.CellSize, exporter.Offset));
            foreach (var segment in segments)
            {
                stringBuilder.Append(segment.MoveToEndSvg(exporter.CellSize, exporter.Offset));
            }
            return stringBuilder.ToString();
        }

        public async Task ExportAsync(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (exporter.Maze.Paths.Count == 0) return;

            await svgWriter.StartElementAsync(_group);
            for (int i = 0; i < exporter.Maze.Paths.Count; ++i)
            {
                SvgPath path = _pathCreator(i);
                path.D = GetSvgPath(exporter, exporter.Maze.Paths[i]);

                await svgWriter.StartElementAsync(path);
                await svgWriter.EndElementAsync();
            }
            await svgWriter.EndElementAsync();
        }

        /// <summary>
        /// Export all solutions.
        /// </summary>
        /// <param name="group">
        /// Optional params for the group in which all solutions will be placed.
        /// Setting <see cref="SvgGroup.Fill"/> to <see cref="SvgFill.None"/> is
        /// recommended for this argument.
        /// </param>
        /// <param name="pathCreator">
        /// Creator for each path.
        /// </param>
        /// <param name="intersectOuterCells">
        /// If <see langword="true"/> then solutions lines will intersect outer cells.
        /// </param>
        public static Solutions All(SvgGroup? group = null, PathCreator? pathCreator = null,
            bool intersectOuterCells = true)
        {
            return new Solutions(group, pathCreator, intersectOuterCells);
        }
    }
}
