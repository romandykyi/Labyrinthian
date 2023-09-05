# Labyrinthian
![labyrinthian-l](https://github.com/romandykyi/Labyrinthian/assets/94003504/36b5090b-a772-412a-a533-ef249a488228)

Labyrinthian is a .NET Standard 2.1 C# library for generating mazes step by step. It provides a versatile toolkit for maze enthusiasts, game developers, and educators, allowing you to effortlessly create mazes with various algorithms and export them in SVG format. 

This library is lightweight and comes with no external dependencies.

## Mazes, that was generated using Labyrinthian.Svg

![gradient-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/71d40c9c-92a1-41aa-a1e4-5031b14ec823)
![lines-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/b84d2bf9-3f71-461c-9231-8f9ff4766330)


## Supported Maze Generation Algorithms
* Aldous Broder
* Binary tree
* Depth-first search
* Edge-based Prim's algorithm
* Growing tree
* Hunt and Kill
* Kruskal's algorithm
* Prim's algorithm
* Recursive backtracker(with customizable cell selection method)
* Recursive division
* Sidewinder
* Wilson's algorithm

## Supported Maze Types
* Delta
* Sigma
* Orthogonal
* Upsilon

## Example
This code snippet demonstrates the creation, generation, and export of an orthogonal maze as an SVG file:
```csharp
using Labyrinthian;
using Labyrinthian.Svg;

// Create an orthogonal maze 30x20
Maze maze = new OrthogonalMaze(30, 20);
// Generate it using Prim's algorithm
MazeGenerator generator = new PrimGeneration(maze);
generator.Generate();

// Create a maze exporter
MazeSvgExporter exporter = new(maze)
{
    Walls.AsOnePath()
};

// Use a FileStream for exporting.
// You can also use any Stream or TextWriter(e.g. StreamWriter) or XmlWriter
using var fs = File.Create(@"d:\orthogonal-maze.svg");
using var svgWriter = new SvgWriter(fs);
// Export a maze
exporter.Export(svgWriter);
```
Possible output:

![orthogonal-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/74e10a4b-6f91-40f3-87d8-a88e16dfbd98)

More examples [here](https://github.com/romandykyi/Labyrinthian/blob/master/Examples/MazeExportingExamples.cs).

## License
This project is licensed under the MIT License. See the [LICENSE](https://github.com/romandykyi/Labyrinthian/blob/master/LICENSE) file for details.
