using AoCToolbox;

Console.WriteLine("--- Day 18: RAM Run ---");

var corruptedBytes = File.ReadAllLines("input.txt").Select(x => ParseCoordinate(x)).ToList();

int size = 70;
int bytes = 1024;

var (minSteps, p) = FindPath(corruptedBytes, size, bytes);

Console.WriteLine($"Part 1: {minSteps}");

var low = 0;
var high = corruptedBytes.Count;

while (low < high)
{
    var min = (low + high) / 2;

    var result = FindPath(corruptedBytes, size, min + 1);
    if(result.possible)
        low = min + 1;
    else
        high = min;
}

Console.WriteLine($"Part 2: {corruptedBytes[high].col}, {corruptedBytes[high].row}");

static (int row, int col) ParseCoordinate(string input)
{
    var parts = input.ParseInts();

    return (parts[1], parts[0]);
}

static (int minSteps, bool possible) FindPath(List<(int row, int col)> corruptedBytes, int size, int bytes)
{
    (int row, int col) start = (0, 0);

    var walkQ = new PriorityQueue<(int row, int col, int steps), int>();
    var seen = new HashSet<(int row, int col)>();

    walkQ.Enqueue((start.row, start.col, 0), 0);
    seen.Add(start);

    var minSteps = 0;

    while (walkQ.Count > 0)
    {
        var (cr, cc, steps) = walkQ.Dequeue();

        foreach (var (dr, dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
        {
            int nr = cr + dr;
            int nc = cc + dc;
            if (IsInCorruptedMemory((nr, nc), bytes)) continue;
            if (nr < 0 || nc < 0 || nr > size || nc > size) continue;
            if (seen.Contains((nr, nc))) continue;

            if (nr == size && nc == size)
            {
                minSteps = steps + 1;
                break;
            }

            seen.Add((nr, nc));
            walkQ.Enqueue((nr, nc, steps + 1), steps + 1);
        }
    }

    return (minSteps, minSteps != 0);

    bool IsInCorruptedMemory((int nr, int nc) position, int noOfBytes)
    {
        var bytPosToCheck = corruptedBytes.Take(noOfBytes);

        return bytPosToCheck.Contains(position);
    }
}