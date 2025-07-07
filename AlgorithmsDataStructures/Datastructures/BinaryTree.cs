using AlgorithmsDataStructures.DataStructures;
using System.Collections;

namespace AlgorithmsDataStructures.Datastructures;

/// <summary>
/// Represents a node in a binary tree.
/// </summary>
/// <typeparam name="T">The type of the value stored in the node.</typeparam>
public class BinaryTreeNode<T>
{
    /// <summary>
    /// Gets or sets the value stored in the node.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets or sets the left child node.
    /// </summary>
    public BinaryTreeNode<T>? Left { get; set; }

    /// <summary>
    /// Gets or sets the right child node.
    /// </summary>
    public BinaryTreeNode<T>? Right { get; set; }

    /// <summary>
    /// Gets a value indicating whether this node is a leaf (has no children).
    /// </summary>
    public bool IsLeaf => Left == null && Right == null;

    /// <summary>
    /// Initializes a new instance of the BinaryTreeNode class with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    public BinaryTreeNode(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the BinaryTreeNode class with the specified value and children.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    /// <param name="left">The left child node.</param>
    /// <param name="right">The right child node.</param>
    public BinaryTreeNode(T value, BinaryTreeNode<T>? left, BinaryTreeNode<T>? right)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}

/// <summary>
/// A generic binary search tree implementation.
/// </summary>
/// <typeparam name="T">The type of elements in the tree</typeparam>
public class BinarySearchTree<T> : IEnumerable<T>
{
    private BinaryTreeNode<T>? _root;
    private readonly IComparer<T> _comparer;
    private int _count;

    /// <summary>
    /// Gets the number of elements in the tree.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the tree is empty.
    /// </summary>
    public bool IsEmpty => _root == null;

    /// <summary>
    /// Gets the root node of the tree.
    /// </summary>
    public BinaryTreeNode<T>? Root => _root;

    /// <summary>
    /// Initializes a new instance of the BinarySearchTree class.
    /// </summary>
    /// <param name="comparer">The comparer to use for ordering elements. If null, uses default comparer.</param>
    public BinarySearchTree(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    /// <summary>
    /// Initializes a new instance of the BinarySearchTree class with elements from the specified collection.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new tree.</param>
    /// <param name="comparer">The comparer to use for ordering elements. If null, uses default comparer.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public BinarySearchTree(IEnumerable<T> collection, IComparer<T>? comparer = null)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        _comparer = comparer ?? Comparer<T>.Default;

        foreach (var item in collection)
            Insert(item);
    }

    /// <summary>
    /// Inserts a value into the tree.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    /// <returns>true if the value was inserted; false if it already exists.</returns>
    public bool Insert(T value)
    {
        if (_root == null)
        {
            _root = new BinaryTreeNode<T>(value);
            _count++;
            return true;
        }

        return InsertHelper(_root, value);
    }

    private bool InsertHelper(BinaryTreeNode<T> node, T value)
    {
        int comparison = _comparer.Compare(value, node.Value);

        if (comparison == 0)
            return false; // Value already exists

        if (comparison < 0)
        {
            if (node.Left == null)
            {
                node.Left = new BinaryTreeNode<T>(value);
                _count++;
                return true;
            }
            return InsertHelper(node.Left, value);
        }
        else
        {
            if (node.Right == null)
            {
                node.Right = new BinaryTreeNode<T>(value);
                _count++;
                return true;
            }
            return InsertHelper(node.Right, value);
        }
    }

    /// <summary>
    /// Searches for a value in the tree.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>true if the value is found; otherwise, false.</returns>
    public bool Contains(T value)
    {
        return ContainsHelper(_root, value);
    }

    private bool ContainsHelper(BinaryTreeNode<T>? node, T value)
    {
        if (node == null)
            return false;

        int comparison = _comparer.Compare(value, node.Value);

        if (comparison == 0)
            return true;
        else if (comparison < 0)
            return ContainsHelper(node.Left, value);
        else
            return ContainsHelper(node.Right, value);
    }

    /// <summary>
    /// Finds a node with the specified value.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The node containing the value if found; otherwise, null.</returns>
    public BinaryTreeNode<T>? Find(T value)
    {
        return FindHelper(_root, value);
    }

    private BinaryTreeNode<T>? FindHelper(BinaryTreeNode<T>? node, T value)
    {
        if (node == null)
            return null;

        int comparison = _comparer.Compare(value, node.Value);

        if (comparison == 0)
            return node;
        else if (comparison < 0)
            return FindHelper(node.Left, value);
        else
            return FindHelper(node.Right, value);
    }

    /// <summary>
    /// Removes a value from the tree.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>true if the value was removed; false if it was not found.</returns>
    public bool Remove(T value)
    {
        var (newRoot, removed) = RemoveHelper(_root, value);
        _root = newRoot;
        if (removed)
            _count--;
        return removed;
    }

