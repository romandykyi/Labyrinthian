using System;

namespace Labyrinthian
{
    [Serializable]
    public sealed class CellsAreNotNeighborsException : Exception
    {
        public CellsAreNotNeighborsException(MazeCell cell1, MazeCell cell2, string extraMessage = "") :
            base($"cell0(index = {cell1.Index}) and cell1(index = {cell2.Index}) are not neighbors. {extraMessage}") { }
    }
}