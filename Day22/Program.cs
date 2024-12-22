Console.WriteLine("--- Day 22: Monkey Market ---");
var lines = File.ReadAllLines("input.txt").ToList();

long total = 0;
foreach (var line in lines)
{
    long num = long.Parse(line);
    for (int i = 0; i < 2000; i++)
    {
        num = GetNext(num);
    }
    total += num;
}
Console.WriteLine($"Part 1: {total}");

var seqToTotal = new Dictionary<(long, long, long, long), long>();

foreach (var line in lines)
{
    long num = long.Parse(line);
    var buyer = new List<long> { num % 10 };

    for (int i = 0; i < 2000; i++)
    {
        num = GetNext(num);
        buyer.Add(num % 10);
    }

    var seen = new HashSet<(long, long, long, long)>();
    for (int i = 0; i < buyer.Count - 4; i++)
    {
        long a = buyer[i];
        long b = buyer[i + 1];
        long c = buyer[i + 2];
        long d = buyer[i + 3];
        long e = buyer[i + 4];

        var seq = (b - a, c - b, d - c, e - d);

        if (seen.Contains(seq))
            continue;

        seen.Add(seq);

        if (!seqToTotal.ContainsKey(seq))
            seqToTotal[seq] = 0;

        seqToTotal[seq] += e;
    }
}

Console.WriteLine($"Part 2: {seqToTotal.Values.Max()}");

static long GetNext(long num)
{
    num = (num ^ (num * 64)) % 16777216;
    num = (num ^ (num / 32)) % 16777216;
    num = (num ^ (num * 2048)) % 16777216;
    return num;
}


