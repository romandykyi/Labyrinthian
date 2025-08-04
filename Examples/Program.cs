using System.IO;
using static LabyrinthianExamples.FluentSyntaxExamples;

string exportFolder = "Mazes";
Directory.CreateDirectory(exportFolder);

ExportOrthogonalMaze(exportFolder);
ExportOrthogonalMazeWithSolution(exportFolder);
ExportOrthogonalMazeWithMultipleSolutions(exportFolder);
ExportCircularMaze(exportFolder);
ExportMazeAsGraph(exportFolder);
ExportMazeAsBinaryTree(exportFolder);
ExportLinesMaze(exportFolder);
ExportRainbowTriangularMaze(exportFolder);
await ExportGenerationVisualizationAsync(exportFolder);
await ExportOriginShiftVisualizationAsync(exportFolder);
