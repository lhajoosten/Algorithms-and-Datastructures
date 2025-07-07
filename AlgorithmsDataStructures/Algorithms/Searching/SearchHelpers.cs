namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class SearchHelpers
{
    /// <summary>
    /// Finds the first occurrence of the target value in a sorted array.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the first occurrence of target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int FindFirst<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        int left = 0, right = array.Length - 1;
        int result = -1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            int comparison = comparer.Compare(array[middle], target);

            if (comparison == 0)
            {
                result = middle;
                right = middle - 1; // Continue searching in the left half
            }
            else if (comparison < 0)
                left = middle + 1;
            else
                right = middle - 1;
        }

        return result;
    }

    /// <summary>
    /// Finds the last occurrence of the target value in a sorted array.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the last occurrence of target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int FindLast<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        int left = 0, right = array.Length - 1;
        int result = -1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            int comparison = comparer.Compare(array[middle], target);

            if (comparison == 0)
            {
                result = middle;
                left = middle + 1; // Continue searching in the right half
            }
            else if (comparison < 0)
                left = middle + 1;
            else
                right = middle - 1;
        }

        return result;
    }

    /// <summary>
    /// Finds the insertion point for a target value in a sorted array.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The sorted array to search in.</param>
    /// <param name="target">The value to find insertion point for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index where the target should be inserted to maintain sorted order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int FindInsertionPoint<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        int left = 0, right = array.Length - 1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            int comparison = comparer.Compare(array[middle], target);

            if (comparison < 0)
                left = middle + 1;
            else
                right = middle - 1;
        }

        return left;
    }
}
