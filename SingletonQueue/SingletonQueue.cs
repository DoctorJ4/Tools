using System;
using System.Collections.Concurrent;

namespace SingletonQueue
{
    public sealed class SingletonQueue<T>
    {
        // Private static instance of the singleton
        private static readonly Lazy<SingletonQueue<T>> _instance = new Lazy<SingletonQueue<T>>(() => new SingletonQueue<T>());

        // Internal queue (thread-safe)
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        // Private constructor to prevent instantiation
        private SingletonQueue() { }

        // Public accessor for the singleton instance
        public static SingletonQueue<T> Instance => _instance.Value;

        /// <summary>
        /// Enqueues an item into the queue.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            _queue.Enqueue(item);
        }

        /// <summary>
        /// Attempts to dequeue an item from the queue.
        /// </summary>
        /// <param name="item">The dequeued item, or default value if the queue is empty.</param>
        /// <returns>True if an item was dequeued; otherwise, false.</returns>
        public bool TryDequeue(out T item)
        {
            return _queue.TryDequeue(out item);
        }

        /// <summary>
        /// Checks whether the queue is empty.
        /// </summary>
        /// <returns>True if the queue is empty; otherwise, false.</returns>
        public bool IsEmpty()
        {
            return _queue.IsEmpty;
        }

        /// <summary>
        /// Gets the current count of items in the queue.
        /// </summary>
        /// <returns>The number of items in the queue.</returns>
        public int Count()
        {
            return _queue.Count;
        }
    }
}