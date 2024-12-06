using System.Diagnostics;

Console.WriteLine("--- Day 6: Guard Gallivant ---");
var sw = new Stopwatch();
sw.Start();
var map = File.ReadAllLines("input.txt").ToList();

var guardPos = FindGuardPosition(map);

Console.WriteLine($"Part 1: {SimulateGuardMovement(map, guardPos)}");

var count = 0;

for (int row = 0; row < map.Count; row++)
{
    for (int col = 0; col < map.Count; col++)
    {
        if (map[row][col] != '.') continue;

        // Insert obstacle
        map[row] = map[row].Remove(col, 1);
        map[row] = map[row].Insert(col, "#");

        if (IsGuardLooping(map, guardPos))
            count++;

        // Remove obstacle
        map[row] = map[row].Remove(col, 1);
        map[row] = map[row].Insert(col, ".");

    }
}

Console.WriteLine($"Part 2: {count}");

sw.Stop();
Console.WriteLine(sw.Elapsed.TotalSeconds);

static bool IsGuardLooping(List<string> map, (int row, int col) guardPos)
{
    var visited = new HashSet<(int row, int col,int dr, int dc)>();
    (int dr, int dc) currentDirection = (-1,0);

    while (true)
    {
        visited.Add((guardPos.row, guardPos.col, currentDirection.dr, currentDirection.dc));

        if(IsOffTheMap(map, (guardPos.row + currentDirection.dr, guardPos.col + currentDirection.dc)))
            return false;

        if (map[guardPos.row + currentDirection.dr][guardPos.col + currentDirection.dc] == '#')
        {
            currentDirection = CycleDirection(currentDirection);
        }
        else
        {
            guardPos.row += currentDirection.dr;
            guardPos.col += currentDirection.dc;
        }

        if (visited.Contains((guardPos.row, guardPos.col, currentDirection.dr, currentDirection.dc)))
            return true;
    }
}

static int SimulateGuardMovement(List<string> map, (int row, int col) guardPos)
{
    var visited = new HashSet<(int row, int col)>();

    (int dr, int dc) currentDirection = (-1,0);

    while (true)
    {
        visited.Add(guardPos);

        if (IsOffTheMap(map, (guardPos.row + currentDirection.dr, guardPos.col + currentDirection.dc)))
            break;
        if (map[guardPos.row + currentDirection.dr][guardPos.col + currentDirection.dc] == '#')
        {
            currentDirection = CycleDirection(currentDirection);
        }
        else
        {
            guardPos.row += currentDirection.dr;
            guardPos.col += currentDirection.dc;    
        }
    }

    return visited.Count;
}

static (int dr, int dc) CycleDirection((int dr, int dc) currentDirection)
{
    currentDirection = currentDirection switch
    {
        (-1,0) => (0,1),
        (0, 1) => (1,0),
        (1, 0) => (0,-1),
        (0, -1) => (-1,0),
        _ => throw new NotImplementedException(),
    };
    return currentDirection;
}

static (int row, int col) FindGuardPosition(List<string> map)
{
    for (int row = 0; row < map.Count; row++)
    {
        for (int col = 0; col < map[row].Length; col++)
        {
            if (map[row][col] == '^')
                return (row, col);
        }
    }

    return (0, 0);
}

static bool IsOffTheMap(List<string> map, (int row, int col) nextPos)
{
    return (nextPos.row >= map.Count || nextPos.col >= map.First().Length) || (nextPos.row < 0 || nextPos.col < 0);
}


