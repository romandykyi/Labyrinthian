# Labyrinthian
![labyrinthian-l](https://github.com/romandykyi/Labyrinthian/assets/94003504/8e4a2bde-4582-4d11-9386-be6121eee432)

Labyrinthian is a .NET Standard 2.1 C# library for generating mazes step-by-step. It provides a versatile toolkit for maze enthusiasts, game developers, and educators, allowing you to effortlessly create mazes with various algorithms and export them in SVG format. 

This library is lightweight and comes with no external dependencies.

You can play around with a Blazor demo [here](https://romandykyi.github.io/LabyrinthianDemo/).

## Mazes, that were generated using Labyrinthian.Svg

![gradient-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/de1fc580-e72a-45dc-ab15-a376addebad9)
![theta-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/b792fc9c-e3dd-4122-b2ae-3cb3b26f2bf6)
![lines-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/4b641816-3976-4eee-8edb-8a3acec09a19)

## Supported Maze Generation Algorithms

* Aldous Broder
* Binary tree
* Depth-first search
* Edge-based Prim's algorithm
* Growing tree
* Hunt and Kill
* Kruskal's algorithm
* Origin Shift (invented by [CaptainLuma](https://github.com/captainluma))
* Prim's algorithm
* Recursive backtracker (with customizable cell selection method)
* Recursive division
* Sidewinder
* Wilson's algorithm

## Supported Maze Types
* Delta
* Sigma
* Orthogonal
* Theta
* Upsilon

## Installation
### Unity
1. [Download the Unity package here](https://github.com/romandykyi/Labyrinthian/releases/tag/Labyrinthian_v1.1.1)
2. In Unity, go to 'Assets > Import package > Custom package'
3. Select the downloaded package
4. Click 'Import'

### Visual Studio
1. Open Package Manager Console window: 'View > Other Windows > Package Manager Console'
2. Navigate to the directory in which the .csproj file exists
3.  Run this command to install the main package:
```
NuGet\Install-Package Labyrinthian
```
4. If you need SVG-export features, also run this command:
```
NuGet\Install-Package Labyrinthian.Svg
```

### .NET CLI
Run this command to install the main package:
```
dotnet add package Labyrinthian
```
If you need SVG-export features, also run this command:
```
dotnet add package Labyrinthian.Svg
```

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

![orthogonal-maze](https://github.com/romandykyi/Labyrinthian/assets/94003504/a6c20704-ab86-4247-8419-4e1b1fc84aa5)

More examples [here](https://github.com/romandykyi/Labyrinthian/blob/master/Examples/MazeExportingExamples.cs).

## License
This project is licensed under the MIT License. See the [LICENSE](https://github.com/romandykyi/Labyrinthian/blob/master/LICENSE) file for details.
