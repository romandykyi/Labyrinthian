//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using System.Numerics;
//using System.Linq;

//namespace Labyrinthian.Svg
//{
//    /// <summary>
//    /// Class that can export mazes into SVG format.
//    /// </summary>
//    /// <example>
//    /// Export an orthogonal maze 10x10 generated using Wilson's algorithm.
//    /// <code>
//    /// Maze maze = new OrthogonalMaze(10, 10);
//    /// MazeGenerator generator = new WilsonGeneration(maze);
//    /// generator.Generate();
//    /// using (var fs = new FileStream(@"D:\Pictures\Maze.svg", FileMode.Create))
//    /// using (var svgExporter = new MazeSvgExporter(maze, fs))
//    /// {
//    ///    var fill = new SvgColorFill(SvgColor.Black);
//    ///    var stroke = new SvgStroke(2f, fill);
//    ///    svgExporter.DrawWalls(stroke);
//    /// }
//    /// </code>
//    /// </example>
//    public sealed class MazeSvgExporter : IDisposable
//    {
//        public const float DefaultCellSize = 32f;
//        public const float DefaultStrokeWidth = 2f;

//        private readonly bool _closeWriter;

//        /// <summary>
//        /// Maze that's being exported.
//        /// </summary>
//        public readonly Maze Maze;
//        /// <summary>
//        /// Size of a maze cell.
//        /// </summary>
//        public readonly float CellSize;
//        /// <summary>
//        /// Offset from top and left.
//        /// </summary>
//        public readonly float Offset;

//        private readonly TextWriter _writer;
//        private readonly StringBuilder _definitions;

//        /// <summary>
//        /// Width of SVG document(in pixels).
//        /// </summary>
//        public readonly float Width;
//        /// <summary>
//        /// Height of SVG document(in pixels).
//        /// </summary>
//        public readonly float Height;

//        private void ApplyFill(SvgFill fill)
//        {
//            //string? definition = fill.Definition;
//            //if (definition != null) _definitions.Append(definition);
//        }

//        /// <summary>
//        /// Make an SVG exporter using <see cref="Stream"/>.
//        /// </summary>
//        /// <param name="maze">Maze that we need to export.</param>
//        /// <param name="stream">Stream which will be used for exporting.</param>
//        /// <param name="cellSize">Size of one cell(in pixels).</param>
//        /// <param name="wallsWidth">Width of walls.</param>
//        /// <param name="padding">Padding.</param>
//        /// <exception cref="ArgumentNullException" />
//        /// <exception cref="NotImplementedException" />
//        public MazeSvgExporter(Maze maze, Stream stream,
//            float cellSize = DefaultCellSize, float wallsWidth = DefaultStrokeWidth, float padding = 0f) :
//            this(maze, new StreamWriter(stream, Encoding.UTF8, 1024, true),
//                cellSize, wallsWidth, padding, true)
//        { }

//        /// <summary>
//        /// Make an SVG exporter using <see cref="StreamWriter"/>.
//        /// </summary>
//        /// <param name="maze">Maze that we need to export.</param>
//        /// <param name="textWriter">TextWriter which will be used for exporting.</param>
//        /// <param name="cellSize">Size of one cell(in pixels).</param>
//        /// <param name="wallsWidth">Width of walls.</param>
//        /// <param name="padding">Padding.</param>
//        /// <param name="closeWriter">
//        /// If <see langword="true" />, <paramref name="textWriter"/> 
//        /// will be closed after exporting is done.
//        /// </param>
//        /// <exception cref="ArgumentNullException" />
//        /// <exception cref="NotImplementedException" />
//        public MazeSvgExporter(Maze maze, TextWriter textWriter,
//            float cellSize = DefaultCellSize, float wallsWidth = DefaultStrokeWidth,
//            float padding = 0f, bool closeWriter = false)
//        {
//            if (maze == null) throw new ArgumentNullException(nameof(maze));
//            if (maze.Dimensions != 2)
//            {
//                throw new NotImplementedException("SVG export of non 2D dimensional mazes is not supported yet.");
//            }

//            Maze = maze;
//            _writer = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
//            CellSize = cellSize;
//            WallsWidth = wallsWidth;
//            _closeWriter = closeWriter;

