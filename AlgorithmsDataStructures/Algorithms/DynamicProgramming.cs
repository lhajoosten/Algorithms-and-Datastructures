namespace AlgorithmsDataStructures.Algorithms.DynamicProgramming;

/// <summary>
/// Collection of dynamic programming algorithm implementations.
/// </summary>
public static class DynamicProgramming
{
    /// <summary>
    /// Calculates the nth Fibonacci number using dynamic programming (bottom-up approach).
    /// Time Complexity: O(n), Space Complexity: O(n).
    /// </summary>
    /// <param name="n">The position in the Fibonacci sequence.</param>
    /// <returns>The nth Fibonacci number.</returns>
    /// <exception cref="ArgumentException">Thrown when n is negative.</exception>
    public static long Fibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("n cannot be negative.", nameof(n));

        if (n <= 1)
            return n;

        var dp = new long[n + 1];
        dp[0] = 0;
        dp[1] = 1;

        for (int i = 2; i <= n; i++)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
        }

        return dp[n];
    }

    /// <summary>
    /// Calculates the nth Fibonacci number using space-optimized dynamic programming.
    /// Time Complexity: O(n), Space Complexity: O(1).
    /// </summary>
    /// <param name="n">The position in the Fibonacci sequence.</param>
    /// <returns>The nth Fibonacci number.</returns>
    /// <exception cref="ArgumentException">Thrown when n is negative.</exception>
    public static long FibonacciOptimized(int n)
    {
        if (n < 0)
            throw new ArgumentException("n cannot be negative.", nameof(n));

        if (n <= 1)
            return n;

        long prev2 = 0, prev1 = 1;

        for (int i = 2; i <= n; i++)
        {
            long current = prev1 + prev2;
            prev2 = prev1;
            prev1 = current;
        }

        return prev1;
    }

    /// <summary>
    /// Solves the 0/1 Knapsack problem using dynamic programming.
    /// Time Complexity: O(n * capacity), Space Complexity: O(n * capacity).
    /// </summary>
    /// <param name="weights">Array of item weights.</param>
    /// <param name="values">Array of item values.</param>
    /// <param name="capacity">The capacity of the knapsack.</param>
    /// <returns>The maximum value that can be obtained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when weights or values is null.</exception>
    /// <exception cref="ArgumentException">Thrown when arrays have different lengths or capacity is negative.</exception>
    public static int Knapsack(int[] weights, int[] values, int capacity)
    {
        if (weights == null)
            throw new ArgumentNullException(nameof(weights));
        if (values == null)
            throw new ArgumentNullException(nameof(values));
        if (weights.Length != values.Length)
            throw new ArgumentException("Weights and values arrays must have the same length.");
        if (capacity < 0)
            throw new ArgumentException("Capacity cannot be negative.", nameof(capacity));

        int n = weights.Length;
        var dp = new int[n + 1, capacity + 1];

        for (int i = 1; i <= n; i++)
        {
            for (int w = 1; w <= capacity; w++)
            {
                if (weights[i - 1] <= w)
                {
                    // Take the maximum of including or excluding the current item
                    dp[i, w] = Math.Max(
                        values[i - 1] + dp[i - 1, w - weights[i - 1]], // Include current item
                        dp[i - 1, w]); // Exclude current item
                }
                else
                {
                    dp[i, w] = dp[i - 1, w]; // Can't include current item
                }
            }
        }

        return dp[n, capacity];
    }

    /// <summary>
    /// Solves the 0/1 Knapsack problem with space optimization.
    /// Time Complexity: O(n * capacity), Space Complexity: O(capacity).
    /// </summary>
    /// <param name="weights">Array of item weights.</param>
    /// <param name="values">Array of item values.</param>
    /// <param name="capacity">The capacity of the knapsack.</param>
    /// <returns>The maximum value that can be obtained.</returns>
    public static int KnapsackOptimized(int[] weights, int[] values, int capacity)
    {
        if (weights == null)
            throw new ArgumentNullException(nameof(weights));
        if (values == null)
            throw new ArgumentNullException(nameof(values));
        if (weights.Length != values.Length)
            throw new ArgumentException("Weights and values arrays must have the same length.");
        if (capacity < 0)
            throw new ArgumentException("Capacity cannot be negative.", nameof(capacity));

        var dp = new int[capacity + 1];

        for (int i = 0; i < weights.Length; i++)
        {
            // Traverse backwards to avoid using updated values
            for (int w = capacity; w >= weights[i]; w--)
            {
                dp[w] = Math.Max(dp[w], dp[w - weights[i]] + values[i]);
            }
        }

        return dp[capacity];
    }

    /// <summary>
    /// Finds the length of the Longest Common Subsequence between two strings.
    /// Time Complexity: O(m * n), Space Complexity: O(m * n).
    /// </summary>
    /// <param name="text1">The first string.</param>
    /// <param name="text2">The second string.</param>
    /// <returns>The length of the longest common subsequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when text1 or text2 is null.</exception>
    public static int LongestCommonSubsequence(string text1, string text2)
    {
        if (text1 == null)
            throw new ArgumentNullException(nameof(text1));
        if (text2 == null)
            throw new ArgumentNullException(nameof(text2));

        int m = text1.Length;
        int n = text2.Length;
        var dp = new int[m + 1, n + 1];

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (text1[i - 1] == text2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                }
                else
                {
                    dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
        }

        return dp[m, n];
    }

    /// <summary>
    /// Finds the actual Longest Common Subsequence string between two strings.
    /// Time Complexity: O(m * n), Space Complexity: O(m * n).
    /// </summary>
    /// <param name="text1">The first string.</param>
    /// <param name="text2">The second string.</param>
    /// <returns>The longest common subsequence string.</returns>
    public static string LongestCommonSubsequenceString(string text1, string text2)
    {
        if (text1 == null)
            throw new ArgumentNullException(nameof(text1));
        if (text2 == null)
            throw new ArgumentNullException(nameof(text2));

        int m = text1.Length;
        int n = text2.Length;
        var dp = new int[m + 1, n + 1];

        // Fill the DP table
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (text1[i - 1] == text2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                }
                else
                {
                    dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
        }

        // Reconstruct the LCS
        var result = new System.Text.StringBuilder();
        int x = m, y = n;

        while (x > 0 && y > 0)
        {
            if (text1[x - 1] == text2[y - 1])
            {
                result.Insert(0, text1[x - 1]);
                x--;
                y--;
            }
            else if (dp[x - 1, y] > dp[x, y - 1])
            {
                x--;
            }
            else
            {
                y--;
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Calculates the minimum edit distance (Levenshtein distance) between two strings.
    /// Time Complexity: O(m * n), Space Complexity: O(m * n).
    /// </summary>
    /// <param name="word1">The first string.</param>
    /// <param name="word2">The second string.</param>
    /// <returns>The minimum number of operations to convert word1 to word2.</returns>
    /// <exception cref="ArgumentNullException">Thrown when word1 or word2 is null.</exception>
    public static int EditDistance(string word1, string word2)
    {
        if (word1 == null)
            throw new ArgumentNullException(nameof(word1));
        if (word2 == null)
            throw new ArgumentNullException(nameof(word2));

        int m = word1.Length;
        int n = word2.Length;
        var dp = new int[m + 1, n + 1];

        // Initialize base cases
        for (int i = 0; i <= m; i++)
            dp[i, 0] = i; // Delete all characters from word1

        for (int j = 0; j <= n; j++)
            dp[0, j] = j; // Insert all characters to get word2

        // Fill the DP table
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (word1[i - 1] == word2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1]; // No operation needed
                }
                else
                {
                    dp[i, j] = 1 + Math.Min(
                        Math.Min(dp[i - 1, j],     // Delete
                                dp[i, j - 1]),     // Insert
                        dp[i - 1, j - 1]);        // Replace
                }
            }
        }

        return dp[m, n];
    }

    /// <summary>
    /// Solves the coin change problem - finds the minimum number of coins needed to make the amount.
    /// Time Complexity: O(amount * coins.Length), Space Complexity: O(amount).
    /// </summary>
    /// <param name="coins">Array of coin denominations.</param>
    /// <param name="amount">The target amount.</param>
    /// <returns>The minimum number of coins needed, or -1 if impossible.</returns>
    /// <exception cref="ArgumentNullException">Thrown when coins is null.</exception>
    /// <exception cref="ArgumentException">Thrown when amount is negative.</exception>
    public static int CoinChange(int[] coins, int amount)
    {
        if (coins == null)
            throw new ArgumentNullException(nameof(coins));
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (amount == 0)
            return 0;

        var dp = new int[amount + 1];
        Array.Fill(dp, amount + 1); // Initialize with impossible value
        dp[0] = 0;

        for (int i = 1; i <= amount; i++)
        {
            foreach (int coin in coins)
            {
                if (coin <= i)
                {
                    dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
                }
            }
        }

        return dp[amount] > amount ? -1 : dp[amount];
    }

    /// <summary>
    /// Finds the length of the Longest Increasing Subsequence.
    /// Time Complexity: O(n²), Space Complexity: O(n).
    /// </summary>
    /// <param name="nums">The input array.</param>
    /// <returns>The length of the longest increasing subsequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when nums is null.</exception>
    public static int LongestIncreasingSubsequence(int[] nums)
    {
        if (nums == null)
            throw new ArgumentNullException(nameof(nums));

        if (nums.Length == 0)
            return 0;

        var dp = new int[nums.Length];
        Array.Fill(dp, 1); // Each element forms a subsequence of length 1

        for (int i = 1; i < nums.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (nums[j] < nums[i])
                {
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
                }
            }
        }

        return dp.Max();
    }

    /// <summary>
    /// Finds the length of the Longest Increasing Subsequence using binary search optimization.
    /// Time Complexity: O(n log n), Space Complexity: O(n).
    /// </summary>
    /// <param name="nums">The input array.</param>
    /// <returns>The length of the longest increasing subsequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when nums is null.</exception>
    public static int LongestIncreasingSubsequenceOptimized(int[] nums)
    {
        if (nums == null)
            throw new ArgumentNullException(nameof(nums));

        if (nums.Length == 0)
            return 0;

        var tails = new List<int>();

        foreach (int num in nums)
        {
            int left = 0, right = tails.Count;

            // Binary search for the position to insert/replace
            while (left < right)
            {
                int mid = left + (right - left) / 2;
                if (tails[mid] < num)
                    left = mid + 1;
                else
                    right = mid;
            }

            if (left == tails.Count)
                tails.Add(num);
            else
                tails[left] = num;
        }

        return tails.Count;
    }

    /// <summary>
    /// Solves the maximum subarray problem using dynamic programming (Kadane's algorithm).
    /// Time Complexity: O(n), Space Complexity: O(1).
    /// </summary>
    /// <param name="nums">The input array.</param>
    /// <returns>The maximum sum of any contiguous subarray.</returns>
    /// <exception cref="ArgumentNullException">Thrown when nums is null.</exception>
    /// <exception cref="ArgumentException">Thrown when nums is empty.</exception>
    public static int MaxSubarraySum(int[] nums)
    {
        if (nums == null)
            throw new ArgumentNullException(nameof(nums));
        if (nums.Length == 0)
            throw new ArgumentException("Array cannot be empty.", nameof(nums));

        int maxSoFar = nums[0];
        int maxEndingHere = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            maxEndingHere = Math.Max(nums[i], maxEndingHere + nums[i]);
            maxSoFar = Math.Max(maxSoFar, maxEndingHere);
        }

        return maxSoFar;
    }

    /// <summary>
    /// Counts the number of ways to climb stairs where you can take 1 or 2 steps at a time.
    /// Time Complexity: O(n), Space Complexity: O(1).
    /// </summary>
    /// <param name="n">The number of stairs.</param>
    /// <returns>The number of distinct ways to climb the stairs.</returns>
    /// <exception cref="ArgumentException">Thrown when n is negative.</exception>
    public static int ClimbStairs(int n)
    {
        if (n < 0)
            throw new ArgumentException("n cannot be negative.", nameof(n));

        if (n <= 2)
            return n;

        int prev2 = 1, prev1 = 2;

        for (int i = 3; i <= n; i++)
        {
            int current = prev1 + prev2;
            prev2 = prev1;
            prev1 = current;
        }

        return prev1;
    }
}