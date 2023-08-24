using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public sealed class MazeCell : IEquatable<MazeCell>
    {
        /// <summary>
        /// Індекс клітинки в масиві
        /// (якщо від'ємний, то ця клітинка знаходиться поза лабіринтом,
        /// (-index - 1) тоді це напрямок, в якому знаходиться сусід)
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// Всі сусіди, які знаходяться в межах лабіринту
        /// </summary>
        public MazeCell[] Neighbors { get; private set; } = null!;
        /// <summary>
        /// Всі сусіди(навіть не в межах лабіринту), розподілені по напрямкам:
        /// парний напрямок(починаючи від 0) - основний, непарний - протилежний до
        /// основного. Тут сусіди можуть бути null, якщо цього напрямку для даної клітинки не існує.
        /// Сама властивість є null, якщо клітинка не є частиною лабіринту, тому властивість не повинна
        /// використовуватися для крайніх клітинок
        /// </summary>
        public MazeCell?[] DirectedNeighbors { get; private set; } = null!;

        public bool IsMazePart => Index >= 0;

        public MazeCell(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Створити клітинку поза межами лабіринту
        /// (вони потрібні для того, щоб можна було легко відобразити крайні стіни)
        /// </summary>
        /// <param name="neighbor">сусід клітинки</param>
        /// <param name="direction">напрямок до сусіда</param>
        public static MazeCell CreateEdgeCell(MazeCell neighbor, int direction)
        {
            return new MazeCell(-direction - 1)
            {
                Neighbors = new MazeCell[1] { neighbor },
                DirectedNeighbors = null! // DirectedNeighbors shouldn't be used for cells that are not parts of maze
            };
        }

        /// <summary>
        /// Задати сусідів клітинці
        /// </summary>
        public void SetNeighbors(params MazeCell?[] directedNeighbors)
        {
            DirectedNeighbors = directedNeighbors;

            Neighbors = (from neighbor in directedNeighbors
                        where neighbor != null && neighbor.Index >= 0
                        select neighbor).ToArray();
        }

        private List<MazeCell> FindNeighbors(MazeCell?[] neighbors, Predicate<MazeCell> predicate)
        {
            List<MazeCell> result = new List<MazeCell>();
            foreach (MazeCell? neighbor in neighbors)
            {
                if (neighbor != null && predicate(neighbor)) result.Add(neighbor);
            }

            return result;
        }

        /// <summary>
        /// Знайти сусідів клітинки, які задовільняють умову
        /// </summary>
        /// <param name="predicate">предикат який вирішує, чи вибирати клітинку</param>
        public List<MazeCell> FindNeighbors(Predicate<MazeCell> predicate)
        {
            return FindNeighbors(Neighbors, predicate);
        }

        /// <summary>
        /// Знайти сусідів клітинки(клітинки поза лабіринтом рахуються), які задовільняють умову
        /// </summary>
        /// <param name="predicate">предикат який вирішує, чи вибирати клітинку</param>
        public List<MazeCell> FindAllNeighbors(Predicate<MazeCell> predicate)
        {
            return FindNeighbors(DirectedNeighbors, predicate);
        }

        /// <summary>
        /// Знайти сусідів клітинки, які задовільняють умову
        /// </summary>
        /// <param name="predicate">предикат який вирішує, чи вибирати клітинку</param>
        /// <param name="includeBorders">чи треба брати крайні стінки</param>
        public List<MazeCell> FindNeighbors(Predicate<MazeCell> predicate, bool includeBorders)
        {
            return FindNeighbors(includeBorders ? DirectedNeighbors : Neighbors, predicate);
        }

        /// <summary>
        /// Знайти сусіда, який виконує задану умову(null, якщо умова не виконана)
        /// </summary>
        public MazeCell? FindNeighbor(Predicate<MazeCell> predicate)
        {
            foreach (var neighbor in Neighbors)
            {
                if (predicate(neighbor)) return neighbor;
            }
            return null;
        }
        /// <summary>
        /// Спробувати знайти сусіда, який виконує задану умову
        /// </summary>
        public bool TryFindNeighbor(Predicate<MazeCell> predicate, out MazeCell neighbor)
        {
            neighbor = FindNeighbor(predicate)!;
            return neighbor != null;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MazeCell);
        }

        public bool Equals(MazeCell? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            // Equality is based on Index
            return Index == other.Index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index);
        }
        public static bool operator ==(MazeCell? left, MazeCell? right)
        {
            if (left is null)
            {
                // Both cells are null
                if (right is null) return true;

                // Only left cell is null
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(MazeCell? left, MazeCell? right)
        {
            return !(left == right);
        }

        public static bool IsNotNullAndMazePart(MazeCell? cell)
        {
            return cell != null && cell.IsMazePart;
        }

        /// <summary>
        /// Перевірити, чи клітинки є сусідами
        /// </summary>
        public static bool AreNeighbors(MazeCell cell1, MazeCell cell2)
        {
            if (cell1.IsMazePart) return cell1.DirectedNeighbors.Contains(cell2);
            return cell1.Neighbors[0] == cell2;
        }
    }
}