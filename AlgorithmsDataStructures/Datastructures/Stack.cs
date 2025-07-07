using System.Collections;

namespace AlgorithmsDataStructures.DataStructures;

/// <summary>
/// A generic stack implementation using a resizable array.
/// Follows LIFO (Last In, First Out) principle.
/// </summary>
/// <typeparam name="T">The type of elements in the stack</typeparam>
public class Stack<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;
    private const int DefaultCapacity = 4;

    /// <summary>
    /// Gets the number of elements in the stack.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the stack is empty.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Gets the current capacity of the stack.
    /// </summary>
    public int Capacity => _items.Length;

    /// <summary>
    /// Initializes a new instance of the Stack class that is empty and has the default initial capacity.
    /// </summary>
    public Stack() : this(DefaultCapacity) { }

    /// <summary>
    /// Initializes a new instance of the Stack class that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial number of elements that the Stack can contain.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity is less than 0.</exception>
    public Stack(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");

        _items = new T[capacity];
        _count = 0;
    }

    /// <summary>
    /// Initializes a new instance of the Stack class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection to copy elements from.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public Stack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        var items = collection.ToArray();
        _items = new T[Math.Max(items.Length, DefaultCapacity)];
        Array.Copy(items, _items, items.Length);
        _count = items.Length;
    }

    /// <summary>
    /// Inserts an element at the top of the stack.
    /// </summary>
    /// <param name="item">The element to push onto the stack.</param>
    public void Push(T item)
    {
        if (_count == _items.Length)
            EnsureCapacity(_count + 1);

        _items[_count++] = item;
    }

    /// <summary>
    /// Removes and returns the element at the top of the stack.
    /// </summary>
    /// <returns>The element removed from the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot pop from an empty stack.");

        var item = _items[--_count];
        _items[_count] = default(T)!; // Clear reference for GC
        return item;
    }

    /// <summary>
    /// Returns the element at the top of the stack without removing it.
    /// </summary>
    /// <returns>The element at the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot peek an empty stack.");

        return _items[_count - 1];
    }

    /// <summary>
    /// Attempts to remove and return the element at the top of the stack.
    /// </summary>
    /// <param name="result">When this method returns, contains the element removed from the top of the stack, if the operation succeeded.</param>
    /// <returns>true if an element was removed and returned successfully; otherwise, false.</returns>
    public bool TryPop(out T result)
    {
        if (IsEmpty)
        {
            result = default(T)!;
            return false;
        }

        result = Pop();
        return true;
    }

    /// <summary>
    /// Attempts to return the element at the top of the stack without removing it.
    /// </summary>
    /// <param name="result">When this method returns, contains the element at the top of the stack, if the operation succeeded.</param>
    /// <returns>true if an element was returned successfully; otherwise, false.</returns>
    public bool TryPeek(out T result)
    {
        if (IsEmpty)
        {
            result = default(T)!;
            return false;
        }

        result = Peek();
        return true;
    }

    /// <summary>
    /// Removes all elements from the stack.
    /// </summary>
    public void Clear()
    {
        if (_count > 0)
        {
            Array.Clear(_items, 0, _count);
            _count = 0;
        }
    }

    /// <summary>
    /// Determines whether an element is in the stack.
    /// </summary>
    /// <param name="item">The element to locate in the stack.</param>
    /// <returns>true if item is found in the stack; otherwise, false.</returns>
    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < _count; i++)
        {
            if (comparer.Equals(_items[i], item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Copies the stack elements to an existing one-dimensional array, starting at the specified array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from stack.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arrayIndex is less than 0.</exception>
    /// <exception cref="ArgumentException">Thrown when the number of elements in the source stack is greater than the available space from arrayIndex to the end of the destination array.</exception>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index cannot be negative.");
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Destination array is too small.");

        // Copy in reverse order to maintain stack semantics
        for (int i = 0; i < _count; i++)
            array[arrayIndex + i] = _items[_count - 1 - i];
    }

    /// <summary>
    /// Copies the stack to a new array.
    /// </summary>
    /// <returns>A new array containing copies of the elements of the stack.</returns>
    public T[] ToArray()
    {
        var result = new T[_count];
        CopyTo(result, 0);
        return result;
    }

    /// <summary>
    /// Sets the capacity to the actual number of elements in the stack, if that number is less than 90 percent of current capacity.
    /// </summary>
    public void TrimExcess()
    {
        int threshold = (int)(_items.Length * 0.9);
        if (_count < threshold)
        {
            var newItems = new T[_count];
            Array.Copy(_items, newItems, _count);
            _items = newItems;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the stack.
    /// </summary>
    /// <returns>An enumerator for the stack.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _count - 1; i >= 0; i--)
            yield return _items[i];
    }

    /// <summary>
    /// Returns an enumerator that iterates through the stack.
    /// </summary>
    /// <returns>An enumerator for the stack.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void EnsureCapacity(int min)
    {
        if (_items.Length < min)
        {
            int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
            if (newCapacity < min)
                newCapacity = min;

            var newItems = new T[newCapacity];
            if (_count > 0)
                Array.Copy(_items, newItems, _count);
            _items = newItems;
        }
    }
}