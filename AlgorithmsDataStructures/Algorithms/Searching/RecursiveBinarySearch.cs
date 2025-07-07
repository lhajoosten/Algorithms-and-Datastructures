namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class RecursiveBinarySearchAlgorithm
{
    /// <summary>
    /// Performs a binary search using recursion on a sorted array.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(log n) due to recursion.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int BinarySearchRecursive<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        return BinarySearchRecursiveHelper(array, 0, array.Length - 1, target, comparer);
    }

    private static int BinarySearchRecursiveHelper<T>(T[] array, int left, int right, T target, IComparer<T> comparer)
    {
        if (left > right)
            return -1;

        int middle = left + (right - left) / 2;
        int comparison = comparer.Compare(array[middle], target);

        if (comparison == 0)
            return middle;
        else if (comparison < 0)
            return BinarySearchRecursiveHelper(array, middle + 1, right, target, comparer);
        else
            return BinarySearchRecursiveHelper(array, left, middle - 1, target, comparer);
    }
}
