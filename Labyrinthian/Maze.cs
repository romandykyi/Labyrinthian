using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Labyrinthian
{
    /// <summary>
    /// Клас для репрезентації лабіринтів у вигляді графу
    /// </summary>
    public abstract class Maze
    {
        private float[] _sizes = null!;

        // Всі з'єднання клітинок(наявність двох клітинок тут означає, що між ними є прохід)
        protected readonly HashSet<MazeEdge> Connections;
        /// <summary>
        /// Клітинки лабіринту
        /// </summary>
        public MazeCell[] Cells { get; protected set; } = null!;

        public readonly List<MazePath> Paths;

        public string Description { get; set; }

        /// <summary>
        /// Розмір лабіринту в різних вимірах
        /// </summary>
        public virtual float[] Sizes => _sizes ?? CalculateSizes();
        /// <summary>
        /// Кількість вимірів лабіринту
        /// </summary>
        public abstract int Dimensions { get; }

        public delegate void RelationChangedEvent(Maze owner, MazeEdge relation, bool isConnected);

        /// <summary>
        /// Подія, яка викликається при зміни стану стіни між двома клітинками
        /// </summary>
        public event RelationChangedEvent? RelationChanged;

        private float[] CalculateSizes()
        {
            _sizes = Enumerable.Repeat(0f, Dimensions).ToArray();
            foreach (var cell in Cells)
            {
                for (int i = 0; i < GetCellPointsNumber(cell); i++)
                {
                    float[] point = GetCellPoint(cell, i);
                    for (int dim = 0; dim < Dimensions; dim++)
                    {
                        if (point[dim] > _sizes[dim])
                            _sizes[dim] = point[dim];
                    }
                }
            }
            return _sizes;
        }

        protected Maze()
        {
            Description = string.Empty;
            Connections = new HashSet<MazeEdge>();
            Paths = new List<MazePath>();
        }

        /// <summary>
        /// Створення графу лабіринту
        /// </summary>
        /// <param name="size">розмір лабіринту(кількість клітинок)</param>
        protected Maze(int size) : this()
        {
            Cells = new MazeCell[size];
            for (int i = 0; i < Cells.Length; ++i)
            {
                Cells[i] = new MazeCell(i);
            }
        }

        // Перевірка, чи всі клітинки з'єднані
        protected void CheckIfGraphConnected()
        {
            int k = 0;
            MarkedCells visited = new MarkedCells(this);
            Stack<MazeCell> dfsStack = new Stack<MazeCell>(1);
            dfsStack.Push(Cells[0]);

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

            if (k != Cells.Length)
            {
                throw new MazeGraphIsDisconnectedException(k, this);
            }
        }

        /// <summary>
        /// З'єднати дві клітинки(тепер між клітинками не буде стіни)
        /// </summary>
        /// <returns>true - клітинки з'єднано, false - клітинки вже з'єднані</returns>
        /// <exception cref="CellIsNotMazePartException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        /// <exception cref="ArgumentNullException" />
        public bool ConnectCells(MazeCell cell1, MazeCell cell2)
        {
            // Null checks just in case
            if (cell1 is null)
                throw new ArgumentNullException(nameof(cell1), "cells cannot be null");
            if (cell2 is null)
                throw new ArgumentNullException(nameof(cell2), "cells cannot be null");

            if (!cell1.IsMazePart)
                throw new CellIsNotMazePartException(nameof(cell1), "You cannot connect cells which are not parts of maze");
            if (!cell2.IsMazePart)
                throw new CellIsNotMazePartException(nameof(cell2), "You cannot connect cells which are not parts of maze");
            if (!MazeCell.AreNeighbors(cell1, cell2))
                throw new CellsAreNotNeighborsException(cell1, cell2, "You can connect only neighboring cells");

            MazeEdge relation = MazeEdge.GetMinMax(cell1, cell2);
            bool result = Connections.Add(relation);
            if (result)
                RelationChanged?.Invoke(this, relation, true);

            return result;
        }

        /// <summary>
        /// Роз'єднати дві клітинки(тепер між клітинками буде стіна)
        /// </summary>
        /// <returns>true - клітинки роз'єднано, false - клітинки вже роз'єднані</returns>
        /// <exception cref="CellIsNotMazePartException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        /// <exception cref="ArgumentNullException" />
        public bool BlockCells(MazeCell cell1, MazeCell cell2)
        {
            if (cell1 is null)
                throw new ArgumentNullException(nameof(cell1), "cells cannot be null");
            if (cell2 is null)
                throw new ArgumentNullException(nameof(cell2), "cells cannot be null");
            if (!cell1.IsMazePart)
                throw new CellIsNotMazePartException(nameof(cell1), "You cannot block cells which are not parts of maze");
            if (!cell2.IsMazePart)
                throw new CellIsNotMazePartException(nameof(cell2), "You cannot block cells which are not parts of maze");
            if (!MazeCell.AreNeighbors(cell1, cell2))
                throw new CellsAreNotNeighborsException(cell1, cell2, "You can block only neighboring cells");

            MazeEdge relation = MazeEdge.GetMinMax(cell1, cell2);
            bool result = Connections.Remove(relation);
            if (result)
                RelationChanged?.Invoke(this, relation, false);

            return result;
        }

        /// <summary>
        /// Чи між двома клітинками існує прохід
        /// </summary>
        /// <returns>true - між клітинками існує прохід, false - між клітинками стіна/вони не сусіди</returns>
        /// <exception cref="CellsAreNotNeighborsException" />
        /// <exception cref="ArgumentNullException" />
        public bool AreCellsConnected(MazeCell cell1, MazeCell cell2)
        {
            if (cell1 is null)
                throw new ArgumentNullException(nameof(cell1), "cells cannot be null");
            if (cell2 is null)
                throw new ArgumentNullException(nameof(cell2), "cells cannot be null");

            return cell1.IsMazePart && cell2.IsMazePart &&
                Connections.Contains(MazeEdge.GetMinMax(cell1, cell2));
        }

        /// <summary>
        /// Ініціалізувати граф лабіринту
        /// </summary>
        protected void InitGraph()
        {
            foreach (var cell in Cells) cell.SetNeighbors(GetDirectedNeighbors(cell));
            CheckIfGraphConnected();
        }

        protected abstract MazeCell?[] GetDirectedNeighbors(MazeCell cell);

        /// <summary>
        /// Отримати кількість точок клітинки
        /// </summary>
        public abstract int GetCellPointsNumber(MazeCell cell);

        /// <summary>
        /// Отримати точки клітинки
        /// </summary>
        /// <param name="cell">клітинка, точку якої ми отримуємо</param>
        /// <param name="pointIndex">індекс точки</param>
        /// <returns>вектор точки клітинки</returns>
        public abstract float[] GetCellPoint(MazeCell cell, int pointIndex);

        /// <summary>
        /// Отримати точку центру клітинки
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public virtual float[] GetCellCenter(MazeCell cell)
        {
            if (cell is null) throw new ArgumentNullException(nameof(cell), "cell cannot be null");

            // Дефолтний алгоритм знаходження центру багатокутника
            int n = GetCellPointsNumber(cell);
            float[] center = Enumerable.Repeat(0f, Dimensions).ToArray();
            for (int i = 0; i < n; ++i)
            {
                float[] point = GetCellPoint(cell, i);
                for (int j = 0; j < center.Length; ++j)
                {
                    center[j] += point[j];
                }
            }
            for (int i = 0; i < center.Length; ++i)
            {
                center[i] /= n;
            }

            return center;
        }

        /// <summary>
        /// Отримати розташування стіни
        /// </summary>
        public abstract PathSegment GetWallPosition(MazeEdge wall);

        protected abstract PathSegment GetPath(MazeEdge relation);

        /// <summary>
        /// Отримати шлях від однієї сусідньої клітинки до іншої
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CellsAreNotNeighborsException" />
        public PathSegment GetPathBetweenCells(MazeCell from, MazeCell to)
        {
            if (from is null)
                throw new ArgumentNullException(nameof(from), "cells cannot be null");
            if (to is null)
                throw new ArgumentNullException(nameof(to), "cells cannot be null");

            return GetPathBetweenCells(new MazeEdge(from, to));
        }

        public virtual PathSegment GetPathBetweenCells(MazeEdge relation)
        {
            if (!MazeCell.AreNeighbors(relation.Cell0, relation.Cell1))
                throw new CellsAreNotNeighborsException(relation.Cell0, relation.Cell1, "This function designed for getting path only between two neighboring cells");

            float[] start, end;
            if (!relation.Cell0.IsMazePart)
            {
                start = GetWallPosition(relation.Inverted).Center;
                end = GetCellCenter(relation.Cell1);
            }
            else if (!relation.Cell1.IsMazePart)
            {
                start = GetCellCenter(relation.Cell0);
                end = GetWallPosition(relation).Center;
            }
            else
            {
                return GetPath(relation);
            }

            Vector2 startV = PositionTo2DPoint(start),
                endV = PositionTo2DPoint(end);

            return new Line(startV, endV);
        }

        /// <summary>
        /// Перенести точку на площину(для SVG конвертації)
        /// </summary>
        /// <param name="position"></param>
        public abstract Vector2 PositionTo2DPoint(float[] position);

        /// <summary>
        /// Конвертувати клітинку в svg
        /// </summary>
        /// <param name="cell">клітинка, яку потрібно конвертувати</param>
        /// <param name="cellSize">розмір клітинки</param>
        /// <param name="offset">відступ</param>
        public abstract string CellToSvg(MazeCell cell, float cellSize, float offset);

        public override string ToString()
        {
            return Description;
        }
    }
}