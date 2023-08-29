using NUnit.Framework;

namespace Labyrinthian.Tests.Mazes
{
    internal class UpsilonMazeTest
    {
        [Test]
        public void CenterCellConnections3x3()
        {
            UpsilonMaze maze = new(3, 3);

            Assert.That(maze.Cells, Has.Length.EqualTo(9));

            MazeCell centerCell = maze.Cells[4];
            for (int i = 0; i < 9; ++i)
            {
                if (i != 4 && !centerCell.TryFindNeighbor(c => c == maze.Cells[i], out _))
                {
                    Assert.Fail($"Center cell is not connected with cell #{i}");
                }
            }
        }
    }
}
