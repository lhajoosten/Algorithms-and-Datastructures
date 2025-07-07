namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class LinearSearchAlgorithm
{
    /// <summary>
    /// Performs a linear search on an array to find the specified value.
    /// Time Complexity: O(n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine equality. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int LinearSearch<T>(T[] array, T target, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= EqualityComparer<T>.Default;

        for (int i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], target))
                return i;
        }

        return -1;
    }
}
