using System.Collections;

namespace AlgorithmsDataStructures.DataStructures;

/// <summary>
/// A generic queue implementation using a circular buffer.
/// Follows FIFO (First In, First Out) principle.
/// </summary>
/// <typeparam name="T">The type of elements in the queue</typeparam>
public class Queue<T> : IEnumerable<T>
{
    private T[] _items;
    private int _head;
    private int _tail;
    private int _count;
    private const int DefaultCapacity = 4;

    /// <summary>
    /// Gets the number of elements in the queue.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the queue is empty.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Gets the current capacity of the queue.
    /// </summary>
    public int Capacity => _items.Length;

    /// <summary>
    /// Initializes a new instance of the Queue class that is empty and has the default initial capacity.
    /// </summary>
    public Queue() : this(DefaultCapacity) { }

    /// <summary>
    /// Initializes a new instance of the Queue class that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial number of elements that the Queue can contain.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity is less than 0.</exception>
    public Queue(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");

        _items = new T[capacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    /// <summary>
    /// Initializes a new instance of the Queue class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection to copy elements from.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public Queue(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        var items = collection.ToArray();
        var capacity = Math.Max(items.Length, DefaultCapacity);
        _items = new T[capacity];

        if (items.Length > 0)
        {
            Array.Copy(items, _items, items.Length);
            _tail = items.Length % capacity;
        }

        _head = 0;
        _count = items.Length;
    }

    /// <summary>
    /// Adds an element to the end of the queue.
    /// </summary>
    /// <param name="item">The element to add to the queue.</param>
    public void Enqueue(T item)
    {
        if (_count == _items.Length)
            EnsureCapacity(_count + 1);

        _items[_tail] = item;
        _tail = (_tail + 1) % _items.Length;
        _count++;
    }

    /// <summary>
    /// Removes and returns the element at the beginning of the queue.
    /// </summary>
    /// <returns>The element removed from the beginning of the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot dequeue from an empty queue.");

        var item = _items[_head];
        _items[_head] = default(T)!;
        _head = (_head + 1) % _items.Length;
        _count--;
        return item;
    }

    /// <summary>
    /// Returns the element at the beginning of the queue without removing it.
    /// </summary>
    /// <returns>The element at the beginning of the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot peek an empty queue.");

        return _items[_head];
    }

    /// <summary>
    /// Attempts to remove and return the element at the beginning of the queue.
    /// </summary>
    /// <param name="result">When this method returns, contains the element removed from the beginning of the queue, if the operation succeeded.</param>
    /// <returns>true if an element was removed and returned successfully; otherwise, false.</returns>
    public bool TryDequeue(out T result)
    {
        if (IsEmpty)
        {
            result = default(T)!;
            return false;
        }

        result = Dequeue();
        return true;
    }

    /// <summary>
    /// Attempts to return the element at the beginning of the queue without removing it.
    /// </summary>
    /// <param name="result">When this method returns, contains the element at the beginning of the queue, if the operation succeeded.</param>
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
    /// Removes all elements from the queue.
    /// </summary>
    public void Clear()
    {
        if (_count > 0)
        {
            if (_head < _tail)
            {
                Array.Clear(_items, _head, _count);
            }
            else
            {
                Array.Clear(_items, _head, _items.Length - _head);
                Array.Clear(_items, 0, _tail);
            }

            _head = 0;
            _tail = 0;
            _count = 0;
        }
    }

    /// <summary>
    /// Determines whether an element is in the queue.
    /// </summary>
    /// <param name="item">The element to locate in the queue.</param>
    /// <returns>true if item is found in the queue; otherwise, false.</returns>
    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;

        if (_count == 0)
            return false;

        if (_head < _tail)
        {
            for (int i = _head; i < _tail; i++)
            {
                if (comparer.Equals(_items[i], item))
                    return true;
            }
        }
        else
        {
            for (int i = _head; i < _items.Length; i++)
            {
                if (comparer.Equals(_items[i], item))
                    return true;
            }
            for (int i = 0; i < _tail; i++)
            {
                if (comparer.Equals(_items[i], item))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Copies the queue elements to an existing one-dimensional array, starting at the specified array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from queue.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arrayIndex is less than 0.</exception>
    /// <exception cref="ArgumentException">Thrown when the number of elements in the source queue is greater than the available space from arrayIndex to the end of the destination array.</exception>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index cannot be negative.");
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Destination array is too small.");

        if (_count == 0)
            return;

        if (_head < _tail)
        {
            Array.Copy(_items, _head, array, arrayIndex, _count);
        }
        else
        {
            int firstPart = _items.Length - _head;
            Array.Copy(_items, _head, array, arrayIndex, firstPart);
            Array.Copy(_items, 0, array, arrayIndex + firstPart, _tail);
        }
    }

    /// <summary>
    /// Copies the queue to a new array.
    /// </summary>
    /// <returns>A new array containing copies of the elements of the queue.</returns>
    public T[] ToArray()
    {
        var result = new T[_count];
        CopyTo(result, 0);
        return result;
    }

    /// <summary>
    /// Sets the capacity to the actual number of elements in the queue, if that number is less than 90 percent of current capacity.
    /// </summary>
    public void TrimExcess()
    {
        int threshold = (int)(_items.Length * 0.9);
        if (_count < threshold)
        {
            var newItems = new T[_count];
            CopyTo(newItems, 0);
            _items = newItems;
            _head = 0;
            _tail = _count;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the queue.
    /// </summary>
    /// <returns>An enumerator for the queue.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        if (_count == 0)
            yield break;

        if (_head < _tail)
        {
            for (int i = _head; i < _tail; i++)
                yield return _items[i];
        }
        else
        {
            for (int i = _head; i < _items.Length; i++)
                yield return _items[i];
            for (int i = 0; i < _tail; i++)
                yield return _items[i];
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the queue.
    /// </summary>
    /// <returns>An enumerator for the queue.</returns>
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
                CopyTo(newItems, 0);

            _items = newItems;
            _head = 0;
            _tail = _count;
        }
    }
}