//            Offset = wallsWidth / 2f + padding;
//            Width = Maze.Sizes[0] * cellSize + Offset * 2f;
//            Height = Maze.Sizes[1] * cellSize + Offset * 2f;

//            Maze = maze;

//            _writer.Write("<?xml version=\"1.0\" standalone=\"yes\"?>");
//            _writer.Write("<svg ");
//            _writer.Write("xmlns=\"http://www.w3.org/2000/svg\" ");
//            _writer.Write("xmlns:xlink=\"http://www.w3.org/1999/xlink\" ");
//            _writer.Write("version=\"1.1\" ");
//            _writer.Write($"width=\"{Width.ToInvariantString()}\" ");
//            _writer.Write($"height=\"{Height.ToInvariantString()}\" >");

//            _definitions = new StringBuilder();
//        }


//        /// <summary>
//        /// Draw walls using fill, specified in the constructor width and square linecap.
//        /// </summary>
//        /// <param name="fill">Fill of the walls.</param>
//        /// <param name="separatePaths">
//        /// If <see langword="true"/> one &lt;path&gt; will be used for one wall;
//        /// otherwise, all walls will be drawn only with single &lt;path&gt;
//        /// </param>
//        /// <exception cref="ArgumentNullException"/>
//        public void DrawWalls(SvgFill fill, bool separatePaths = false)
//        {
//            var stroke = new SvgStroke(WallsWidth, fill, SvgStroke.StrokeLinecap.Square);
//            DrawWalls(stroke, separatePaths);
//        }

//        /// <summary>
//        /// Draw walls.
//        /// </summary>
//        /// <param name="stroke">Stroke of the walls.</param>
//        /// <param name="separatePaths">
//        /// If <see langword="true"/> one &lt;path&gt; will be used for one wall;
//        /// otherwise, all walls will be drawn only with single &lt;path&gt;
//        /// </param>
//        /// <exception cref="ArgumentNullException"/>
//        public void DrawWalls(SvgStroke stroke, bool separatePaths = false)
//        {
//            if (stroke == null)
//                throw new ArgumentNullException(nameof(stroke));

//            ApplyFill(stroke.Fill);

//            _writer.Write("<g id=\"walls\" ");
//            _writer.Write("fill=\"none\" ");
//            _writer.Write(stroke);
//            _writer.Write(">");

//            if (separatePaths) DrawWalls();
//            else DrawWallsAsOnePath();

//            _writer.Write($"</g>");
//        }

//        /// <summary>
//        /// Write a metadata about generation of the maze.
//        /// </summary>
//        public void WriteMetadata()
//        {
//            _writer.Write("<metadata>");
//            _writer.Write($"<desc>{Maze.Description}</desc>");
//            _writer.Write("<generator>Labyrinthian</generator>");
//            _writer.Write($"</metadata>");
//        }

//        /// <summary>
//        /// Add background.
//        /// </summary>
//        /// <param name="fill">Fill used for background.</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void AddBackground(SvgFill fill)
//        {
//            if (fill == null)
//                throw new SvgFillNullException(nameof(fill));

//            ApplyFill(fill);
//            _writer.Write($"<rect id=\"background\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"{fill}\"/>");
//        }

//        /// <summary>
//        /// Draw all solutions(<see cref="Maze.Paths"/>).
//        /// </summary>
//        /// <param name="strokes">
//        /// Strokes used for each solution. 
//        /// If length is less than solutions number, than some strokes will be used more than once.
//        /// Can't be empty.
//        /// </param>
//        /// <exception cref="ArgumentException"/>
//        /// <exception cref="ArgumentNullException"/>
//        public void DrawSolutions(params SvgStroke[] strokes)
//        {
//            if (strokes == null)
//                throw new ArgumentNullException(nameof(strokes), "fills cannot be null");
//            if (strokes.Length == 0)
//            {
//                throw new ArgumentException($"{nameof(strokes)} array cannot be empty", nameof(strokes));
//            }
//            if (Maze.Paths.Count == 0) return;

