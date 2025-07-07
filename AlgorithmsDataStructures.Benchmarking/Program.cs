using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using AlgorithmsDataStructures.Benchmarking.Benchmarks;

namespace AlgorithmsDataStructures.Benchmarking;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("🧮 Algorithms & Data Structures Benchmarks");
        Console.WriteLine("==========================================");
        Console.WriteLine();

        if (args.Length == 0)
        {
            ShowMenu();
            return;
        }

        // Command line argument processing
        switch (args[0].ToLower())
        {
            case "sorting":
            RunSortingBenchmarks();
            break;
            case "datastructures":
            RunDataStructureBenchmarks();
            break;
            case "search":
            RunSearchBenchmarks();
            break;
            case "all":
            RunAllBenchmarks();
            break;
            default:
            Console.WriteLine($"Unknown benchmark: {args[0]}");
            ShowMenu();
            break;
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Available benchmarks:");
        Console.WriteLine("  sorting         - Run sorting algorithm benchmarks");
        Console.WriteLine("  datastructures  - Run data structure benchmarks");
        Console.WriteLine("  search          - Run search algorithm benchmarks");
        Console.WriteLine("  all             - Run all benchmarks");
        Console.WriteLine();
        Console.WriteLine("Usage: dotnet run [benchmark_name]");
        Console.WriteLine("Example: dotnet run sorting");
        Console.WriteLine();
        Console.WriteLine("Press any key to run interactive mode...");
        Console.ReadKey();
        Console.WriteLine();

        RunInteractiveMode();
    }

    private static void RunInteractiveMode()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Select a benchmark to run:");
            Console.WriteLine("1. Sorting Algorithms");
            Console.WriteLine("2. Data Structures");
            Console.WriteLine("3. Search Algorithms");
            Console.WriteLine("4. All Benchmarks");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                RunSortingBenchmarks();
                break;
                case "2":
                RunDataStructureBenchmarks();
                break;
                case "3":
                RunSearchBenchmarks();
                break;
                case "4":
                RunAllBenchmarks();
                break;
                case "5":
                Console.WriteLine("Goodbye! 👋");
                return;
                default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
            }
        }
    }

    private static void RunSortingBenchmarks()
    {
        Console.WriteLine("🔄 Running Sorting Algorithm Benchmarks...");
        Console.WriteLine("This will test various sorting algorithms with different data patterns.");
        Console.WriteLine();

        try
        {
            BenchmarkRunner.Run<SortingBenchmarks>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running sorting benchmarks: {ex.Message}");
        }
    }

    private static void RunDataStructureBenchmarks()
    {
        Console.WriteLine("🏗️ Running Data Structure Benchmarks...");
        Console.WriteLine("This will compare custom implementations with system collections.");
        Console.WriteLine();

        try
        {
            BenchmarkRunner.Run<DataStructureBenchmarks>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running data structure benchmarks: {ex.Message}");
        }
    }

    private static void RunSearchBenchmarks()
    {
        Console.WriteLine("🔍 Running Search Algorithm Benchmarks...");
        Console.WriteLine("This will test various search algorithms on different array sizes.");
        Console.WriteLine();

        try
        {
            BenchmarkRunner.Run<SearchBenchmarks>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running search benchmarks: {ex.Message}");
        }
    }

    private static void RunAllBenchmarks()
    {
        Console.WriteLine("🚀 Running All Benchmarks...");
        Console.WriteLine("This will take a while - grab some coffee! ☕");
        Console.WriteLine();

        try
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .WithOptions(ConfigOptions.JoinSummary);

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args: new[] { "*" }, config);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running all benchmarks: {ex.Message}");
        }
    }
}