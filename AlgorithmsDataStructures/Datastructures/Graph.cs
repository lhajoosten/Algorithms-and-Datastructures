using AlgorithmsDataStructures.DataStructures;
using System.Collections;

namespace AlgorithmsDataStructures.Datastructures;

/// <summary>
/// Represents an edge in a graph.
/// </summary>
/// <typeparam name="T">The type of vertex values.</typeparam>
public class GraphEdge<T>
{
    /// <summary>
    /// Gets the source vertex of the edge.
    /// </summary>
    public T From { get; }

    /// <summary>
    /// Gets the destination vertex of the edge.
    /// </summary>
    public T To { get; }

    /// <summary>
    /// Gets the weight of the edge.
    /// </summary>
    public double Weight { get; }

    /// <summary>
    /// Initializes a new instance of the GraphEdge class.
    /// </summary>
    /// <param name="from">The source vertex.</param>
    /// <param name="to">The destination vertex.</param>
    /// <param name="weight">The weight of the edge. Default is 1.0.</param>
    public GraphEdge(T from, T to, double weight = 1.0)
    {
        From = from;
        To = to;
        Weight = weight;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{From} -> {To} (Weight: {Weight})";
}

/// <summary>
/// A generic graph implementation using adjacency list representation.
/// </summary>
/// <typeparam name="T">The type of vertex values</typeparam>
public class Graph<T> : IEnumerable<T> where T : notnull
{
    private readonly Dictionary<T, List<GraphEdge<T>>> _adjacencyList;
    private readonly bool _isDirected;
    private readonly IEqualityComparer<T> _comparer;

    /// <summary>
    /// Gets the number of vertices in the graph.
    /// </summary>
    public int VertexCount => _adjacencyList.Count;

    /// <summary>
    /// Gets the total number of edges in the graph.
    /// </summary>
    public int EdgeCount
    {
        get
        {
            int count = _adjacencyList.Values.Sum(list => list.Count);
            return _isDirected ? count : count / 2;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the graph is directed.
    /// </summary>
    public bool IsDirected => _isDirected;

    /// <summary>
    /// Gets all vertices in the graph.
    /// </summary>
    public IEnumerable<T> Vertices => _adjacencyList.Keys;

    /// <summary>
    /// Gets all edges in the graph.
    /// </summary>
    public IEnumerable<GraphEdge<T>> Edges
    {
        get
        {
            var edges = new HashSet<GraphEdge<T>>();
            foreach (var vertex in _adjacencyList.Keys)
            {
                foreach (var edge in _adjacencyList[vertex])
                {
                    if (_isDirected)
                    {
                        edges.Add(edge);
                    }
                    else
                    {
                        // For undirected graphs, only add each edge once
                        var reverseEdge = new GraphEdge<T>(edge.To, edge.From, edge.Weight);
                        if (!edges.Contains(reverseEdge))
                            edges.Add(edge);
                    }
                }
            }
            return edges;
        }
    }

    /// <summary>
    /// Initializes a new instance of the Graph class.
    /// </summary>
    /// <param name="isDirected">true for directed graph; false for undirected graph.</param>
    /// <param name="comparer">The equality comparer for vertices. If null, uses default comparer.</param>
    public Graph(bool isDirected = false, IEqualityComparer<T>? comparer = null)
    {
        _isDirected = isDirected;
        _comparer = comparer ?? EqualityComparer<T>.Default;
        _adjacencyList = new Dictionary<T, List<GraphEdge<T>>>(_comparer);
    }

    /// <summary>
    /// Adds a vertex to the graph.
    /// </summary>
    /// <param name="vertex">The vertex to add.</param>
    /// <returns>true if the vertex was added; false if it already exists.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    public bool AddVertex(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));

        if (_adjacencyList.ContainsKey(vertex))
            return false;

        _adjacencyList[vertex] = new List<GraphEdge<T>>();
        return true;
    }

    /// <summary>
    /// Removes a vertex and all its edges from the graph.
    /// </summary>
    /// <param name="vertex">The vertex to remove.</param>
    /// <returns>true if the vertex was removed; false if it was not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    public bool RemoveVertex(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));

        if (!_adjacencyList.ContainsKey(vertex))
            return false;

        // Remove all edges pointing to this vertex
        foreach (var adjacentList in _adjacencyList.Values)
        {
            adjacentList.RemoveAll(edge => _comparer.Equals(edge.To, vertex));
        }

        // Remove the vertex itself
        _adjacencyList.Remove(vertex);
        return true;
    }

