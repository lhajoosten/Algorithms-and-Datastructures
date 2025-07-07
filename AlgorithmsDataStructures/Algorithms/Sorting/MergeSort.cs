namespace AlgorithmsDataStructures.Algorithms.Sorting
{
    /// <summary>
    /// Collection of various sorting algorithms implementations.
    /// </summary>
    public static class MergeSorting
    {
        /// <summary>
        /// Sorts an array using the merge sort algorithm.
        /// Time Complexity: O(n log n) in all cases.
        /// Space Complexity: O(n).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void MergeSort<T>(T[] array, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(array);

            comparer ??= Comparer<T>.Default;

            if (array.Length <= 1)
                return;

            MergeSortHelper(array, 0, array.Length - 1, comparer);
        }

        private static void MergeSortHelper<T>(T[] array, int left, int right, IComparer<T> comparer)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                // Sort first and second halves
                MergeSortHelper(array, left, middle, comparer);
                MergeSortHelper(array, middle + 1, right, comparer);

                // Merge the sorted halves
                Merge(array, left, middle, right, comparer);
            }
        }

        private static void Merge<T>(T[] array, int left, int middle, int right, IComparer<T> comparer)
        {
            // Calculate sizes of two subarrays to be merged
            int leftSize = middle - left + 1;
            int rightSize = right - middle;

            // Create temporary arrays
            T[] leftArray = new T[leftSize];
            T[] rightArray = new T[rightSize];

            // Copy data to temporary arrays
            Array.Copy(array, left, leftArray, 0, leftSize);
            Array.Copy(array, middle + 1, rightArray, 0, rightSize);

            // Merge the temporary arrays back into array[left..right]
            int i = 0, j = 0, k = left;

            while (i < leftSize && j < rightSize)
            {
                if (comparer.Compare(leftArray[i], rightArray[j]) <= 0)
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            // Copy the remaining elements of leftArray[], if any
            while (i < leftSize)
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }

            // Copy the remaining elements of rightArray[], if any
            while (j < rightSize)
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }
    }
}
