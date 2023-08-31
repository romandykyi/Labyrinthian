//using System;
//using System.Runtime.CompilerServices;
//using System.Text;

//namespace Labyrinthian.Svg
//{
//    public sealed class Walls : IExportModule
//    {
//        private const float DefaultStrokeWidth = 2f;
//        private const SvgStroke.StrokeLinecap DefaultLinecap = SvgStroke.StrokeLinecap.Square;

//        private readonly bool _separatePaths;
        
//        public readonly SvgStroke Stroke;

//        private Walls(SvgStroke stroke, bool separatePaths)
//        {
//            Stroke = stroke;
//            _separatePaths = separatePaths;
//        }

//        public SvgTag Export(Maze maze)
//        {
//            throw new NotImplementedException();
//        }

//        private static SvgStroke GetDefaultStroke(SvgFill fill)
//        {
//            if (fill == null)
//            {
//                throw new SvgFillNullException(nameof(fill));
//            }

//            return new SvgStroke(DefaultStrokeWidth, fill, DefaultLinecap);
//        }

//        private SvgTag AsOnePath(MazeSvgExporter exporter)
//        {
//            SvgTag path = new SvgTag("path");
//            PathSegment? previous = null;
//            StringBuilder pathD = new StringBuilder();
//            foreach (var wall in exporter.Maze.GetWalls())
//            {
//                PathSegment segment = exporter.Maze.GetWallPosition(wall);
//                string line = segment.MoveNext(previous, exporter.CellSize, exporter.Offset);
//                pathD.Append(line);

//                previous = segment;
//            }
//            path.AddAttribute("d", pathD);
//            return path;
//        }

//        private SvgTag AsSeparatePaths(MazeSvgExporter exporter)
//        {
//            SvgTag group = new SvgTag("g");
//            foreach (var wall in exporter.Maze.GetWalls())
//            {
//                SvgTag path = new SvgTag("path");
//                string pathD = 
//                    exporter.Maze.GetWallPosition(wall).
//                    FromStartToEnd(exporter.CellSize, exporter.Offset);
//                path.AddAttribute("d", pathD);
//                group.Children.Add(path);
//            }
//            return group;
//        }

//        public static Walls AsOnePath(SvgStroke stroke)
//        {
//            return new Walls(stroke, false);
//        }

//        public static Walls AsSeparatePaths(SvgStroke stroke)
//        {
//            return new Walls(stroke, true);
//        }
//    }
//}
