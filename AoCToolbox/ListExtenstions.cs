using System.Text;

namespace AoCToolbox;

public static class ListExtenstions
{
    public static bool AddIfNotNull<T>(this List<T> list, T? element)
        where T : class
    {
        if (element == null)
        {
            return false;
        }

        list.Add(element);
        return true;
    }

    public static IEnumerable<IList<T>> PermuteList<T>(this IList<T> sequence)
    {
        return Permute(sequence, 0, sequence.Count);

        static IEnumerable<IList<T>> Permute(IList<T> sequence, int k, int m)
        {
            if (k == m)
            {
                yield return sequence;
            }
            else
            {
                for (int i = k; i < m; i++)
                {
                    SwapPlaces(sequence, k, i);

                    foreach (var newSquence in Permute(sequence, k + 1, m))
                    {
                        yield return newSquence;
                    }

                    SwapPlaces(sequence, k, i);
                }
            }
        }

        static void SwapPlaces(IList<T> sequence, int indexA, int indexB)
        {
            T temp = sequence[indexA];
            sequence[indexA] = sequence[indexB];
            sequence[indexB] = temp;
        }
    }

    public static IEnumerable<IList<T>> GetAllOrdersOfList<T>(this IList<T> sequence)
    {
        if (sequence.Count == 1) yield return sequence;

        foreach (var element in sequence)
        {
            var list = new List<T>();
            var listWithoutElement = sequence.ToList();
            listWithoutElement.Remove(element);
            list.Add(element);

            foreach (var subsequence in listWithoutElement.GetAllOrdersOfList())
            {
                var copyList = list.ToList();

                copyList.AddRange(subsequence);
                yield return copyList;
            }

        }
    }

    /// <summary>
    /// Returns a sliding window elements *ending* with element on index lastIndexOfWindow.
    /// </summary>
    /// <param name="input">The list to perform the operation on</param>
    /// <param name="lastIndexOfWindow">The index of the last element to be included</param>
    /// <param name="windowLength">The width of the window</param>
    /// <returns>A new sublist containing the elements from the original list</returns>
    public static IList<T> GetSlidingWindow<T>(this IList<T> input, int lastIndexOfWindow, int windowLength)
    {
        int skip = lastIndexOfWindow - windowLength + 1;
        if (skip < 0)
        {
            windowLength = windowLength + skip;
            skip = 0;
        }

        return input.Skip(skip).Take(windowLength).ToList();
    }

    /// <summary>
    /// Returns a sliding window sum for a sublist *ending* with element on index poz.
    /// </summary>
    /// <param name="input">The list to perform the operation on</param>
    /// <param name="lastIndexOfWindow">The index of the last element to be included</param>
    /// <param name="windowLength">The width of the window</param>
    /// <returns>A sum of the elements</returns>
    public static long GetSlidingWindowSum<T>(this IList<T> input, int lastIndexOfWindow, int windowLength)
    {
        return input.GetSlidingWindow(lastIndexOfWindow, windowLength)
            .Sum(w => Convert.ToInt64(w));
    }

    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static List<List<string>> SplitStringListBy(this List<string> input, string splitValue)
    {
        var listOfLists = new List<List<string>>();

        var tempList = new List<string>();

        foreach (var item in input)
        {
            if(item != splitValue)
            {
                tempList.Add(item);
            }
            else
            {
                listOfLists.Add(tempList);
                tempList = new List<string>();
            }
        }

        listOfLists.Add(tempList);

        return listOfLists;
    }

    public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
    {
        return input
           .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
           .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
           .Select(s => shouldTrim ? s.Trim() : s)
           .ToList();
    }

