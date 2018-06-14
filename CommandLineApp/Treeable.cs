using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CommandLineApp
{
    /// <summary>
    /// Contains shared method related with hierarchical information (i.e., tree-structured).
    /// </summary>
    public static class Treeable
    {
        /// <summary>
        /// Iterates a tree in pre order.
        /// </summary>
        /// <param name="source">The tree to iterate over.</param>
        /// <param name="children">Supports the access to the children of a node.</param>
        /// <typeparam name="T">The concrete type of the nodes.</typeparam>
        /// <returns>The nodes of the tree enumerated in pre order.</returns>
        public static IEnumerable<T> PreOrder<T>(this T source, Func<T, IEnumerable<T>> children)
        {
            Debug.Assert(children != null);
            yield return source;
            foreach (var descendant in children(source).SelectMany(child => child.PreOrder(children)))
            {
                yield return descendant;
            }
        }

        /// <summary>
        /// Iterates a tree in post order.
        /// </summary>
        /// <param name="source">The tree to iterate over.</param>
        /// <param name="children">Supports the access to the children of a node.</param>
        /// <typeparam name="T">The concrete type of the nodes.</typeparam>
        /// <returns>The nodes of the tree enumerated in post order.</returns>
        public static IEnumerable<T> PostOrder<T>(this T source, Func<T, IEnumerable<T>> children)
        {
            Debug.Assert(children != null);
            foreach (var childDescendant in children(source).SelectMany(child => child.PostOrder(children)))
            {
                yield return childDescendant;
            }
            yield return source;
        }

        /// <summary>
        /// Iterates a tree in breadth first.
        /// </summary>
        /// <param name="source">The tree to iterate over.</param>
        /// <param name="children">Supports the access to the children of a node.</param>
        /// <typeparam name="T">The concrete type of the nodes.</typeparam>
        /// <returns>The nodes of the tree enumerated in breadth-first order.</returns>
        public static IEnumerable<T> BreadthFirstOrder<T>(this T source, Func<T, IEnumerable<T>> children)
        {
            Debug.Assert(children != null);
            var queue = new Queue<T>();
            queue.Enqueue(source);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var child in children(current))
                {
                    queue.Enqueue(child);
                }

                yield return current;
            }
        }

        /// <summary>
        /// Applies an action over each element.
        /// </summary>
        /// <typeparam name="T">type of the elements</typeparam>
        /// <param name="source">collection to iterate over.</param>
        /// <param name="action">the action to apply for each element.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Debug.Assert(action != null);
            foreach (var item in source) action(item);
        }

        /// <summary>
        /// Returns the ancestors of the current subtree.
        /// </summary>
        /// <param name="source">element for which returning the ancestors.</param>
        /// <param name="parent">the how to access the parent for each element.</param>
        /// <returns>the ancestors from the current node until the root.</returns>
        public static IEnumerable<T> Ancestors<T>(this T source, Func<T, T> parent)
        {
            Debug.Assert(parent != null);
            var current = source;
            while (parent(current) != null)
            {
                yield return parent(current);
                current = parent(current);
            }
        }
    }
}
