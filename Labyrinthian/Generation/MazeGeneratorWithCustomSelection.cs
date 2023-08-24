using System;

namespace Labyrinthian
{
    public delegate int MazeCellSelection(Random rnd, int number);

    /// <summary>
    /// √енератор лаб≥ринт≥в з≥ зм≥нюваною виб≥ркою
    /// </summary>
    public abstract class MazeGeneratorWithCustomSelection : MazeGenerator
    {
        public readonly static MazeCellSelection RandomSelection = (rnd, n) => rnd.Next(n);

        protected readonly MazeCellSelection Selection;

        protected MazeGeneratorWithCustomSelection(Maze maze, int seed, MazeCellSelection? selection = null, MazeCell? initialCell = null)
            : base(maze, seed, initialCell) 
        {
            Selection = selection ?? RandomSelection;
        }
        protected MazeGeneratorWithCustomSelection(Maze maze, MazeCellSelection? selection = null, MazeCell? initialCell = null) :
            this(maze, Environment.TickCount, selection, initialCell) { }
    }
}