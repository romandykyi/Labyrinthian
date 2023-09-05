using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class that can export mazes into SVG format.
    /// </summary>
    /// <example>
    /// This code snippet demonstrates the creation, generation, and export of an orthogonal maze as an SVG file:
    /// <code>
    /// using Labyrinthian;
    /// using Labyrinthian.Svg;
    /// 
    /// // Create an orthogonal maze 30x20
    /// Maze maze = new OrthogonalMaze(30, 20);
    /// // Generate it using Prim's algorithm
    /// MazeGenerator generator = new PrimGeneration(maze);
    /// generator.Generate();
    /// 
    /// // Create a maze exporter(it doesn't need to be closed or disposed)
    /// MazeSvgExporter exporter = new MazeSvgExporter(maze)
    /// {
    ///     // Here we're adding export modules.
    ///     // You can also use MazeSvgExporter.Add(IExportModule) method for this
    ///     Walls.AsOnePath()
    ///
    ///     /*
    ///      Besides Walls there are others modules, including:
    ///    
    ///       * Background
    ///       * Cells
    ///       * Edges
    ///       * MazeDescription
    ///       * Nodes
    ///       * Solutions
    ///     */
    /// };
    /// 
    /// // Use a FileStream for exporting.
    /// // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
    /// using var fs = File.Create(@"d:\orthogonal-maze.svg");
    /// using var svgWriter = new SvgWriter(fs);
    /// // Export a maze
    /// exporter.Export(svgWriter);
    /// 
    /// // Note: SvgWriter should be disposed after exporting.
    /// // Here we do it with 'using'
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
            if (_wallsModule != null) Offset += _wallsModule.WallsWidth / 2f;

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
        /// Optional &lt;svg&gt; element parameters.
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
            svgRoot.ViewBox ??= new SvgViewBox(0f, 0f, Width, Height);

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