    public static List<string> SplitByDoubleNewline(this string input, bool blankLines = false, bool shouldTrim = true)
    {
        return input
           .Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None)
           .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
           .Select(s => shouldTrim ? s.Trim() : s)
           .ToList();
    }

    /// <summary>
    /// Splits the input into columns, this is sometimes nice for maps drawing. 
    /// Automatically expands to a full rectangle iff needed based on max length and number of rows. 
    /// Empty cells are denoted as ' ' (Space character)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string[] SplitIntoColumns(this string input)
    {
        var rows = input.SplitByNewline(false, false);
        int numColumns = rows.Max(x => x.Length);

        var res = new string[numColumns];
        for (int i = 0; i < numColumns; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                try
                {
                    sb.Append(row[i]);
                }
                catch (IndexOutOfRangeException)
                {
                    sb.Append(' ');
                }
            }
            res[i] = sb.ToString();
        }
        return res;
    }

    public static List<int> CreateIntListFromTupleRange(this Tuple<int, int> tuple)
    {
        var list = new List<int>();

        for (int i = tuple.Item1; i < tuple.Item2+1; i++)
        {
            list.Add(i);
        }

        return list;
    }

    public static bool Contains(this List<int> source, List<int> compareList)
    {
        foreach (var item in compareList) 
        { 
            if(source.Contains(item))
            {
                return true;
            }
        }

        return false;
    }

    public static T Pop<T>(this IList<T> source, int index = -1)
    {
        if (index == -1)
        {
            var itemToReturn = source.Last();
            source.RemoveAt(source.Count - 1);
            return itemToReturn;
        }
        else
        {
            var itemToReturn = source[0];
            source.RemoveAt(index);
            return itemToReturn;
        }

    }

    public static int[] PruneListByBitwiseSelection(this IList<string> list, int length, Func<int, int, int> criteriaSelector)
    {
        for (int i = 0; i < length && list.Count > 1; i++)
        {
            (int noOf0, int noOf1) = CountBitValuesInArrayAtIndex(i, list);

            var valueToMatch = criteriaSelector(noOf0, noOf1) == 1 ? '1' : '0';

            list = list.Where(w => w[i] == valueToMatch)
                    .ToList();
        }

        if (list.Count != 1) throw new Exception();

        return list[0].ToCharArray().Select(w => int.Parse(w.ToString())).ToArray();
    }

    static (int noOf0, int noOf1) CountBitValuesInArrayAtIndex(int index, IList<string> list)
    {
        int noOf1 = list.Select(w => w[index] == '1' ? 1 : 0).Sum();
        int noOf0 = list.Count - noOf1;

        return (noOf0, noOf1);
    }

    public static IEnumerable<IList<T>> Rotate<T>(this IEnumerable<IList<T>> sequences)
    {
        var list = sequences as IList<IList<T>> ?? sequences.ToList();
        int maxCount = list.Max(l => l.Count);
        return Enumerable.Range(0, maxCount)
            .Select(i => list.Select(l => l.ElementAtOrDefault(i)).ToList());
    }

    public static IEnumerable<int[]> EnumerateAllDistributionsOfX<T>(this IList<T> source, int numberOfParticipants, int X)
    {
        if (numberOfParticipants == 1)
        {
            yield return new[] { X };
            yield break;
        }

        for (int i = 0; i <= X; i++)
        {
            var remainder = EnumerateAllDistributionsOfX(source,numberOfParticipants - 1, X - i);

            foreach (var r in remainder)
            {
                yield return r.Append(i).ToArray();
            }
        }
    }

    public static List<string> Transpose(this string[] input)
    {
        if (input.Length == 0)
            return new List<string>();

        // Find the maximum width
        int maxWidth = input.Max(row => row.Length);

        // Initialize result list
        var result = new List<string>();

        // Process each column
        for (int col = 0; col < maxWidth; col++)
        {
            var transposedColumn = new char[input.Length];

            // Process each row
            for (int row = 0; row < input.Length; row++)
            {
                // Handle cases where the current row might be shorter than maxWidth
                transposedColumn[row] = col < input[row].Length ? input[row][col] : ' ';
            }

            result.Add(new string(transposedColumn));
        }

        return result;
    }
}
