using FluentAssertions;
using Xunit;
using CustomQueue = AlgorithmsDataStructures.DataStructures.Queue<int>;
using CustomQueueString = AlgorithmsDataStructures.DataStructures.Queue<string>;

namespace AlgorithmsDataStructures.Tests.DataStructures;

public class QueueTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyQueue()
    {
        // Arrange & Act
        var queue = new CustomQueue();

        // Assert
        queue.Count.Should().Be(0);
        queue.IsEmpty.Should().BeTrue();
        queue.Capacity.Should().Be(4); // Default capacity
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateQueueWithSpecifiedCapacity()
    {
        // Arrange & Act
        var queue = new CustomQueue(10);

        // Assert
        queue.Count.Should().Be(0);
        queue.IsEmpty.Should().BeTrue();
        queue.Capacity.Should().Be(10);
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Action act = () => new CustomQueue(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithCollection_ShouldCreateQueueWithCollectionElements()
    {
        // Arrange
        var collection = new[] { 1, 2, 3, 4, 5 };

        // Act
        var queue = new CustomQueue(collection);

        // Assert
        queue.Count.Should().Be(5);
        queue.IsEmpty.Should().BeFalse();
        queue.Peek().Should().Be(1); // First element should be at front
    }

    [Fact]
    public void Enqueue_SingleElement_ShouldIncreaseCount()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act
        queue.Enqueue(42);

        // Assert
        queue.Count.Should().Be(1);
        queue.IsEmpty.Should().BeFalse();
        queue.Peek().Should().Be(42);
    }

    [Fact]
    public void Enqueue_MultipleElements_ShouldMaintainFIFOOrder()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Assert
        queue.Count.Should().Be(3);
        queue.Peek().Should().Be(1); // First enqueued should be at front
    }

    [Fact]
    public void Dequeue_FromNonEmptyQueue_ShouldReturnFrontElementAndDecreaseCount()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = queue.Dequeue();

        // Assert
        result.Should().Be(1);
        queue.Count.Should().Be(2);
        queue.Peek().Should().Be(2);
    }

    [Fact]
    public void Dequeue_FromEmptyQueue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act & Assert
        Action act = () => queue.Dequeue();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot dequeue from an empty queue.");
    }

    [Fact]
    public void Peek_FromNonEmptyQueue_ShouldReturnFrontElementWithoutRemoving()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(42);

        // Act
        var result = queue.Peek();

        // Assert
        result.Should().Be(42);
        queue.Count.Should().Be(1); // Count should remain the same
    }

    [Fact]
    public void Peek_FromEmptyQueue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act & Assert
        Action act = () => queue.Peek();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot peek an empty queue.");
    }

    [Fact]
    public void TryDequeue_FromNonEmptyQueue_ShouldReturnTrueAndElement()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(42);

        // Act
        var success = queue.TryDequeue(out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(42);
        queue.Count.Should().Be(0);
    }

    [Fact]
    public void TryDequeue_FromEmptyQueue_ShouldReturnFalse()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act
        var success = queue.TryDequeue(out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(0); // Default value for int
    }

    [Fact]
    public void TryPeek_FromNonEmptyQueue_ShouldReturnTrueAndElement()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(42);

        // Act
        var success = queue.TryPeek(out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(42);
        queue.Count.Should().Be(1); // Should not remove element
    }

    [Fact]
    public void TryPeek_FromEmptyQueue_ShouldReturnFalse()
    {
        // Arrange
        var queue = new CustomQueue();

        // Act
        var success = queue.TryPeek(out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(0); // Default value for int
    }

    [Fact]
    public void Clear_ShouldRemoveAllElements()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        queue.Clear();

        // Assert
        queue.Count.Should().Be(0);
        queue.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Contains_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act & Assert
        queue.Contains(2).Should().BeTrue();
        queue.Contains(1).Should().BeTrue();
        queue.Contains(3).Should().BeTrue();
    }

    [Fact]
    public void Contains_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act & Assert
        queue.Contains(5).Should().BeFalse();
    }

    [Fact]
    public void ToArray_ShouldReturnElementsInCorrectOrder()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var array = queue.ToArray();

        // Assert
        array.Should().Equal(1, 2, 3); // FIFO order
    }

    [Fact]
    public void CopyTo_ValidArray_ShouldCopyElementsInCorrectOrder()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        var array = new int[5];

        // Act
        queue.CopyTo(array, 1);

        // Assert
        array.Should().Equal(0, 1, 2, 3, 0);
    }

    [Fact]
    public void CopyTo_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);

        // Act & Assert
        Action act = () => queue.CopyTo(null!, 0);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetEnumerator_ShouldIterateInFIFOOrder()
    {
        // Arrange
        var queue = new CustomQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = new List<int>();
        foreach (var item in queue)
        {
            result.Add(item);
        }

        // Assert
        result.Should().Equal(1, 2, 3); // FIFO order
    }

    [Fact]
    public void CircularBuffer_EnqueueDequeueOperations_ShouldWorkCorrectly()
    {
        // Arrange
        var queue = new CustomQueue(4);

        // Act - Fill the queue
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        // Dequeue some elements
        queue.Dequeue().Should().Be(1);
        queue.Dequeue().Should().Be(2);

        // Add more elements (should wrap around)
        queue.Enqueue(5);
        queue.Enqueue(6);

        // Assert
        queue.Count.Should().Be(4);
        queue.Dequeue().Should().Be(3);
        queue.Dequeue().Should().Be(4);
        queue.Dequeue().Should().Be(5);
        queue.Dequeue().Should().Be(6);
        queue.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Enqueue_BeyondInitialCapacity_ShouldGrowAutomatically()
    {
        // Arrange
        var queue = new CustomQueue(2);

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3); // Should trigger resize
        queue.Enqueue(4);

        // Assert
        queue.Count.Should().Be(4);
        queue.Capacity.Should().BeGreaterThan(2);
        queue.Dequeue().Should().Be(1);
        queue.Dequeue().Should().Be(2);
        queue.Dequeue().Should().Be(3);
        queue.Dequeue().Should().Be(4);
    }

    [Fact]
    public void TrimExcess_WhenLargeCapacity_ShouldReduceCapacity()
    {
        // Arrange
        var queue = new CustomQueue(100);
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.TrimExcess();

        // Assert
        queue.Capacity.Should().BeLessThan(100);
        queue.Count.Should().Be(2);
        queue.Contains(1).Should().BeTrue();
        queue.Contains(2).Should().BeTrue();
    }

    [Fact]
    public void FifoCycle_EnqueueAndDequeueOperations_ShouldMaintainCorrectOrder()
    {
        // Arrange
        var queue = new CustomQueueString();

        // Act & Assert
        queue.Enqueue("first");
        queue.Enqueue("second");
        queue.Enqueue("third");

        queue.Dequeue().Should().Be("first");
        queue.Dequeue().Should().Be("second");

        queue.Enqueue("fourth");
        queue.Dequeue().Should().Be("third");
        queue.Dequeue().Should().Be("fourth");

        queue.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Contains_WithCircularBuffer_ShouldWorkCorrectly()
    {
        // Arrange
        var queue = new CustomQueue(4);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        // Dequeue some and add more to test wrap-around
        queue.Dequeue();
        queue.Dequeue();
        queue.Enqueue(5);
        queue.Enqueue(6);

        // Act & Assert
        queue.Contains(3).Should().BeTrue();
        queue.Contains(4).Should().BeTrue();
        queue.Contains(5).Should().BeTrue();
        queue.Contains(6).Should().BeTrue();
        queue.Contains(1).Should().BeFalse();
        queue.Contains(2).Should().BeFalse();
    }
}