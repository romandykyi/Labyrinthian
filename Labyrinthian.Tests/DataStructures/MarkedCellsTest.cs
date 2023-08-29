using System.Linq;
using NUnit.Framework;

namespace Labyrinthian.Tests.DataStructures
{
    internal class MarkedCellsTest
    {
        private readonly Maze maze = new OrthogonalMaze(3, 3);

        [Test]
        public void FalseByDefault()
        {
            MarkedCells markedCells = new(maze);
            Assert.That(markedCells.All(mark => !mark.Value));
        }

        [Test]
        public void ConstructorChangesDefaultValue()
        {
            MarkedCells markedCells = new(maze, true);
            Assert.That(markedCells.All(mark => mark.Value));
        }

        [Test]
        public void SetAllWorks()
        {
            MarkedCells markedCells = new(maze, false);
            markedCells.SetAll(true);
            Assert.That(markedCells.All(mark => mark.Value));
        }

        [Test]
        public void AssignWorks()
        {
            MarkedCells markedCells = new(maze);
            MazeCell trueCell = maze.Cells[0];
            markedCells[trueCell] = true;
            Assert.That(markedCells.Single(mark => mark.Value).Cell, Is.EqualTo(trueCell));
        }

        [Test]
        public void MarkChangedInvoked()
        {
            MarkedCells markedCells = new(maze);
            CellMark changedMark = new(null!, false);
            markedCells.MarkChanged += mark => changedMark = mark;

            MazeCell trueCell = maze.Cells[3];
            markedCells[trueCell] = true;

            Assert.That(changedMark.Cell, Is.EqualTo(trueCell));
            Assert.That(changedMark.Value, Is.True);
        }
    }
}
