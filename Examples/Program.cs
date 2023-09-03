using System.Xml;
using Labyrinthian;
using Labyrinthian.Svg;
#pragma warning disable IDE0051 // Remove unused private members

namespace LabyrinthianExamples
{
    public class Program
    {
        private static readonly XmlWriterSettings s_settings = new()
        {
            Indent = true
        };

        // Default orthogonal maze
        private static void ExportOrthogonalMaze()
        {
            // Create an orthogonal maze 30x20
            Maze maze = new OrthogonalMaze(30, 20);
            // Generate it using Prim's algorithm
            MazeGenerator generator = new PrimGeneration(maze);
            generator.Generate();

            // Create a maze exporter(it doesn't need to be closed or disposed)
            MazeSvgExporter exporter = new(maze)
            {
                // Here we're adding export modules.
                // You can also use MazeSvgExporter.Add(IExportModule) method for this
                Walls.AsOnePath()

                /*
                 Besides Walls there are others modules, including:

                    * Background
                    * Cells
                    * Edges
                    * MazeDescription
                    * Nodes
                    * Solutions
                 */
            };

            // Use a FileStream for exporting.
            // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
            using var fs = File.Create(@"d:\orthogonal-maze.svg");
            using var svgWriter = new SvgWriter(fs);
            // Export a maze
            exporter.Export(svgWriter);

            // Note: SvgWriter should be disposed after exporting.
            // Here we do it with 'using'
        }

        // Orthogonal maze with a solution
        private static void ExportOrthogonalMazeWithSolution()
        {
            // Create an orthogonal maze 30x20
            Maze maze = new OrthogonalMaze(30, 20);

            // Specify entry and exit of the maze.
            // You can also do it after maze generation
            MazeEdge entry = maze.GetOuterWalls().First(); // First outer wall
            MazeEdge exit = maze.GetOuterWalls().Last(); // Last outer wall

            // Add a path
            maze.Paths.Add(new(maze, entry, exit));

            // Watch ExportOrthogonalMazeWithMultipleSolutions if you want to know
            // how to specify which walls you want to be entries/exits.

            // Generate maze using Hunt and Kill algorithm
            MazeGenerator generator = new HuntAndKillGeneration(maze);
            generator.Generate();

            // Create a maze exporter(it doesn't need to be closed or disposed)
            MazeSvgExporter exporter = new(maze, padding: 5f)
            {
                // This module will write a description about the maze in SVG-file.
                // It doesn't affect maze appearance
                MazeDescription.Default,
                // Export walls
                Walls.AsOnePath(),
                // And solution
                Solutions.All()
            };

            // Use a FileStream for exporting.
            // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
            using var fs = File.Create(@"d:\orthogonal-maze-with-solution.svg");
            using var svgWriter = new SvgWriter(fs);
            // Export a maze
            exporter.Export(svgWriter);

            // Note: SvgWriter should be disposed after exporting.
            // Here we do it with 'using'
        }

        // Orthogonal maze with multiple solutions
        private static void ExportOrthogonalMazeWithMultipleSolutions()
        {
            // Create an orthogonal maze 25x25
            GridMaze2D maze = new OrthogonalMaze(25, 25);

            // For any GridMaze2D you can use [row, column] indexer.
            // For mazes with custom cells structure or with an inner room
            // it can return null, but in our case it will never return null.
            MazeCell northCell = maze[0, maze.Columns / 2]!;
            MazeCell southCell = maze[maze.Rows - 1, maze.Columns / 2]!;
            MazeCell westCell = maze[maze.Rows / 2, 0]!;
            MazeCell eastCell = maze[maze.Rows / 2, maze.Columns - 1]!;

            // We have entry/exits cells. Now we need to find outer walls
            // to specify entries and exits.
            // We can use 'MazeCell' property 'DirectedNeighbors' for this purpose.
            // Indices there mean directions, for orthogonal mazes it always:
            // 0 - east, 1 - west, 2 - south, 3 - north
            MazeEdge northWall = new(northCell, northCell.DirectedNeighbors[3]!);
            MazeEdge southWall = new(southCell, southCell.DirectedNeighbors[2]!);
            MazeEdge westWall = new(westCell, westCell.DirectedNeighbors[1]!);
            MazeEdge eastWall = new(eastCell, eastCell.DirectedNeighbors[0]!);
            // When creating maze edges for paths make sure that second cell is always outer.

            // Add paths
            maze.Paths.Add(new(maze, northWall, westWall)); // N-W
            maze.Paths.Add(new(maze, northWall, eastWall)); // N-E
            maze.Paths.Add(new(maze, southWall, westWall)); // S-W
            maze.Paths.Add(new(maze, southWall, eastWall)); // S-E

            // Generate maze using Randomized Depth-first search algorithm
            MazeGenerator generator = new DFSGeneration(maze);
            // Additonaly remove some dead ends from the maze using MazeBraider
            generator.PostProcessors.Add(new MazeBraider(0.5f));
            generator.Generate();

            // Group for paths
            SvgGroup pathsGroup = new()
            {
                Fill = SvgFill.None, // This setting is strongly recommended
                StrokeWidth = 3f,
                StrokeOpacity = 0.6f
            };

            // Different colors for solutions
            SvgFill[] solutionsFills = new SvgFill[4]
            {
                SvgColor.Orange,
                SvgColor.Magenta,
                SvgColor.Green,
                SvgColor.Blue
            };

            // Create a maze exporter(it doesn't need to be closed or disposed)
            MazeSvgExporter exporter = new(maze, padding: 5f)
            {
                // This module will write a description about the maze in SVG-file.
                // It doesn't affect maze appearance
                MazeDescription.Default,
                // Export walls
                Walls.AsOnePath(),
                // And solutions with different stroke colors
                Solutions.All(pathsGroup, i => new SvgPath() {Stroke = solutionsFills[i]})
            };

            // Use a FileStream for exporting.
            // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
            using var fs = File.Create(@"d:\orthogonal-maze-with-solutions.svg");
            using var svgWriter = new SvgWriter(fs);
            // Export a maze
            exporter.Export(svgWriter);

            // Note: SvgWriter should be disposed after exporting.
            // Here we do it with 'using'
        }

