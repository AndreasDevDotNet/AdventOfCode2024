Console.WriteLine("--- Day 8: Resonant Collinearity ---");
var map = File.ReadAllLines("input.txt").ToList();
var antennaDict = new Dictionary<char, List<(int x, int y)>>();

for (int y = 0;y < map.Count();y++)
{
    for (int x = 0;x < map[y].Length;x++)
    {
        if (map[y][x] != '.')
        {
            if (!antennaDict.ContainsKey(map[y][x]))
                antennaDict[map[y][x]] = new List<(int x, int y)> { (x, y) };
            else
                antennaDict[map[y][x]].Add((x, y));
        }
    }
}

var antiNodesP1 = new HashSet<(int x, int y)>();
var antiNodesP2 = new HashSet<(int x, int y)>();

var antennaList = antennaDict.Values.ToList();

foreach (var antennaRange in antennaList)
{
    for (int i = 0; i < antennaRange.Count; i++)
    {
        for (int j = 0; j < antennaRange.Count; j++)
        {
            if (i == j) continue;

            var a1 = antennaRange[i];
            var a2 = antennaRange[j];

            // Add antinodes on each side
            antiNodesP1.Add((2 * a1.x - a2.x, 2 * a1.y - a2.y));
            antiNodesP1.Add((2 * a2.x - a1.x, 2 * a2.y - a1.y));

            var dy = a2.y - a1.y;
            var dx = a2.x - a1.x;

            var x = a1.x;
            var y = a1.y;

            while (y >= 0 && y < map.Count && x >= 0 && x < map[0].Length)
            {
                antiNodesP2.Add((x, y));

                x += dx;
                y += dy;
            }
        }
    } 
}

var visibleAntiNodes = antiNodesP1.Where(pos => pos.y >= 0 && pos.y < map.Count && pos.x >= 0 && pos.x < map[0].Length);

Console.WriteLine($"Part 1: {visibleAntiNodes.Count()}");
Console.WriteLine($"Part 2: {antiNodesP2.Count()}");


