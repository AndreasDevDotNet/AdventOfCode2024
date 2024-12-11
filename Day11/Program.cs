Console.WriteLine("--- Day 11: Plutonian Pebbles ---");
var stones = File.ReadAllText("input.txt").Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

var stoneCounts = stones
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => (long)g.Count());

for (int i = 0; i < 75; i++)
{
    stoneCounts = Blink(stoneCounts);

    if (i == 24)
    {
        Console.WriteLine($"Part 1: {stoneCounts.Values.Sum()}");
    }
}

Console.WriteLine($"Part 2: {stoneCounts.Values.Sum()}");

static Dictionary<string, long> Blink(Dictionary<string, long> counts)
{
    var transformed = new Dictionary<string, long>();

    foreach (var kvp in counts)
    {
        string s = kvp.Key;
        long count = kvp.Value;

        if (s == "0")
        {
            if (!transformed.ContainsKey("1"))
                transformed["1"] = 0;
            transformed["1"] += count;
        }
        else
        {
            int digits = s.Length;
            if (digits % 2 == 0)
            {
                // Even number of digits => split
                int half = digits / 2;
                string leftPart = long.Parse(s.Substring(0, half)).ToString();
                string rightPart = long.Parse(s.Substring(half)).ToString();

                if (!transformed.ContainsKey(leftPart))
                    transformed[leftPart] = 0;
                transformed[leftPart] += count;

                if (!transformed.ContainsKey(rightPart))
                    transformed[rightPart] = 0;
                transformed[rightPart] += count;
            }
            else
            {
                // Odd number of digits => multiply by 2024
                long val = long.Parse(s) * 2024;
                string valStr = val.ToString();

                if (!transformed.ContainsKey(valStr))
                    transformed[valStr] = 0;
                transformed[valStr] += count;
            }
        }
    }

    return transformed;
}
