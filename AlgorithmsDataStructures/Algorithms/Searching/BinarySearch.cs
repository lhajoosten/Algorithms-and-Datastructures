namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class BinarySearchAlgorithm
{
    /// <summary>
    /// Performs a binary search on a sorted array to find the specified value.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, the bitwise complement of the index where it should be inserted.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int BinarySearch<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        return BinarySearchInternal(array, 0, array.Length - 1, target, comparer);
    }

    /// <summary>
    /// Performs a binary search on a sorted array within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="startIndex">The starting index of the range to search.</param>
    /// <param name="count">The number of elements in the range to search.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, the bitwise complement of the index where it should be inserted.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when startIndex or count is invalid.</exception>
    public static int BinarySearch<T>(T[] array, int startIndex, int count, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);
        
        if (startIndex < 0 || startIndex >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex));

        if (count < 0 || startIndex + count > array.Length)
            throw new ArgumentOutOfRangeException(nameof(count));

        comparer ??= Comparer<T>.Default;
        return BinarySearchInternal(array, startIndex, startIndex + count - 1, target, comparer);
    }

    internal static int BinarySearchInternal<T>(T[] array, int left, int right, T target, IComparer<T> comparer)
    {
        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            int comparison = comparer.Compare(array[middle], target);

            if (comparison == 0)
                return middle;
            else if (comparison < 0)
                left = middle + 1;
            else
                right = middle - 1;
        }

        return ~left;
    }
}
