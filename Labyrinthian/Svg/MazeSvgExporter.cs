using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;
using System.Linq;

namespace Labyrinthian
{
    /// <summary>
    /// ���� ��� �������� �������� � ������ SVG
    /// </summary>
    public sealed class MazeSvgExporter : IDisposable
    {
        private readonly bool _closeWriter;

        public const float DefaultCellSize = 32f;
        public const float DefaultStrokeWidth = 2f;

        private readonly Maze _maze;
        private readonly float _cellSize, _wallsStroke, _offset;

        private readonly StreamWriter _writer;
        private readonly StringBuilder _definitions;

        /// <summary>
        /// ������� ����� Svg
        /// </summary>
        public readonly float Width;
        /// <summary>
        /// ������ ����� Svg
        /// </summary>
        public readonly float Height;

        private void ApplyFill(SvgFill fill)
        {
            string? definition = fill.Definition;
            if (definition != null) _definitions.Append(definition);
        }

        /// <summary>
        /// �������� ���������
        /// </summary>
        /// <param name="maze">�������, ���� ���� ��������������</param>
        /// <param name="stream">����, ��� ����� ���������� StreamWriter. ��� ���� �� ����� stream</param>
        /// <param name="cellSize">����� �������</param>
        /// <param name="wallsStroke">������ ���</param>
        /// <param name="padding">������ � ��� �����</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="NotImplementedException" />
        public MazeSvgExporter(Maze maze, Stream stream,
            float cellSize = DefaultCellSize, float wallsStroke = DefaultStrokeWidth, float padding = 0f) :
            this(maze, new StreamWriter(stream, Encoding.UTF8, 1024, true),
                cellSize, wallsStroke, padding, true)
        { }

        /// <summary>
        /// �������� ���������
        /// </summary>
        /// <param name="maze">�������, ���� ���� ��������������</param>
        /// <param name="streamWriter">StreamWriter � ���� ���� �������� �����������</param>
        /// <param name="cellSize">����� �������</param>
        /// <param name="wallsStroke">������ ���</param>
        /// <param name="padding">������ � ��� �����</param>
        /// <param name="closeWriter">�� ������� ������� streamWriter �� ��� �������� ����� �����</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="NotImplementedException" />
        public MazeSvgExporter(Maze maze, StreamWriter streamWriter,
            float cellSize = DefaultCellSize, float wallsStroke = DefaultStrokeWidth,
            float padding = 0f, bool closeWriter = false)
        {
            if (maze == null) throw new ArgumentNullException(nameof(maze));
            if (maze.Dimensions != 2)
            {
                throw new NotImplementedException();
            }

            _maze = maze;
            _writer = streamWriter ?? throw new ArgumentNullException(nameof(streamWriter));
            _cellSize = cellSize;
            _wallsStroke = wallsStroke;
            _closeWriter = closeWriter;

            _offset = wallsStroke / 2f + padding;
            Width = _maze.Sizes[0] * cellSize + _offset * 2f;
            Height = _maze.Sizes[1] * cellSize + _offset * 2f;

            _maze = maze;

            _writer.Write("<?xml version=\"1.0\" standalone=\"yes\"?>");
            _writer.Write("<svg ");
            _writer.Write("xmlns=\"http://www.w3.org/2000/svg\" ");
            _writer.Write("xmlns:xlink=\"http://www.w3.org/1999/xlink\" ");
            _writer.Write("version=\"1.1\" ");
            _writer.Write($"width=\"{Width.ToInvariantString()}\" ");
            _writer.Write($"height=\"{Height.ToInvariantString()}\" >");

            _definitions = new StringBuilder();
        }

        private void DrawWallsAsOnePath()
        {
            PathSegment? previous = null;
            _writer.Write("<path d=\"");
            foreach (var wall in _maze.GetWalls())
            {
                PathSegment segment = _maze.GetWallPosition(wall);
                string line = segment.MoveNext(previous, _cellSize, _offset);
                _writer.Write(line);

                previous = segment;
            }
            _writer.Write("\"/>");
        }

