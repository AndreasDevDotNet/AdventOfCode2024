using AoCToolbox;

Console.WriteLine("--- Day 5: Print Queue ---");
var input = File.ReadAllText("input.txt").SplitByDoubleNewline();
string[] rulesSection = input[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);
string[] updatesSection = input[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);

List<(int, int)> rules = rulesSection
    .Select(line => line.Split('|'))
    .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
    .ToList();

List<List<int>> updates = updatesSection
    .Select(line => line.Split(',').Select(int.Parse).ToList())
    .ToList();

List<int> middlePages = new List<int>();
foreach (var update in updates)
{
    if (IsCorrectOrder(update, rules))
    {
        int middleIdx = update.Count / 2;
        middlePages.Add(update[middleIdx]);
    }
}

int middlePagesSum = middlePages.Sum();


Console.WriteLine($"Part 1: {middlePagesSum}");

List<int> correctedMiddlePages = new List<int>();
foreach (var update in updates)
{
    if (!IsCorrectOrder(update, rules))
    {
        var sortedUpdate = SortHelpers.TopologicalSort(update, rules);
        int middleIdx = sortedUpdate.Count / 2;
        correctedMiddlePages.Add(sortedUpdate[middleIdx]);
    }
}

int correctedMiddlePagesSum = correctedMiddlePages.Sum();

Console.WriteLine($"Part 2: {correctedMiddlePagesSum}");

bool IsCorrectOrder(List<int> update, List<(int, int)> rules)
{
    Dictionary<int, int> position = update
        .Select((value, index) => (value, index))
        .ToDictionary(pair => pair.value, pair => pair.index);

    foreach (var (x, y) in rules)
    {
        if (position.ContainsKey(x) && position.ContainsKey(y) && position[x] > position[y])
            return false;
    }

    return true;
}

