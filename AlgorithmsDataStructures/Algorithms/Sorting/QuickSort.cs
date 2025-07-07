namespace AlgorithmsDataStructures.Algorithms.Sorting
{
    public static class QuickSorting
    {
        /// <summary>
        /// Sorts an array using the quick sort algorithm.
        /// Time Complexity: O(n log n) average case, O(n²) worst case.
        /// Space Complexity: O(log n) average case due to recursion.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void QuickSort<T>(T[] array, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(array);

            comparer ??= Comparer<T>.Default;

            if (array.Length <= 1)
                return;

            QuickSortHelper(array, 0, array.Length - 1, comparer);
        }

        private static void QuickSortHelper<T>(T[] array, int low, int high, IComparer<T> comparer)
        {
            if (low < high)
            {
                // Partition the array and get the pivot index
                int pivotIndex = Partition(array, low, high, comparer);

                // Recursively sort elements before and after partition
                QuickSortHelper(array, low, pivotIndex - 1, comparer);
                QuickSortHelper(array, pivotIndex + 1, high, comparer);
            }
        }

        private static int Partition<T>(T[] array, int low, int high, IComparer<T> comparer)
        {
            // Choose the rightmost element as pivot
            T pivot = array[high];

            // Index of smaller element indicates the right position of pivot found so far
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // If current element is smaller than or equal to pivot
                if (comparer.Compare(array[j], pivot) <= 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            // Swap the pivot element with the element at i+1
            Swap(array, i + 1, high);
            return i + 1;
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