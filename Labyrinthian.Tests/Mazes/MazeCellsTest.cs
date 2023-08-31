using NUnit.Framework;
using System;

namespace Labyrinthian.Tests.Mazes
{
    internal class MazeCellsTest
    {
        [Test]
        public void EqualByReference()
        {
            MazeCell cell1 = new(42);
            MazeCell cell2 = cell1;

            Assert.That(cell1, Is.EqualTo(cell2));
        }

        [Test]
        public void NotEqualToNull()
        {
            MazeCell cell1 = new(143);

            Assert.That(cell1, Is.Not.EqualTo(null));
        }

        [Test]
        public void SameIndicesNotEqual()
        {
            MazeCell cell1 = new(5);
            MazeCell cell2 = new(5);

            Assert.That(cell1, Is.Not.EqualTo(cell2));
        }

        [Test]
        public void ConstructorCreatesMazePart()
        {
            MazeCell cell = new(1);

            Assert.That(cell.IsMazePart, Is.True);
        }

        [Test]
        public void ConstructorThrowsWhenIndexIsNegative()
        {
            MazeCell cell;
            Assert.Throws<ArgumentOutOfRangeException>(() => cell = new(-1));
        }

        [Test]
        public void CreateOuterCellCreatesOuterCell()
        {
            MazeCell neighbor = new(7312);
            MazeCell cell = MazeCell.CreateOuterCell(neighbor, 0);

            Assert.That(cell.IsMazePart, Is.False);
        }

        [Test]
        public void CreateOuterCellDoesntAcceptNegativeDirection()
        {
            MazeCell neighbor = new(504030);

            Assert.Throws<ArgumentOutOfRangeException>(() => MazeCell.CreateOuterCell(neighbor, -403020));
        }

        [Test]
        public void SetNeighborsWorks()
        {
            MazeCell cell = new(0);
            MazeCell neighbor1 = new(1), neighbor2 = new(2);
            MazeCell outerNeighbor = MazeCell.CreateOuterCell(cell, 1);

            MazeCell[] neighbors = { neighbor1, neighbor2 };
            MazeCell?[] directedNeighbors = { outerNeighbor, neighbor1, null, neighbor2, null };

            cell.SetNeighbors(directedNeighbors);

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(neighbors, cell.Neighbors);
                CollectionAssert.AreEqual(directedNeighbors, cell.DirectedNeighbors);
            });
        }

        [Test]
        public void AreNeighborsDetectsNeighbors()
        {
            MazeCell cell0 = new(0), cell1 = new(1), cell2 = new(2);
            // cell0<->cell1<->cell2
            cell0.SetNeighbors(cell1);
            cell1.SetNeighbors(cell0, cell2);
            cell2.SetNeighbors(cell1);

            Assert.Multiple(() =>
            {
                Assert.That(MazeCell.AreNeighbors(cell0, cell1), Is.True);
                Assert.That(MazeCell.AreNeighbors(cell1, cell2), Is.True);
            });
        }

        [Test]
        public void AreNeighborsDetectsNonNeighbors()
        {
            // cell0<->cell1<->cell2
            MazeCell cell0 = new(0), cell1 = new(1), cell2 = new(2);
            cell0.SetNeighbors(cell1, null);
            cell1.SetNeighbors(cell0, cell2);
            cell2.SetNeighbors(null, cell1);

            Assert.Multiple(() =>
            {
                Assert.That(MazeCell.AreNeighbors(cell0, cell2), Is.False);
                Assert.That(MazeCell.AreNeighbors(cell2, cell0), Is.False);
            });
        }

        [Test]
        public void AreNeigbhorsDetectsOuterNeighbor()
        {
            MazeCell normalCell = new(0);
            MazeCell outerCell = MazeCell.CreateOuterCell(normalCell, 1);
            normalCell.SetNeighbors(outerCell);

            Assert.Multiple(() =>
            {
                Assert.That(MazeCell.AreNeighbors(normalCell, outerCell), Is.True);
                Assert.That(MazeCell.AreNeighbors(outerCell, normalCell), Is.True);
            });
        }

        [Test]
        public void AreNeigbhorsDetectsOuterNonNeighbor()
        {
            MazeCell normalCell0 = new(0);
            MazeCell normalCell1 = new(1);
            MazeCell outerCell = MazeCell.CreateOuterCell(normalCell0, 1);
            normalCell0.SetNeighbors(outerCell, normalCell1);
            normalCell1.SetNeighbors(normalCell0, null);

            Assert.Multiple(() =>
            {
                Assert.That(MazeCell.AreNeighbors(normalCell1, outerCell), Is.False);
                Assert.That(MazeCell.AreNeighbors(outerCell, normalCell1), Is.False);
            });
        }

        [Test]
        public void AreNeighborsReturnsFalseForTwoOuterCells()
        {
            MazeCell cell = new(0);
            MazeCell outerCell1 = MazeCell.CreateOuterCell(cell, 1);
            MazeCell outerCell2 = MazeCell.CreateOuterCell(cell, 0);
            cell.SetNeighbors(outerCell1, outerCell2);

            Assert.That(MazeCell.AreNeighbors(outerCell1, outerCell2), Is.False);
        }
    }
}