//            _writer.Write("<g id=\"solutions\" fill=\"none\">");
//            for (int i = 0; i < Maze.Paths.Count; ++i)
//            {
//                // Apply fill only if it wasn't applied.
//                if (i < strokes.Length)
//                {
//                    ApplyFill(strokes[i].Fill);
//                }
//                string stroke = strokes[i % strokes.Length].ToString();
//                _writer.Write($"{Maze.Paths[i].ToSVG(CellSize, Offset, stroke)}");
//            }
//            _writer.Write("</g>");
//        }

//        /// <summary>
//        /// Draw cells. Same as <see cref="FillCells(IEnumerable{MazeCell}, SvgFill, string?)"/>.
//        /// </summary>
//        /// <param name="fill">Fill, used for cells.</param>
//        /// <param name="gID">ID of cells group(optional).</param>
//        /// <param name="cells">Cells that will be drawn.</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void FillCells(SvgFill fill, string? gID = null, params MazeCell[] cells)
//        {
//            FillCells(cells, fill, gID);
//        }

//        /// <summary>
//        /// Draw cells.
//        /// </summary>
//        /// <param name="fill">Fill, used for cells.</param>
//        /// <param name="gID">ID of cells group(optional).</param>
//        /// <param name="cells">Cells that will be drawn.</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void FillCells(IEnumerable<MazeCell> cells, SvgFill fill, string? gID = null)
//        {
//            if (cells == null)
//                throw new SvgFillNullException(nameof(cells));

//            string fillStr = fill.ToString();
//            ApplyFill(fill);

//            _writer.Write($"<g ");
//            if (!string.IsNullOrEmpty(gID)) _writer.Write($"id=\"{gID}\" ");
//            _writer.Write($"stroke=\"{fillStr}\" ");
//            _writer.Write($"stroke-width=\"{WallsWidth.ToInvariantString()}\" ");
//            _writer.Write($"fill=\"{fillStr}\">");
//            foreach (MazeCell cell in cells)
//            {
//                string line = Maze.CellToSvgString(cell, CellSize, Offset);
//                _writer.Write(line);
//            }
//            _writer.Write("</g>");
//        }

//        /// <summary>
//        /// Draw all cells.
//        /// </summary>
//        /// <param name="fill">Fill, used for cells.</param>
//        /// <param name="gID">ID of cells group(optional).</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void FillAllCells(SvgFill fill, string? gID = null)
//        {
//            FillCells(Maze.Cells, fill, gID);
//        }

//        /// <summary>
//        /// Draw nodes of maze's base graph as circles.
//        /// </summary>
//        /// <param name="nodes">Nodes that will be drawn.</param>
//        /// <param name="circleFill">Fill used for node.</param>
//        /// <param name="circleStroke">Stroke used for node.</param>
//        /// <param name="circleRadius">Radius of node.</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void DrawMazeGraphNodes(IEnumerable<MazeCell> nodes,
//            SvgFill circleFill, SvgStroke circleStroke,
//            float circleRadius = 4.5f)
//        {
//            if (circleFill == null)
//                throw new SvgFillNullException(nameof(circleFill));
//            if (circleStroke == null)
//                throw new SvgFillNullException(nameof(circleStroke));

//            ApplyFill(circleFill);
//            ApplyFill(circleStroke.Fill);

//            _writer.Write($"<g id=\"nodes\" fill=\"{circleFill}\" ");
//            _writer.Write($"{circleStroke}>");
//            foreach (var cell in nodes)
//            {
//                float[] cellCenter = Maze.GetCellCenter(cell);
//                Vector2 position = Maze.PositionTo2DPoint(cellCenter);
//                position *= CellSize;
//                position.X += Offset;
//                position.Y += Offset;

//                _writer.Write("<circle ");
//                _writer.Write($"cx=\"{position.X.ToInvariantString()}\" ");
//                _writer.Write($"cy=\"{position.Y.ToInvariantString()}\" ");
//                _writer.Write($"r=\"{circleRadius.ToInvariantString()}\"/>");
//            }
//            _writer.Write("</g>");
//        }

