using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinthian
{
    public sealed class MazeCell : IEquatable<MazeCell>
    {
        /// <summary>
        /// ������ ������� � �����
        /// (���� ��'�����, �� �� ������� ����������� ���� ���������,
        /// (-index - 1) ��� �� ��������, � ����� ����������� ����)
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// �� �����, �� ����������� � ����� ��������
        /// </summary>
        public MazeCell[] Neighbors { get; private set; } = null!;
        /// <summary>
        /// �� �����(����� �� � ����� ��������), ��������� �� ���������:
        /// ������ ��������(��������� �� 0) - ��������, �������� - ����������� ��
        /// ���������. ��� ����� ������ ���� null, ���� ����� �������� ��� ���� ������� �� ����.
        /// ���� ���������� � null, ���� ������� �� � �������� ��������, ���� ���������� �� �������
        /// ����������������� ��� ������ �������
        /// </summary>
        public MazeCell?[] DirectedNeighbors { get; private set; } = null!;

        public bool IsMazePart => Index >= 0;

        public MazeCell(int index)
        {
            Index = index;
        }

        /// <summary>
        /// �������� ������� ���� ������ ��������
        /// (���� ������ ��� ����, ��� ����� ���� ����� ���������� ����� ����)
        /// </summary>
        /// <param name="neighbor">���� �������</param>
        /// <param name="direction">�������� �� �����</param>
        public static MazeCell CreateEdgeCell(MazeCell neighbor, int direction)
        {
            return new MazeCell(-direction - 1)
            {
                Neighbors = new MazeCell[1] { neighbor },
                DirectedNeighbors = null! // DirectedNeighbors shouldn't be used for cells that are not parts of maze
            };
        }

        /// <summary>
        /// ������ ����� �������
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
        /// ������ ����� �������, �� ������������ �����
        /// </summary>
        /// <param name="predicate">�������� ���� �����, �� �������� �������</param>
        public List<MazeCell> FindNeighbors(Predicate<MazeCell> predicate)
        {
            return FindNeighbors(Neighbors, predicate);
        }

        /// <summary>
        /// ������ ����� �������(������� ���� ��������� ���������), �� ������������ �����
        /// </summary>
        /// <param name="predicate">�������� ���� �����, �� �������� �������</param>
        public List<MazeCell> FindAllNeighbors(Predicate<MazeCell> predicate)
        {
            return FindNeighbors(DirectedNeighbors, predicate);
        }

        /// <summary>
        /// ������ ����� �������, �� ������������ �����
        /// </summary>
        /// <param name="predicate">�������� ���� �����, �� �������� �������</param>
        /// <param name="includeBorders">�� ����� ����� ����� �����</param>
        public List<MazeCell> FindNeighbors(Predicate<MazeCell> predicate, bool includeBorders)
        {
            return FindNeighbors(includeBorders ? DirectedNeighbors : Neighbors, predicate);
        }

        /// <summary>
        /// ������ �����, ���� ������ ������ �����(null, ���� ����� �� ��������)
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
        /// ���������� ������ �����, ���� ������ ������ �����
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
        /// ���������, �� ������� � �������
        /// </summary>
        public static bool AreNeighbors(MazeCell cell1, MazeCell cell2)
        {
            if (cell1.IsMazePart) return cell1.DirectedNeighbors.Contains(cell2);
            return cell1.Neighbors[0] == cell2;
        }
    }
}