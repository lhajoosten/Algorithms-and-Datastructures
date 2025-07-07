using AlgorithmsDataStructures.Datastructures;
using FluentAssertions;
using Xunit;

namespace AlgorithmsDataStructures.Tests.DataStructures;

public class BinarySearchTreeTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyTree()
    {
        // Arrange & Act
        var tree = new BinarySearchTree<int>();

        // Assert
        tree.Count.Should().Be(0);
        tree.IsEmpty.Should().BeTrue();
        tree.Root.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithCollection_ShouldCreateTreeWithElements()
    {
        // Arrange
        var collection = new[] { 5, 3, 7, 1, 9 };

        // Act
        var tree = new BinarySearchTree<int>(collection);

        // Assert
        tree.Count.Should().Be(5);
        tree.IsEmpty.Should().BeFalse();
        tree.Contains(5).Should().BeTrue();
        tree.Contains(3).Should().BeTrue();
        tree.Contains(7).Should().BeTrue();
        tree.Contains(1).Should().BeTrue();
        tree.Contains(9).Should().BeTrue();
    }

    [Fact]
    public void Insert_SingleElement_ShouldIncreaseCount()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();

        // Act
        var result = tree.Insert(5);

        // Assert
        result.Should().BeTrue();
        tree.Count.Should().Be(1);
        tree.Root!.Value.Should().Be(5);
    }

    [Fact]
    public void Insert_DuplicateElement_ShouldReturnFalse()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);

        // Act
        var result = tree.Insert(5);

        // Assert
        result.Should().BeFalse();
        tree.Count.Should().Be(1);
    }

    [Fact]
    public void Insert_MultipleElements_ShouldMaintainBSTProperty()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();

        // Act
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);
        tree.Insert(6);
        tree.Insert(9);

        // Assert
        tree.Count.Should().Be(7);
        tree.Root!.Value.Should().Be(5);
        tree.Root.Left!.Value.Should().Be(3);
        tree.Root.Right!.Value.Should().Be(7);
        tree.Root.Left.Left!.Value.Should().Be(1);
        tree.Root.Left.Right!.Value.Should().Be(4);
        tree.Root.Right.Left!.Value.Should().Be(6);
        tree.Root.Right.Right!.Value.Should().Be(9);
    }

    [Fact]
    public void Contains_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);

        // Act & Assert
        tree.Contains(5).Should().BeTrue();
        tree.Contains(3).Should().BeTrue();
        tree.Contains(7).Should().BeTrue();
    }

    [Fact]
    public void Contains_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);

        // Act & Assert
        tree.Contains(2).Should().BeFalse();
        tree.Contains(10).Should().BeFalse();
    }

    [Fact]
    public void Find_ExistingElement_ShouldReturnNode()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);

        // Act
        var node = tree.Find(3);

        // Assert
        node.Should().NotBeNull();
        node!.Value.Should().Be(3);
    }

    [Fact]
    public void Find_NonExistingElement_ShouldReturnNull()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);

        // Act
        var node = tree.Find(10);

        // Assert
        node.Should().BeNull();
    }

    [Fact]
    public void Remove_LeafNode_ShouldRemoveCorrectly()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);

        // Act
        var result = tree.Remove(1);

        // Assert
        result.Should().BeTrue();
        tree.Count.Should().Be(3);
        tree.Contains(1).Should().BeFalse();
        tree.Root!.Left!.Left.Should().BeNull();
    }

    [Fact]
    public void Remove_NodeWithOneChild_ShouldRemoveCorrectly()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(6);

        // Act
        var result = tree.Remove(7);

        // Assert
        result.Should().BeTrue();
        tree.Count.Should().Be(3);
        tree.Contains(7).Should().BeFalse();
        tree.Root!.Right!.Value.Should().Be(6);
    }

    [Fact]
    public void Remove_NodeWithTwoChildren_ShouldRemoveCorrectly()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);
        tree.Insert(6);
        tree.Insert(9);

        // Act
        var result = tree.Remove(3);

        // Assert
        result.Should().BeTrue();
        tree.Count.Should().Be(6);
        tree.Contains(3).Should().BeFalse();
        tree.Root!.Left!.Value.Should().Be(4); // Successor should replace removed node
        tree.Root.Left.Left!.Value.Should().Be(1);
        tree.Root.Left.Right.Should().BeNull();
    }

    [Fact]
    public void Remove_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);

        // Act
        var result = tree.Remove(10);

        // Assert
        result.Should().BeFalse();
        tree.Count.Should().Be(1);
    }

    [Fact]
    public void FindMin_NonEmptyTree_ShouldReturnMinimumValue()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(9);

        // Act
        var min = tree.FindMin();

        // Assert
        min.Should().Be(1);
    }

    [Fact]
    public void FindMin_EmptyTree_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();

        // Act & Assert
        Action act = () => tree.FindMin();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Tree is empty.");
    }

    [Fact]
    public void FindMax_NonEmptyTree_ShouldReturnMaximumValue()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(9);

        // Act
        var max = tree.FindMax();

        // Assert
        max.Should().Be(9);
    }

    [Fact]
    public void FindMax_EmptyTree_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();

        // Act & Assert
        Action act = () => tree.FindMax();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Tree is empty.");
    }

    [Fact]
    public void GetHeight_EmptyTree_ShouldReturnMinusOne()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();

        // Act
        var height = tree.GetHeight();

        // Assert
        height.Should().Be(-1);
    }

    [Fact]
    public void GetHeight_BalancedTree_ShouldReturnCorrectHeight()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);

        // Act
        var height = tree.GetHeight();

        // Assert
        height.Should().Be(2); // Root at level 0, leaves at level 2
    }

    [Fact]
    public void InOrderTraversal_ShouldReturnElementsInSortedOrder()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        var values = new[] { 5, 3, 7, 1, 4, 6, 9 };
        foreach (var value in values)
        {
            tree.Insert(value);
        }

        // Act
        var result = tree.InOrderTraversal().ToList();

        // Assert
        result.Should().Equal(1, 3, 4, 5, 6, 7, 9);
    }

    [Fact]
    public void PreOrderTraversal_ShouldReturnElementsInCorrectOrder()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);

        // Act
        var result = tree.PreOrderTraversal().ToList();

        // Assert
        result.Should().Equal(5, 3, 1, 4, 7); // Root, Left subtree, Right subtree
    }

    [Fact]
    public void PostOrderTraversal_ShouldReturnElementsInCorrectOrder()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);

        // Act
        var result = tree.PostOrderTraversal().ToList();

        // Assert
        result.Should().Equal(1, 4, 3, 7, 5); // Left subtree, Right subtree, Root
    }

    [Fact]
    public void BreadthFirstTraversal_ShouldReturnElementsInLevelOrder()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(4);
        tree.Insert(6);
        tree.Insert(9);

        // Act
        var result = tree.BreadthFirstTraversal().ToList();

        // Assert
        result.Should().Equal(5, 3, 7, 1, 4, 6, 9); // Level by level
    }

    [Fact]
    public void Clear_ShouldRemoveAllElements()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);

        // Act
        tree.Clear();

        // Assert
        tree.Count.Should().Be(0);
        tree.IsEmpty.Should().BeTrue();
        tree.Root.Should().BeNull();
    }

    [Fact]
    public void GetEnumerator_ShouldIterateInSortedOrder()
    {
        // Arrange
        var tree = new BinarySearchTree<int>();
        var values = new[] { 5, 3, 7, 1, 9 };
        foreach (var value in values)
        {
            tree.Insert(value);
        }

        // Act
        var result = new List<int>();
        foreach (var value in tree)
        {
            result.Add(value);
        }

        // Assert
        result.Should().Equal(1, 3, 5, 7, 9);
    }

    [Fact]
    public void Tree_WithCustomComparer_ShouldWorkCorrectly()
    {
        // Arrange
        var reverseComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var tree = new BinarySearchTree<int>(comparer: reverseComparer);

        // Act
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);

        // Assert
        var inOrder = tree.InOrderTraversal().ToList();
        inOrder.Should().Equal(7, 5, 3); // Reverse order due to custom comparer
    }

    [Fact]
    public void BinaryTreeNode_Properties_ShouldWorkCorrectly()
    {
        // Arrange
        var node = new BinaryTreeNode<int>(5);
        var leftChild = new BinaryTreeNode<int>(3);
        var rightChild = new BinaryTreeNode<int>(7);

        // Act
        node.Left = leftChild;
        node.Right = rightChild;

        // Assert
        node.Value.Should().Be(5);
        node.Left.Should().Be(leftChild);
        node.Right.Should().Be(rightChild);
        node.IsLeaf.Should().BeFalse();

        leftChild.IsLeaf.Should().BeTrue();
        rightChild.IsLeaf.Should().BeTrue();
    }
}