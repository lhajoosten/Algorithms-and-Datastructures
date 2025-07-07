using System.Collections;

namespace AlgorithmsDataStructures.DataStructures;

/// <summary>
/// Represents a key-value pair in the hash table.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class HashTableEntry<TKey, TValue>
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public TKey Key { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public TValue Value { get; set; }

    /// <summary>
    /// Gets or sets the next entry in the chain (for collision resolution).
    /// </summary>
    public HashTableEntry<TKey, TValue>? Next { get; set; }

    /// <summary>
    /// Initializes a new instance of the HashTableEntry class.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public HashTableEntry(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}

/// <summary>
/// A generic hash table implementation using separate chaining for collision resolution.
/// </summary>
/// <typeparam name="TKey">The type of keys in the hash table</typeparam>
/// <typeparam name="TValue">The type of values in the hash table</typeparam>
public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull
{
    private HashTableEntry<TKey, TValue>?[] _buckets;
    private int _count;
    private readonly IEqualityComparer<TKey> _comparer;
    private const int DefaultCapacity = 16;
    private const double LoadFactorThreshold = 0.75;

    /// <summary>
    /// Gets the number of key-value pairs in the hash table.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the hash table is empty.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Gets the current capacity of the hash table.
    /// </summary>
    public int Capacity => _buckets.Length;

    /// <summary>
    /// Gets the current load factor of the hash table.
    /// </summary>
    public double LoadFactor => (double)_count / _buckets.Length;

    /// <summary>
    /// Gets an enumerable collection of keys in the hash table.
    /// </summary>
    public IEnumerable<TKey> Keys
    {
        get
        {
            foreach (var bucket in _buckets)
            {
                var entry = bucket;
                while (entry != null)
                {
                    yield return entry.Key;
                    entry = entry.Next;
                }
            }
        }
    }

    /// <summary>
    /// Gets an enumerable collection of values in the hash table.
    /// </summary>
    public IEnumerable<TValue> Values
    {
        get
        {
            foreach (var bucket in _buckets)
            {
                var entry = bucket;
                while (entry != null)
                {
                    yield return entry.Value;
                    entry = entry.Next;
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>The value associated with the specified key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when getting a value and the key is not found.</exception>
    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue? value))
                return value;
            throw new KeyNotFoundException($"The key '{key}' was not found in the hash table.");
        }
        set => Put(key, value);
    }

    /// <summary>
    /// Initializes a new instance of the HashTable class with default capacity.
    /// </summary>
    /// <param name="comparer">The equality comparer for keys. If null, uses default comparer.</param>
    public HashTable(IEqualityComparer<TKey>? comparer = null) : this(DefaultCapacity, comparer) { }

    /// <summary>
    /// Initializes a new instance of the HashTable class with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the hash table.</param>
    /// <param name="comparer">The equality comparer for keys. If null, uses default comparer.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity is less than 1.</exception>
    public HashTable(int capacity, IEqualityComparer<TKey>? comparer = null)
    {
        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be at least 1.");

        _buckets = new HashTableEntry<TKey, TValue>[GetNextPowerOfTwo(capacity)];
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
        _count = 0;
    }

    /// <summary>
    /// Initializes a new instance of the HashTable class with elements from the specified collection.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new hash table.</param>
    /// <param name="comparer">The equality comparer for keys. If null, uses default comparer.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public HashTable(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer = null)
        : this(DefaultCapacity, comparer)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        foreach (var pair in collection)
            Put(pair.Key, pair.Value);
    }

    /// <summary>
    /// Adds or updates a key-value pair in the hash table.
    /// Time Complexity: O(1) average case, O(n) worst case.
    /// </summary>
    /// <param name="key">The key of the element to add or update.</param>
    /// <param name="value">The value of the element to add or update.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public void Put(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if (LoadFactor >= LoadFactorThreshold)
            Resize();

        int bucketIndex = GetBucketIndex(key);
        var entry = _buckets[bucketIndex];

        // Check if key already exists
        while (entry != null)
        {
            if (_comparer.Equals(entry.Key, key))
            {
                entry.Value = value; // Update existing value
                return;
            }
            entry = entry.Next;
        }

        // Add new entry at the beginning of the chain
        var newEntry = new HashTableEntry<TKey, TValue>(key, value)
        {
            Next = _buckets[bucketIndex]
        };
        _buckets[bucketIndex] = newEntry;
        _count++;
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// Time Complexity: O(1) average case, O(n) worst case.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <returns>The value associated with the specified key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
    public TValue Get(TKey key)
    {
        if (TryGetValue(key, out TValue? value))
            return value;
        throw new KeyNotFoundException($"The key '{key}' was not found in the hash table.");
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// Time Complexity: O(1) average case, O(n) worst case.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <param name="value">When this method returns, the value associated with the specified key, if found; otherwise, default value.</param>
    /// <returns>true if the hash table contains an element with the specified key; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int bucketIndex = GetBucketIndex(key);
        var entry = _buckets[bucketIndex];

        while (entry != null)
        {
            if (_comparer.Equals(entry.Key, key))
            {
                value = entry.Value;
                return true;
            }
            entry = entry.Next;
        }

        value = default(TValue)!;
        return false;
    }

    /// <summary>
    /// Determines whether the hash table contains the specified key.
    /// Time Complexity: O(1) average case, O(n) worst case.
    /// </summary>
    /// <param name="key">The key to locate in the hash table.</param>
    /// <returns>true if the hash table contains an element with the specified key; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public bool ContainsKey(TKey key)
    {
        return TryGetValue(key, out _);
    }

    /// <summary>
    /// Determines whether the hash table contains the specified value.
    /// Time Complexity: O(n).
    /// </summary>
    /// <param name="value">The value to locate in the hash table.</param>
    /// <returns>true if the hash table contains an element with the specified value; otherwise, false.</returns>
    public bool ContainsValue(TValue value)
    {
        var valueComparer = EqualityComparer<TValue>.Default;

        foreach (var bucket in _buckets)
        {
            var entry = bucket;
            while (entry != null)
            {
                if (valueComparer.Equals(entry.Value, value))
                    return true;
                entry = entry.Next;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes the element with the specified key from the hash table.
    /// Time Complexity: O(1) average case, O(n) worst case.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>true if the element is successfully removed; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int bucketIndex = GetBucketIndex(key);
        var entry = _buckets[bucketIndex];
        HashTableEntry<TKey, TValue>? previous = null;

        while (entry != null)
        {
            if (_comparer.Equals(entry.Key, key))
            {
                if (previous == null)
                {
                    _buckets[bucketIndex] = entry.Next;
                }
                else
                {
                    previous.Next = entry.Next;
                }
                _count--;
                return true;
            }
            previous = entry;
            entry = entry.Next;
        }

        return false;
    }

    /// <summary>
    /// Removes all keys and values from the hash table.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_buckets, 0, _buckets.Length);
        _count = 0;
    }

    /// <summary>
    /// Copies the hash table elements to an existing one-dimensional array.
    /// </summary>
    /// <param name="array">The array that is the destination of the elements copied from hash table.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arrayIndex is less than 0.</exception>
    /// <exception cref="ArgumentException">Thrown when the destination array is too small.</exception>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index cannot be negative.");
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Destination array is too small.");

        foreach (var pair in this)
        {
            array[arrayIndex++] = pair;
        }
    }

    /// <summary>
    /// Returns statistics about the hash table distribution.
    /// </summary>
    /// <returns>A string containing distribution statistics.</returns>
    public string GetStatistics()
    {
        int emptyBuckets = 0;
        int maxChainLength = 0;
        int totalChainLength = 0;
        var chainLengths = new List<int>();

        foreach (var bucket in _buckets)
        {
            int chainLength = 0;
            var entry = bucket;

            while (entry != null)
            {
                chainLength++;
                entry = entry.Next;
            }

            if (chainLength == 0)
                emptyBuckets++;
            else
                chainLengths.Add(chainLength);

            maxChainLength = Math.Max(maxChainLength, chainLength);
            totalChainLength += chainLength;
        }

        double averageChainLength = chainLengths.Count > 0 ? chainLengths.Average() : 0;

        return $"""
            Hash Table Statistics:
            =====================
            Total Buckets: {_buckets.Length}
            Used Buckets: {_buckets.Length - emptyBuckets}
            Empty Buckets: {emptyBuckets}
            Total Items: {_count}
            Load Factor: {LoadFactor:F3}
            Max Chain Length: {maxChainLength}
            Average Chain Length: {averageChainLength:F2}
            """;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the hash table.
    /// </summary>
    /// <returns>An enumerator for the hash table.</returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var bucket in _buckets)
        {
            var entry = bucket;
            while (entry != null)
            {
                yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                entry = entry.Next;
            }
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the hash table.
    /// </summary>
    /// <returns>An enumerator for the hash table.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int GetBucketIndex(TKey key)
    {
        int hashCode = _comparer.GetHashCode(key);
        return Math.Abs(hashCode) % _buckets.Length;
    }

    private void Resize()
    {
        var oldBuckets = _buckets;
        _buckets = new HashTableEntry<TKey, TValue>[_buckets.Length * 2];
        _count = 0;

        // Rehash all existing entries
        foreach (var bucket in oldBuckets)
        {
            var entry = bucket;
            while (entry != null)
            {
                Put(entry.Key, entry.Value);
                entry = entry.Next;
            }
        }
    }

    private static int GetNextPowerOfTwo(int value)
    {
        if (value <= 0)
            return 1;

        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        return value + 1;
    }
}