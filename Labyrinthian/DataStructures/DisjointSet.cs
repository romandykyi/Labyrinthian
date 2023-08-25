using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Union-Find data structure. Inspired by https://github.com/mondrasovic/UnionFind
    /// </summary>
    public sealed class DisjointSet<T> : IEnumerable<T> where T : IEquatable<T>
    {
        private readonly Dictionary<T, DisjointSetNode<T>> _nodes = new Dictionary<T, DisjointSetNode<T>>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nodes.Keys.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _nodes.Keys.GetEnumerator();
        }

        public DisjointSet() { }

        public DisjointSet(IEnumerable<T> nodes)
        {
            foreach (T node in nodes) Add(node);
        }

        private DisjointSetNode<T> Find(DisjointSetNode<T> node)
        {
            if (node.Parent != node)
            {
                node.Parent = Find(node.Parent);
            }
            return node.Parent;
        }

        public bool Add(T value)
        {
            if (!_nodes.ContainsKey(value))
            {
                _nodes[value] = new DisjointSetNode<T>(value);
                return true;
            }
            return false;
        }

        public T Find(T value)
        {
            return Find(_nodes[value]).Value;
        }

        public bool Union(T x, T y)
        {
            DisjointSetNode<T> 
                parentX = _nodes[x].Parent, 
                parentY = _nodes[y].Parent;

            if (parentX == parentY)
            {
                return false;
            }

            if (parentX.Rank < parentY.Rank)
            {
                (parentX, parentY) = (parentY, parentX);
            }

            parentY.Parent = parentX;
            if (parentX.Rank == parentY.Rank)
            {
                parentX.Rank++;
            }

            return true;
        }

        public bool Contains(T element)
        {
            return _nodes.ContainsKey(element);
        }
    }
}
