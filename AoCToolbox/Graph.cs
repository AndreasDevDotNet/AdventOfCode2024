using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    private readonly Dictionary<string, HashSet<string>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<string, HashSet<string>>();
    }

    public void AddVertex(string vertex)
    {
        if (!adjacencyList.ContainsKey(vertex))
        {
            adjacencyList[vertex] = new HashSet<string>();
        }
    }

    public void AddEdge(string v1, string v2)
    {
        // Add vertices if they don't exist
        AddVertex(v1);
        AddVertex(v2);

        // Add edges in both directions
        adjacencyList[v1].Add(v2);
        adjacencyList[v2].Add(v1);
    }

    public List<HashSet<string>> FindMaximalCliques()
    {
        var result = new List<HashSet<string>>();
        var r = new HashSet<string>();                   // Current clique being built
        var p = new HashSet<string>(adjacencyList.Keys); // Remaining vertices to consider
        var x = new HashSet<string>();                   // Excluded vertices

        BronKerbosch(r, p, x, result);
        return result;
    }

    public List<HashSet<string>> FindAllCliques()
    {
        var allCliques = new List<HashSet<string>>();
        var maximalCliques = FindMaximalCliques();

        foreach (var maximalClique in maximalCliques)
        {
            // Generate all possible subsets of the maximal clique
            GenerateSubsets(maximalClique, allCliques);
        }

        // Remove duplicates and sort by size
        return allCliques
            .Distinct(new HashSetComparer<string>())
            .OrderBy(clique => clique.Count)
            .ToList();
    }

    private void GenerateSubsets(HashSet<string> clique, List<HashSet<string>> allCliques)
    {
        var elements = clique.ToList();
        var n = elements.Count;

        // Generate all possible combinations using binary counting
        // We start from 1 to exclude empty set
        for (int i = 1; i < (1 << n); i++)
        {
            var subset = new HashSet<string>();
            for (int j = 0; j < n; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    subset.Add(elements[j]);
                }
            }
            // Only add if it's a valid clique (all vertices are connected)
            if (IsClique(subset))
            {
                allCliques.Add(subset);
            }
        }
    }

    private bool IsClique(HashSet<string> vertices)
    {
        foreach (var vertex in vertices)
        {
            foreach (var otherVertex in vertices)
            {
                if (vertex != otherVertex && !adjacencyList[vertex].Contains(otherVertex))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private readonly Dictionary<(HashSet<string>, HashSet<string>, HashSet<string>), List<HashSet<string>>> memo = new();

    private void BronKerbosch(HashSet<string> r, HashSet<string> p, HashSet<string> x, List<HashSet<string>> result)
    {
        var key = (new HashSet<string>(r), new HashSet<string>(p), new HashSet<string>(x));
        if (memo.TryGetValue(key, out var cachedResult))
        {
            result.AddRange(cachedResult);
            return;
        }

        if (p.Count == 0 && x.Count == 0)
        {
            if (r.Count > 0)
            {
                result.Add(new HashSet<string>(r));
            }
            memo[key] = new List<HashSet<string>>(result);
            return;
        }

        var pivot = ChoosePivot(p, x);
        var candidates = new HashSet<string>(p);

        if (pivot != null)
        {
            candidates.ExceptWith(adjacencyList[pivot]);
        }

        foreach (var v in candidates.ToList())
        {
            var neighbors = adjacencyList[v];
            r.Add(v);
            BronKerbosch(r,
                        new HashSet<string>(p.Intersect(neighbors)),
                        new HashSet<string>(x.Intersect(neighbors)),
                        result);
            r.Remove(v);

            p.Remove(v);
            x.Add(v);
        }

        memo[key] = new List<HashSet<string>>(result);
    }

    private string ChoosePivot(HashSet<string> p, HashSet<string> x)
    {
        if (p.Count == 0 && x.Count == 0)
            return null;

        var unionSet = new HashSet<string>(p);
        unionSet.UnionWith(x);

        int maxDegree = -1;
        string pivot = null;

        foreach (var v in unionSet)
        {
            int degree = adjacencyList[v].Count(n => p.Contains(n));
            if (degree > maxDegree)
            {
                maxDegree = degree;
                pivot = v;
            }
        }

        return pivot;
    }
}

// Helper class to compare HashSets for equality
public class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
{
    public bool Equals(HashSet<T> x, HashSet<T> y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<T> obj)
    {
        int hash = 0;
        foreach (var item in obj.OrderBy(x => x))
        {
            hash = hash * 31 + item.GetHashCode();
        }
        return hash;
    }
}