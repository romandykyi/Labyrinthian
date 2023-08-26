using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// A list with fast access time by index, supporting fast removal(by index) and addition without
    /// saving the order of elements. Capacity cannot be changed.
    /// </summary>
    public sealed class StaticUnorderedList<T> : IList<T>
    {
        private readonly T[] _arr;

        /// <summary>
        /// The number of available elements.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// The maximum number of available elements(cannot be changed).
        /// </summary>
        public readonly int Capacity;

        public bool IsReadOnly => false;

        private void CheckBounds(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Find the index of a specific element.
        /// An O(N) operation.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns>The index of the 'item' element, or -1 if not found.</returns>
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
        /// Insert an element at a specific index.
        /// An O(1) operation; the previous element at the given index moves to the end.
        /// </summary>
        /// <param name="index">The index where the element should be inserted.</param>
        /// <param name="item">The element to insert.</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Insert(int index, T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = _arr[index];
            _arr[index] = item;
        }

        /// <summary>
        /// Remove an element from the list.
        /// An O(N) operation.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>True if the element was removed, false if the element does not exist.</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Remove an element at the specified index. After the operation, the last element 
        /// of the list moves to the position of the removed element.
        /// An O(1) operation.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            this[index] = this[^1];
            this[^1] = default!;

            Count--;
        }

        /// <summary>
        /// Add an element to the end.
        /// An O(1) operation.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Add(T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = item;
        }

        /// <summary>
        /// Get an element from the list and remove it.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>The element at the specified index.</returns>
        public T Pop(int index)
        {
            T result = this[index];
            RemoveAt(index);

            return result;
        }

        /// <summary>
        /// Clear the list.
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
        /// Check if an element exists in the list.
        /// </summary>
        /// <param name="item">The element to check for existence.</param>
        /// <returns>True if the element exists, otherwise false.</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Copy the list to an array.
        /// </summary>
        /// <param name="array">The array to copy the list into.</param>
        /// <param name="arrayIndex">The starting index in the array.</param>
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

        /// <summary>
        /// Get element of the list by the index.
        /// </summary>
        /// <returns>
        /// Element of the list with the given index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"/>
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
