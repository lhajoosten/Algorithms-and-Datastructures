using System.Collections;

namespace AlgorithmsDataStructures.DataStructures;

/// <summary>
/// Represents a node in a doubly linked list.
/// </summary>
/// <typeparam name="T">The type of the value stored in the node.</typeparam>
public class LinkedListNode<T>
{
    internal LinkedList<T>? _list;
    internal LinkedListNode<T>? _next;
    internal LinkedListNode<T>? _prev;

    /// <summary>
    /// Gets the value contained in the node.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets the LinkedList that the LinkedListNode belongs to.
    /// </summary>
    public LinkedList<T>? List => _list;

    /// <summary>
    /// Gets the next node in the LinkedList.
    /// </summary>
    public LinkedListNode<T>? Next => _next == _list?._head ? null : _next;

    /// <summary>
    /// Gets the previous node in the LinkedList.
    /// </summary>
    public LinkedListNode<T>? Previous => _prev == _list?._head ? null : _prev;

    /// <summary>
    /// Initializes a new instance of the LinkedListNode class, containing the specified value.
    /// </summary>
    /// <param name="value">The value to contain in the LinkedListNode.</param>
    public LinkedListNode(T value)
    {
        Value = value;
    }

    internal LinkedListNode(LinkedList<T> list, T value)
    {
        _list = list;
        Value = value;
    }

    internal void Invalidate()
    {
        _list = null;
        _next = null;
        _prev = null;
    }
}

/// <summary>
/// A generic doubly linked list implementation.
/// </summary>
/// <typeparam name="T">The type of elements in the linked list</typeparam>
public class LinkedList<T> : IEnumerable<T>
{
    internal LinkedListNode<T>? _head;
    private int _count;

    /// <summary>
    /// Gets the number of nodes actually contained in the LinkedList.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the LinkedList is empty.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Gets the first node of the LinkedList.
    /// </summary>
    public LinkedListNode<T>? First => IsEmpty ? null : _head;

    /// <summary>
    /// Gets the last node of the LinkedList.
    /// </summary>
    public LinkedListNode<T>? Last => IsEmpty ? null : _head?._prev;

    /// <summary>
    /// Initializes a new instance of the LinkedList class that is empty.
    /// </summary>
    public LinkedList() { }