        private void DrawWalls()
        {
            foreach (var wall in _maze.GetWalls())
            {
                string d = _maze.GetWallPosition(wall).FromStartToEnd(_cellSize, _offset);
                _writer.Write($"<path d=\"{d}\"/>");
            }
        }

        public static void GetSizes(Maze maze, float cellSize, float padding, float wallsStroke, out float width, out float height)
        {
            width = maze.Sizes[0] * cellSize + wallsStroke + padding * 2f;
            height = maze.Sizes[1] * cellSize + wallsStroke + padding * 2f;
        }

        /// <summary>
        /// ���������� ���� ������ ��������
        /// </summary>
        /// <param name="separatePaths">�� ������� �������� svg-������� path �� ������� ������</param>
        public void DrawWalls(bool separatePaths = false)
        {
            DrawWalls(new SvgColorFill(SvgColor.Black), separatePaths);
        }

        /// <summary>
        /// ���������� ����
        /// </summary>
        /// <param name="fill">������� ���</param>
        /// <param name="separatePaths">�� ������� �������� svg-������� path �� ������� ������</param>
        /// <exception cref="SvgFillNullException"/>
        public void DrawWalls(SvgFill fill, bool separatePaths = false)
        {
            if (fill == null)
                throw new SvgFillNullException(nameof(fill));

            ApplyFill(fill);

            _writer.Write("<g id=\"walls\" ");
            _writer.Write("fill=\"none\" ");
            _writer.Write($"stroke=\"{fill}\" ");
            _writer.Write($"stroke-width=\"{_wallsStroke.ToInvariantString()}\" ");
            _writer.Write("stroke-linecap=\"square\">");

            if (separatePaths) DrawWalls();
            else DrawWallsAsOnePath();

            _writer.Write($"</g>");
        }

        /// <summary>
        /// �������� ������� � ����
        /// </summary>
        public void WriteMetadata()
        {
            _writer.Write("<metadata>");
            _writer.Write($"<desc>{_maze.Description}</desc>");
            _writer.Write("<generator>Labyrinthian</generator>");
            _writer.Write($"</metadata>");
        }

        /// <summary>
        /// ������ ���
        /// </summary>
        /// <param name="fill">������� ����</param>
        /// <exception cref="SvgFillNullException"/>
        public void AddBackground(SvgFill fill)
        {
            if (fill == null)
                throw new SvgFillNullException(nameof(fill));

            ApplyFill(fill);
            _writer.Write($"<rect id=\"background\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"{fill}\"/>");
        }

        /// <summary>
        /// ���������� �� ����'���� �������� Maze.Paths ��������� ��������(����)
        /// </summary>
        /// <param name="strokeWidth">������ �� ����'����</param>
        public void DrawSolutions(float strokeWidth = DefaultStrokeWidth)
        {
            DrawSolutions(strokeWidth, new SvgColorFill(SvgColor.Blue));
        }

        /// <summary>
        /// ���������� �� ����'���� �������� Maze.Paths
        /// </summary>
        /// <param name="fillings">�������, �� ������ ������� ����������������� ��� ��������� ������� ����'����(�� ���� ���� ������)</param>
        /// <param name="strokeWidth">������ �� ����'����</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public void DrawSolutions(float strokeWidth = DefaultStrokeWidth, params SvgFill[] fillings)
        {
            if (fillings == null)
                throw new ArgumentNullException(nameof(fillings), "fillings cannot be null");
            if (fillings.Length == 0)
            {
                throw new ArgumentException($"{nameof(fillings)} array cannot be empty", nameof(fillings));
            }
            if (_maze.Paths.Count == 0) return;

            _writer.Write("<g id=\"solutions\" fill=\"none\" stroke-linecap=\"square\" ");
            _writer.Write($"stroke-width=\"{strokeWidth.ToInvariantString()}\">");
            for (int i = 0; i < _maze.Paths.Count; ++i)
            {
                if (i < fillings.Length)
                {
                    ApplyFill(fillings[i]);
                }
                string color = fillings[i % fillings.Length].ToString();
                _writer.Write($"{_maze.Paths[i].ToSVG(_cellSize, _offset, color)}");
            }
            _writer.Write("</g>");
        }

