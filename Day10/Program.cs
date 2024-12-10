using AoCToolbox;

Console.WriteLine("--- Day 10: Hoof It ---");
var input = File.ReadAllLines("testinput.txt").ToList();

var map = new List<List<int>>();
var rows = input.Count;
var cols = input[0].Length;

var trailHeads = new List<(int row, int col)>();

for (int row = 0;row < rows; row++)
{
    var mapCols = new List<int>();  
    for (int col = 0;col < cols;col++)
    {
        mapCols.Add(int.Parse(input[row][col].ToString()));
        if (int.Parse(input[row][col].ToString()) == 0)
        {
            trailHeads.Add((row, col));
        }
    }
    map.Add(mapCols);
}

var trailheadScore = trailHeads.Sum(x => CountTrailheadScore(x, map));
Console.WriteLine($"Part 1: {trailheadScore}");

var trailRating = trailHeads.Sum(x => GetTrailheadRating(x, map));
Console.WriteLine($"Part 2: {trailRating}");

static int CountTrailheadScore((int row, int col) startPos, List<List<int>> map)
{
    var seen = new HashSet<(int row, int col)>();
    var walkQ = new Queue<(int cr, int cc)>();

    walkQ.Enqueue((startPos.row, startPos.col));

    int score = 0;

    while (walkQ.Count > 0)
    {
        var next = walkQ.Dequeue();

        foreach ((int nr, int nc) in new[] { (next.cr -1, next.cc), (next.cr, next.cc + 1), (next.cr + 1, next.cc), (next.cr, next.cc - 1) })
        {
            if (nr < 0 || nc < 0 || nr >= map.Count || nc >= map[0].Count) continue;
            if (map[nr][nc] != map[next.cr][next.cc] + 1) continue;
            if(seen.Contains((nr,nc))) continue;

            seen.Add((nr,nc));

            if (map[nr][nc] == 9)
                score++;
            else
                walkQ.Enqueue((nr,nc));
        }
    }

    return score;
}

static int GetTrailheadRating((int row, int col) startPos, List<List<int>> map)
{
    var seen = new Dictionary<(int row, int col),int>();
    seen.Add((startPos.row, startPos.col), 1);
    var walkQ = new Queue<(int cr, int cc)>();

    walkQ.Enqueue((startPos.row, startPos.col));

    int trails = 0;

    while (walkQ.Count > 0)
    {
        var next = walkQ.Dequeue();
        if (map[next.cr][next.cc] == 9)
            trails += seen[(next.cr, next.cc)];

        foreach ((int nr, int nc) in new[] { (next.cr - 1, next.cc), (next.cr, next.cc + 1), (next.cr + 1, next.cc), (next.cr, next.cc - 1) })
        {
            if (nr < 0 || nc < 0 || nr >= map.Count || nc >= map[0].Count) continue;
            if (map[nr][nc] != map[next.cr][next.cc] + 1) continue;
            if (seen.ContainsKey((nr, nc)))
            {
                seen[(nr, nc)] += seen[(next.cr, next.cc)];
                continue;
            }

            seen.Add((nr, nc), seen[(next.cr, next.cc)]);
            walkQ.Enqueue((nr, nc));
        }
    }

    return trails;
}
