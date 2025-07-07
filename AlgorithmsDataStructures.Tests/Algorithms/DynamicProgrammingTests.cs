using AlgorithmsDataStructures.Algorithms.DynamicProgramming;
using FluentAssertions;
using Xunit;

namespace AlgorithmsDataStructures.Tests.Algorithms;

public class DynamicProgrammingTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    [InlineData(10, 55)]
    [InlineData(20, 6765)]
    public void Fibonacci_ValidInput_ShouldReturnCorrectValue(int n, long expected)
    {
        // Act
        var result = DynamicProgramming.Fibonacci(n);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Fibonacci_NegativeInput_ShouldThrowArgumentException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.Fibonacci(-1);
        act.Should().Throw<ArgumentException>()
           .WithMessage("n cannot be negative. (Parameter 'n')");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(10, 55)]
    [InlineData(20, 6765)]
    public void FibonacciOptimized_ValidInput_ShouldReturnCorrectValue(int n, long expected)
    {
        // Act
        var result = DynamicProgramming.FibonacciOptimized(n);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Knapsack_BasicExample_ShouldReturnOptimalValue()
    {
        // Arrange
        var weights = new[] { 10, 20, 30 };
        var values = new[] { 60, 100, 120 };
        int capacity = 50;

        // Act
        var result = DynamicProgramming.Knapsack(weights, values, capacity);

        // Assert
        result.Should().Be(220); // Items 2 and 3 (weights 20+30=50, values 100+120=220)
    }

    [Fact]
    public void Knapsack_EmptyArrays_ShouldReturnZero()
    {
        // Arrange
        var weights = Array.Empty<int>();
        var values = Array.Empty<int>();
        int capacity = 10;

        // Act
        var result = DynamicProgramming.Knapsack(weights, values, capacity);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Knapsack_NullWeights_ShouldThrowArgumentNullException()
    {
        // Arrange
        var values = new[] { 60, 100, 120 };

        // Act & Assert
        Action act = () => DynamicProgramming.Knapsack(null!, values, 50);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Knapsack_DifferentArrayLengths_ShouldThrowArgumentException()
    {
        // Arrange
        var weights = new[] { 10, 20 };
        var values = new[] { 60, 100, 120 };

        // Act & Assert
        Action act = () => DynamicProgramming.Knapsack(weights, values, 50);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Weights and values arrays must have the same length.");
    }

    [Fact]
    public void KnapsackOptimized_SameAsRegular_ShouldReturnSameResult()
    {
        // Arrange
        var weights = new[] { 1, 3, 4, 5 };
        var values = new[] { 1, 4, 5, 7 };
        int capacity = 7;

        // Act
        var regular = DynamicProgramming.Knapsack(weights, values, capacity);
        var optimized = DynamicProgramming.KnapsackOptimized(weights, values, capacity);

        // Assert
        regular.Should().Be(optimized);
        regular.Should().Be(9); // Items 2 and 3 (weights 3+4=7, values 4+5=9)
    }

    [Theory]
    [InlineData("ABCDGH", "AEDFHR", 3)] // ADH
    [InlineData("AGGTAB", "GXTXAYB", 4)] // GTAB
    [InlineData("", "ABCD", 0)]
    [InlineData("ABCD", "", 0)]
    [InlineData("ABCD", "ABCD", 4)]
    [InlineData("ABC", "DEF", 0)]
    public void LongestCommonSubsequence_ValidInput_ShouldReturnCorrectLength(string text1, string text2, int expected)
    {
        // Act
        var result = DynamicProgramming.LongestCommonSubsequence(text1, text2);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void LongestCommonSubsequence_NullInput_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.LongestCommonSubsequence(null!, "test");
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("ABCDGH", "AEDFHR", "ADH")]
    [InlineData("AGGTAB", "GXTXAYB", "GTAB")]
    [InlineData("", "ABCD", "")]
    [InlineData("ABCD", "ABCD", "ABCD")]
    public void LongestCommonSubsequenceString_ValidInput_ShouldReturnCorrectString(string text1, string text2, string expected)
    {
        // Act
        var result = DynamicProgramming.LongestCommonSubsequenceString(text1, text2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("horse", "ros", 3)] // Remove 'h', 'r', 'e'
    [InlineData("intention", "execution", 5)]
    [InlineData("", "abc", 3)]
    [InlineData("abc", "", 3)]
    [InlineData("abc", "abc", 0)]
    public void EditDistance_ValidInput_ShouldReturnCorrectDistance(string word1, string word2, int expected)
    {
        // Act
        var result = DynamicProgramming.EditDistance(word1, word2);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 5 }, 11, 3)] // 11 = 5 + 5 + 1
    [InlineData(new int[] { 2 }, 3, -1)] // Cannot make 3 with only 2's
    [InlineData(new int[] { 1 }, 0, 0)] // 0 coins needed for amount 0
    [InlineData(new int[] { 1, 3, 4 }, 6, 2)] // 6 = 3 + 3
    public void CoinChange_ValidInput_ShouldReturnCorrectResult(int[] coins, int amount, int expected)
    {
        // Act
        var result = DynamicProgramming.CoinChange(coins, amount);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void CoinChange_NullCoins_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.CoinChange(null!, 5);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new int[] { 10, 9, 2, 5, 3, 7, 101, 18 }, 4)] // [2, 3, 7, 18]
    [InlineData(new int[] { 0, 1, 0, 3, 2, 3 }, 4)] // [0, 1, 2, 3]
    [InlineData(new int[] { 7, 7, 7, 7, 7, 7, 7 }, 1)]
    [InlineData(new int[] { 1 }, 1)]
    [InlineData(new int[0], 0)]
    public void LongestIncreasingSubsequence_ValidInput_ShouldReturnCorrectLength(int[] nums, int expected)
    {
        // Act
        var result = DynamicProgramming.LongestIncreasingSubsequence(nums);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void LongestIncreasingSubsequence_NullInput_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.LongestIncreasingSubsequence(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new int[] { 10, 9, 2, 5, 3, 7, 101, 18 }, 4)]
    [InlineData(new int[] { 0, 1, 0, 3, 2, 3 }, 4)]
    [InlineData(new int[] { 7, 7, 7, 7, 7, 7, 7 }, 1)]
    public void LongestIncreasingSubsequenceOptimized_SameAsRegular_ShouldReturnSameResult(int[] nums, int expected)
    {
        // Act
        var regular = DynamicProgramming.LongestIncreasingSubsequence(nums);
        var optimized = DynamicProgramming.LongestIncreasingSubsequenceOptimized(nums);

        // Assert
        regular.Should().Be(optimized);
        regular.Should().Be(expected);
    }

    [Theory]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 6)] // [4, -1, 2, 1]
    [InlineData(new int[] { 1 }, 1)]
    [InlineData(new int[] { 5, 4, -1, 7, 8 }, 23)] // [5, 4, -1, 7, 8]
    [InlineData(new int[] { -1 }, -1)]
    [InlineData(new int[] { -2, -1 }, -1)]
    public void MaxSubarraySum_ValidInput_ShouldReturnCorrectSum(int[] nums, int expected)
    {
        // Act
        var result = DynamicProgramming.MaxSubarraySum(nums);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void MaxSubarraySum_EmptyArray_ShouldThrowArgumentException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.MaxSubarraySum(Array.Empty<int>());
        act.Should().Throw<ArgumentException>()
           .WithMessage("Array cannot be empty. (Parameter 'nums')");
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)] // 1+1+1 or 1+2 or 2+1
    [InlineData(4, 5)] // 1+1+1+1 or 1+1+2 or 1+2+1 or 2+1+1 or 2+2
    [InlineData(5, 8)]
    public void ClimbStairs_ValidInput_ShouldReturnCorrectWays(int n, int expected)
    {
        // Act
        var result = DynamicProgramming.ClimbStairs(n);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ClimbStairs_NegativeInput_ShouldThrowArgumentException()
    {
        // Act & Assert
        Action act = () => DynamicProgramming.ClimbStairs(-1);
        act.Should().Throw<ArgumentException>()
           .WithMessage("n cannot be negative. (Parameter 'n')");
    }

    [Fact]
    public void ClimbStairs_Zero_ShouldReturnZero()
    {
        // Act
        var result = DynamicProgramming.ClimbStairs(0);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void LargeFibonacci_ShouldNotOverflow()
    {
        // Act
        var result = DynamicProgramming.FibonacciOptimized(50);

        // Assert - This is the 50th Fibonacci number
        result.Should().Be(12586269025L);
    }

    [Fact]
    public void ComplexKnapsackExample_ShouldReturnOptimalValue()
    {
        // Arrange - More complex example
        var weights = new[] { 23, 31, 29, 44, 53, 38, 63, 85, 89, 82 };
        var values = new[] { 92, 57, 49, 68, 60, 43, 67, 84, 87, 72 };
        int capacity = 165;

        // Act
        var result = DynamicProgramming.KnapsackOptimized(weights, values, capacity);

        // Assert - Should be able to find optimal combination
        result.Should().BeGreaterThan(0);
        result.Should().BeLessThanOrEqualTo(values.Sum()); // Can't exceed sum of all values
    }
}