        // Export graph representation of a maze
        private static void ExportMazeAsGraph()
        {
            // Create a Hexagonal Sigma Maze
            Maze maze = new SigmaMaze(15);
            // Generate it using Aldous Broder's algorithm
            MazeGenerator generator = new AldousBroderGeneration(maze);
            generator.Generate();

            // Create a maze exporter(it doesn't need to be closed or disposed)
            MazeSvgExporter exporter = new(maze)
            {
                Edges.OfPassagesGraph(),
                Nodes.All()
                
                // Remember, that order here matters and nodes should be added
                // after edges to be displayed correctly.
            };

            // Use a FileStream for exporting.
            // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
            using var fs = File.Create(@"d:\sigma-maze-as-graph.svg");
            using var svgWriter = new SvgWriter(fs);
            // Export a maze
            exporter.Export(svgWriter);

            // Note: SvgWriter should be disposed after exporting.
            // Here we do it with 'using'
        }

        // Export binary tree representation of an orthogonal maze
        // generated with Binary tree algorithn
        private static void ExportMazeAsBinaryTree()
        {
            // Create an Orthogonal Maze 10x10
            Maze maze = new OrthogonalMaze(10, 10);
            // Generate it using Binary tree algorithm.
            // Note that you can use it only for orthogonal mazes.
            MazeGenerator generator = new BinaryTreeGeneration(maze);
            generator.Generate();

            // Create a maze exporter(it doesn't need to be closed or disposed)
            MazeSvgExporter exporter = new(maze)
            {
                Edges.OfPassagesGraph(),
                Nodes.All()
                
                // Remember, that order here matters and nodes should be added
                // after edges to be displayed correctly.
            };

            float cX = exporter.Width / 2f, cY = exporter.Height / 2f;
            // Define custom SvgRoot
            SvgRoot root = new()
            {
                // Rotate the maze to display it as binary tree.
                // Note that in some applications or browsers this transform
                // will have no effect
                Transform = $"translate({cX.ToInvariantString()}, {cY.ToInvariantString()}) rotate(225)"
            };

            // Use a FileStream for exporting.
            // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
            using var fs = File.Create(@"d:\binary-tree-maze.svg");
            using var svgWriter = new SvgWriter(fs);
            // Export a maze
            exporter.Export(svgWriter, root);

            // Note: SvgWriter should be disposed after exporting.
            // Here we do it with 'using'
        }

