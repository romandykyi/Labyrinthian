using Labyrinthian.Svg;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Xml;

namespace Labyrinthian.Tests.Svg
{
    internal class MazeSvgExporterTest
    {
        private static SvgPath PathCreator(int solutionIndex)
        {
            if (solutionIndex % 2 == 0)
            {
                return new SvgPath()
                {
                    Stroke = SvgColor.Blue
                };
            }

            return new SvgPath()
            {
                Stroke = SvgColor.Red,
                StrokeDasharray = [5f]
            };
        }

        [Test]
        public void ValidXml()
        {
            // Create an orthogonal maze with arbitary entries and exits.
            Maze maze = new OrthogonalMaze(10, 10, 2, 2);
            var outerWalls = maze.GetOuterWalls().ToArray();
            maze.Paths.Add(new(maze, outerWalls[0], outerWalls[^8]));
            maze.Paths.Add(new(maze, outerWalls[9], outerWalls[^1]));
            // Generate it using Prim's algorithm
            MazeGenerator generator = new PrimGeneration(maze, 15_031_989);
            generator.Generate();

            SvgGroup cellsGroup = new()
            {
                Fill = SvgColor.Cyan
            };
            MazeSvgExporter exporter = new(maze, 32f, 5f)
            {
                Background.Create(SvgColor.Orange),
                Cells.All(cellsGroup),
                Walls.AsOnePath(),
                Edges.OfPassagesGraph(),
                Solutions.All(pathCreator: PathCreator),
                Nodes.All()
            };

            string xml;
            using (StringWriter stringWriter = new())
            {
                using (SvgWriter svgWriter = new(stringWriter))
                {
                    exporter.Export(svgWriter);
                }
                xml = stringWriter.ToString();
            }

            XmlDocument xmlDoc = new();
            Assert.DoesNotThrow(() => xmlDoc.LoadXml(xml), $"Invalid xml: {xml}");
        }
    }
}
