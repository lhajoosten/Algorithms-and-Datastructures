namespace AlgorithmsDataStructures.Algorithms.Sorting
{
    /// <summary>
    /// Collection of various sorting algorithms implementations.
    /// </summary>
    public static class HeapSorting
    {
        /// <summary>
        /// Sorts an array using the heap sort algorithm.
        /// Time Complexity: O(n log n) in all cases.
        /// Space Complexity: O(1).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void HeapSort<T>(T[] array, IComparer<T>? comparer = null)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            comparer ??= Comparer<T>.Default;
            int n = array.Length;

            // Build heap (rearrange array)
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i, comparer);

            // One by one extract an element from heap
            for (int i = n - 1; i > 0; i--)
            {
                // Move current root to end
                Swap(array, 0, i);

                // Call max heapify on the reduced heap
                Heapify(array, i, 0, comparer);
            }
        }

        private static void Heapify<T>(T[] array, int n, int i, IComparer<T> comparer)
        {
            int largest = i; // Initialize largest as root
            int left = 2 * i + 1; // left child
            int right = 2 * i + 2; // right child

            // If left child is larger than root
            if (left < n && comparer.Compare(array[left], array[largest]) > 0)
                largest = left;

            // If right child is larger than largest so far
            if (right < n && comparer.Compare(array[right], array[largest]) > 0)
                largest = right;

            // If largest is not root
            if (largest != i)
            {
                Swap(array, i, largest);

                // Recursively heapify the affected sub-tree
                Heapify(array, n, largest, comparer);
            }
        }

        /// <summary>
        /// Swaps two elements in an array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array containing the elements to swap.</param>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        private static void Swap<T>(T[] array, int i, int j)
        {
            if (i != j)
            {
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