    /// <summary>
    /// Initializes a new instance of the LinkedList class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public LinkedList(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        foreach (var item in collection)
            AddLast(item);
    }

    /// <summary>
    /// Adds a new node containing the specified value at the start of the LinkedList.
    /// </summary>
    /// <param name="value">The value to add at the start of the LinkedList.</param>
    /// <returns>The new LinkedListNode containing value.</returns>
    public LinkedListNode<T> AddFirst(T value)
    {
        var node = new LinkedListNode<T>(this, value);
        if (_head == null)
        {
            InsertNodeToEmptyList(node);
        }
        else
        {
            InsertNodeBefore(_head, node);
            _head = node;
        }
        return node;
    }

    /// <summary>
    /// Adds the specified new node at the start of the LinkedList.
    /// </summary>
    /// <param name="node">The new LinkedListNode to add at the start of the LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node belongs to another LinkedList.</exception>
    public void AddFirst(LinkedListNode<T> node)
    {
        ValidateNewNode(node);

        if (_head == null)
        {
            InsertNodeToEmptyList(node);
        }
        else
        {
            InsertNodeBefore(_head, node);
            _head = node;
        }
        node._list = this;
    }

    /// <summary>
    /// Adds a new node containing the specified value at the end of the LinkedList.
    /// </summary>
    /// <param name="value">The value to add at the end of the LinkedList.</param>
    /// <returns>The new LinkedListNode containing value.</returns>
    public LinkedListNode<T> AddLast(T value)
    {
        var node = new LinkedListNode<T>(this, value);
        if (_head == null)
        {
            InsertNodeToEmptyList(node);
        }
        else
        {
            InsertNodeBefore(_head, node);
        }
        return node;
    }

    /// <summary>
    /// Adds the specified new node at the end of the LinkedList.
    /// </summary>
    /// <param name="node">The new LinkedListNode to add at the end of the LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node belongs to another LinkedList.</exception>
    public void AddLast(LinkedListNode<T> node)
    {
        ValidateNewNode(node);

        if (_head == null)
        {
            InsertNodeToEmptyList(node);
        }
        else
        {
            InsertNodeBefore(_head, node);
        }
        node._list = this;
    }

    /// <summary>
    /// Adds a new node containing the specified value after the specified existing node in the LinkedList.
    /// </summary>
    /// <param name="node">The LinkedListNode after which to insert a new LinkedListNode containing value.</param>
    /// <param name="value">The value to add to the LinkedList.</param>
    /// <returns>The new LinkedListNode containing value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node is not in the current LinkedList.</exception>
    public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        var newNode = new LinkedListNode<T>(this, value);
        InsertNodeBefore(node._next!, newNode);
        return newNode;
    }

    /// <summary>
    /// Adds the specified new node after the specified existing node in the LinkedList.
    /// </summary>
    /// <param name="node">The LinkedListNode after which to insert newNode.</param>
    /// <param name="newNode">The new LinkedListNode to add to the LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when node or newNode is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node is not in the current LinkedList or newNode belongs to another LinkedList.</exception>
    public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
    {
        ValidateNode(node);
        ValidateNewNode(newNode);
        InsertNodeBefore(node._next!, newNode);
        newNode._list = this;
    }

    /// <summary>
    /// Adds a new node containing the specified value before the specified existing node in the LinkedList.
    /// </summary>
    /// <param name="node">The LinkedListNode before which to insert a new LinkedListNode containing value.</param>
    /// <param name="value">The value to add to the LinkedList.</param>
    /// <returns>The new LinkedListNode containing value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node is not in the current LinkedList.</exception>
    public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
    {
        ValidateNode(node);
        var newNode = new LinkedListNode<T>(this, value);
        InsertNodeBefore(node, newNode);
        if (node == _head)
            _head = newNode;
        return newNode;
    }

    /// <summary>
    /// Adds the specified new node before the specified existing node in the LinkedList.
    /// </summary>
    /// <param name="node">The LinkedListNode before which to insert newNode.</param>
    /// <param name="newNode">The new LinkedListNode to add to the LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when node or newNode is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node is not in the current LinkedList or newNode belongs to another LinkedList.</exception>
    public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
    {
        ValidateNode(node);
        ValidateNewNode(newNode);
        InsertNodeBefore(node, newNode);
        newNode._list = this;
        if (node == _head)
            _head = newNode;
    }

    /// <summary>
    /// Removes the first occurrence of the specified value from the LinkedList.
    /// </summary>
    /// <param name="value">The value to remove from the LinkedList.</param>
    /// <returns>true if the element containing value is successfully removed; otherwise, false.</returns>
    public bool Remove(T value)
    {
        var node = Find(value);
        if (node != null)
        {
            RemoveNode(node);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes the specified node from the LinkedList.
    /// </summary>
    /// <param name="node">The LinkedListNode to remove from the LinkedList.</param>
    /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when node is not in the current LinkedList.</exception>
    public void Remove(LinkedListNode<T> node)
    {
        ValidateNode(node);
        RemoveNode(node);
    }

    /// <summary>
    /// Removes the node at the start of the LinkedList.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the LinkedList is empty.</exception>
    public void RemoveFirst()
    {
        if (_head == null)
            throw new InvalidOperationException("Cannot remove from an empty list.");
        RemoveNode(_head);
    }

    /// <summary>
    /// Removes the node at the end of the LinkedList.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the LinkedList is empty.</exception>
    public void RemoveLast()
    {
        if (_head == null)
            throw new InvalidOperationException("Cannot remove from an empty list.");
        RemoveNode(_head._prev!);
    }

    /// <summary>
    /// Removes all nodes from the LinkedList.
    /// </summary>
    public void Clear()
    {
        var current = _head;
        while (current != null)
        {
            var next = current._next;
            current.Invalidate();
            current = next;
            if (current == _head)
                break;
        }

        _head = null;
        _count = 0;
    }

    /// <summary>
    /// Determines whether a value is in the LinkedList.
    /// </summary>
    /// <param name="value">The value to locate in the LinkedList.</param>
    /// <returns>true if value is found in the LinkedList; otherwise, false.</returns>
    public bool Contains(T value) => Find(value) != null;

    /// <summary>
    /// Finds the first node that contains the specified value.
    /// </summary>
    /// <param name="value">The value to locate in the LinkedList.</param>
    /// <returns>The first LinkedListNode that contains the specified value, if found; otherwise, null.</returns>
    public LinkedListNode<T>? Find(T value)
    {
        var node = _head;
        var comparer = EqualityComparer<T>.Default;

        if (node != null)
        {
            if (value != null)
            {
                do
                {
                    if (node != null && comparer.Equals(node.Value, value))
                        return node;
                    node = node?._next;
                } while (node != _head);
            }
            else
            {
                do
                {
                    if (node != null && node.Value == null)
                        return node;
                    node = node?._next;
                } while (node != _head);
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the last node that contains the specified value.
    /// </summary>
    /// <param name="value">The value to locate in the LinkedList.</param>
    /// <returns>The last LinkedListNode that contains the specified value, if found; otherwise, null.</returns>
    public LinkedListNode<T>? FindLast(T value)
    {
        if (_head == null)
            return null;

        var last = _head._prev;
        var node = last;
        var comparer = EqualityComparer<T>.Default;

        if (value != null)
        {
            do
            {
                if (comparer.Equals(node!.Value, value))
                    return node;
                node = node._prev;
            } while (node != last);
        }
        else
        {
            do
            {
                if (node!.Value == null)
                    return node;
                node = node._prev;
            } while (node != last);
        }
        return null;
    }

    /// <summary>
    /// Copies the entire LinkedList to a compatible one-dimensional Array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional Array that is the destination of the elements copied from LinkedList.</param>
    /// <param name="index">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than zero.</exception>
    /// <exception cref="ArgumentException">Thrown when the number of elements in the source LinkedList is greater than the available space from index to the end of the destination array.</exception>
    public void CopyTo(T[] array, int index)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
        if (array.Length - index < _count)
            throw new ArgumentException("Destination array is too small.");

        var node = _head;
        if (node != null)
        {
            do
            {
                array[index++] = node != null ? node.Value : default!;
                node = node?._next;
            } while (node != _head);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the LinkedList.
    /// </summary>
    /// <returns>An IEnumerator for the LinkedList.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        var node = _head;
        if (node != null)
        {
            do
            {
                yield return node != null ? node.Value : default!;
                node = node?._next;
            } while (node != _head);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the LinkedList.
    /// </summary>
    /// <returns>An IEnumerator for the LinkedList.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void InsertNodeToEmptyList(LinkedListNode<T> newNode)
    {
        newNode._next = newNode;
        newNode._prev = newNode;
        _head = newNode;
        _count++;
    }

    private void InsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
    {
        newNode._next = node;
        newNode._prev = node._prev;
        node._prev!._next = newNode;
        node._prev = newNode;
        _count++;
    }

    private void RemoveNode(LinkedListNode<T> node)
    {
        if (node._next == node)
        {
            _head = null;
        }
        else
        {
            node._next!._prev = node._prev;
            node._prev!._next = node._next;
            if (_head == node)
                _head = node._next;
        }
        node.Invalidate();
        _count--;
    }

    private void ValidateNode(LinkedListNode<T> node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));
        if (node._list != this)
            throw new InvalidOperationException("The LinkedList node does not belong to current LinkedList.");
    }

    private void ValidateNewNode(LinkedListNode<T> node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));
        if (node._list != null)
            throw new InvalidOperationException("The LinkedList node already belongs to a LinkedList.");
    }
}