        /// <summary>
        /// ������ ����� �������
        /// </summary>
        /// <param name="fill">������� �������</param>
        /// <param name="gID">ID ����� �������(���� ���� null)</param>
        /// <param name="cells">�������, �� ������� ������</param>
        /// <exception cref="SvgFillNullException"/>
        public void FillCells(SvgFill fill, string? gID = null, params MazeCell[] cells)
        {
            FillCells(cells, fill, gID);
        }

        /// <summary>
        /// ������ ����� �������
        /// </summary>
        /// <param name="cells">�������, �� ������� ������</param>
        /// <param name="fill">������� �������</param>
        /// <param name="gID">ID ����� �������(���� ���� null)</param>
        /// <exception cref="SvgFillNullException"/>
        public void FillCells(IEnumerable<MazeCell> cells, SvgFill fill, string? gID = null)
        {
            if (cells == null)
                throw new SvgFillNullException(nameof(cells));

            string fillStr = fill.ToString();
            ApplyFill(fill);

            _writer.Write($"<g ");
            if (!string.IsNullOrEmpty(gID)) _writer.Write($"id=\"{gID}\" ");
            _writer.Write($"stroke=\"{fillStr}\" ");
            _writer.Write($"stroke-width=\"{_wallsStroke.ToInvariantString()}\" ");
            _writer.Write($"fill=\"{fillStr}\">");
            foreach (MazeCell cell in cells)
            {
                string line = _maze.CellToSvgString(cell, _cellSize, _offset);
                _writer.Write(line);
            }
            _writer.Write("</g>");
        }

        /// <summary>
        /// ������ �� �������
        /// </summary>
        /// <param name="fill">������� �������</param>
        /// <exception cref="SvgFillNullException"/>
        public void FillAllCells(SvgFill fill) => FillCells(_maze.Cells, fill, "cells");

        /// <summary>
        /// ���������� �� ����� ����� �� �����
        /// </summary>
        /// <param name="circleFill">������� �����</param>
        /// <param name="strokeFill">������� ������� �����</param>
        /// <param name="strokeWidth">������ �������</param>
        /// <param name="circleRadius">����� �����</param>
        /// <param name="cells">�������, �� ������� ������(���� null, �� �� ������� ������ ������)</param>
        /// <exception cref="SvgFillNullException"/>
        public void DrawMazeGraphNodes(SvgFill circleFill, SvgFill strokeFill,
            float strokeWidth = DefaultStrokeWidth, float circleRadius = 4.5f,
            IEnumerable<MazeCell>? cells = null)
        {
            if (circleFill == null)
                throw new SvgFillNullException(nameof(circleFill));
            if (strokeFill == null)
                throw new SvgFillNullException(nameof(strokeFill));

            cells ??= _maze.Cells;

            ApplyFill(circleFill);
            ApplyFill(strokeFill);

            _writer.Write($"<g id=\"nodes\" fill=\"{circleFill}\" ");
            _writer.Write($"stroke=\"{strokeFill}\" ");
            _writer.Write($"stroke-width=\"{strokeWidth}\">");
            foreach (var cell in cells)
            {
                float[] cellCenter = _maze.GetCellCenter(cell);
                Vector2 position = _maze.PositionTo2DPoint(cellCenter);
                position *= _cellSize;
                position.X += _offset;
                position.Y += _offset;

                _writer.Write("<circle ");
                _writer.Write($"cx=\"{position.X.ToInvariantString()}\" ");
                _writer.Write($"cy=\"{position.Y.ToInvariantString()}\" ");
                _writer.Write($"r=\"{circleRadius.ToInvariantString()}\"/>");
            }
            _writer.Write("</g>");
        }

        private IEnumerable<MazeEdge> FindEdges(Predicate<MazeEdge> predicate, bool includeExits = false)
        {
            if (includeExits)
            {
                foreach (var path in _maze.Paths)
                {
                    yield return path.Entry;
                    yield return path.Exit;
                }
            }

            foreach (var edge in _maze.GetGraphEdgesDFS(predicate))
                yield return edge;
        }