        // Maze that consists only of lines
        private static void ExportLinesMaze()
        {
            // You can use here any type of maze
            Maze maze = new UpsilonMaze(20, 20, 0, 0);
            MazeGenerator generator = new WilsonGeneration(maze);
            generator.Generate();

            // Predicate for a dead end which has at least one outer neighbor
            bool IsEdgeDeadEnd(MazeCell cell)
            {
                return cell.TryFindNeighbor(c => c.IsMazePart, out _) && maze.IsDeadEnd(cell);
            }

            // Find dead end at the beginning which will be our entry
            MazeCell entry = maze.Cells.First(cell => IsEdgeDeadEnd(cell));
            // Find dead end at the end which will be our exit
            MazeCell exit = maze.Cells.Last(cell => IsEdgeDeadEnd(cell));

            // Other dead ends
            var deadEnds = from c in maze.FindDeadEnds()
                           where c != entry && c != exit
                           select c;

            // Create a path for displaying a solution
            MazeEdge entryEdge = new(entry, entry.DirectedNeighbors.First(c => c.IsNotNullAndOuter())!);
            MazeEdge exitEdge = new(exit, exit.DirectedNeighbors.First(c => c.IsNotNullAndOuter())!);
            maze.Paths.Add(new(maze, entryEdge, exitEdge));

            // Nodes
            SvgPolygon entryTriangle = new()
            {
                Id = "entry",
                Points = new SvgPoint[3] { new(0f, -7.5f), new(7.5f, 7.5f), new(-7.5f, 7.5f) },
                Fill = SvgColor.Green
            };
            SvgPolygon exitTriangle = new()
            {
                Id = "exit",
                Points = new SvgPoint[3] { new(0f, 7.5f), new(7.5f, -7.5f), new(-7.5f, -7.5f) },
                Fill = SvgColor.Red
            };
            SvgCircle deadEndCircle = new()
            {
                Id = "deadEnd",
                R = 4.5f,
                Fill = SvgColor.Blue,
            };

            // Edges
            SvgPath edgesPath = new()
            {
                Fill = SvgFill.None,
                StrokeWidth = 5f,
                StrokeLinecap = SvgLinecap.Round,
                StrokeLinejoin = SvgLinejoin.Round
            };

            // Solution
            SvgGroup solutionsGroup = new()
            {
                Fill = SvgFill.None,
                Stroke = SvgColor.Orange,
                StrokeWidth = edgesPath.StrokeWidth,
                StrokeDasharray = new SvgLength[1] { 10f }
            };

            // Custom style
            SvgRoot root = new()
            {
                Style = "stroke: black; stroke-width: 2.5"
            };

            MazeSvgExporter exporter = new(maze)
            {
                MazeDescription.Default,

                Edges.OfPassagesGraph(edgesPath, false),
                Solutions.All(solutionsGroup, intersectOuterCells: false),

                Nodes.Selected(entry, entryTriangle),
                Nodes.Selected(exit, exitTriangle),
                Nodes.Selected(deadEnds, deadEndCircle)
                
                // Remember, that order here matters and nodes should be added
                // after edges to be displayed correctly.
            };

            using var fs = File.Create(@"d:\lines-maze.svg");
            using var svgWriter = new SvgWriter(fs, s_settings);
            exporter.Export(svgWriter, root);
        }

        // Triangular maze with rainbow walls
        private static void ExportRainbowTriangularMaze()
        {
            // Triangular maze with inner triangular room
            GridMaze2D maze = new DeltaMaze(30, 10);
            // Add entry and exit using cells coordinates.
            // Note that coordinates here are [row, column] and not [x, y]
            MazeEdge entry = new(maze[11, 27]!, maze[11, 27]!.DirectedNeighbors[0]!);
            MazeEdge exit = new(maze[29, 30]!, maze[29, 30]!.DirectedNeighbors[2]!);
            maze.Paths.Add(new(maze, entry, exit));

            // Use Kruskal's algorithm for generation
            MazeGenerator generator = new KruskalGeneration(maze);
            generator.Generate();

            // Specify radient stops
            var stops = new SvgStop[7]
            {
                new(0f / 6f, SvgColor.FromHexCode("#e81416")), // Red
                new(1f / 6f, SvgColor.FromHexCode("#ffa500")), // Orange
                new(2f / 6f, SvgColor.FromHexCode("#faeb36")), // Yellow
                new(3f / 6f, SvgColor.FromHexCode("#79c314")), // Green
                new(4f / 6f, SvgColor.FromHexCode("#487de7")), // Blue
                new(5f / 6f, SvgColor.FromHexCode("#4b369d")), // Indigo
                new(6f / 6f, SvgColor.FromHexCode("#70369d"))  // Violet
            };
            // Create a linear gradient
            SvgGradient rainbowGradient = new SvgLinearGradient()
            {
                Id = "rainbowGradient", // Each gradient should have an Id
                Stops = stops,
                GradientUnits = SvgGradientUnits.UserSpaceOnUse,
                X1 = new SvgLength(0f, SvgLengthUnit.Percentage),
                Y1 = new SvgLength(0f, SvgLengthUnit.Percentage),
                X2 = new SvgLength(100f, SvgLengthUnit.Percentage),
                Y2 = new SvgLength(0f, SvgLengthUnit.Percentage)
            };
            // Custom path for walls
            SvgPath wallsPath = new()
            {
                Fill = SvgFill.None,
                Stroke = rainbowGradient,
            };
            var exporter = new MazeSvgExporter(maze, padding: 5f)
            {
                Background.Create(SvgColor.Black),
                Walls.AsOnePath(wallsPath, 2f)

                // Remember, that order here matters and walls should be added
                // after background to be displayed correctly.
            };

            using var fs = File.Create(@"d:\triangular-rainbow-maze.svg");
            using var svgWriter = new SvgWriter(fs, s_settings);
            exporter.Export(svgWriter);
        }

        private static void Main()
        {
            // ExportOrthogonalMaze();
            // ExportOrthogonalMazeWithSolution();
            // ExportOrthogonalMazeWithMultipleSolutions();
            // ExportMazeAsGraph();
            ExportMazeAsBinaryTree();
            // ExportLinesMaze();
            ExportRainbowTriangularMaze();
        }
    }
}
#pragma warning restore IDE0051 // Remove unused private members