    /// <summary>
    /// Adds an edge between two vertices.
    /// </summary>
    /// <param name="from">The source vertex.</param>
    /// <param name="to">The destination vertex.</param>
    /// <param name="weight">The weight of the edge. Default is 1.0.</param>
    /// <returns>true if the edge was added; false if it already exists.</returns>
    /// <exception cref="ArgumentNullException">Thrown when from or to is null.</exception>
    /// <exception cref="ArgumentException">Thrown when from or to vertex doesn't exist in the graph.</exception>
    public bool AddEdge(T from, T to, double weight = 1.0)
    {
        if (from == null)
            throw new ArgumentNullException(nameof(from));
        if (to == null)
            throw new ArgumentNullException(nameof(to));

        // Ensure both vertices exist
        if (!_adjacencyList.ContainsKey(from))
            AddVertex(from);
        if (!_adjacencyList.ContainsKey(to))
            AddVertex(to);

        // Check if edge already exists
        if (_adjacencyList[from].Any(edge => _comparer.Equals(edge.To, to)))
            return false;

        // Add the edge
        _adjacencyList[from].Add(new GraphEdge<T>(from, to, weight));

        // If undirected, add the reverse edge
        if (!_isDirected && !_comparer.Equals(from, to))
        {
            _adjacencyList[to].Add(new GraphEdge<T>(to, from, weight));
        }

        return true;
    }

    /// <summary>
    /// Removes an edge between two vertices.
    /// </summary>
    /// <param name="from">The source vertex.</param>
    /// <param name="to">The destination vertex.</param>
    /// <returns>true if the edge was removed; false if it was not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when from or to is null.</exception>
    public bool RemoveEdge(T from, T to)
    {
        if (from == null)
            throw new ArgumentNullException(nameof(from));
        if (to == null)
            throw new ArgumentNullException(nameof(to));

        if (!_adjacencyList.ContainsKey(from))
            return false;

        bool removed = _adjacencyList[from].RemoveAll(edge => _comparer.Equals(edge.To, to)) > 0;

        // If undirected, remove the reverse edge
        if (!_isDirected && _adjacencyList.ContainsKey(to))
        {
            _adjacencyList[to].RemoveAll(edge => _comparer.Equals(edge.To, from));
        }

        return removed;
    }

    /// <summary>
    /// Checks if there is an edge between two vertices.
    /// </summary>
    /// <param name="from">The source vertex.</param>
    /// <param name="to">The destination vertex.</param>
    /// <returns>true if an edge exists; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when from or to is null.</exception>
    public bool HasEdge(T from, T to)
    {
        if (from == null)
            throw new ArgumentNullException(nameof(from));
        if (to == null)
            throw new ArgumentNullException(nameof(to));

        return _adjacencyList.ContainsKey(from) &&
               _adjacencyList[from].Any(edge => _comparer.Equals(edge.To, to));
    }

    /// <summary>
    /// Checks if a vertex exists in the graph.
    /// </summary>
    /// <param name="vertex">The vertex to check.</param>
    /// <returns>true if the vertex exists; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    public bool ContainsVertex(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));

        return _adjacencyList.ContainsKey(vertex);
    }

    /// <summary>
    /// Gets the adjacent vertices of a given vertex.
    /// </summary>
    /// <param name="vertex">The vertex to get neighbors for.</param>
    /// <returns>An enumerable of adjacent vertices.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when vertex doesn't exist in the graph.</exception>
    public IEnumerable<T> GetNeighbors(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException($"Vertex {vertex} does not exist in the graph.");

        return _adjacencyList[vertex].Select(edge => edge.To);
    }

    /// <summary>
    /// Gets the edges from a given vertex.
    /// </summary>
    /// <param name="vertex">The vertex to get edges for.</param>
    /// <returns>An enumerable of edges from the vertex.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when vertex doesn't exist in the graph.</exception>
    public IEnumerable<GraphEdge<T>> GetEdges(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException($"Vertex {vertex} does not exist in the graph.");

        return _adjacencyList[vertex];
    }