    private (BinaryTreeNode<T>?, bool) RemoveHelper(BinaryTreeNode<T>? node, T value)
    {
        if (node == null)
            return (null, false);

        int comparison = _comparer.Compare(value, node.Value);

        if (comparison < 0)
        {
            var (leftChild, removed) = RemoveHelper(node.Left, value);
            node.Left = leftChild;
            return (node, removed);
        }
        else if (comparison > 0)
        {
            var (rightChild, removed) = RemoveHelper(node.Right, value);
            node.Right = rightChild;
            return (node, removed);
        }
        else
        {
            // Node to be deleted found
            if (node.Left == null)
                return (node.Right, true);
            else if (node.Right == null)
                return (node.Left, true);
            else
            {
                // Node with two children: Get the inorder successor
                var successor = FindMin(node.Right);
                node.Value = successor.Value;
                var (rightChild, _) = RemoveHelper(node.Right, successor.Value);
                node.Right = rightChild;
                return (node, true);
            }
        }
    }

    /// <summary>
    /// Finds the minimum value in the tree.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <returns>The minimum value if the tree is not empty.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the tree is empty.</exception>
    public T FindMin()
    {
        if (_root == null)
            throw new InvalidOperationException("Tree is empty.");

        return FindMin(_root).Value;
    }

    private BinaryTreeNode<T> FindMin(BinaryTreeNode<T> node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    /// <summary>
    /// Finds the maximum value in the tree.
    /// Time Complexity: O(log n) average case, O(n) worst case.
    /// </summary>
    /// <returns>The maximum value if the tree is not empty.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the tree is empty.</exception>
    public T FindMax()
    {
        if (_root == null)
            throw new InvalidOperationException("Tree is empty.");

        return FindMax(_root).Value;
    }

    private BinaryTreeNode<T> FindMax(BinaryTreeNode<T> node)
    {
        while (node.Right != null)
            node = node.Right;
        return node;
    }

    /// <summary>
    /// Gets the height of the tree.
    /// Time Complexity: O(n).
    /// </summary>
    /// <returns>The height of the tree, or -1 if the tree is empty.</returns>
    public int GetHeight()
    {
        return GetHeightHelper(_root);
    }

    private int GetHeightHelper(BinaryTreeNode<T>? node)
    {
        if (node == null)
            return -1;

        int leftHeight = GetHeightHelper(node.Left);
        int rightHeight = GetHeightHelper(node.Right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    /// <summary>
    /// Removes all elements from the tree.
    /// </summary>
    public void Clear()
    {
        _root = null;
        _count = 0;
    }

    /// <summary>
    /// Performs an in-order traversal of the tree.
    /// Time Complexity: O(n).
    /// </summary>
    /// <returns>An enumerable of values in sorted order.</returns>
    public IEnumerable<T> InOrderTraversal()
    {
        return InOrderTraversalHelper(_root);
    }

    private IEnumerable<T> InOrderTraversalHelper(BinaryTreeNode<T>? node)
    {
        if (node != null)
        {
            foreach (var value in InOrderTraversalHelper(node.Left))
                yield return value;

            yield return node.Value;

            foreach (var value in InOrderTraversalHelper(node.Right))
                yield return value;
        }
    }

    /// <summary>
    /// Performs a pre-order traversal of the tree.
    /// Time Complexity: O(n).
    /// </summary>
    /// <returns>An enumerable of values in pre-order.</returns>
    public IEnumerable<T> PreOrderTraversal()
    {
        return PreOrderTraversalHelper(_root);
    }

    private IEnumerable<T> PreOrderTraversalHelper(BinaryTreeNode<T>? node)
    {
        if (node != null)
        {
            yield return node.Value;

            foreach (var value in PreOrderTraversalHelper(node.Left))
                yield return value;

            foreach (var value in PreOrderTraversalHelper(node.Right))
                yield return value;
        }
    }

    /// <summary>
    /// Performs a post-order traversal of the tree.
    /// Time Complexity: O(n).
    /// </summary>
    /// <returns>An enumerable of values in post-order.</returns>
    public IEnumerable<T> PostOrderTraversal()
    {
        return PostOrderTraversalHelper(_root);
    }

    private IEnumerable<T> PostOrderTraversalHelper(BinaryTreeNode<T>? node)
    {
        if (node != null)
        {
            foreach (var value in PostOrderTraversalHelper(node.Left))
                yield return value;

            foreach (var value in PostOrderTraversalHelper(node.Right))
                yield return value;

            yield return node.Value;
        }
    }

    /// <summary>
    /// Performs a breadth-first (level-order) traversal of the tree.
    /// Time Complexity: O(n).
    /// Space Complexity: O(w) where w is the maximum width of the tree.
    /// </summary>
    /// <returns>An enumerable of values in level order.</returns>
    public IEnumerable<T> BreadthFirstTraversal()
    {
        if (_root == null)
            yield break;

        var queue = new System.Collections.Generic.Queue<BinaryTreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node.Value;

            if (node.Left != null)
                queue.Enqueue(node.Left);

            if (node.Right != null)
                queue.Enqueue(node.Right);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the tree in sorted order.
    /// </summary>
    /// <returns>An enumerator for the tree.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the tree.
    /// </summary>
    /// <returns>An enumerator for the tree.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}