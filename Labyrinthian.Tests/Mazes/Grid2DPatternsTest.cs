using NUnit.Framework;

namespace Labyrinthian.Tests.Mazes
{
    internal class Grid2DPatternsTest
    {
        // Gap
        private const bool _ = false;
        // Cell
        private const bool C = true;

        private static void CompareGrids(bool[,] expectedGrid, GridMaze2D actualMaze)
        {
            // Check grid dimensions
            Assert.Multiple(() =>
            {
                Assert.That(expectedGrid.GetLength(0), Is.EqualTo(actualMaze.Rows), "Rows don't match.");
                Assert.That(expectedGrid.GetLength(1), Is.EqualTo(actualMaze.Columns), "Columns don't match.");
            });

            Assert.Multiple(() =>
            {
                for (int row = 0; row < actualMaze.Rows; ++row)
                {
                    for (int column = 0; column < actualMaze.Columns; ++column)
                    {
                        if (actualMaze[row, column] != null)
                        {
                            Assert.That(expectedGrid[row, column], Is.True, $"Unexpected gap at row {row} and column {column}.");
                        }
                        else
                        {
                            Assert.That(expectedGrid[row, column], Is.False, $"Unexpected cell at row {row} and column {column}.");
                        }
                    }
                }
            });
        }

        [Test]
        public void RectangularPattern()
        {
            bool[,] expectedGrid = new bool[4, 5]
            {
               {C, C, C, C, C},
               {C, C, C, C, C},
               {C, C, C, C, C},
               {C, C, C, C, C},
            };
            GridMaze2D actualMaze = new OrthogonalMaze(5, 4);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void RectangularPattern_InnerRoom()
        {
            bool[,] expectedGrid = new bool[5, 5]
            {
               {C, C, C, C, C},
               {C, _, _, _, C},
               {C, _, _, _, C},
               {C, C, C, C, C},
               {C, C, C, C, C},
            };
            GridMaze2D actualMaze = new OrthogonalMaze(5, 5, 3, 2);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_OddSide()
        {
            bool[,] expectedGrid = new bool[3, 5]
            {
               {_, _, C, _, _},
               {_, C, C, C, _},
               {C, C, C, C, C},
            };
            GridMaze2D actualMaze = new DeltaMaze(3);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_EvenSide()
        {
            bool[,] expectedGrid = new bool[4, 7]
            {
               {_, _, _, C, _, _, _},
               {_, _, C, C, C, _, _},
               {_, C, C, C, C, C, _},
               {C, C, C, C, C, C, C},
            };
            GridMaze2D actualMaze = new DeltaMaze(4);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_OddOutSide_OddInSide()
        {
            bool[,] expectedGrid = new bool[7, 13]
            {
                {_, _, _, _, _, _, C, _, _, _, _, _, _ },
                {_, _, _, _, _, C, C, C, _, _, _, _, _ },
                {_, _, _, _, C, C, _, C, C, _, _, _, _ },
                {_, _, _, C, C, _, _, _, C, C, _, _, _ },
                {_, _, C, C, _, _, _, _, _, C, C, _, _ },
                {_, C, C, C, C, C, C, C, C, C, C, C, _ },
                {C, C, C, C, C, C, C, C, C, C, C, C, C },
            };
            GridMaze2D actualMaze = new DeltaMaze(7, 3);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_OddOutSide_EvenInSide()
        {
            bool[,] expectedGrid = new bool[7, 13]
            {
                {_, _, _, _, _, _, C, _, _, _, _, _, _ },
                {_, _, _, _, _, C, C, C, _, _, _, _, _ },
                {_, _, _, _, C, C, _, C, C, _, _, _, _ },
                {_, _, _, C, C, _, _, _, C, C, _, _, _ },
                {_, _, C, C, C, C, C, C, C, C, C, _, _ },
                {_, C, C, C, C, C, C, C, C, C, C, C, _ },
                {C, C, C, C, C, C, C, C, C, C, C, C, C },
            };
            GridMaze2D actualMaze = new DeltaMaze(7, 2);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_EvenOutSide_EvenInSide()
        {
            bool[,] expectedGrid = new bool[8, 15]
            {
                {_, _, _, _, _, _, _, C, _, _, _, _, _, _, _ },
                {_, _, _, _, _, _, C, C, C, _, _, _, _, _, _ },
                {_, _, _, _, _, C, C, _, C, C, _, _, _, _, _ },
                {_, _, _, _, C, C, _, _, _, C, C, _, _, _, _ },
                {_, _, _, C, C, _, _, _, _, _, C, C, _, _, _ },
                {_, _, C, C, _, _, _, _, _, _, _, C, C, _, _ },
                {_, C, C, C, C, C, C, C, C, C, C, C, C, C, _ },
                {C, C, C, C, C, C, C, C, C, C, C, C, C, C, C },
            };
            GridMaze2D actualMaze = new DeltaMaze(8, 4);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void TriangularPattern_EvenOutSide_OddInSide()
        {
            bool[,] expectedGrid = new bool[8, 15]
            {
                {_, _, _, _, _, _, _, C, _, _, _, _, _, _, _ },
                {_, _, _, _, _, _, C, C, C, _, _, _, _, _, _ },
                {_, _, _, _, _, C, C, _, C, C, _, _, _, _, _ },
                {_, _, _, _, C, C, _, _, _, C, C, _, _, _, _ },
                {_, _, _, C, C, _, _, _, _, _, C, C, _, _, _ },
                {_, _, C, C, C, C, C, C, C, C, C, C, C, _, _ },
                {_, C, C, C, C, C, C, C, C, C, C, C, C, C, _ },
                {C, C, C, C, C, C, C, C, C, C, C, C, C, C, C },
            };
            GridMaze2D actualMaze = new DeltaMaze(8, 3);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void HexagonalPattern()
        {
            bool[,] expectedGrid = new bool[5, 5]
            {
                {_, C, C, C, _ },
                {C, C, C, C, C },
                {C, C, C, C, C },
                {C, C, C, C, C },
                {_, _, C, _, _ }
            };
            GridMaze2D actualMaze = new SigmaMaze(3);
            CompareGrids(expectedGrid, actualMaze);
        }

        [Test]
        public void HexagonalPattern_InnerRoom()
        {
            bool[,] expectedGrid = new bool[9, 9]
            {
                {_, _, _, C, C, C, _, _, _ },
                {_, C, C, C, C, C, C, C, _ },
                {C, C, C, _, _, _, C, C, C },
                {C, C, _, _, _, _, _, C, C },
                {C, C, _, _, _, _, _, C, C },
                {C, C, _, _, _, _, _, C, C },
                {C, C, C, C, _, C, C, C, C },
                {_, _, C, C, C, C, C, _, _ },
                {_, _, _, _, C, _, _, _, _ }
            };
            GridMaze2D actualMaze = new SigmaMaze(5, 3);
            CompareGrids(expectedGrid, actualMaze);
        }

        // TODO: Add tests for GridMaze2D.DeltaHexagonalPattern
    }
}
