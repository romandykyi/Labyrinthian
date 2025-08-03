using System;
using System.Collections.Generic;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// A builder for <see cref="MazeSvgExporter"/> instances.
    /// </summary>
    public class MazeSvgExporterBuilder
    {
        private readonly Maze _maze;
        private float _cellSize = 20f;
        private float _padding = 10f;

        private readonly List<IExportModule> _exportModules = new List<IExportModule>();

        private MazeSvgExporterBuilder(Maze maze)
        {
            _maze = maze ?? throw new ArgumentNullException(nameof(maze));
        }

        public static MazeSvgExporterBuilder For(Maze maze)
        {
            return new MazeSvgExporterBuilder(maze);
        }

        /// <summary>
        /// Sets the cell size for the SVG export.
        /// </summary>
        /// <param name="size">Size of the cell in pixels.</param>
        public MazeSvgExporterBuilder WithCellSize(float size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Cell size must be greater than zero.");
            }

            _cellSize = size;
            return this;
        }

        /// <summary>
        /// Sets the padding for the SVG export.
        /// </summary>
        /// <param name="padding">Padding in pixels.</param>
        public MazeSvgExporterBuilder WithPadding(float padding)
        {
            if (padding < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(padding), "Padding cannot be negative.");
            }

            _padding = padding;
            return this;
        }

        /// <summary>
        /// Adds a custom export module to the exporter.
        /// </summary>
        /// <param name="exportModule">The export module to add.</param>
        public MazeSvgExporterBuilder AddExportModule(IExportModule exportModule)
        {
            _exportModules.Add(exportModule ?? throw new ArgumentNullException(nameof(exportModule)));
            return this;
        }

        /// <summary>
        /// Builds the <see cref="MazeSvgExporter"/> instance.
        /// </summary>
        public MazeSvgExporter Build()
        {
            return new MazeSvgExporter(_maze, _exportModules, _cellSize, _padding);
        }
    }
}
