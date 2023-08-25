using System;

namespace Labyrinthian
{
    [Serializable]
    public sealed class PathNotFoundException : Exception
    { 
        public PathNotFoundException() : 
            base("Couldn't find the path between two cells. Make sure that both cells belong to the same maze and that the maze has no isolated cells.")
        { }
    }
}