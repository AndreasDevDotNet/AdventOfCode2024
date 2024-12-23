Console.WriteLine("--- Day 23: LAN Party ---");

List<(string NodeA, string NodeB)> connections = File.ReadAllLines("input.txt").Select(x => x.Split('-')).Select(x => (x[0], x[1])).ToList();

var graph = new Graph();

foreach (var connection in connections)
{
    graph.AddEdge(connection.NodeA, connection.NodeB);
}

var allCliques = graph.FindAllCliques();

var allThreeWithComputerT = allCliques.Where(x => x.Count == 3 && x.Any(c => c.StartsWith("t"))).ToList();

Console.WriteLine($"Part 1: {allThreeWithComputerT.Count}");

var maxConnected = allCliques.OrderByDescending(x => x.Count);

Console.WriteLine($"Part 2: {string.Join(",", maxConnected.First().Order())}");


// Diffrent implementation

var nodes = new Dictionary<string, HashSet<string>>();

foreach (var connection in connections)
{
    if (!nodes.ContainsKey(connection.NodeA))
        nodes[connection.NodeA] = new HashSet<string>();
    if (!nodes.ContainsKey(connection.NodeB))
        nodes[connection.NodeB] = new HashSet<string>();
    nodes[connection.NodeA].Add(connection.NodeB);
    nodes[connection.NodeB].Add(connection.NodeA);
}

var sets = new HashSet<string>();
foreach (var x in nodes.Keys)
{
    foreach (var y in nodes[x])
    {
        foreach (var z in nodes[y])
        {
            if (x != z && nodes[z].Contains(x))
            {
                var triangle = new[] { x, y, z }.OrderBy(v => v);
                sets.Add(string.Join(",", triangle));
            }
        }
    }
}

var count = sets.Count(s => s.Split(',').Any(node => node.StartsWith("t")));
Console.WriteLine($"Part 1: {count}");

sets.Clear();

foreach (var x in nodes.Keys)
{
    Search(x, new HashSet<string> { x });
}

var largestSet = sets.OrderByDescending(s => s.Count(c => c == ',') + 1).First();
Console.WriteLine($"Part 2: {largestSet}");

void Search(string node, HashSet<string> req)
{
    var key = string.Join(",", req.OrderBy(x => x));
    if (sets.Contains(key)) return;
    sets.Add(key);

    foreach (var neighbor in nodes[node])
    {
        if (req.Contains(neighbor)) continue;

        bool isConnectedToAll = req.All(query => nodes[query].Contains(neighbor));
        if (!isConnectedToAll) continue;

        var newReq = new HashSet<string>(req) { neighbor };
        Search(neighbor, newReq);
    }
}






