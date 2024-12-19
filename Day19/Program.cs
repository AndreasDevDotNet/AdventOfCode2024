using AoCToolbox;

Console.WriteLine("--- Day 19: Linen Layout ---");

var input = File.ReadAllText("input.txt").SplitByDoubleNewline();
var patterns = input[0].Split(", ").ToHashSet();
var designs = input[1].SplitByNewline();

long available = 0;

foreach (var design in designs)
{
    if(IsDesignPossible(design, patterns))
        available++;

}

Console.WriteLine($"Part 1: {available}");

available = 0;

var sortedPatterns = SortPatternsByLength(patterns);
foreach (var design in designs)
{
    var ways = CountAllPatternWays(design, sortedPatterns);
    available += ways;
}

Console.WriteLine($"Part 2: {available}");

static bool IsDesignPossible(string design, HashSet<string> availablePatterns, Dictionary<string, bool> memo = null)
{
    memo ??= new Dictionary<string, bool>();

    if (string.IsNullOrEmpty(design))
        return true;

    if (memo.ContainsKey(design))
        return memo[design];

    foreach (var pattern in availablePatterns)
    {
        if (design.StartsWith(pattern))
        {
            string remaining = design[pattern.Length..];
            if (IsDesignPossible(remaining, availablePatterns, memo))
            {
                memo[design] = true;
                return true;
            }
        }
    }

    memo[design] = false;
    return false;
}

static long CountAllPatternWays(string design, Dictionary<int, List<string>> availablePatternsByLength, Dictionary<string, long> memo = null)
{
    memo ??= new Dictionary<string, long>();

    if (string.IsNullOrEmpty(design))
        return 1;

    if (memo.ContainsKey(design))
        return memo[design];

    long totalWays = 0;

    foreach (var length in availablePatternsByLength.Keys.Where(l => l <= design.Length))
    {
        foreach (var pattern in availablePatternsByLength[length])
        {
            if (IsPatternMatch(design, pattern, length))
            {
                string remaining = design[pattern.Length..];
                totalWays += CountAllPatternWays(remaining, availablePatternsByLength, memo);
            }
        } 
    }

    memo[design] = totalWays;
    return totalWays;

    bool IsPatternMatch(string design, string pattern, int length)
    {
        ReadOnlySpan<char> designSpan = design;
        ReadOnlySpan<char> patternSpan = pattern;
        return designSpan[..length].SequenceEqual(patternSpan);
    }
}

static Dictionary<int, List<string>> SortPatternsByLength(HashSet<string> patterns)
{
    var sortedPatterns = new HashSet<string>();

    var patternsByLength = new Dictionary<int, List<string>>();
    foreach (var pattern in patterns)
    {
        if (!patternsByLength.ContainsKey(pattern.Length))
        {
            patternsByLength[pattern.Length] = new List<string>();
        }
        patternsByLength[pattern.Length].Add(pattern);
    }

    return patternsByLength;
}

