using AlgorithmsDataStructures.Algorithms.Sorting;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using SystemHashSet = System.Collections.Generic.HashSet<int>;
using SystemLinkedList = System.Collections.Generic.LinkedList<int>;
using SystemQueue = System.Collections.Generic.Queue<int>;
using SystemStack = System.Collections.Generic.Stack<int>;
using CustomStack = AlgorithmsDataStructures.DataStructures.Stack<int>;
using CustomQueue = AlgorithmsDataStructures.DataStructures.Queue<int>;
using CustomLinkedList = AlgorithmsDataStructures.DataStructures.LinkedList<int>;
using CustomHashTable = AlgorithmsDataStructures.DataStructures.HashTable<int, bool>;
using AlgorithmsDataStructures.Algorithms.Searching;

namespace AlgorithmsDataStructures.Benchmarking.Benchmarks;

[Config(typeof(Config))]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SortingBenchmarks
{
    private class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.Mean);
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.StdDev);
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.Median);
        }
    }

    private int[]? _randomData;
    private int[]? _sortedData;
    private int[]? _reverseSortedData;
    private int[]? _duplicatesData;

    [Params(100, 1000, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42); // Fixed seed for reproducible results

        // Random data
        _randomData = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _randomData[i] = random.Next(0, Size);
        }

        // Already sorted data (best case for some algorithms)
        _sortedData = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _sortedData[i] = i;
        }

        // Reverse sorted data (worst case for some algorithms)
        _reverseSortedData = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _reverseSortedData[i] = Size - i - 1;
        }

        // Data with many duplicates
        _duplicatesData = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _duplicatesData[i] = random.Next(0, Size / 10); // Many duplicates
        }
    }

    #region Random Data Benchmarks

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Random")]
    public void BubbleSort_Random()
    {
        var data = (int[])_randomData!.Clone();
        BubbleSorting.BubbleSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Random")]
    public void SelectionSort_Random()
    {
        var data = (int[])_randomData!.Clone();
        SelectionSorting.SelectionSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Random")]
    public void MergeSort_Random()
    {
        var data = (int[])_randomData!.Clone();
        MergeSorting.MergeSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Random")]
    public void QuickSort_Random()
    {
        var data = (int[])_randomData!.Clone();
        QuickSorting.QuickSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Random")]
    public void HeapSort_Random()
    {
        var data = (int[])_randomData!.Clone();
        HeapSorting.HeapSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Random")]
    public void SystemArraySort_Random()
    {
        var data = (int[])_randomData!.Clone();
        Array.Sort(data);
    }

    #endregion

    #region Sorted Data Benchmarks (Best Case)

    [Benchmark]
    [BenchmarkCategory("Sorted")]
    public void BubbleSort_Sorted()
    {
        var data = (int[])_sortedData!.Clone();
        BubbleSorting.BubbleSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Sorted")]
    public void MergeSort_Sorted()
    {
        var data = (int[])_sortedData!.Clone();
        MergeSorting.MergeSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Sorted")]
    public void QuickSort_Sorted()
    {
        var data = (int[])_sortedData!.Clone();
        QuickSorting.QuickSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Sorted")]
    public void HeapSort_Sorted()
    {
        var data = (int[])_sortedData!.Clone();
        HeapSorting.HeapSort(data);
    }

    #endregion

    #region Reverse Sorted Data Benchmarks (Worst Case)

    [Benchmark]
    [BenchmarkCategory("ReverseSorted")]
    public void BubbleSort_ReverseSorted()
    {
        var data = (int[])_reverseSortedData!.Clone();
        BubbleSorting.BubbleSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("ReverseSorted")]
    public void MergeSort_ReverseSorted()
    {
        var data = (int[])_reverseSortedData!.Clone();
        MergeSorting.MergeSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("ReverseSorted")]
    public void QuickSort_ReverseSorted()
    {
        var data = (int[])_reverseSortedData!.Clone();
        QuickSorting.QuickSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("ReverseSorted")]
    public void HeapSort_ReverseSorted()
    {
        var data = (int[])_reverseSortedData!.Clone();
        HeapSorting.HeapSort(data);
    }

    #endregion

    #region Duplicates Data Benchmarks

    [Benchmark]
    [BenchmarkCategory("Duplicates")]
    public void MergeSort_Duplicates()
    {
        var data = (int[])_duplicatesData!.Clone();
        MergeSorting.MergeSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Duplicates")]
    public void QuickSort_Duplicates()
    {
        var data = (int[])_duplicatesData!.Clone();
        QuickSorting.QuickSort(data);
    }

    [Benchmark]
    [BenchmarkCategory("Duplicates")]
    public void HeapSort_Duplicates()
    {
        var data = (int[])_duplicatesData!.Clone();
        HeapSorting.HeapSort(data);
    }

    #endregion
}

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DataStructureBenchmarks
{
    private const int OperationCount = 10000;
    private int[] _testData = null!;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _testData = new int[OperationCount];
        for (int i = 0; i < OperationCount; i++)
        {
            _testData[i] = random.Next();
        }
    }

    [Benchmark(Baseline = true)]
    public void SystemStack()
    {
        var stack = new SystemStack();

        // Push operations
        foreach (var item in _testData)
        {
            stack.Push(item);
        }

        // Pop operations
        while (stack.Count > 0)
        {
            stack.Pop();
        }
    }

    [Benchmark]
    public void CustomStack()
    {
        var stack = new CustomStack();

        // Push operations
        foreach (var item in _testData)
        {
            stack.Push(item);
        }

        // Pop operations
        while (!stack.IsEmpty)
        {
            stack.Pop();
        }
    }

    [Benchmark]
    public void SystemQueue()
    {
        var queue = new SystemQueue();

        // Enqueue operations
        foreach (var item in _testData)
        {
            queue.Enqueue(item);
        }

        // Dequeue operations
        while (queue.Count > 0)
        {
            queue.Dequeue();
        }
    }

    [Benchmark]
    public void CustomQueue()
    {
        var queue = new CustomQueue();

        // Enqueue operations
        foreach (var item in _testData)
        {
            queue.Enqueue(item);
        }

        // Dequeue operations
        while (!queue.IsEmpty)
        {
            queue.Dequeue();
        }
    }

    [Benchmark]
    public void SystemLinkedList()
    {
        var list = new SystemLinkedList();

        // Add operations
        foreach (var item in _testData)
        {
            list.AddLast(item);
        }

        // Remove operations
        while (list.Count > 0)
        {
            list.RemoveFirst();
        }
    }

    [Benchmark]
    public void CustomLinkedList()
    {
        var list = new CustomLinkedList();

        // Add operations
        foreach (var item in _testData)
        {
            list.AddLast(item);
        }

        // Remove operations
        while (!list.IsEmpty)
        {
            list.RemoveFirst();
        }
    }

    [Benchmark]
    public void SystemHashSet()
    {
        var hashSet = new SystemHashSet();

        // Add operations
        foreach (var item in _testData)
        {
            hashSet.Add(item);
        }

        // Contains operations
        foreach (var item in _testData)
        {
            hashSet.Contains(item);
        }
    }

    [Benchmark]
    public void CustomHashTable()
    {
        var hashTable = new CustomHashTable();

        // Put operations
        foreach (var item in _testData)
        {
            hashTable.Put(item, true);
        }

        // Contains operations
        foreach (var item in _testData)
        {
            hashTable.ContainsKey(item);
        }
    }
}

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SearchBenchmarks
{
    private int[] _sortedArray = null!;
    private int[] _searchTargets = null!;

    [Params(1000, 10000, 100000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // Create sorted array
        _sortedArray = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _sortedArray[i] = i * 2; // Even numbers to ensure some targets won't be found
        }

        // Create search targets (mix of existing and non-existing values)
        var random = new Random(42);
        _searchTargets = new int[1000];
        for (int i = 0; i < 1000; i++)
        {
            _searchTargets[i] = random.Next(0, Size * 2);
        }
    }

    [Benchmark(Baseline = true)]
    public void LinearSearch()
    {
        int found = 0;
        foreach (var target in _searchTargets)
        {
            if (LinearSearchAlgorithm.LinearSearch(_sortedArray, target) >= 0)
                found++;
        }
    }

    [Benchmark]
    public void BinarySearch()
    {
        int found = 0;
        foreach (var target in _searchTargets)
        {
            if (BinarySearchAlgorithm.BinarySearch(_sortedArray, target) >= 0)
                found++;
        }
    }

    [Benchmark]
    public void BinarySearchRecursive()
    {
        int found = 0;
        foreach (var target in _searchTargets)
        {
            if (RecursiveBinarySearchAlgorithm.BinarySearchRecursive(_sortedArray, target) >= 0)
                found++;
        }
    }

    [Benchmark]
    public void SystemArrayBinarySearch()
    {
        int found = 0;
        foreach (var target in _searchTargets)
        {
            if (Array.BinarySearch(_sortedArray, target) >= 0)
                found++;
        }
    }

    [Benchmark]
    public void ExponentialSearch()
    {
        int found = 0;
        foreach (var target in _searchTargets)
        {
            if (ExponentialSearchAlgorithm.ExponentialSearch(_sortedArray, target) >= 0)
                found++;
        }
    }
}