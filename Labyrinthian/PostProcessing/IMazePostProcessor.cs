using System.Collections.Generic;

namespace Labyrinthian
{
    public interface IMazePostProcessor
    {
        IEnumerable<Maze> Process(MazeGenerator caller);
    }
}
