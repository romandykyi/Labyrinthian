using System;
using System.Collections;
using System.Collections.Generic;

namespace Labyrinthian
{
    /// <summary>
    /// Union-Find data structure. Inspired by <seealso href="https://github.com/mondrasovic/UnionFind"/>
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

        /// <summary>
        /// Add a new element.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if element was added;
        /// <see langword="false"/> otherwise if element was already present.
        /// </returns>
        public bool Add(T x)
        {
            if (!_nodes.ContainsKey(x))
            {
                _nodes[x] = new DisjointSetNode<T>(x);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Follows the chain of parent pointers from a specified query 
        /// node <paramref name="x"/> until it reaches a root element.
        /// </summary>
        /// <returns>The root element of the <paramref name="x"/>.</returns>
        public T Find(T x)
        {
            return Find(_nodes[x]).Value;
        }

        /// <summary>
        /// Replaces the set containing <paramref name="x"/>
        /// and the set containing <paramref name="y"/> with their union.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if two sets were merged;
        /// <see langword="false"/> otherwise if two sets were already merged.
        /// </returns>
        /// <exception cref="KeyNotFoundException" />
        public bool Union(T x, T y)
        {
            if (!_nodes.ContainsKey(x))
            {
                throw new KeyNotFoundException($"Key {x} is not found");
            }
            if (!_nodes.ContainsKey(y))
            {
                throw new KeyNotFoundException($"Key {y} is not found");
            }

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

        /// <summary>
        /// Check whether the <paramref name="element"/> exists in the set
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="element"/> exists;
        /// <see langword="false"/> otherwises.
        /// </returns>
        public bool Contains(T element)
        {
            return _nodes.ContainsKey(element);
        }
    }
}
