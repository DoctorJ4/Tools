namespace SingletonQueue.Tests
{
    [TestFixture]
    public class SingletonQueueTests
    {
        [SetUp]
        public void SetUp()
        {
            // Clear the queue before each test (no built-in method in the original class; this simulates a fresh start).
            while (SingletonQueue<int>.Instance.TryDequeue(out _)) { }
        }

        /// <summary>
        /// Verifies that enqueuing an item increases the queue count.
        /// </summary>
        [Test]
        public void Enqueue_ShouldAddItemToQueue()
        {
            var queue = SingletonQueue<int>.Instance;

            queue.Enqueue(10);

            Assert.That(queue.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Checks if dequeuing works correctly by returning the correct item and removing it from the queue.
        /// </summary>
        [Test]
        public void TryDequeue_ShouldRemoveAndReturnItem()
        {
            var queue = SingletonQueue<int>.Instance;

            queue.Enqueue(20);

            bool result = queue.TryDequeue(out int dequeuedItem);

            Assert.Multiple(() =>
            {
                Assert.That(result);
                Assert.That(dequeuedItem, Is.EqualTo(20));
                Assert.That(queue.IsEmpty());
            });
        }

        /// <summary>
        /// Ensures dequeuing from an empty queue does not throw an exception and returns false.
        /// </summary>
        [Test]
        public void TryDequeue_OnEmptyQueue_ShouldReturnFalse()
        {
            var queue = SingletonQueue<int>.Instance;

            bool result = queue.TryDequeue(out int dequeuedItem);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(dequeuedItem, Is.EqualTo(0)); // Default value of int
            });
        }

        /// <summary>
        /// Confirms that IsEmpty correctly identifies an empty queue.
        /// </summary>
        [Test]
        public void IsEmpty_ShouldReturnTrueForEmptyQueue()
        {
            var queue = SingletonQueue<int>.Instance;

            Assert.That(queue.IsEmpty(), Is.True);
        }

        /// <summary>
        /// Verifies that IsEmpty returns false when the queue contains items.
        /// </summary>
        [Test]
        public void IsEmpty_ShouldReturnFalseWhenQueueIsNotEmpty()
        {
            var queue = SingletonQueue<int>.Instance;

            queue.Enqueue(30);

            Assert.That(queue.IsEmpty(), Is.False);
        }

        /// <summary>
        /// Checks if Count correctly reports the number of items in the queue.
        /// </summary>
        [Test]
        public void Count_ShouldReturnCorrectNumberOfItems()
        {
            var queue = SingletonQueue<int>.Instance;

            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            Assert.That(queue.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Ensures the singleton nature of the class by verifying that two references point to the same instance.
        /// </summary>
        [Test]
        public void SingletonBehavior_ShouldEnsureOnlyOneInstance()
        {
            var queue1 = SingletonQueue<int>.Instance;
            var queue2 = SingletonQueue<int>.Instance;

            queue1.Enqueue(50);

            bool result = queue2.TryDequeue(out int dequeuedItem);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(dequeuedItem, Is.EqualTo(50));
                Assert.That(queue2, Is.SameAs(queue1)); // Both references should point to the same instance
            });
        }

        /// <summary>
        /// Tests thread safety by performing concurrent enqueue operations and verifying the final count.
        /// </summary>
        [Test]
        public void ThreadSafety_ShouldHandleConcurrentAccess()
        {
            var queue = SingletonQueue<int>.Instance;

            Parallel.For(0, 1000, i =>
            {
                queue.Enqueue(i);
            });

            Assert.That(queue.Count(), Is.EqualTo(1000));
        }
    }
}