using NUnit.Framework;
using System;
using System.Linq;
using System.Numerics;

namespace Labyrinthian.Tests.Mazes
{
    internal class MazeTest
    {
        /// <summary>
        /// The most basic implementation of Maze class.
        /// It's just 1D maze, it cannot be generated because of that but it will be used
        /// for testing some Maze methods and MazeUtils extensions
        /// </summary>
        private sealed class TestMaze : Maze
        {
            public override int Dimensions => 2;
            public override float[] Sizes => new float[2] { Cells.Length, -Cells.Length };

            public TestMaze(int size) : base(size)
            {
                if (size < 2) throw new ArgumentOutOfRangeException(nameof(size));

                InitGraph();
            }

            protected override MazeCell[] GetDirectedNeighbors(MazeCell cell)
            {
                if (cell.Index == 0)
                    return new MazeCell[] { Cells[1], MazeCell.CreateOuterCell(Cells[0], 0) };
                else if (cell.Index == Cells.Length - 1)
                    return new MazeCell[] { MazeCell.CreateOuterCell(Cells[^1], 1), Cells[^2] };
                else
                    return new MazeCell[] { Cells[cell.Index + 1], Cells[cell.Index - 1] };
            }

            public override string CellToSvgString(MazeCell cell, float cellSize, float offset)
            {
                throw new NotImplementedException();
            }

            public override float[] GetCellPoint(MazeCell cell, int pointIndex)
            {
                return new float[2] { pointIndex, -pointIndex };
            }

            public override int GetCellPointsNumber(MazeCell cell) => 2;

            public override PathSegment GetWallPosition(MazeEdge wall)
            {
                throw new NotImplementedException();
            }

            public override Vector2 PositionTo2DPoint(float[] position)
            {
                throw new NotImplementedException();
            }

            protected override PathSegment GetPath(MazeEdge relation)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void ConnectAllCellsTest()
        {
            TestMaze maze = new(10);
            maze.ConnectAllCells();
            Assert.That(maze.GetWalls(false).Count(), Is.EqualTo(0));
        }

        [Test]
        public void AreCellsConnectedTest()
        {
            TestMaze maze = new(5); // All cells should be disconnected by default
            Assert.Multiple(() =>
            {
                Assert.That(maze.AreCellsConnected(maze.Cells[0], maze.Cells[1]), Is.False);
                Assert.That(maze.AreCellsConnected(maze.Cells[0], maze.Cells[2]), Is.False);
            });
        }

        [Test]
        public void ConnectCellsAndAreCellsConnectedTest()
        {
            TestMaze maze = new(5);

            MazeCell cell0 = maze.Cells[0], cell1 = maze.Cells[1],
                cell2 = maze.Cells[2], cell4 = maze.Cells[4];
            Assert.Multiple(() =>
            {
                // Connecting unconnected cells
                Assert.That(maze.ConnectCells(cell0, cell1), Is.True);
                // Connection already connected cells
                Assert.That(maze.ConnectCells(cell1, cell0), Is.False);

                // Try to connect cells with cells that aren't parts of maze
                Assert.Throws<CellIsOuterException>(() => maze.ConnectCells(cell0, cell0.DirectedNeighbors[1]!));
                Assert.Throws<CellIsOuterException>(() => maze.ConnectCells(cell4.DirectedNeighbors[0]!, cell4));
                // Try to connect cells that are not neighbors
                Assert.Throws<CellsAreNotNeighborsException>(() => maze.ConnectCells(cell0, cell2));

                // Checking whether connected cells are connected
                Assert.That(maze.AreCellsConnected(cell0, cell1), Is.True);
                Assert.That(maze.AreCellsConnected(cell1, cell0), Is.True);
                // Checking whether unconnected cells are unconnected(there's wall between them)
                Assert.That(maze.AreCellsConnected(cell1, cell2), Is.False);
                Assert.That(maze.AreCellsConnected(cell2, cell1), Is.False);
                // Checking whether non-neighbors are unconnected
                Assert.That(maze.AreCellsConnected(cell0, cell2), Is.False);
                Assert.That(maze.AreCellsConnected(cell1, cell4), Is.False);
            });
        }

        [Test]
        public void BlockCellsTest()
        {
            TestMaze maze = new(5);

            maze.ConnectAllCells(); // All cells are connected now

            MazeCell cell0 = maze.Cells[0], cell1 = maze.Cells[1], cell2 = maze.Cells[2];

            maze.BlockCells(cell0, cell1); // Only cells 0 and 1 now blocked
            Assert.Multiple(() =>
            {
                Assert.That(maze.AreCellsConnected(cell1, cell0), Is.False);
                Assert.That(maze.AreCellsConnected(cell0, cell1), Is.False);
                Assert.That(maze.AreCellsConnected(cell2, cell1), Is.True);

                // Try to block non-neighbors cells
                Assert.Throws<CellsAreNotNeighborsException>(() => maze.BlockCells(cell0, cell2));
            });
        }

    }
}
