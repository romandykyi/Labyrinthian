using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Labyrinthian
{
    public static class MazeUtils
    {
        /// <summary>
        /// З'єднати всі сусідні клітинки між собою(лабіринт не буде мати стін)
        /// </summary>
        public static void ConnectAllCells(this Maze maze)
        {
            foreach (MazeCell cell in maze.Cells)
            {
                foreach (MazeCell neighbor in cell.Neighbors)
                {
                    maze.ConnectCells(cell, neighbor);
                }
            }
        }

        /// <summary>
        /// Отримати всі стіни клітинки(в межах лабіринту)
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static MazeEdge[] GetWallsOfCell(this Maze maze, MazeCell cell)
        {
            if (cell is null)
                throw new ArgumentNullException(nameof(cell), "cell cannot be null");

            List<MazeCell> neighbors = cell.FindNeighbors(
                neighbor => !maze.AreCellsConnected(cell, neighbor)
                );
            MazeEdge[] walls = new MazeEdge[neighbors.Count];
            for (int i = 0; i < neighbors.Count; ++i)
            {
                walls[i] = new MazeEdge(cell, neighbors[i]);
            }
            return walls;
        }

        /// <summary>
        /// Знайти всі стіни
        /// </summary>
        public static IEnumerable<MazeEdge> GetWalls(this Maze maze, bool includeBorders = true)
        {
            // Входи, які не треба малювати
            HashSet<MazeEdge> entries;
            if (includeBorders)
            {
                entries = new HashSet<MazeEdge>(maze.Paths.Count * 2);
                foreach (MazePath path in maze.Paths)
                {
                    entries.Add(path.Entry);
                    entries.Add(path.Exit);
                }
            }
            else
            {
                entries = new HashSet<MazeEdge>();
            }

            foreach (MazeCell cell in maze.Cells)
            {
                var blockedNeighbors =
                    cell.FindNeighbors(c => !maze.AreCellsConnected(c, cell), includeBorders);
                foreach (MazeCell neighbor in blockedNeighbors)
                {
                    if (cell.Index < neighbor.Index || !neighbor.IsMazePart)
                    {
                        MazeEdge relation = new MazeEdge(cell, neighbor);
                        if (!entries.Contains(relation))
                            yield return relation;
                    }
                }
            }
        }

        /// <summary>
        /// Знайти всі крайні стіни
        /// </summary>
        public static IEnumerable<MazeEdge> GetEdgeWalls(this Maze maze)
        {
            foreach (var cell in maze.Cells)
            {
                // maze.Cells should contain only cells that are maze parts
                // so cell.DirectedNeighbors should not be null here with correct
                // implementation of Maze class
                foreach (var neighbor in cell.DirectedNeighbors)
                {
                    if (neighbor != null && !neighbor.IsMazePart)
                        yield return new MazeEdge(cell, neighbor);
                }
            }
        }

        /// <summary>
        /// Знайти усі глухі кути
        /// </summary>
        public static IEnumerable<MazeCell> FindDeadEnds(this Maze maze)
        {
            return from cell in maze.Cells
                   where cell.FindNeighbors(
                       neighbor => maze.AreCellsConnected(cell, neighbor)).Count == 1
                   select cell;
        }

        /// <summary>
        /// Знайти усі ребра лабіринту(пошук по масиву клітинок)
        /// </summary>
        public static IEnumerable<MazeEdge> GetGraphEdges(this Maze maze)
        {
            foreach (MazeCell cell in maze.Cells)
            {
                foreach (MazeCell neighbor in cell.Neighbors)
                {
                    if (cell.Index < neighbor.Index)
                    {
                        yield return new MazeEdge(cell, neighbor);
                    }
                }
            }
        }

        /// <summary>
        /// Знайти ребра лабіринту(використовуючи DFS)
        /// </summary>
        /// <param name="maze"></param>
        public static IEnumerable<MazeEdge> GetGraphEdgesDFS(this Maze maze, Predicate<MazeEdge> predicate)
        {
            Stack<MazeEdge> dfsStack = new Stack<MazeEdge>();

            MarkedCells visited = new MarkedCells(maze);
            int visitedNumber = 0;
            int lastVisitedIndex = 0;

            while (visitedNumber < maze.Cells.Length)
            {
                MazeCell? currentCell = null;
                if (dfsStack.Count == 0)
                {
                    while (lastVisitedIndex < maze.Cells.Length)
                    {
                        MazeCell cell = maze.Cells[lastVisitedIndex++];
                        if (!visited[cell])
                        {
                            currentCell = cell;
                            break;
                        }
                    }
                    if (currentCell == null) yield break;
                }
                else
                {
                    MazeEdge currentEdge = dfsStack.Pop();
                    currentCell = currentEdge.Cell1;

                    yield return currentEdge;
                }

                visited[currentCell] = true;
                visitedNumber++;

                foreach (var neighbor in currentCell.Neighbors)
                {
                    MazeEdge edge = new MazeEdge(currentCell, neighbor);
                    if (predicate(edge) && !visited[neighbor])
                    {
                        dfsStack.Push(edge);
                    }
                }
            }
        }
    }
}