//using NUnit.Framework;
//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using Labyrinthian.Svg;

//namespace Labyrinthian.Tests.Svg
//{
//    internal class SvgExporterTest
//    {
//        private class TestWriter : TextWriter
//        {
//            public bool Disposed { get; private set; } = false;

//            public override Encoding Encoding => Encoding.Default;

//            protected override void Dispose(bool disposing)
//            {
//                base.Dispose(disposing);
//                Disposed = true;
//            }
//        }

//        private Maze maze;
//        private readonly SvgColorFill defaultFill = new(SvgColor.Black);
//        private readonly SvgStroke defaultStroke =
//            new(2f, new SvgColorFill(SvgColor.Blue), SvgStroke.StrokeLinecap.Square, 1f, 2f);

//        private static void CheckXml(string xml)
//        {
//            XmlDocument xmlDoc = new();
//            Assert.DoesNotThrow(() => xmlDoc.LoadXml(xml), $"Invalid xml: {xml}");
//        }

//        [OneTimeSetUp]
//        public void Init()
//        {
//            maze = new OrthogonalMaze(10, 10, 2, 2);
//            var outerWalls = maze.GetOuterWalls().ToArray();
//            maze.Paths.Add(new(maze, outerWalls[0], outerWalls[^8]));
//            maze.Paths.Add(new(maze, outerWalls[9], outerWalls[^1]));
//            MazeGenerator generator = new PrimGeneration(maze, 15_031_989);
//            generator.Generate();
//        }

//        [Test]
//        public void DoesNotCloseStream()
//        {
//            using MemoryStream ms = new();
//            using (MazeSvgExporter exporter = new(maze, ms)) { }
//            Assert.That(ms.CanWrite, Is.True);
//        }

//        [Test]
//        public void DoesNotCloseWriter()
//        {
//            using TestWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer)) { }
//            Assert.That(writer.Disposed, Is.False);
//        }

//        [Test]
//        public void ClosesWriterWhenNeeded()
//        {
//            TestWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer, closeWriter: true)) { }
//            Assert.That(writer.Disposed, Is.True);
//        }

//        [Test]
//        public void WriteMetadataValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.WriteMetadata();
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void DrawWallsValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.DrawWalls(defaultFill);
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void AddBackgroundValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.AddBackground(defaultFill);
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void DrawSolutionsValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.DrawSolutions(defaultStroke);
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void DrawSolutionsDoesNotAcceptsEmptyStrokes()
//        {
//            using StringWriter writer = new();
//            using MazeSvgExporter exporter = new(maze, writer);
//            Assert.Throws<ArgumentException>(() => exporter.DrawSolutions());
//        }

//        [Test]
//        public void FillAllCellsValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.FillAllCells(defaultFill);
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void DrawAllMazeGraphNodesValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.DrawAllMazeGraphNodes(defaultFill, defaultStroke);
//            }
//            CheckXml(writer.ToString());
//        }

//        [Test]
//        public void DrawPassagesGraphEdgesValidXml()
//        {
//            using StringWriter writer = new();
//            using (MazeSvgExporter exporter = new(maze, writer))
//            {
//                exporter.DrawPassagesGraphEdges(defaultStroke);
//            }
//            CheckXml(writer.ToString());
//        }
//    }
//}
