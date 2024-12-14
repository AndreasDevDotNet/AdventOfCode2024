Console.WriteLine("--- Day 14: Restroom Redoubt ---");
var input = File.ReadAllLines("input.txt").ToList();

int width = 101;
int height = 103;

var positions = new List<List<int>>();
var velocities = new List<List<int>>();

foreach (var line in input)
{
    var parts = line.Split(' ');
    var p = parts[0].Split('=')[1].Split(',').Select(int.Parse).ToList();
    var v = parts[1].Split('=')[1].Split(',').Select(int.Parse).ToList();

    positions.Add(p);
    velocities.Add(v);

}

Console.WriteLine($"Part 1: {CalculateSafetyFactor(width, height, positions, velocities)}");

positions = new List<List<int>>();
velocities = new List<List<int>>();

foreach (var line in input)
{
    var parts = line.Split(' ');
    var p = parts[0].Split('=')[1].Split(',').Select(int.Parse).ToList();
    var v = parts[1].Split('=')[1].Split(',').Select(int.Parse).ToList();

    positions.Add(p);
    velocities.Add(v);

}
Console.WriteLine($"Part 2: {GetSecondsToFormBotChristmasTree(width, height, positions, velocities)}");

static long GetSecondsToFormBotChristmasTree(int width, int height, List<List<int>> positions, List<List<int>> velocities)
{
    long seconds = 0;
    while(true)
    {
        seconds++;

        MoveBots(width, height, positions, velocities);

        //if (seconds >= 7750)
        //    PrintRobots(seconds, positions.Where(x => x[0] >= 30 && x[1] >= 30).ToList(), width, height, 10, 10);

        var positionTuple = positions.Select(p => (p[0], p[1])).ToList();
        if (positionTuple.Distinct().Count() == positionTuple.Count)
            break;
        //var distinctCount = positionTuple.Distinct().Count();
        //if (seconds >= 7750)
        //    Console.WriteLine($"Distinct count {distinctCount}");
        //if (seconds == 7754)

        //    break;
        //if (seconds >= 7750)
        //    Thread.Sleep(2000);
    }

    PrintRobots(seconds, positions, width, height);

    return seconds;
}

static long CalculateSafetyFactor(int width, int height, List<List<int>> positions, List<List<int>> velocities)
{
    for (var i = 0; i < 100; i++)
    {
        MoveBots(width, height, positions, velocities);
    }

int q1 = 0, q2 = 0, q3 = 0, q4 = 0;
    foreach (var position in positions)
    {
        if (position[0] < width / 2)
        {
            if (position[1] < height / 2)
                q1++;
            else if (position[1] > height / 2)
                q2++;
        }
        else if (position[0] > width / 2)
        {
            if (position[1] < height / 2)
                q3++;
            else if (position[1] > height / 2)
                q4++;
        }
    }

    var safetyFactor = q1 * q2 * q3 * q4;

    return safetyFactor;
}

static void MoveBots(int width, int height, List<List<int>> positions, List<List<int>> velocities)
{
    for (var i = 0; i < positions.Count; i++)
    {
        var position = positions[i];
        var velocity = velocities[i];

        position[0] += velocity[0];
        while (position[0] < 0)
            position[0] += width;
        while (position[0] >= width)
            position[0] -= width;

        position[1] += velocity[1];
        while (position[1] < 0)
            position[1] += height;
        while (position[1] >= height)
            position[1] -= height;
    }
}

static void PrintRobots(long seconds, List<List<int>> positions, int width, int height,  int startY = 0, int startX = 0)
{
    //Console.Clear();
    Console.WriteLine($"---- {seconds} ----");

    for (int i = startY; i < height; i++)
    {
        for (int j = startX; j < width; j++)
        {
            if (positions.Any(p => p[0] == j && p[1] == i))
                Console.Write("█");
            else
                Console.Write(" ");
        }
        Console.WriteLine();
    }
}