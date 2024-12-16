Console.WriteLine("--- Day 16: Reindeer Maze ---");

var grid = File.ReadLines("input.txt").ToList();

var rows = grid.Count;
var cols = grid[0].Length;

var startRow = -1;
var startCol = -1;

for (var r = 0; r < rows; r++)
{
    for (var c = 0; c < cols; c++)
    {
        if (grid[r][c] == 'S')
        {
            startRow = r;
            startCol = c;
            break;
        }
    }
    if (startRow != -1)
        break;
}

Console.WriteLine($"Part 1: {GetLowestScore(grid, startRow, startCol)}");
Console.WriteLine($"Part 2: {GetNumberOfBestSeats(grid, startRow, startCol)}");

static int GetNumberOfBestSeats(List<string> grid, int startRow, int startCol)
{
    var lowestPoints = new Dictionary<(int row, int col, int dr, int dc), int>();
    var pq = new PriorityQueue<(int points, int row, int col, int dr, int dc, int lr, int lc, int ldr, int ldc), int>();
    var backTrack = new Dictionary<(int row, int col, int dr, int dc), HashSet<(int lr, int lc, int ldr, int ldc)>>();
    var endStates = new HashSet<(int row, int col, int dr, int dc)>();

    pq.Enqueue((0, startRow, startCol, 0, 1, -1, -1, -1, -1), 0);
    lowestPoints[(startRow, startCol, 0, 1)] = 0;

    int bestPoints = int.MaxValue;

    while (pq.Count > 0)
    {
        var (points, cr, cc, dr, dc, lr, lc, ldr, ldc) = pq.Dequeue();
        if (lowestPoints.ContainsKey((cr, cc, dr, dc)) && points > lowestPoints[(cr, cc, dr, dc)]) continue;
        lowestPoints[(cr, cc, dr, dc)] = points;

        if (grid[cr][cc] == 'E')
        {
            if (points > bestPoints) break;
            bestPoints = points;
            endStates.Add((cr, cc, dr, dc));
        }

        if (!backTrack.ContainsKey((cr, cc, dr, dc)))
            backTrack[(cr, cc, dr, dc)] = new HashSet<(int lr, int lc, int ldr, int ldc)>();

        backTrack[(cr, cc, dr, dc)].Add((lr, lc, ldr, ldc));

        foreach (var (nr, nc, ndr, ndc, newPoints) in GetAvailableStates(cr, cc, dr, dc, points))
        {
            if (grid[nr][nc] == '#') continue;
            if (lowestPoints.ContainsKey((nr, nc, ndr, ndc)) && points > lowestPoints[(nr, nc, ndr, ndc)]) continue;

            pq.Enqueue((newPoints, nr, nc, ndr, ndc, cr, cc, dr, dc), newPoints);
        }
    }

    var states = new Queue<(int row, int col, int dr, int dc)>(endStates);
    var seen = new HashSet<(int row, int col, int dr, int dc)>(endStates);

    while (states.Count > 0)
    {
        var bkey = states.Dequeue();
        if (!backTrack.ContainsKey(bkey)) continue;
        foreach (var last in backTrack[bkey])
        {
            if (seen.Contains(last)) continue;
            if (last.lr == -1 && last.lc == -1) continue;

            seen.Add(last);
            states.Enqueue(last);
        }
    }

    var distinctLocations = seen.Select(x => (x.row, x.col)).Distinct().ToList();

    return distinctLocations.Count;
}

static int GetLowestScore(List<string> grid, int startRow, int startCol)
{
    var seen = new HashSet<(int row, int col, int dr, int dc)>();
    var pq = new PriorityQueue<(int row, int col, int dr, int dc, int points), int>();

    pq.Enqueue((startRow, startCol, 0, 1, 0), 0);
    seen.Add((startRow, startCol, 0, 1));

    int totalPoints = 0;

    while (pq.Count > 0)
    {
        var (cr, cc, dr, dc, points) = pq.Dequeue();

        if (grid[cr][cc] == 'E')
        {
            totalPoints = points;
            break;
        }

        seen.Add((cr, cc, dr, dc));

        foreach (var (nr, nc, ndr, ndc, newPoints) in GetAvailableStates(cr, cc, dr, dc, points))
        {
            if (grid[nr][nc] == '#') continue;
            if (seen.Contains((nr, nc, ndr, ndc))) continue;

            pq.Enqueue((nr, nc, ndr, ndc, newPoints), newPoints);
        }
    }

    return totalPoints;
}

static List<(int nr, int nc, int ndr, int ndc, int points)> GetAvailableStates(int cr, int cc, int dr, int dc, int points)
{
    return [
                (cr + dr, cc + dc,dr, dc,points + 1),
                (cr,cc,dc,-dr, points + 1000),
                (cr,cc,-dc,dr, points + 1000)
           ];
}