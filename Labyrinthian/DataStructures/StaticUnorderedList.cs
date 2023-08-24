using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// ������ � ������� �����
    /// ������� �� �������, ��������� � ���������
    /// </summary>
    public sealed class StaticUnorderedList<T> : IList<T>
    {
        private readonly T[] _arr;

        /// <summary>
        /// ʳ������ ��������� ��������
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// ����������� ������� ��������� ��������
        /// </summary>
        public readonly int Capacity;

        public bool IsReadOnly => false;

        private void CheckBounds(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// ������ ������ ������� ��������.
        /// O(N) ��������
        /// </summary>
        /// <param name="item">�������, ���� ������</param>
        /// <returns>������ �������� item, ��� -1 ���� �������� �� ��������</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (EqualityComparer<T>.Default.Equals(_arr[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// �������� ������� �� ������ ������.
        /// O(1) ��������, ��������� ������� �� �������� ������� ��� � �����
        /// </summary>
        /// <param name="index">������, ���� ����� �������� �������</param>
        /// <param name="item">�������, ���� ����� ��������</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Insert(int index, T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = _arr[index];
            _arr[index] = item;
        }

        /// <summary>
        /// �������� ������� � ������.
        /// O(N) ��������
        /// </summary>
        /// <param name="item">�������</param>
        /// <returns>true - ������� ��������, false - �������� �� ����</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// �������� ������� �� �������� �������. ϳ��� �������� ������� ������� ������ ��� �� ���� ����������.
        /// O(1) ��������
        /// </summary>
        /// <param name="index">������ ��������, ���� ������� ��������</param>
        public void RemoveAt(int index)
        {
            this[index] = this[^1];
            this[^1] = default!;

            Count--;
        }

        /// <summary>
        /// ������ ������� � �����.
        /// O(1) ��������
        /// </summary>
        /// <param name="item">�������, ���� ������� ������</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Add(T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = item;
        }

        /// <summary>
        /// �������� ������� � ������ �� �������� ����
        /// </summary>
        /// <param name="index">������ ��������</param>
        /// <returns>������� �� �������� �������</returns>
        public T Pop(int index)
        {
            T result = this[index];
            RemoveAt(index);

            return result;
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        public void Clear()
        {
            Count = 0;

            for (int i = 0; i < _arr.Length; ++i)
            {
                _arr[i] = default!;
            }
        }

        /// <summary>
        /// ��������� �������� �������� � ������
        /// </summary>
        /// <param name="item">�������, �������� ����� ������� ���������</param>
        /// <returns>true ���� ������� ����, ������ false</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// �������� ������ � �����
        /// </summary>
        /// <param name="array">�����, � ���� ������� ��������� ������</param>
        /// <param name="arrayIndex">���������� ������ ������</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_arr, 0, array, arrayIndex, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i) yield return _arr[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; ++i) yield return _arr[i];
        }

        public StaticUnorderedList(int capacity)
        {
            Count = 0;
            Capacity = capacity;

            _arr = new T[capacity];
        }
        public StaticUnorderedList(T[] array) : this(array, array.Length) { }
        public StaticUnorderedList(T[] array, int capacity)
        {
            Count = array.Length;
            Capacity = capacity;

            _arr = new T[Count];
            array.CopyTo(_arr, 0);
        }
        public StaticUnorderedList(IEnumerable<T> enumerable, int capacity) : this(capacity)
        {
            foreach (T obj in enumerable)
            {
                if (Count == Capacity) break;
                _arr[Count++] = obj;
            }
        }

        public T this[int index]
        {
            get
            {
                CheckBounds(index);
                return _arr[index];
            }
            set
            {
                CheckBounds(index);
                _arr[index] = value;
            }
        }
    }
}