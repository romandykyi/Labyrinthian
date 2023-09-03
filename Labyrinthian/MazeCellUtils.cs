using System;
using System.Collections.Generic;

namespace Labyrinthian
{
    public static class MazeCellUtils
    {
        private static List<MazeCell> FindNeighbors(MazeCell?[] neighbors, Predicate<MazeCell> predicate)
        {
            List<MazeCell> result = new List<MazeCell>();
            foreach (MazeCell? neighbor in neighbors)
            {
                if (neighbor != null && predicate(neighbor)) result.Add(neighbor);
            }

            return result;
        }

        /// <summary>
        /// Find all neighbors inside <see cref="MazeCell.Neighbors"/> that satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="predicate">Condition that found neighbor should satisfy</param>
        /// <returns>List of neighbors that satisfy <paramref name="predicate"/></returns>
        public static List<MazeCell> FindNeighbors(this MazeCell cell, Predicate<MazeCell> predicate)
        {
            return FindNeighbors(cell.Neighbors, predicate);
        }

        /// <summary>
        /// Find all neighbors inside <see cref="MazeCell.DirectedNeighbors"/> that satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="predicate">Condition that found neighbor should satisfy</param>
        /// <returns>List of neighbors that satisfy <paramref name="predicate"/></returns>
        public static List<MazeCell> FindAllNeighbors(this MazeCell cell, Predicate<MazeCell> predicate)
        {
            return FindNeighbors(cell.DirectedNeighbors, predicate);
        }

        /// <summary>
        /// Find all neighbors that satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="predicate">Condition that found neighbor should satisfy</param>
        /// <param name="includeOuterCells">
        /// if <see langword="true"/> then <see cref="MazeCell.DirectedNeighbors"/> is searched;
        /// otherwise <see cref="MazeCell.Neighbors"/> is searched
        /// </param>
        /// <returns>List of neighbors that satisfy <paramref name="predicate"/></returns>
        public static List<MazeCell> FindNeighbors(this MazeCell cell, Predicate<MazeCell> predicate, bool includeOuterCells)
        {
            return FindNeighbors(includeOuterCells ? cell.DirectedNeighbors : cell.Neighbors, predicate);
        }

        /// <summary>
        /// Find a first neighbor inside <see cref="MazeCell.Neighbors"/> that satisfies <paramref name="predicate"/>
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="predicate">Condition that found neighbor should satisfy</param>
        /// <returns>
        /// First found neighbor that satisfies <paramref name="predicate"/> or 
        /// <see langword="null"/> if neighbor is not found
        /// </returns>
        public static MazeCell? FindNeighbor(this MazeCell cell, Predicate<MazeCell> predicate)
        {
            foreach (var neighbor in cell.Neighbors)
            {
                if (predicate(neighbor)) return neighbor;
            }
            return null;
        }

        /// <summary>
        /// Try to find a first neighbor inside <see cref="MazeCell.Neighbors"/> that satisfies <paramref name="predicate"/>
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="predicate">Condition that found neighbor should satisfy</param>
        /// <param name="neighbor">Found neighbor(null! if isn't found)</param>
        /// <returns>
        /// <see langword="true"/> if neighbor is found;
        /// <see langword="false"/> if neighbor is not found
        /// </returns>
        public static bool TryFindNeighbor(this MazeCell cell, Predicate<MazeCell> predicate, out MazeCell neighbor)
        {
            neighbor = cell.FindNeighbor(predicate)!;
            return neighbor != null;
        }

        /// <summary>
        /// Check whether cell is not null and a maze part.
        /// </summary>
        /// <returns>
        /// <param name="cell"></param>
        /// <see langword="true"/> if cell is not null and a maze part;
        /// <see langword="false"/> if cell is null or an outer.
        /// </returns>
        public static bool IsNotNullAndMazePart(this MazeCell? cell)
        {
            return cell != null && cell.IsMazePart;
        }

        /// <summary>
        /// Check whether cell is not null and an outer.
        /// </summary>
        /// <returns>
        /// <param name="cell"></param>
        /// <see langword="true"/> if cell is not null and an outer;
        /// <see langword="false"/> if cell is null or a maze part.
        /// </returns>
        public static bool IsNotNullAndOuter(this MazeCell? cell)
        {
            return cell != null && !cell.IsMazePart;
        }
    }
}
