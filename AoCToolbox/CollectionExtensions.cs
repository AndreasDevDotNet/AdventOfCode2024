using System.Collections.Immutable;

namespace AoCToolbox;

public static class CollectionExtensions
{
    /// <summary>
    /// Attempt to remove all specified <see cref="keys"/> from the <see cref="Dictionary{TKey,TValue}"/> instance
    /// </summary>
    public static void RemoveMany<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        where TKey : notnull
    {
        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    /// Filter the dictionary entries using a Value Predicate
    /// </summary>
    public static Dictionary<TKey, TValue> WhereValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        Predicate<TValue> predicate) where TKey : notnull
    {
        return dictionary
            .Where(kvp => predicate(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    /// <summary>
    /// Initialize a <see cref="IReadOnlyCollection{T}"/> using the elements from <paramref name="source"/>
    /// </summary>
    public static IReadOnlyCollection<T> Freeze<T>(this IEnumerable<T> source)
    {
        return new List<T>(source);
    }
    
    /// <summary>
    /// Remove a single element from <paramref name="source"/>, if it exists
    /// </summary>
    public static ICollection<T> Except<T>(this IEnumerable<T> source, T single)
    {
        return new List<T>(collection: source.Except(new[] { single }));
    }

    /// <summary>
    /// Find the intersection of multiple collections
    /// </summary>
    /// <param name="collections">The collections to intersect</param>
    /// <returns>A set containing the elements which are present in all provided collections</returns>
    public static ISet<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> collections)
    {
        var enumerable = collections.ToList();
        var presentInAll = enumerable
            .Skip(1)
            .Aggregate(
                seed: new HashSet<T>(collection: enumerable.First()),
                func: (inAll, nextCollection) =>
                {
                    inAll.IntersectWith(nextCollection);
                    return inAll;
                }
            );

        return presentInAll;
    } 

    /// <summary>
    /// Normalize the values of the collection such that the "smallest" becomes <see cref="Vector2D.Zero"/>
    /// </summary>
    public static IEnumerable<Vector2D> Normalize(this IEnumerable<Vector2D> collection)
    {
        var enumerated = collection.ToList();
        var dx = int.MaxValue;
        var dy = int.MaxValue;

        foreach (var vector in enumerated)
        {
            dx = Math.Min(dx, vector.X);
            dy = Math.Min(dy, vector.Y);
        }

        var delta = new Vector2D(dx, dy);
        foreach (var vector in enumerated)
        {
            yield return vector - delta;
        }
    }

    /// <summary>
    /// Return the mode of the collection, i.e. the element with the highest frequency, 
    /// </summary>
    public static T Mode<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection
            .GroupBy(e => e)
            .MaxBy(g => g.Count())!.Key;
    }
    
    /// <summary>
    /// Chunk the source based on a <paramref name="takePredicate"/> and an optional <paramref name="skipPredicate"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="takePredicate">Select which elements should be included in a chunk</param>
    /// <param name="skipPredicate">Select which elements to skip between chunks, if not provided
    /// then <paramref name="takePredicate"/> is used and negated</param>
    /// <typeparam name="T">The type associated with each element in the source</typeparam>
    /// <returns></returns>
    public static IEnumerable<T[]> ChunkBy<T>(this IEnumerable<T> source, Predicate<T> takePredicate,
        Predicate<T>? skipPredicate = null)
    {
        var chunks = new List<T[]>();
        var enumerable = source as IList<T> ?? source.ToArray();
        
        for (var i = 0; i < enumerable.Count;)
        {
            var chunk = enumerable
                .Skip(i)
                .TakeWhile(takePredicate.Invoke)
                .ToArray();

            if (chunk.Length > 0)
            {
                chunks.Add(chunk);   
            }
            
            i += chunk.Length;
            i += enumerable
                .Skip(i)
                .TakeWhile(element => skipPredicate?.Invoke(element) ?? !takePredicate.Invoke(element))
                .Count();
        }

        return chunks;
    }

    /// <summary>
    /// Returns all permutations of the source sequence elements
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> elements)
    {
        var enumerated = elements.ToArray();
        if (enumerated.Length == 0)
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }

