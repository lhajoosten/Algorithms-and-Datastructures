namespace AlgorithmsDataStructures.Algorithms.Sorting
{
    /// <summary>
    /// Collection of various sorting algorithms implementations.
    /// </summary>
    public static class SelectionSorting
    {
        /// <summary>
        /// Sorts an array using the selection sort algorithm.
        /// Time Complexity: O(n²) in all cases.
        /// Space Complexity: O(1).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void SelectionSort<T>(T[] array, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(array);

            comparer ??= Comparer<T>.Default;
            int n = array.Length;

            // One by one move boundary of unsorted subarray
            for (int i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (comparer.Compare(array[j], array[minIndex]) < 0)
                        minIndex = j;
                }

                // Swap the found minimum element with the first element
                if (minIndex != i)
                    Swap(array, minIndex, i);
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
