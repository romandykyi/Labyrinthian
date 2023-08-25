using NUnit.Framework;

namespace Labyrinthian.Tests
{
    internal class MazeCellsTest
    {
        [Test]
        public void EqualsTest()
        {
            MazeCell cell1 = new(1);
            MazeCell cell2 = new(1);
            MazeCell? cell3 = new(2);

            Assert.Multiple(() =>
            {
                Assert.That(cell1, Is.Not.EqualTo(cell2));
                Assert.That(cell2, Is.Not.EqualTo(cell3));
            });

            cell2 = cell1;
            cell3 = null;
            Assert.Multiple(() =>
            {
                Assert.That(cell1, Is.EqualTo(cell2));
                Assert.That(cell3, Is.EqualTo(null));
                Assert.That(cell1, Is.Not.EqualTo(null));
            });
        }

        [Test]
        public void IsMazePartWorksProperly()
        {
            MazeCell mazePart1 = new(0);
            MazeCell mazePart2 = new(5);
            MazeCell notMazePart1 = MazeCell.CreateOuterCell(mazePart1, 1);
            MazeCell notMazePart2 = MazeCell.CreateOuterCell(mazePart2, 0);
            
            Assert.Multiple(() =>
            {
                Assert.That(mazePart1.IsMazePart, Is.True);
                Assert.That(mazePart2.IsMazePart, Is.True);
                Assert.That(notMazePart1.IsMazePart, Is.False);
                Assert.That(notMazePart2.IsMazePart, Is.False);
            });
        }

        [Test]
        public void AreNeighborsWorksProperly()
        {
            MazeCell mazePart1 = new(0);
            MazeCell mazePart2 = new(1);
            MazeCell mazePart3 = new(2);
            MazeCell notMazePart1 = MazeCell.CreateOuterCell(mazePart1, 0);
            MazeCell notMazePart2 = MazeCell.CreateOuterCell(mazePart2, 1);

            mazePart1.SetNeighbors(mazePart2, notMazePart1);
            mazePart2.SetNeighbors(notMazePart2, mazePart1);

            Assert.Multiple(() =>
            {
                Assert.That(MazeCell.AreNeighbors(mazePart1, mazePart2), Is.True);
                Assert.That(MazeCell.AreNeighbors(mazePart1, mazePart3), Is.False);

                Assert.That(MazeCell.AreNeighbors(notMazePart1, mazePart1), Is.True);
                Assert.That(MazeCell.AreNeighbors(mazePart2, notMazePart2), Is.True);

                Assert.That(MazeCell.AreNeighbors(notMazePart1, mazePart2), Is.False);
                Assert.That(MazeCell.AreNeighbors(mazePart1, notMazePart2), Is.False);
            });
        }
    }
}
