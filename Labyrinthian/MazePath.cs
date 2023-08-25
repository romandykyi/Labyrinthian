using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public sealed class MazePath
    {
        private MazeCell[]? _path = null;
        private MazeEdge m_entry, m_exit;

        public readonly Maze Maze;
        public MazeEdge Entry
        {
            get => m_entry;
            set
            {
                m_entry = value;
                _path = null;
            }
        }
        public MazeEdge Exit
        {
            get => m_exit;
            set
            {
                m_exit = value;
                _path = null;
            }
        }

        public MazeCell[] Path => _path ?? FindPath();

        public MazePath(Maze maze, MazeEdge entry, MazeEdge exit)
        {
            Maze = maze;
            Entry = entry;
            Exit = exit;
        }

        private MazeCell[] FindPath()
        {
            Queue<List<MazeCell>> bfsQueue = new Queue<List<MazeCell>>(1);
            MarkedCells visited = new MarkedCells(Maze);

            bfsQueue.Enqueue(new List<MazeCell>() { Entry.Cell1 });

            while (bfsQueue.Count > 0)
            {
                List<MazeCell> currentPath = bfsQueue.Dequeue();
                MazeCell currentCell = currentPath[^1];
                if (currentCell == Exit.Cell1)
                {
                    return _path = currentPath.ToArray();
                }
                if (visited[currentCell]) continue;
                visited[currentCell] = true;

                foreach (MazeCell cell in currentCell.Neighbors)
                {
                    if (!Maze.AreCellsConnected(cell, currentCell)) continue;

                    List<MazeCell> newPath = new List<MazeCell>(currentPath.Count + 1);
                    newPath.AddRange(currentPath);
                    newPath.Add(cell);

                    bfsQueue.Enqueue(newPath);
                }
            }

            throw new PathNotFoundException();
        }

        public IEnumerable<PathSegment> GetSegments()
        {
            MazeCell previousCell = Entry.Cell1;
            if (!Entry.Cell2.IsMazePart)
            {
                yield return Maze.GetPathBetweenCells(Entry.Cell2, Entry.Cell1);
            }
            else
            {
                yield return Maze.GetPathBetweenCells(Entry.Cell1, Entry.Cell2);
            }

            var path = Path;
            for (int i = 1; i < path.Length; ++i)
            {
                yield return Maze.GetPathBetweenCells(previousCell, path[i]);
                previousCell = path[i];
            }
            yield return Maze.GetPathBetweenCells(Exit.Cell1, Exit.Cell2);
        }

        public void Recalculate()
        {
            _path = FindPath();
        }

        public string ToSVG(float cellSize, float offset, string strokeColor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<path stroke=\"{strokeColor}\" d=\"");

            var path = GetSegments();
            stringBuilder.Append(path.First().MoveToStartSvg(cellSize, offset));
            foreach (var segment in path)
            {
                stringBuilder.Append(segment.MoveToEndSvg(cellSize, offset));
            }

            stringBuilder.Append($"\"/>");
            return stringBuilder.ToString();
        }
    }
}