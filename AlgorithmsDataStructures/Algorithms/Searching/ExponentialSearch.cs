namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class ExponentialSearchAlgorithm
{
    /// <summary>
    /// Performs an exponential search (also called doubling search or galloping search).
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int ExponentialSearch<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;

        if (array.Length == 0)
            return -1;

        // If target is at first position
        if (comparer.Compare(array[0], target) == 0)
            return 0;

        // Find range for binary search by doubling
        int bound = 1;
        while (bound < array.Length && comparer.Compare(array[bound], target) < 0)
            bound *= 2;

        // Perform binary search in the found range
        int left = bound / 2;
        int right = Math.Min(bound, array.Length - 1);

        return BinarySearchAlgorithm.BinarySearchInternal(array, left, right, target, comparer);
    }
}