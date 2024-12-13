using System.Numerics;

Console.WriteLine("--- Day 12: Garden Groups ---");
string[] lines = File.ReadAllLines("testinput.txt");

Dictionary<Complex, char> grid = new Dictionary<Complex, char>();

for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        grid[new Complex(x, y)] = lines[y][x];
    }
}

var regions = new List<(char Symbol, HashSet<Complex> Positions)>();
var uncovered = new HashSet<Complex>(grid.Keys);

while (uncovered.Count > 0)
{
    Complex start = uncovered.First();
    uncovered.Remove(start);
    var region = FloodFill(grid, start);
    uncovered.ExceptWith(region);
    regions.Add((grid[start], region));
}

int totalPrice = 0;
foreach (var region in regions)
{
    int area = region.Positions.Count;
    int perimeter = CalculatePerimeter(region);
    totalPrice += area * perimeter;
}

Console.WriteLine($"Part 1: {totalPrice}");

totalPrice = 0;
foreach (var region in regions)
{
    int area = region.Positions.Count;
    int sides = GetSidesCount(region);
    totalPrice += area * sides;
}

Console.WriteLine($"Part 2: {totalPrice}");

static HashSet<Complex> FloodFill(Dictionary<Complex, char> grid, Complex start)
{
    var region = new HashSet<Complex> { start };
    var symbol = grid[start];
    var queue = new Queue<Complex>();
    queue.Enqueue(start);

    Complex[] directions = { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne };

    while (queue.Count > 0)
    {
        Complex pos = queue.Dequeue();
        foreach (Complex d in directions)
        {
            Complex newPos = pos + d;
            if (grid.ContainsKey(newPos) && !region.Contains(newPos) && grid[newPos] == symbol)
            {
                region.Add(newPos);
                queue.Enqueue(newPos);
            }
        }
    }
    return region;
}

static int CalculatePerimeter((char Symbol, HashSet<Complex> Positions) region)
{
    int perimeter = 0;
    Complex[] directions = { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne };

    foreach (Complex pos in region.Positions)
    {
        foreach (Complex d in directions)
        {
            Complex newPos = pos + d;
            if (!region.Positions.Contains(newPos))
            {
                perimeter++;
            }
        }
    }
    return perimeter;
}

static int GetSidesCount((char Symbol, HashSet<Complex> Positions) region)
{
    var perimeterObjects = new HashSet<(Complex Pos, Complex Dir)>();
    Complex[] directions = { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne };

    foreach (Complex pos in region.Positions)
    {
        foreach (Complex d in directions)
        {
            Complex newPos = pos + d;
            if (!region.Positions.Contains(newPos))
            {
                perimeterObjects.Add((newPos, d));
            }
        }
    }

    int distinctSides = 0;
    while (perimeterObjects.Count > 0)
    {
        var (pos, d) = perimeterObjects.First();
        perimeterObjects.Remove((pos, d));
        distinctSides++;

        var next = pos + d * Complex.ImaginaryOne;
        while (perimeterObjects.Contains((next, d)))
        {
            perimeterObjects.Remove((next, d));
            next += d * Complex.ImaginaryOne;
        }

        next = pos + d * -Complex.ImaginaryOne;
        while (perimeterObjects.Contains((next, d)))
        {
            perimeterObjects.Remove((next, d));
            next += d * -Complex.ImaginaryOne;
        }
    }

    return distinctSides;
}


