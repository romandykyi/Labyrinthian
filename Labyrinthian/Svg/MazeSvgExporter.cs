using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Collections;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class that can export mazes into SVG format.
    /// </summary>
    /// <example>
    /// Export an orthogonal maze 10x10 generated using Wilson's algorithm.
    /// <code>
    /// Maze maze = new OrthogonalMaze(10, 10);
    /// MazeGenerator generator = new WilsonGeneration(maze);
    /// generator.Generate();
    /// using (var fs = new FileStream(@"D:\Pictures\Maze.svg", FileMode.Create))
    /// using (var svgExporter = new MazeSvgExporter(maze, fs))
    /// {
    ///    var fill = new SvgColorFill(SvgColor.Black);
    ///    var stroke = new SvgStroke(2f, fill);
    ///    svgExporter.DrawWalls(stroke);
    /// }
    /// </code>
    /// </example>
    public sealed class MazeSvgExporter : IEnumerable<IExportModule>
    {
        private float _cellSize, _padding;

        private Walls? _wallsModule;
        private readonly List<IExportModule> _exportModules = new List<IExportModule>();

        /// <summary>
        /// Maze that's being exported.
        /// </summary>
        public readonly Maze Maze;
        /// <summary>
        /// Size of a maze cell.
        /// </summary>
        public float CellSize
        {
            get => _cellSize;
            set
            {
                _cellSize = value;
                RecalculateSizes();
            }
        }
        /// <summary>
        /// Padding from all sides.
        /// </summary>
        public float Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                RecalculateSizes();
            }
        }
        /// <summary>
        /// Offset from top and left. Can't be set.
        /// </summary>
        public float Offset { get; private set; }

        /// <summary>
        /// Width of SVG document(in pixels). Can't be set.
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// Height of SVG document(in pixels). Can't be set.
        /// </summary>
        public float Height { get; private set; }

        private void RecalculateSizes()
        {
            Offset = Padding;
            if (_wallsModule != null) Offset += _wallsModule.WallsWidth;

            Width = Maze.Width2D * CellSize + Offset * 2f;
            Height = Maze.Height2D * CellSize + Offset * 2f;
        }

        /// <summary>
        /// Create an SVG-exporter.
        /// </summary>
        /// <param name="maze">Maze that will be exported.</param>
        /// <param name="cellSize">Size of one maze cell.</param>
        /// <param name="padding">Padding from all sides.</param>
        public MazeSvgExporter(Maze maze, float cellSize = 32f, float padding = 0f)
        {
            Maze = maze;
            _cellSize = cellSize;
            _padding = padding;
            RecalculateSizes();
        }

        /// <summary>
        /// Add a module for future exporting.
        /// </summary>
        /// <remarks>
        /// Order, in which modules are added, matters. 
        /// For instance, if you add background first and walls later, 
        /// then walls will be drawn in the top of the background.
        /// </remarks>
        /// <param name="exportModule">Export module to be added.</param>
        public void Add(IExportModule exportModule)
        {
            _exportModules.Add(exportModule);

            if (exportModule is Walls wallsModule)
            {
                _wallsModule = wallsModule;
                RecalculateSizes();
            }
        }

        /// <summary>
        /// Export a maze using provided SVG-writer.
        /// </summary>
        /// <param name="svgWriter">
        /// SVG-writer to be used.
        /// </param>
        /// <param name="root">
        /// Optional &lt;svg&gt; element parameters. Note, that 'viewBox' will be 
        /// overwritten.
        /// </param>
        /// <param name="style">
        /// Optional CSS style.
        /// </param>
        /// <exception cref="InvalidOperationException" />
        public void Export(SvgWriter svgWriter, SvgRoot? root = null, string? style = null)
        {
            if (_exportModules.Count == 0)
            {
                throw new InvalidOperationException("Export modules are not added. Please make sure, that before exporting a maze, you have added at least one export module using method Add.");
            }

            SvgRoot svgRoot = root ?? new SvgRoot();
            svgRoot.ViewBox = new SvgViewBox(0f, 0f, Width, Height);

            svgWriter.StartRoot(svgRoot);
            if (style != null)
            {
                svgWriter.WriteStringElement("style", style);
            }
            foreach (IExportModule exportModule in _exportModules)
            {
                exportModule.Export(this, svgWriter);
            }
            svgWriter.EndRoot();
        }
        
        public IEnumerator<IExportModule> GetEnumerator()
        {
            return ((IEnumerable<IExportModule>)_exportModules).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_exportModules).GetEnumerator();
        }
    }
}
