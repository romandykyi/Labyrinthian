using System;

namespace Labyrinthian
{
    [Serializable]
    public sealed class CellIsNotMazePartException : Exception
    {
        public CellIsNotMazePartException(string variableName, string extraMessage = "") :
            base($"{variableName} != a part of maze. {extraMessage}") { }
    }
}