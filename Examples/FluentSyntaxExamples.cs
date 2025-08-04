using Labyrinthian;
using Labyrinthian.Svg;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LabyrinthianExamples;

// Example class that demonstrates how to use the fluent syntax for exporting mazes.
// This syntax is preferred over the direct syntax as it allows for a more readable
// and shorter code structure.
public static class FluentSyntaxExamples
{
    // Export default orthogonal maze
    public static void ExportOrthogonalMaze(string directory)
    {
        // Create an orthogonal maze 30x20
        Maze maze = new OrthogonalMaze(30, 20);
        // Generate it using Prim's algorithm
        MazeGenerator generator = new PrimGeneration(maze);
        generator.Generate();

        // Export the maze to SVG file
        MazeSvgExporterBuilder.For(maze)
            .AddWallsAsSinglePath()
            .Build()
            .ExportToFile(Path.Combine(directory, "orthogonal-maze.svg"));
    }

    // Export orthogonal maze with a solution
    public static void ExportOrthogonalMazeWithSolution(string directory)
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

        // Export the maze
        MazeSvgExporterBuilder.For(maze)
            .WithPadding(5f)
            .IncludeMetadata() // This will write a description about the maze in SVG-file.
            .AddWallsAsSinglePath()
            .AddSolutions()
            .Build()
            .ExportToFile(Path.Combine(directory, "orthogonal-maze-with-solution.svg"));
    }

    // Export orthogonal maze with multiple solutions
    public static void ExportOrthogonalMazeWithMultipleSolutions(string directory)
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
        MazeEdge northWall = new(northCell, northCell.DirectedNeighbors[OrthogonalMaze.North]!);
        MazeEdge southWall = new(southCell, southCell.DirectedNeighbors[OrthogonalMaze.South]!);
        MazeEdge westWall = new(westCell, westCell.DirectedNeighbors[OrthogonalMaze.West]!);
        MazeEdge eastWall = new(eastCell, eastCell.DirectedNeighbors[OrthogonalMaze.East]!);
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
        SvgFill[] solutionsFills =
        [
            SvgColor.Orange,
            SvgColor.Magenta,
            SvgColor.Green,
            SvgColor.Blue
        ];

        // Create a maze exporter(it doesn't need to be closed or disposed)
        MazeSvgExporterBuilder.For(maze)
            .WithPadding(5f)
            .IncludeMetadata()
            .AddWallsAsSinglePath()
            .AddSolutions(pathsGroup, i => new SvgPath() { Stroke = solutionsFills[i] })
            .Build()
            .ExportToFile(Path.Combine(directory, "orthogonal-maze-with-multiple-solutions.svg"));
    }

    // Export Theta(circular) maze
    public static void ExportCircularMaze(string directory)
    {
        // Create a Theta maze with radius 18, inner radius 3 and 40 cells on each circle
        Maze maze = new ThetaMaze(18, 3, 40);

        // Specify entry and exit of the maze.
        // You can also do it after maze generation
        MazeEdge entry = maze.GetOuterWalls().First(); // First outer wall
        MazeEdge exit = maze.GetOuterWalls().Last(); // Last outer wall
        maze.Paths.Add(new(maze, entry, exit));

        // Selection method for a cell.
        // It's a newest cell with 50% probability, or a random cell
        static int NewestOrRandom(System.Random rnd, int n) => rnd.Next(2) == 0 ? n - 1 : rnd.Next(n);
        // Generate it using Growing tree algorithm
        MazeGenerator generator = new GrowingTreeGeneration(maze, NewestOrRandom);
        generator.Generate();

        // Specify gradient stops
        var stops = new SvgStop[2]
        {
            new(0f, SvgColor.FromHexCode("#ff0000")),
            new(1f, SvgColor.FromHexCode("#ffa600")),
        };
        // Create a radial gradient
        SvgGradient radialGradient = new SvgRadialGradient()
        {
            Id = "wallsGradient", // Each gradient should have an Id
            Stops = stops,
            GradientUnits = SvgGradientUnits.UserSpaceOnUse,
            Cx = new SvgLength(50f, SvgLengthUnit.Percentage),
            Cy = new SvgLength(50f, SvgLengthUnit.Percentage),
            R = new SvgLength(50f, SvgLengthUnit.Percentage),
        };

        MazeSvgExporterBuilder.For(maze)
            .WithPadding(2.5f)
            .AddWallsAsSinglePath(new SvgPath()
            {
                Stroke = radialGradient,
                StrokeWidth = 5f,
                Fill = SvgFill.None,
                StrokeLinecap = SvgLinecap.Round,
                StrokeLinejoin = SvgLinejoin.Round
            })
            .Build()
            .ExportToFile(Path.Combine(directory, "theta-maze.svg"));
    }

    // Export graph representation of a maze
    public static void ExportMazeAsGraph(string directory)
    {
        // Create a Hexagonal Sigma Maze
        Maze maze = new SigmaMaze(15);
        // Generate it using Aldous Broder's algorithm
        MazeGenerator generator = new AldousBroderGeneration(maze);
        generator.Generate();

        // Order matters here, so we should add edges before nodes
        MazeSvgExporterBuilder.For(maze)
            .AddPassagesGraphEdges()
            .AddAllNodes()
            .Build()
            .ExportToFile(Path.Combine(directory, "sigma-maze-as-graph.svg"));
    }

    // Export binary tree representation of an orthogonal maze
    // generated with Binary tree algorithn
    public static void ExportMazeAsBinaryTree(string directory)
    {
        // Create an Orthogonal Maze 10x10
        Maze maze = new OrthogonalMaze(10, 10);
        // Generate it using Binary tree algorithm.
        // Note that you can use it only for orthogonal mazes.
        MazeGenerator generator = new BinaryTreeGeneration(maze);
        generator.Generate();

        // Create a maze exporter
        MazeSvgExporter exporter = MazeSvgExporterBuilder.For(maze)
            .AddPassagesGraphEdges()
            .AddAllNodes()
            .Build();

        float cX = exporter.Width / 2f, cY = exporter.Height / 2f;
        // Define custom SvgRoot
        SvgRoot root = new()
        {
            // Rotate the maze to display it as binary tree.
            // Note that in some applications or browsers this transform
            // will have no effect
            Transform = $"translate({cX.ToInvariantString()}, {cY.ToInvariantString()}) rotate(225)"
        };

        // Export the maze
        exporter.ExportToFile(Path.Combine(directory, "binary-tree-maze.svg"), root);
    }

    // Maze that consists only of lines
    public static void ExportLinesMaze(string directory)
    {
        // You can use here any type of maze
        Maze maze = new UpsilonMaze(20, 20);
        MazeGenerator generator = new WilsonGeneration(maze);
        generator.Generate();

        // Predicate for a dead end which has at least one outer neighbor
        bool IsEdgeDeadEnd(MazeCell cell)
        {
            return cell.TryFindNeighbor(c => c.IsMazePart, out _) && maze.IsDeadEnd(cell);
        }

        // Find dead end at the beginning which will be our entry
        MazeCell entry = maze.Cells.First(IsEdgeDeadEnd);
        // Find dead end at the end which will be our exit
        MazeCell exit = maze.Cells.Last(IsEdgeDeadEnd);

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
            Points = [new(0f, -7.5f), new(7.5f, 7.5f), new(-7.5f, 7.5f)],
            Fill = SvgColor.Green
        };
        SvgPolygon exitTriangle = new()
        {
            Id = "exit",
            Points = [new(0f, 7.5f), new(7.5f, -7.5f), new(-7.5f, -7.5f)],
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
            StrokeDasharray = [10f]
        };

        // Custom style
        SvgRoot root = new()
        {
            Style = "stroke: black; stroke-width: 2.5"
        };

        MazeSvgExporterBuilder.For(maze)
            .IncludeMetadata()
            .AddPassagesGraphEdges(edgesPath, false)
            .AddSolutions(solutionsGroup, intersectOuterCells: false)
            .AddNode(entry, entryTriangle)
            .AddNode(exit, exitTriangle)
            .AddNodes(deadEnds, deadEndCircle)
            .Build()
            .ExportToFile(Path.Combine(directory, "lines-maze.svg"), root);
    }

    // Triangular maze with rainbow walls
    public static void ExportRainbowTriangularMaze(string directory)
    {
        // Triangular maze with inner triangular room
        GridMaze2D maze = new DeltaMaze(30, 10);
        // Add entry and exit using cells coordinates.
        // Note that coordinates here are [row, column] and not [x, y]
        MazeEdge entry = new(maze[11, 27]!, maze[11, 27]!.DirectedNeighbors[DeltaMaze.East]!);
        MazeEdge exit = new(maze[29, 30]!, maze[29, 30]!.DirectedNeighbors[DeltaMaze.South]!);
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

        MazeSvgExporterBuilder.For(maze)
            .WithPadding(5f)
            .AddBackground(SvgColor.Black)
            .AddWallsAsSinglePath(new()
            {
                Fill = SvgFill.None,
                Stroke = rainbowGradient,
            }, wallsWidth: 2f)
            .Build()
            .ExportToFile(Path.Combine(directory, "triangular-rainbow-maze.svg"));
    }

    // Export visualization of generation process as multiple SVG-files asynchronously
    public static async Task ExportGenerationVisualizationAsync(string directory)
    {
        // Create an orthogonal maze 10x10
        Maze maze = new OrthogonalMaze(10, 10);
        // Create a generator
        MazeGenerator generator = new DFSGeneration(maze);

        // Create a maze exporter
        var exporter = MazeSvgExporterBuilder.For(maze)
            .AddUnvisitedCells(generator, new SvgGroup()
            {
                Fill = SvgColor.Black,
                Stroke = SvgColor.Black
            })
            .AddHighlightedCells(generator, new SvgGroup()
            {
                Fill = SvgColor.Gray,
                Stroke = SvgColor.Gray
            })
            .AddSelectedCell(generator, new SvgGroup()
            {
                Fill = SvgColor.Red,
                Stroke = SvgColor.Red
            })
            .AddWallsAsSinglePath()
            .Build();

        string animationDirectory = Path.Combine(directory, "maze-frames");
        // Create a directory for SVG frames
        Directory.CreateDirectory(animationDirectory);

        int i = 0;
        foreach (Maze frame in generator.GenerateStepByStep())
        {
            // Export the maze generation frame
            string path = Path.Combine(animationDirectory, $"{i++:000}.svg");
            await exporter.ExportToFileAsync(path);
        }
    }

    // Export visualization of Origin Shift algorithm as multiple SVG-files asynchronously
    public static async Task ExportOriginShiftVisualizationAsync(string directory)
    {
        // Create an orthogonal maze 5x5
        Maze maze = new OrthogonalMaze(5, 5);
        // Parameters that simulate author's original approach
        OriginShiftParams @params = new()
        {
            GenerateUntilAllCellsAreVisited = false,
            MaxIterations = 250,
            NeighborSelector = new UnweightedNeighborSelector()
        };
        // Create a generator
        OriginShiftGeneration originShiftGenerator = new(maze, @params);

        // Marker for directed edges arrow
        SvgMarker arrowMarker = new()
        {
            Id = "directedEdgeArrow", // ID is mandatory for SvgMarker to work
            Orient = SvgMarkerOrient.Auto,
            MarkerWidth = 3f,
            MarkerHeight = 4f,
            RefX = 4.5f,
            RefY = 1.5f,
            Shape = new SvgPath()
            {
                D = "M0,0 V3 L2.5,1.5 Z",
                Fill = SvgColor.Black,
                Stroke = SvgFill.None
            }
        };

        // Group for edges
        SvgGroup edgesGroup = new()
        {
            Fill = SvgFill.None,
            Stroke = SvgColor.Black,
            StrokeWidth = 2f
        };
        // Path used for each edge
        SvgPath edgePath = new()
        {
            MarkerEnd = arrowMarker
        };

        // Group for nodes
        SvgGroup nodesGroup = new()
        {
            Id = "nodes"
        };
        // Circle for nodes
        SvgCircle nodeCircle = new()
        {
            Id = "node",
            Fill = SvgColor.White,
            Stroke = SvgColor.Black,
            StrokeWidth = 2f,
            R = 5f
        };
        // Circle for an "origin"
        SvgCircle originCircle = new()
        {
            Id = "origin",
            Fill = SvgColor.Red,
            Stroke = SvgColor.Black,
            StrokeWidth = 2f,
            R = 5f
        };

        var exporter = MazeSvgExporterBuilder.For(maze)
            .AddDirectedEdges(originShiftGenerator.DirectedMaze.DirectedEdges, edgesGroup, edgePath)
            .AddAllNodes(nodeCircle, nodesGroup)
            .AddSelectedNode(originShiftGenerator, originCircle)
            .Build();

        string animationDirectory = Path.Combine(directory, "origin-shift-maze-frames");
        // Create a directory for SVG frames
        Directory.CreateDirectory(animationDirectory);

        int i = 0;
        foreach (Maze frame in originShiftGenerator.GenerateStepByStep())
        {
            // Export the maze generation frame
            string path = Path.Combine(animationDirectory, $"{i++:000}.svg");
            await exporter.ExportToFileAsync(path);
        }
    }
}
