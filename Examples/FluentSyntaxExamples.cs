using Labyrinthian;
using Labyrinthian.Svg;
using System.Collections.Generic;
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

        string filename = Path.Combine(directory, "orthogonal-maze.svg");

        // Use a FileStream for exporting.
        // You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
        using var fs = File.Create(filename);
        using var svgWriter = new SvgWriter(fs);
        // Export the maze
        exporter.Export(svgWriter);

        // Shorter version of the code above:
        //exporter.ExportToFile(filename);
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

        // Export the maze
        exporter.ExportToFile(Path.Combine(directory, "orthogonal-maze-with-solution.svg"));
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

        // Export the maze
        exporter.ExportToFile(Path.Combine(directory, "orthogonal-maze-with-solutions.svg"));
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
        // Custom path for walls.
        SvgPath wallsPath = new()
        {
            Stroke = radialGradient,
            StrokeWidth = 5f,
            Fill = SvgFill.None,
            StrokeLinecap = SvgLinecap.Round,
            StrokeLinejoin = SvgLinejoin.Round
        };

        // Create a maze exporter(it doesn't need to be closed or disposed)
        MazeSvgExporter exporter = new(maze, padding: 2.5f)
        {
            // This module will write a description about the maze in SVG-file.
            // It doesn't affect maze appearance
            MazeDescription.Default,
            // Export walls
            Walls.AsOnePath(wallsPath),
        };

        // Export the maze
        exporter.ExportToFile(Path.Combine(directory, "theta-maze.svg"));
    }

    // Export graph representation of a maze
    public static void ExportMazeAsGraph(string directory)
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

        // Export the maze
        exporter.ExportToFile(Path.Combine(directory, "sigma-maze-as-graph.svg"));
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

        MazeSvgExporter exporter = new(maze)
        {
            MazeDescription.Default,

            Edges.OfPassagesGraph(edgesPath, false),
            Solutions.All(solutionsGroup, intersectOuterCells: false),

            Nodes.Selected(entry, entryTriangle),
            Nodes.Selected(exit, exitTriangle),
            Nodes.Selected(deadEnds, deadEndCircle),
                
            // Remember, that order here matters and nodes should be added
            // after edges to be displayed correctly.
        };

        exporter.ExportToFile(Path.Combine(directory, "lines-maze.svg"), root);
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

        exporter.ExportToFile(Path.Combine(directory, "triangular-rainbow-maze.svg"));
    }

    // Export visualization of generation process as multiple SVG-files asynchronously
    public static async Task ExportGenerationVisualizationAsync(string directory)
    {
        // Create an orthogonal maze 10x10
        Maze maze = new OrthogonalMaze(10, 10);
        // Create a generator
        MazeGenerator generator = new DFSGeneration(maze);

        // IEnumerable for selected cell of generator
        IEnumerable<MazeCell> SelectedCellEnumerable()
        {
            if (generator.SelectedCell != null)
                yield return generator.SelectedCell;
        }

        // Group for unvisited cells
        SvgGroup unvisitedCellsGroup = new()
        {
            Fill = SvgColor.Black,
            Stroke = SvgColor.Black
        };
        // Group for highlighted cells
        SvgGroup highilightedCellsGroup = new()
        {
            Fill = SvgColor.Gray,
            Stroke = SvgColor.Gray
        };
        // Group for selected cell
        SvgGroup selectedCellGroup = new()
        {
            Fill = SvgColor.Red,
            Stroke = SvgColor.Red
        };

        // Create a maze exporter
        MazeSvgExporter exporter = new(maze)
        {
            // Unvisited cells that also are not highlighted
            Cells.Selected(
                maze.Cells.Where(c => !generator.VisitedCells[c] && !generator.HighlightedCells[c]),
                unvisitedCellsGroup),
            // Highlighted cells
            Cells.Selected(
                maze.Cells.Where(c => generator.HighlightedCells[c]),
                highilightedCellsGroup),
            // Selected cell
            Cells.Selected(SelectedCellEnumerable(), selectedCellGroup),
            // Walls
            Walls.AsOnePath()
                
            // Remember, that order here matters and walls should be added
            // after cells to be displayed correctly.
        };

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

        // IEnumerable for selected cell of generator
        IEnumerable<MazeCell> SelectedCellEnumerable()
        {
            if (originShiftGenerator.SelectedCell != null)
                yield return originShiftGenerator.SelectedCell;
        }

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

        // Create a maze exporter
        MazeSvgExporter exporter = new(maze)
        {
            // Directed edges
            Edges.Directed(originShiftGenerator.DirectedMaze.DirectedEdges, edgesGroup, edgePath),
            // Nodes
            Nodes.All(nodesGroup: nodesGroup, nodeShape: nodeCircle),
            // "Origin"
            Nodes.Selected(SelectedCellEnumerable(), nodeShape: originCircle),
                
            // Remember, that order here matters and nodes should be added
            // after edges to be displayed correctly.
        };

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