    /// <summary>
    /// Gets the degree of a vertex (number of edges connected to it).
    /// </summary>
    /// <param name="vertex">The vertex to get degree for.</param>
    /// <returns>The degree of the vertex.</returns>
    /// <exception cref="ArgumentNullException">Thrown when vertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when vertex doesn't exist in the graph.</exception>
    public int GetDegree(T vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException($"Vertex {vertex} does not exist in the graph.");

        if (_isDirected)
        {
            // For directed graphs, return out-degree
            return _adjacencyList[vertex].Count;
        }
        else
        {
            // For undirected graphs, count all connections
            return _adjacencyList[vertex].Count;
        }
    }

    /// <summary>
    /// Performs a breadth-first search starting from the specified vertex.
    /// Time Complexity: O(V + E) where V is vertices and E is edges.
    /// </summary>
    /// <param name="startVertex">The vertex to start the search from.</param>
    /// <returns>An enumerable of vertices in BFS order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when startVertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when startVertex doesn't exist in the graph.</exception>
    public IEnumerable<T> BreadthFirstSearch(T startVertex)
    {
        if (startVertex == null)
            throw new ArgumentNullException(nameof(startVertex));
        if (!_adjacencyList.ContainsKey(startVertex))
            throw new ArgumentException($"Start vertex {startVertex} does not exist in the graph.");

        var visited = new HashSet<T>(_comparer);
        var queue = new System.Collections.Generic.Queue<T>();

        queue.Enqueue(startVertex);
        visited.Add(startVertex);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    /// <summary>
    /// Performs a depth-first search starting from the specified vertex.
    /// Time Complexity: O(V + E) where V is vertices and E is edges.
    /// </summary>
    /// <param name="startVertex">The vertex to start the search from.</param>
    /// <returns>An enumerable of vertices in DFS order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when startVertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when startVertex doesn't exist in the graph.</exception>
    public IEnumerable<T> DepthFirstSearch(T startVertex)
    {
        if (startVertex == null)
            throw new ArgumentNullException(nameof(startVertex));
        if (!_adjacencyList.ContainsKey(startVertex))
            throw new ArgumentException($"Start vertex {startVertex} does not exist in the graph.");

        var visited = new HashSet<T>(_comparer);
        return DepthFirstSearchHelper(startVertex, visited);
    }

    private IEnumerable<T> DepthFirstSearchHelper(T vertex, HashSet<T> visited)
    {
        visited.Add(vertex);
        yield return vertex;

        foreach (var neighbor in GetNeighbors(vertex))
        {
            if (!visited.Contains(neighbor))
            {
                foreach (var result in DepthFirstSearchHelper(neighbor, visited))
                    yield return result;
            }
        }
    }

    /// <summary>
    /// Performs an iterative depth-first search using a stack.
    /// Time Complexity: O(V + E) where V is vertices and E is edges.
    /// </summary>
    /// <param name="startVertex">The vertex to start the search from.</param>
    /// <returns>An enumerable of vertices in DFS order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when startVertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown when startVertex doesn't exist in the graph.</exception>
    public IEnumerable<T> DepthFirstSearchIterative(T startVertex)
    {
        if (startVertex == null)
            throw new ArgumentNullException(nameof(startVertex));
        if (!_adjacencyList.ContainsKey(startVertex))
            throw new ArgumentException($"Start vertex {startVertex} does not exist in the graph.");

        var visited = new HashSet<T>(_comparer);
        var stack = new System.Collections.Generic.Stack<T>();

        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (!visited.Contains(current))
            {
                visited.Add(current);
                yield return current;

                // Push neighbors in reverse order to maintain consistent ordering
                var neighbors = GetNeighbors(current).Reverse();
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
                }
            }
        }
    }

    /// <summary>
    /// Finds a path between two vertices using BFS.
    /// Time Complexity: O(V + E).
    /// </summary>
    /// <param name="start">The starting vertex.</param>
    /// <param name="end">The ending vertex.</param>
    /// <returns>A list representing the path from start to end, or null if no path exists.</returns>
    /// <exception cref="ArgumentNullException">Thrown when start or end is null.</exception>
    /// <exception cref="ArgumentException">Thrown when start or end vertex doesn't exist in the graph.</exception>
    public List<T>? FindPath(T start, T end)
    {
        if (start == null)
            throw new ArgumentNullException(nameof(start));
        if (end == null)
            throw new ArgumentNullException(nameof(end));
        if (!_adjacencyList.ContainsKey(start))
            throw new ArgumentException($"Start vertex {start} does not exist in the graph.");
        if (!_adjacencyList.ContainsKey(end))
            throw new ArgumentException($"End vertex {end} does not exist in the graph.");

        if (_comparer.Equals(start, end))
            return new List<T> { start };

        var visited = new HashSet<T>(_comparer);
        var queue = new System.Collections.Generic.Queue<T>();
        var parent = new Dictionary<T, T>(_comparer);

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    parent[neighbor] = current;
                    queue.Enqueue(neighbor);

                    if (_comparer.Equals(neighbor, end))
                    {
                        // Reconstruct path
                        var path = new List<T>();
                        var pathNode = end;
                        while (!_comparer.Equals(pathNode, start))
                        {
                            path.Add(pathNode);
                            pathNode = parent[pathNode];
                        }
                        path.Add(start);
                        path.Reverse();
                        return path;
                    }
                }
            }
        }

        return null; // No path found
    }

    /// <summary>
    /// Checks if the graph is connected (for undirected graphs) or strongly connected (for directed graphs).
    /// Time Complexity: O(V + E).
    /// </summary>
    /// <returns>true if the graph is connected; otherwise, false.</returns>
    public bool IsConnected()
    {
        if (VertexCount == 0)
            return true;

        var firstVertex = _adjacencyList.Keys.First();
        var reachableCount = BreadthFirstSearch(firstVertex).Count();

        return reachableCount == VertexCount;
    }

    /// <summary>
    /// Detects if the graph contains a cycle.
    /// Time Complexity: O(V + E).
    /// </summary>
    /// <returns>true if the graph contains a cycle; otherwise, false.</returns>
    public bool HasCycle()
    {
        var visited = new HashSet<T>(_comparer);
        var recursionStack = new HashSet<T>(_comparer);

        foreach (var vertex in _adjacencyList.Keys)
        {
            if (!visited.Contains(vertex))
            {
                if (HasCycleHelper(vertex, visited, recursionStack))
                    return true;
            }
        }

        return false;
    }

    private bool HasCycleHelper(T vertex, HashSet<T> visited, HashSet<T> recursionStack)
    {
        visited.Add(vertex);
        recursionStack.Add(vertex);

        foreach (var neighbor in GetNeighbors(vertex))
        {
            if (!visited.Contains(neighbor))
            {
                if (HasCycleHelper(neighbor, visited, recursionStack))
                    return true;
            }
            else if (recursionStack.Contains(neighbor))
            {
                return true; // Back edge found, cycle detected
            }
        }

        recursionStack.Remove(vertex);
        return false;
    }

    /// <summary>
    /// Removes all vertices and edges from the graph.
    /// </summary>
    public void Clear()
    {
        _adjacencyList.Clear();
    }

    /// <summary>
    /// Returns an enumerator that iterates through all vertices in the graph.
    /// </summary>
    /// <returns>An enumerator for the vertices.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _adjacencyList.Keys.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through all vertices in the graph.
    /// </summary>
    /// <returns>An enumerator for the vertices.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public override string ToString()
    {
        if (VertexCount == 0)
            return "Empty graph";

        var result = new System.Text.StringBuilder();
        result.AppendLine($"Graph ({(_isDirected ? "Directed" : "Undirected")}):");
        result.AppendLine($"Vertices: {VertexCount}, Edges: {EdgeCount}");

        foreach (var vertex in _adjacencyList.Keys)
        {
            var edges = string.Join(", ", _adjacencyList[vertex].Select(e => $"{e.To}({e.Weight})"));
            result.AppendLine($"{vertex}: [{edges}]");
        }

        return result.ToString();
    }
}