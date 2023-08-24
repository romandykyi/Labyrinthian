using System;

namespace Labyrinthian
{
    [Serializable]
    public sealed class MazeGraphIsDisconnectedException : Exception
    {
        public MazeGraphIsDisconnectedException(int cellsConnected, Maze maze) :
            base($"Graph of the maze is disconnected. {cellsConnected}/{maze.Cells.Length} found cells are connected") { }
    }
}