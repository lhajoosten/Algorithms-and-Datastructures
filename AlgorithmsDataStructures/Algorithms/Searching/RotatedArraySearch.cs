namespace AlgorithmsDataStructures.Algorithms.Searching;

/// <summary>
/// Collection of various searching algorithms implementations.
/// </summary>
public static class RotatedArraySearchAlgorithm
{
    /// <summary>
    /// Searches for a value in a rotated sorted array.
    /// Time Complexity: O(log n).
    /// Space Complexity: O(1).
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The rotated sorted array to search in.</param>
    /// <param name="target">The value to search for.</param>
    /// <param name="comparer">The comparer to determine the order of elements. If null, uses default comparer.</param>
    /// <returns>The index of the target if found; otherwise, -1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    public static int SearchInRotatedArray<T>(T[] array, T target, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(array);

        comparer ??= Comparer<T>.Default;
        int left = 0, right = array.Length - 1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;

            if (comparer.Compare(array[middle], target) == 0)
                return middle;

            // Check if left half is sorted
            if (comparer.Compare(array[left], array[middle]) <= 0)
            {
                // Target is in left sorted half
                if (comparer.Compare(array[left], target) <= 0 && comparer.Compare(target, array[middle]) < 0)
                    right = middle - 1;
                else
                    left = middle + 1;
            }
            else // Right half is sorted
            {
                // Target is in right sorted half
                if (comparer.Compare(array[middle], target) < 0 && comparer.Compare(target, array[right]) <= 0)
                    left = middle + 1;
                else
                    right = middle - 1;
            }
        }

        return -1;
    }
}
