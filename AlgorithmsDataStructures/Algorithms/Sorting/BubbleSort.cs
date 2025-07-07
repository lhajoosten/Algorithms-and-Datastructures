namespace AlgorithmsDataStructures.Algorithms.Sorting;

/// <summary>
/// Collection of various sorting algorithms implementations.
/// </summary>
public static class BubbleSorting
{
    /// <summary>
    /// Sorts an array using the bubble sort algorithm.
    /// Time Complexity: O(n²) worst and average case, O(n) best case.
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static void BubbleSort<T>(T[] array, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        int n = array.Length;

        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;

            // Last i elements are already in place
            for (int j = 0; j < n - i - 1; j++)
            {
                if (comparer.Compare(array[j], array[j + 1]) > 0)
                {
                    Swap(array, j, j + 1);
                    swapped = true;
                }
            }

            // If no swapping occurred, array is sorted
            if (!swapped)
                break;
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