//        /// <summary>
//        /// Draw all nodes of maze's base graph as circles.
//        /// </summary>
//        /// <param name="circleFill">Fill, used for node.</param>
//        /// <param name="circleStroke">Edges stroke.</param>
//        /// <param name="circleRadius">Radius of node.</param>
//        /// <exception cref="SvgFillNullException"/>
//        public void DrawAllMazeGraphNodes(SvgFill circleFill, SvgStroke circleStroke,
//            float circleRadius = 4.5f)
//        {
//            DrawMazeGraphNodes(Maze.Cells, circleFill, circleStroke, circleRadius);
//        }

//        /// <summary>
//        /// Draw edges of the maze.
//        /// </summary>
//        /// <param name="edges">Edges that will be drawn.</param>
//        /// <param name="stroke">Edges stroke.</param>
//        /// <param name="gID">ID of edges group(optional).</param>
//        /// <exception cref="ArgumentNullException" />
//        /// <exception cref="SvgFillNullException" />
//        public void DrawEdges(IEnumerable<MazeEdge> edges, SvgStroke stroke, string? gID = null)
//        {
//            if (stroke == null)
//                throw new SvgFillNullException(nameof(stroke));
//            if (edges is null)
//                throw new ArgumentNullException(nameof(edges));

//            if (!edges.Any()) return;

//            _writer.Write("<path ");
//            if (!string.IsNullOrEmpty(gID)) _writer.Write($"id=\"{gID}\" ");
//            _writer.Write("d=\"");

//            MazeCell firstCell = Maze.Cells[0];

//            MarkedCells visited = new MarkedCells(Maze);
//            visited[firstCell] = true;

//            PathSegment? previousSegment = null;
//            foreach (var edge in edges)
//            {
//                PathSegment currentSegment = Maze.GetPathBetweenCells(edge);
//                _writer.Write(currentSegment.MoveNext(previousSegment, CellSize, Offset));
//                previousSegment = currentSegment;
//            }

//            _writer.Write($"\" {stroke}/>");

//            ApplyFill(stroke.Fill);
//        }

//        /// <summary>
//        /// Draw edges of the base graph of the maze.
//        /// </summary>
//        /// <param name="stroke">Edges stroke.</param>
//        /// <param name="gID">ID of edges group(optional).</param>
//        /// <exception cref="SvgFillNullException" />
//        public void DrawBaseGraphEdges(SvgStroke stroke, string? gID = null)
//        {
//            DrawEdges(Maze.GetBaseGraphEdges(), stroke, gID);
//        }

//        /// <summary>
//        /// Draw edges of the passages graph of the maze.
//        /// </summary>
//        /// <param name="stroke">Edges stroke.</param>
//        /// <param name="includeExits">
//        /// If <see langword="true"/> then edges that lead to entries/exits will be 
//        /// also drawn.
//        /// </param>
//        /// <param name="gID">ID of edges group(optional).</param>
//        /// <exception cref="SvgFillNullException" />
//        public void DrawPassagesGraphEdges(SvgStroke stroke,
//            bool includeExits = true, string? gID = null)
//        {
//            var edges = Maze.FindGraphEdgesDFS(
//                edge => Maze.AreCellsConnected(edge.Cell1, edge.Cell2), includeExits);
//            DrawEdges(edges, stroke, gID);
//        }

//        /// <summary>
//        /// Finish exporting and dispose used resources.
//        /// </summary>
//        public void Dispose()
//        {
//            if (_definitions.Length > 0)
//            {
//                _writer.Write("<defs>");
//                _writer.Write(_definitions.ToString());
//                _writer.Write("</defs>");
//            }
//            _writer.Write("</svg>");
//            if (_closeWriter) _writer.Close();
//        }

//        /// <summary>
//        /// Finish exporting and dispose used resources.
//        /// Same as calling <see cref="Dispose"/>.
//        /// </summary>
//        public void Close()
//        {
//            Dispose();
//        }

//        /// <summary>
//        /// Pre-calculate width and height of the maze without exporting it.
//        /// </summary>
//        public static void GetSizes(Maze maze, float cellSize, float padding,
//            float wallsStroke, out float width, out float height)
//        {
//            width = maze.Sizes[0] * cellSize + wallsStroke + padding * 2f;
//            height = maze.Sizes[1] * cellSize + wallsStroke + padding * 2f;
//        }
//    }
//}
