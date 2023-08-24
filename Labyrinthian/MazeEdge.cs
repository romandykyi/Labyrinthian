using System;

namespace Labyrinthian
{
    /// <summary>
    /// Структура для зберігання інформації про дві з'єднані клітинки
    /// </summary>
    public readonly struct MazeEdge : IEquatable<MazeEdge>
    {
        public readonly MazeCell Cell0;
        public readonly MazeCell Cell1;

        /// <summary>
        /// Напрямок другої клітинки відносно першої
        /// </summary>
        public int Direction => Cell0.IsMazePart ?
            Array.IndexOf(Cell0.DirectedNeighbors, Cell1): -Cell0.Index - 1;

        /// <summary>
        /// Інвертоване відношення
        /// </summary>
        public MazeEdge Inverted => new MazeEdge(Cell1, Cell0);

        public MazeEdge(MazeCell cell0, MazeCell cell1) : this()
        {
            Cell0 = cell0;
            Cell1 = cell1;
        }
        
        /// <summary>
        /// Створити структуру, в якій Cell0.index &lt; Cell1.index
        /// </summary>
        public static MazeEdge GetMinMax(MazeCell cell0, MazeCell cell1)
        {
            if (cell0.Index < cell1.Index)
            {
                return new MazeEdge(cell0, cell1);
            }
            else
            {
                return new MazeEdge(cell1, cell0);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is MazeEdge relation && Equals(relation);
        }

        public bool Equals(MazeEdge other)
        {
            return Cell0 == other.Cell0 && Cell1 == other.Cell1;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cell0, Cell1);
        }

        public static bool operator ==(MazeEdge left, MazeEdge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MazeEdge left, MazeEdge right)
        {
            return !(left == right);
        }
    }
}