using System.Linq;
using System.Text;

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

        private Solutions(SvgGroup? group, PathCreator? pathCreator)
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
        }

        private static string GetSvgPath(MazeSvgExporter exporter, MazePath path)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var segments = path.GetSegments();
            // Move to the start at the beginning(we shouldn't use MoveNext here)
            stringBuilder.Append(segments.First().MoveToStartSvg(exporter.CellSize, exporter.Offset));
            foreach (var segment in segments)
            {
                stringBuilder.Append(segment.MoveToEndSvg(exporter.CellSize, exporter.Offset));
            }
            return stringBuilder.ToString();
        }

        public void Export(MazeSvgExporter exporter, SvgWriter svgWriter)
        {
            if (exporter.Maze.Paths.Count == 0) return;

            svgWriter.StartElement(_group);
            for (int i = 0; i < exporter.Maze.Paths.Count; ++i)
            {
                SvgPath path = _pathCreator(i);
                path.D = GetSvgPath(exporter, exporter.Maze.Paths[i]);

                svgWriter.StartElement(path);
                svgWriter.EndElement();
            }
            svgWriter.EndElement();
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
        public static Solutions All(SvgGroup? group = null, PathCreator? pathCreator = null)
        {
            return new Solutions(group, pathCreator);
        }
    }
}
