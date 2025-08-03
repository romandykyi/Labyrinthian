using System.Threading.Tasks;

namespace LabyrinthianExamples
{
    internal partial class Program
    {
        private static async Task Main()
        {
            //ExportOrthogonalMaze();
            ExportOrthogonalMazeWithSolution();
            //ExportOrthogonalMazeWithMultipleSolutions();
            //ExportCircularMaze();
            //ExportMazeAsGraph();
            //ExportMazeAsBinaryTree();
            //ExportLinesMaze();
            //ExportRainbowTriangularMaze();
            //await ExportGenerationVisualizationAsync();
            await ExportOriginShiftVisualizationAsync();
        }
    }
}
