using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Path that represents a path in the maze from entry to exit(edges with outer cell).
    /// </summary>
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
                // Reset the path
                _path = null;
            }
        }
        public MazeEdge Exit
        {
            get => m_exit;
            set
            {
                m_exit = value;
                // Reset the path
                _path = null;
            }
        }

        /// <summary>
        /// Path from <see cref="Entry"/> to <see cref="Exit"/> found using BFS.
        /// </summary>
        public MazeCell[] Path => _path ?? FindPath();

        /// <summary>
        /// Create a maze path.
        /// </summary>
        /// <param name="maze">Maze where path will be placed.</param>
        /// <param name="entry">Edge that represents an entry(second node should be outer cell).</param>
        /// <param name="exit">Edge that represents an exit(second node should be outer cell).</param>
        /// <exception cref="ArgumentException" />
        public MazePath(Maze maze, MazeEdge entry, MazeEdge exit)
        {
            if (entry.Cell2.IsMazePart)
            {
                throw new ArgumentException($"{nameof(entry.Cell2)} should be outer cell.", nameof(entry));
            }
            if (exit.Cell2.IsMazePart)
            {
                throw new ArgumentException($"{nameof(exit.Cell2)} should be outer cell.", nameof(exit));
            }

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

        /// <summary>
        /// Recalculate the path.
        /// </summary>
        public void Recalculate()
        {
            _path = FindPath();
        }

        /// <summary>
        /// Get all vector path segments that represent this path.
        /// </summary>
        /// <param name="includeOuterCells">
        /// If <see langword="true"/> then edges with outer cells will be returned too.
        /// </param>
        public IEnumerable<PathSegment> GetSegments(bool includeOuterCells = true)
        {
            MazeCell previousCell = Entry.Cell1;
            if (!Entry.Cell2.IsMazePart)
            {
                if (includeOuterCells)
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
            if (Exit.Cell2.IsMazePart || includeOuterCells)
            {
                yield return Maze.GetPathBetweenCells(Exit.Cell1, Exit.Cell2);
            }
        }
    }
}