        /// <summary>
        /// ���������� ������ ����� ����� ��������
        /// </summary>
        /// <param name="strokeFill">������� �������</param>
        /// <param name="edges">�����, �� ���������</param>
        /// <param name="strokeWidth">������ �������</param>
        /// <param name="gID">ID ����� �����(���� ���� null)</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="SvgFillNullException" />
        public void DrawMazeGraphEdges(SvgFill strokeFill, IEnumerable<MazeEdge> edges,
            float strokeWidth = DefaultStrokeWidth, string? gID = null)
        {
            if (strokeFill == null)
                throw new SvgFillNullException(nameof(strokeFill));
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            if (!edges.Any()) return;

            _writer.Write("<path ");
            if (!string.IsNullOrEmpty(gID)) _writer.Write($"id=\"{gID}\" ");
            _writer.Write("d=\"");

            MazeCell firstCell = _maze.Cells[0];

            MarkedCells visited = new MarkedCells(_maze);
            visited[firstCell] = true;

            PathSegment? previousSegment = null;
            foreach (var edge in edges)
            {
                PathSegment currentSegment = _maze.GetPathBetweenCells(edge);
                _writer.Write(currentSegment.MoveNext(previousSegment, _cellSize, _offset));
                previousSegment = currentSegment;
            }

            _writer.Write($"\" stroke=\"{strokeFill}\" ");
            _writer.Write($"stroke-width=\"{strokeWidth}\" ");
            _writer.Write($"stroke-linecap=\"square\" ");
            _writer.Write($"fill=\"none\"/>");

            ApplyFill(strokeFill);
        }

        /// <summary>
        /// ���������� ������ ����� ����� ��������
        /// </summary>
        /// <param name="strokeFill">�������</param>
        /// <param name="predicate">��������</param>
        /// <param name="includeExits">�� ������� �������� ����� �����/������</param>
        /// <param name="strokeWidth">������ �������</param>
        /// <param name="gID">ID ����� �����(���� ���� null)</param>
        public void DrawMazeGraphEdges(SvgFill strokeFill, Predicate<MazeEdge> predicate,
            bool includeExits = true, float strokeWidth = DefaultStrokeWidth, string? gID = null)
        {
            var edges = FindEdges(predicate, includeExits);
            DrawMazeGraphEdges(strokeFill, edges, strokeWidth, gID);
        }

        /// <summary>
        /// ���������� �� ����� ����� ��������
        /// </summary>
        /// <param name="strokeFill">������� �������</param>
        /// <param name="includeExits">�� ������� �������� ����� �����/������</param>
        /// <param name="strokeWidth">������ �������</param>
        /// <param name="gID">ID ����� �����(���� ���� null)</param>
        /// <exception cref="SvgFillNullException" />
        public void DrawAllMazeGraphEdges(SvgFill strokeFill, bool includeExits = true,
            float strokeWidth = DefaultStrokeWidth, string? gID = null)
        {
            DrawMazeGraphEdges(strokeFill, _ => true, includeExits, strokeWidth, gID);
        }

        /// <summary>
        /// ���������� �� ����� ����� ��������, ����� �� ����� ������
        /// </summary>
        /// <param name="strokeFill">������� �������</param>
        /// <param name="includeExits">�� ������� �������� ����� �����/������</param>
        /// <param name="strokeWidth">������ �������</param>
        /// <param name="gID">ID ����� �����(���� ���� null)</param>
        /// <exception cref="SvgFillNullException" />
        public void DrawMazePassageEdges(SvgFill strokeFill, bool includeExits = true,
            float strokeWidth = DefaultStrokeWidth, string? gID = null)
        {
            DrawMazeGraphEdges(strokeFill, 
                edge => _maze.AreCellsConnected(edge.Cell1, edge.Cell2), 
                includeExits, strokeWidth, gID);
        }

        public void Dispose()
        {
            if (_definitions.Length > 0)
            {
                _writer.Write("<defs>");
                _writer.Write(_definitions.ToString());
                _writer.Write("</defs>");
            }
            _writer.Write("</svg>");
            if (_closeWriter) _writer.Close();
        }

        public void Close()
        {
            Dispose();
        }
    }
}