        var startingElementIndex = 0;
        foreach (var startingElement in enumerated)
        {
            var index = startingElementIndex;
            var remainingItems = enumerated.Where((_, i) => i != index);

            foreach (var permutationOfRemainder in remainingItems.Permute())
            {
                yield return permutationOfRemainder.Prepend(startingElement);
            }

            startingElementIndex++;
        }
    }

    /// <summary>
    /// Return all combinations of the source sequence elements of size <paramref name="k"/>
    /// </summary>
    /// <param name="elements">The source sequence</param>
    /// <param name="k">The number of elements in each returned combination</param>
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
    {
        if (k < 0)
        {
            throw new InvalidOperationException();
        }

        if (elements == null)
        {
            throw new ArgumentNullException(nameof(elements));
        }

        var enumerated = elements as T[] ?? elements.ToArray();
        return 
            from combination in Combinations(n: enumerated.Length, k)
            select enumerated.ZipWhere(combination);
    }
    
    /// <summary>
    /// Takes two numbers non-negative numbers, <paramref name="n"/> and <paramref name="k"/>, and
    /// produces all sequences of n bits with k true bits and n-k false bits.
    /// </summary>
    /// <param name="n">The total number of bits in each sequence</param>
    /// <param name="k">The number of set bits in each sequence</param>
    /// <returns></returns>
    private static IEnumerable<ImmutableStack<bool>> Combinations(int n, int k)
    {
        if (k == 0 && n == 0)
        {
            yield return ImmutableStack<bool>.Empty;
            yield break;
        }
        
        if (n < k)
        {
            yield break;   
        }

        //  First produce all the sequences that start with true,
        //  and then all the sequences that start with false.
        //
        //  At least one of n or k is not zero, and 0 <= k <= n,
        //  therefore n is not zero. But k could be.
        //
        if (k > 0)
        {
            foreach (var r in Combinations(n: n - 1, k: k - 1))
            {
                yield return r.Push(true);
            }
        }

        foreach (var r in Combinations(n: n - 1, k: k))
        {
            yield return r.Push(false);
        }
    }
    
    private static IEnumerable<T> ZipWhere<T>(this IEnumerable<T> items, IEnumerable<bool> selectors)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }
            
        if (selectors == null)
        {
            throw new ArgumentNullException(nameof(selectors));
        }

        return ZipWhereIterator(items, selectors);
    }
    
    private static IEnumerable<T> ZipWhereIterator<T>(IEnumerable<T> items, IEnumerable<bool> selectors)
    {
        using var itemEnumerator = items.GetEnumerator();
        using var selectorEnumerator = selectors.GetEnumerator();

        while (itemEnumerator.MoveNext() && selectorEnumerator.MoveNext())
        {
            if (selectorEnumerator.Current)
            {
                yield return itemEnumerator.Current;   
            }
        }
    }

    public static T GetMostFrequentElement<T>(this IEnumerable<T> elements)
    {
        return elements.GroupBy(w => w).OrderByDescending(w => w.Count()).Select(w => w.Key).First();
    }

    public static IEnumerable<T> SelectByIndex<T>(this IEnumerable<T> source, int startIndex, int length)
    {
        var selected = new List<T>();

        for (int i = NumberExtensions.GetWrappedIndex(startIndex, source.Count()); i < source.Count() && length > 0; i++, length--)
        {
            selected.Add(source.ElementAt(i));
            if (i + 1 == source.Count())
                i = -1;
        }

        return selected;
    }

    public static void ReplaceByIndex<T>(this List<T> source, int startIndex, int length, IEnumerable<T> replaceItems)
    {
        int replacementIndex = 0;

        for (int i = NumberExtensions.GetWrappedIndex(startIndex,source.Count); i < source.Count() && length > 0; i++,length--)
        {
            source[i] = replaceItems.ElementAt(replacementIndex);
            replacementIndex++;
            if (i + 1 == source.Count())
                i = -1;
        }
    }

}