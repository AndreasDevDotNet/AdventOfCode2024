Console.WriteLine("--- Day 9: Disk Fragmenter ---");

var input = File.ReadAllText("input.txt").Trim();
var testinput = File.ReadAllText("testinput.txt").Trim();

Console.WriteLine($"Part 1 (testdata): {FragmentByBlocks(testinput)}");
Console.WriteLine($"Part 1: {FragmentByBlocks(input)}");
Console.WriteLine($"Part 2 (testdata): {FragmentByFiles(testinput)}");
Console.WriteLine($"Part 2: {FragmentByFiles(input)}");

static long FragmentByBlocks(string input)
{
    var disk = new List<long>();
    long fileId = 0;

    for (int i = 0; i < input.Length; i++)
    {
        int x = int.Parse(input[i].ToString());
        if (i % 2 == 0)
        {
            disk.AddRange(Enumerable.Repeat(fileId, x));
            fileId++;
        }
        else
        {
            disk.AddRange(Enumerable.Repeat(-1L, x));
        }
    }

    var blanks = disk.Select((x, i) => new { Value = x, Index = i })
                     .Where(item => item.Value == -1)
                     .Select(item => item.Index)
                     .ToList();

    foreach (int i in blanks)
    {
        while (disk[disk.Count - 1] == -1) disk.RemoveAt(disk.Count - 1);
        if (disk.Count <= i) break;
        disk[i] = disk[disk.Count - 1];
        disk.RemoveAt(disk.Count - 1);
    }

    return disk.Select((x, i) => i * x).Sum();
}

static long FragmentByFiles(string input)
{
    Dictionary<int, (int pos, int size)> files = new Dictionary<int, (int, int)>();
    List<(int start, int length)> blanks = new List<(int, int)>();

    int fileId = 0;
    int pos = 0;

    for (int i = 0; i < input.Length; i++)
    {
        int x = int.Parse(input[i].ToString());
        if (i % 2 == 0)
        {
            if (x == 0)
            {
                throw new InvalidOperationException("Unexpected x=0 for file");
            }
            files[fileId] = (pos, x);
            fileId++;
        }
        else
        {
            if (x != 0)
            {
                blanks.Add((pos, x));
            }
        }
        pos += x;
    }

    while (fileId > 0)
    {
        fileId--;
        var (position, size) = files[fileId];

        for (int i = 0; i < blanks.Count; i++)
        {
            var (start, length) = blanks[i];
            if (start >= position)
            {
                blanks = blanks.Take(i).ToList();
                break;
            }

            if (size <= length)
            {
                files[fileId] = (start, size);
                if (size == length)
                {
                    blanks.RemoveAt(i);
                }
                else
                {
                    blanks[i] = (start + size, length - size);
                }
                break;
            }
        }
    }

    long total = 0;
    foreach (var (fid, (p, size)) in files)
    {
        for (int x = p; x < p + size; x++)
        {
            total += fid * x;
        }
    }

    return total;
}
