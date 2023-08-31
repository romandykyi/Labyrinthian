using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public static class MazeUtils
    {
        /// <summary>
        /// Connect all cells in the maze.
        /// There will be no walls in the maze after calling this method(not including outer walls)
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
        /// Get all walls of the given cell
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static MazeEdge[] GetWallsOfCell(this Maze maze, MazeCell cell)
        {
            if (cell is null)
                throw new ArgumentNullException(nameof(cell));

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
        /// Get all walls.
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="includeOuter">
        /// if <see langword="true"/> outer walls will be also returned;
        /// otherwise if <see langword="false"/> only inner walls will be returned.
        /// </param>
        /// <returns>
        /// inner and outer walls if <paramref name="includeOuter"/> is <see langword="true"/>;
        /// only inner walls if <paramref name="includeOuter"/> is <see langword="false"/>.
        /// </returns>
        public static IEnumerable<MazeEdge> GetWalls(this Maze maze, bool includeOuter = true)
        {
            // Entries that will not be returned
            HashSet<MazeEdge> entries;
            if (includeOuter)
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
                    cell.FindNeighbors(c => !maze.AreCellsConnected(c, cell), includeOuter);
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
        /// Get all outer walls.
        /// </summary>
        /// <returns>
        /// Outer walls of the maze.
        /// </returns>
        public static IEnumerable<MazeEdge> GetOuterWalls(this Maze maze)
        {
            foreach (var cell in maze.Cells)
            {
                foreach (var neighbor in cell.DirectedNeighbors)
                {
                    if (neighbor != null && !neighbor.IsMazePart)
                        yield return new MazeEdge(cell, neighbor);
                }
            }
        }

        /// <summary>
        /// Find all dead ends.
        /// </summary>
        /// <returns>
        /// All dead ends.
        /// </returns>
        public static IEnumerable<MazeCell> FindDeadEnds(this Maze maze)
        {
            return from cell in maze.Cells
                   where cell.FindNeighbors(
                       neighbor => maze.AreCellsConnected(cell, neighbor)).Count == 1
                   select cell;
        }

        /// <summary>
        /// Get all base graph maze edges.
        /// </summary>
        /// <returns>
        /// All base graph edges.
        /// </returns>
        public static IEnumerable<MazeEdge> GetBaseGraphEdges(this Maze maze)
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
        /// Find all edges of passages graph of the maze using DFS.
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="predicate">Predicate which determines whether we should include an edge into DFS or ignore it.</param>
        /// <param name="includeExits">
        /// If <see langword="true"/> then edges that lead to entries/exits will be 
        /// returned. Note that <paramref name="predicate"/> will not be called for those edges.
        /// </param>
        public static IEnumerable<MazeEdge> FindGraphEdgesDFS(this Maze maze,
            Predicate<MazeEdge> predicate, bool includeExits = false)
        {
            if (includeExits)
            {
                foreach (var path in maze.Paths)
                {
                    yield return path.Entry;
                    yield return path.Exit;
                }
            }

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
                    currentCell = currentEdge.Cell2;

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

        /// <summary>
        /// Check if base graph of maze is connected.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if base graph is connected; <see langword="false" /> otherwise.
        /// </returns>
        public static bool CheckIfGraphConnected(this Maze maze)
        {
            int k = 0;
            MarkedCells visited = new MarkedCells(maze);
            Stack<MazeCell> dfsStack = new Stack<MazeCell>(1);
            dfsStack.Push(maze.Cells[0]);

            while (dfsStack.Count > 0)
            {
                MazeCell current = dfsStack.Pop();

                if (visited[current]) continue;
                visited[current] = true;
                foreach (var neighbor in current.Neighbors)
                {
                    dfsStack.Push(neighbor);
                }
                k++;
            }

            return k == maze.Cells.Length;
        }
    }
}
