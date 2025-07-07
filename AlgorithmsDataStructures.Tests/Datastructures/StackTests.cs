using FluentAssertions;
using Xunit;
using CustomStack = AlgorithmsDataStructures.DataStructures.Stack<int>;
using CustomStackString = AlgorithmsDataStructures.DataStructures.Stack<string>;

namespace AlgorithmsDataStructures.Tests.DataStructures;

public class StackTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyStack()
    {
        // Arrange & Act
        var stack = new CustomStack();

        // Assert
        stack.Count.Should().Be(0);
        stack.IsEmpty.Should().BeTrue();
        stack.Capacity.Should().Be(4); // Default capacity
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateStackWithSpecifiedCapacity()
    {
        // Arrange & Act
        var stack = new CustomStack(10);

        // Assert
        stack.Count.Should().Be(0);
        stack.IsEmpty.Should().BeTrue();
        stack.Capacity.Should().Be(10);
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Action act = () => new CustomStack(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithCollection_ShouldCreateStackWithCollectionElements()
    {
        // Arrange
        var collection = new[] { 1, 2, 3, 4, 5 };

        // Act
        var stack = new CustomStack(collection);

        // Assert
        stack.Count.Should().Be(5);
        stack.IsEmpty.Should().BeFalse();
        stack.Peek().Should().Be(5); // Last element should be on top
    }

    [Fact]
    public void Push_SingleElement_ShouldIncreaseCount()
    {
        // Arrange
        var stack = new CustomStack();

        // Act
        stack.Push(42);

        // Assert
        stack.Count.Should().Be(1);
        stack.IsEmpty.Should().BeFalse();
        stack.Peek().Should().Be(42);
    }

    [Fact]
    public void Push_MultipleElements_ShouldMaintainLIFOOrder()
    {
        // Arrange
        var stack = new CustomStack();

        // Act
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Assert
        stack.Count.Should().Be(3);
        stack.Peek().Should().Be(3); // Last pushed should be on top
    }

    [Fact]
    public void Pop_FromNonEmptyStack_ShouldReturnTopElementAndDecreaseCount()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var result = stack.Pop();

        // Assert
        result.Should().Be(3);
        stack.Count.Should().Be(2);
        stack.Peek().Should().Be(2);
    }

    [Fact]
    public void Pop_FromEmptyStack_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var stack = new CustomStack();

        // Act & Assert
        Action act = () => stack.Pop();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot pop from an empty stack.");
    }

    [Fact]
    public void Peek_FromNonEmptyStack_ShouldReturnTopElementWithoutRemoving()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(42);

        // Act
        var result = stack.Peek();

        // Assert
        result.Should().Be(42);
        stack.Count.Should().Be(1); // Count should remain the same
    }

    [Fact]
    public void Peek_FromEmptyStack_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var stack = new CustomStack();

        // Act & Assert
        Action act = () => stack.Peek();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot peek an empty stack.");
    }

    [Fact]
    public void TryPop_FromNonEmptyStack_ShouldReturnTrueAndElement()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(42);

        // Act
        var success = stack.TryPop(out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(42);
        stack.Count.Should().Be(0);
    }

    [Fact]
    public void TryPop_FromEmptyStack_ShouldReturnFalse()
    {
        // Arrange
        var stack = new CustomStack();

        // Act
        var success = stack.TryPop(out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(0); // Default value for int
    }

    [Fact]
    public void TryPeek_FromNonEmptyStack_ShouldReturnTrueAndElement()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(42);

        // Act
        var success = stack.TryPeek(out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(42);
        stack.Count.Should().Be(1); // Should not remove element
    }

    [Fact]
    public void TryPeek_FromEmptyStack_ShouldReturnFalse()
    {
        // Arrange
        var stack = new CustomStack();

        // Act
        var success = stack.TryPeek(out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(0); // Default value for int
    }

    [Fact]
    public void Clear_ShouldRemoveAllElements()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        stack.Clear();

        // Assert
        stack.Count.Should().Be(0);
        stack.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Contains_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act & Assert
        stack.Contains(2).Should().BeTrue();
        stack.Contains(1).Should().BeTrue();
        stack.Contains(3).Should().BeTrue();
    }

    [Fact]
    public void Contains_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);

        // Act & Assert
        stack.Contains(5).Should().BeFalse();
    }

    [Fact]
    public void ToArray_ShouldReturnElementsInReverseOrder()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var array = stack.ToArray();

        // Assert
        array.Should().Equal(3, 2, 1); // LIFO order
    }

    [Fact]
    public void CopyTo_ValidArray_ShouldCopyElementsInCorrectOrder()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        var array = new int[5];

        // Act
        stack.CopyTo(array, 1);

        // Assert
        array.Should().Equal(0, 3, 2, 1, 0);
    }

    [Fact]
    public void CopyTo_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);

        // Act & Assert
        Action act = () => stack.CopyTo(null!, 0);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CopyTo_InvalidIndex_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        var array = new int[5];

        // Act & Assert
        Action act = () => stack.CopyTo(array, -1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetEnumerator_ShouldIterateInLIFOOrder()
    {
        // Arrange
        var stack = new CustomStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var result = new List<int>();
        foreach (var item in stack)
        {
            result.Add(item);
        }

        // Assert
        result.Should().Equal(3, 2, 1); // LIFO order
    }

    [Fact]
    public void TrimExcess_WhenLargeCapacity_ShouldReduceCapacity()
    {
        // Arrange
        var stack = new CustomStack(100);
        stack.Push(1);
        stack.Push(2);

        // Act
        stack.TrimExcess();

        // Assert
        stack.Capacity.Should().BeLessThan(100);
        stack.Count.Should().Be(2);
        stack.Contains(1).Should().BeTrue();
        stack.Contains(2).Should().BeTrue();
    }

    [Fact]
    public void Push_BeyondInitialCapacity_ShouldGrowAutomatically()
    {
        // Arrange
        var stack = new CustomStack(2);

        // Act
        stack.Push(1);
        stack.Push(2);
        stack.Push(3); // Should trigger resize
        stack.Push(4);

        // Assert
        stack.Count.Should().Be(4);
        stack.Capacity.Should().BeGreaterThan(2);
        stack.Pop().Should().Be(4);
        stack.Pop().Should().Be(3);
        stack.Pop().Should().Be(2);
        stack.Pop().Should().Be(1);
    }

    [Fact]
    public void LifoCycle_PushAndPopOperations_ShouldMaintainCorrectOrder()
    {
        // Arrange
        var stack = new CustomStackString();

        // Act & Assert
        stack.Push("first");
        stack.Push("second");
        stack.Push("third");

        stack.Pop().Should().Be("third");
        stack.Pop().Should().Be("second");

        stack.Push("fourth");
        stack.Pop().Should().Be("fourth");
        stack.Pop().Should().Be("first");

        stack.IsEmpty.Should().BeTrue();
    }
}