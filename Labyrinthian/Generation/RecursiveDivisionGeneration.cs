using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public sealed class RecursiveDivisionGeneration : MazeGenerator
    {
        private readonly struct Rect
        {
            public readonly int Row, Column;
            public readonly int Rows, Columns;

            public Rect(int row, int column, int rows, int columns) : this()
            {
                Row = row;
                Column = column;
                Rows = rows;
                Columns = columns;
            }
        }

        private readonly OrthogonalMaze _maze;
        private readonly float _horizontalBias;

        public RecursiveDivisionGeneration(Maze maze, float horizontalBias = 1f) :
            this(maze, Environment.TickCount, horizontalBias)
        { }
        public RecursiveDivisionGeneration(Maze maze, int seed, float horizontalBias = 1f) :
            base(maze, seed, defaultVisited: true)
        {
            if (horizontalBias < 0f)
                throw new ArgumentOutOfRangeException(nameof(horizontalBias));

            _maze = (OrthogonalMaze)maze;
            _horizontalBias = horizontalBias;
        }

        protected override bool IsSuitableFor(Maze maze) => IsMazeDefaultOrthogonal(maze);

        private void DivideHorizontally(int row, int column, int columns)
        {
            int entryColumn = Rnd.Next(column, column + columns);
            for (int i = column; i < column + columns; ++i)
            {
                if (i == entryColumn) continue;

                MazeCell? up = _maze[row, i],
                    down = _maze[row + 1, i];
                if (up.IsNotNullAndMazePart() &&
                    up.IsNotNullAndMazePart())
                {
                    _maze.BlockCells(up!, down!);
                }
            }
        }

        private void DivideVertically(int row, int column, int rows)
        {
            int entryRow = Rnd.Next(row, row + rows);
            for (int i = row; i < row + rows; ++i)
            {
                if (i == entryRow) continue;

                MazeCell? left = _maze[i, column],
                    right = _maze[i, column + 1];
                if (left.IsNotNullAndMazePart() &&
                    right.IsNotNullAndMazePart())
                {
                    _maze.BlockCells(left!, right!);
                }
            }
        }

        protected override IEnumerable<Maze> Generation()
        {
            _maze.ConnectAllCells();
            yield return _maze;

            Rect startRect = new Rect(0, 0, _maze.Rows, _maze.Columns);
            Stack<Rect> stack = new Stack<Rect>();
            stack.Push(startRect);

            while (stack.Count > 0)
            {
                Rect currentRect = stack.Pop();
                if (currentRect.Columns < 2 || currentRect.Rows < 2) continue;

                float n = currentRect.Columns + currentRect.Rows * _horizontalBias;
                float verticalCutProbability = currentRect.Columns / n;
                if ((float)Rnd.NextDouble() <= verticalCutProbability)
                {
                    int column = Rnd.Next(currentRect.Column, currentRect.Column + currentRect.Columns - 1);
                    DivideVertically(currentRect.Row, column, currentRect.Rows);

                    int leftColumns = column - currentRect.Column + 1;
                    Rect left = new Rect(
                        currentRect.Row, currentRect.Column,
                        currentRect.Rows, leftColumns
                        );
                    Rect right = new Rect(
                        currentRect.Row, column + 1,
                        currentRect.Rows, currentRect.Columns - leftColumns
                        );

                    stack.Push(left);
                    stack.Push(right);
                }
                else
                {
                    int row = Rnd.Next(currentRect.Row, currentRect.Row + currentRect.Rows - 1);
                    DivideHorizontally(row, currentRect.Column, currentRect.Columns);

                    int downRows = row - currentRect.Row + 1;
                    Rect down = new Rect(
                        currentRect.Row, currentRect.Column,
                        downRows, currentRect.Columns
                        );
                    Rect up = new Rect(
                        row + 1, currentRect.Column,
                        currentRect.Rows - downRows, currentRect.Columns
                        );

                    stack.Push(down);
                    stack.Push(up);
                }

                yield return _maze;
            }
        }

        public override string ToString()
        {
            return "Recursive division";
        }
    }
}