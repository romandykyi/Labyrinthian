using NUnit.Framework;

namespace Labyrinthian.Tests
{
    internal class Grid2DPatternsTest
    {
        [Test]
        public void DeltaTriangular4()
        {
            DeltaMaze maze = new(4);
            Assert.Multiple(() =>
            {
                Assert.That(maze[0, 3], Is.Not.Null);
                Assert.That(maze[0, 4], Is.Null);
                Assert.That(maze[0, 3], Is.Not.Null);
                Assert.That(maze[3, 0], Is.Not.Null);
                Assert.That(maze[3, 6], Is.Not.Null);
                Assert.That(maze.Rows, Is.EqualTo(4));
                Assert.That(maze.Columns, Is.EqualTo(7));
                Assert.That(maze.Cells, Has.Length.EqualTo(16));
            });
        }

        [Test]
        public void DeltaHexagon3()
        {
            DeltaMaze maze = DeltaMaze.CreateHexagonal(3);
            Assert.Multiple(() =>
            {
                Assert.That(maze[0, 4], Is.Null);
                Assert.That(maze[9, 11], Is.Null);
                Assert.That(maze[8, 13], Is.Null);
            });
        }

        [Test]
        public void SigmaHexagon3()
        {
            SigmaMaze maze = new(3);
            Assert.Multiple(() =>
            {
                Assert.That(maze[0, 0], Is.Null);
                Assert.That(maze[0, 4], Is.Null);
                Assert.That(maze[4, 0], Is.Null);
                Assert.That(maze[4, 1], Is.Null);
                Assert.That(maze[4, 3], Is.Null);
                Assert.That(maze[4, 4], Is.Null);
                Assert.That(maze[2, 2], Is.Not.Null);
            });
        }

        [Test]
        public void SigmaHexagon4()
        {
            SigmaMaze maze = new(4);
            Assert.Multiple(() =>
            {
                Assert.That(maze[0, 2], Is.Not.Null);
                Assert.That(maze[2, 2], Is.Not.Null);
                Assert.That(maze[5, 6], Is.Null);
                Assert.That(maze[6, 2], Is.Null);
                Assert.That(maze[5, 1], Is.Not.Null);
            });
        }
    }
}