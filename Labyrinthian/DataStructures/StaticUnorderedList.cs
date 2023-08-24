using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Список зі швидким часом
    /// доступу по індексу, видалення і додавання
    /// </summary>
    public sealed class StaticUnorderedList<T> : IList<T>
    {
        private readonly T[] _arr;

        /// <summary>
        /// Кількість доступних елементів
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Максимальна кількість доступних елементів
        /// </summary>
        public readonly int Capacity;

        public bool IsReadOnly => false;

        private void CheckBounds(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Знайти індекс певного елементу.
        /// O(N) операція
        /// </summary>
        /// <param name="item">елемент, який шукаємо</param>
        /// <returns>індекс елементу item, або -1 якщо елементу не знайдено</returns>
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
        /// Вставити елемент на певний індекс.
        /// O(1) операція, попередній елемент по заданому індексу йде в кінець
        /// </summary>
        /// <param name="index">індекс, куди треба вставити елемент</param>
        /// <param name="item">елемент, який треба вставити</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Insert(int index, T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = _arr[index];
            _arr[index] = item;
        }

        /// <summary>
        /// Видалити елемент зі списку.
        /// O(N) операція
        /// </summary>
        /// <param name="item">елемент</param>
        /// <returns>true - елемент видалено, false - елементу не існує</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Видалити елемент по заданому індексу. Після операції останній елемент списку стає на місце видаленого.
        /// O(1) операція
        /// </summary>
        /// <param name="index">індекс елементу, який потрібно видалити</param>
        public void RemoveAt(int index)
        {
            this[index] = this[^1];
            this[^1] = default!;

            Count--;
        }

        /// <summary>
        /// Додати елемент в кінець.
        /// O(1) операція
        /// </summary>
        /// <param name="item">елемент, який потрібно додати</param>
        /// <exception cref="IndexOutOfRangeException" />
        public void Add(T item)
        {
            if (Capacity == Count)
                throw new IndexOutOfRangeException("StaticUnorderedList cannot be extended");

            _arr[Count++] = item;
        }

        /// <summary>
        /// Отримати елемент зі списку та видалити його
        /// </summary>
        /// <param name="index">індекс елементу</param>
        /// <returns>елемент по заданому індексу</returns>
        public T Pop(int index)
        {
            T result = this[index];
            RemoveAt(index);

            return result;
        }

        /// <summary>
        /// Очистити список
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
        /// Перевірити наявність елементу в списку
        /// </summary>
        /// <param name="item">елемент, наявність якого потрібно перевірити</param>
        /// <returns>true якщо елемент існує, інакше false</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Копіювати список в масив
        /// </summary>
        /// <param name="array">масив, в який потрібно скопіювати список</param>
        /// <param name="arrayIndex">початковий індекс масиву</param>
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