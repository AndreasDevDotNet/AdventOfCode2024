using AoCToolbox;

Console.WriteLine("--- Day 2: Red-Nosed Reports ---");
var reports = File.ReadAllLines("input.txt").Select(x => x.ParseInts()).ToList();

var numSafe = reports.Count(x => IsSafe(x));
var numSafeWithProblemDampner = reports.Count(x => CanBecomeSafeByRemovingOneLevel(x));

Console.WriteLine($"Part 1: {numSafe}");
Console.WriteLine($"Part 2: {numSafeWithProblemDampner}");

static bool IsSafe(IList<int> levels)
{
    bool isIncreasing = true;
    bool isDecreasing = true;

    for (int i = 1; i < levels.Count; i++)
    {
        int diff = Math.Abs(levels[i] - levels[i - 1]);

        if (diff < 1 || diff > 3)
            return false;

        if (levels[i] > levels[i - 1])
            isDecreasing = false;  
        if (levels[i] < levels[i - 1])
            isIncreasing = false;  
    }

    return isIncreasing || isDecreasing;
}

static bool CanBecomeSafeByRemovingOneLevel(IList<int> levels)
{
    for (int i = 0; i < levels.Count; i++)
    {
        var newLevels = levels.Where((val, index) => index != i).ToList();

        if (IsSafe(newLevels))
            return true;
    }

    return false;
}
