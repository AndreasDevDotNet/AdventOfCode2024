using AoCToolbox;

Console.WriteLine("--- Day 25: Code Chronicle ---");

var blocks = File.ReadAllText("input.txt").SplitByDoubleNewline();

var locks = new List<List<int>>();
var keys = new List<List<int>>();

foreach (var block in blocks)
{
    var lines = block.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    var grid = lines.Transpose();

    if (grid[0][0] == '#')
    {
        locks.Add(grid.Select(row => row.Count(c => c == '#') - 1).ToList());
    }
    else
    {
        keys.Add(grid.Select(row => row.Count(c => c == '#') - 1).ToList());
    }
}

var numberOfKeyPairs = 0;

foreach (var lockPattern in locks)
{
    foreach (var keyPattern in keys)
    {
        if (lockPattern.Zip(keyPattern, (x, y) => x + y <= 5).All(result => result))
        {
            numberOfKeyPairs++;
        }
    }
}

Console.WriteLine($"Part 1: {numberOfKeyPairs}");

