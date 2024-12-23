using System.Text;

Console.WriteLine("--- Day 21: Keypad Conundrum ---");
Console.WriteLine("Numeric keypad      Directional keypad");
Console.WriteLine("+---+---+---+       +---+---+---+ ");
Console.WriteLine("| 7 | 8 | 9 |       |   | ^ | A | ");
Console.WriteLine("+---+---+---+       +---+---+---+ ");
Console.WriteLine("| 4 | 5 | 6 |       | < | v | > | ");
Console.WriteLine("+---+---+---+       + ---+---+--+");
Console.WriteLine("| 1 | 2 | 3 |");
Console.WriteLine("+---+---+---+");
Console.WriteLine("    | 0 | A |");
Console.WriteLine("    +---+---+");
Console.WriteLine("");


var numKeypad = new char[,]
{
    { '7', '8', '9' },
    { '4', '5', '6' },
    { '1', '2', '3' },
    { '#', '0', 'A' }
};

var dirKeypad = new char[,]
{
    { '#', '^', 'A' },
    { '<', 'v', '>' }
};

var shortestSequences = new Dictionary<(char from, char to), List<string>>();

ComputeSequences(numKeypad);
ComputeSequences(dirKeypad);

var codes = File.ReadAllLines("input.txt").ToList();

var shortestSequenceMemo = new Dictionary<(string code, int depth), long>();
long total = 0;

foreach (var code in codes)
{
    var codeNumber = int.Parse(code[..^1]);
    var shortestLength = GetShortestSequenceForCode(code, 0, 3);

    total += shortestLength * codeNumber;
}

Console.WriteLine($"Part 1: {total}");

shortestSequenceMemo = new Dictionary<(string code, int depth), long>();
total = 0;

foreach (var code in codes)
{
    var codeNumber = int.Parse(code[..^1]);
    var shortestLength = GetShortestSequenceForCode(code, 0, 26);

    total += shortestLength * codeNumber;
}

Console.WriteLine($"Part 2: {total}");

long GetShortestSequenceForCode(string code, int depth, int maxDepth)
{
    if(shortestSequenceMemo.TryGetValue((code,depth), out var memo))
        return memo;

    if (depth == maxDepth)
    {
        shortestSequenceMemo[(code, depth)] = (long)code.Length;
        return (long)code.Length;
    }


    var prevChar = 'A';
    long bestLength = 0;

    foreach (var c in code)
    {
        var shortestSeqs = shortestSequences[(prevChar, c)];

        long currentBest = long.MaxValue;
        foreach (var s in shortestSeqs)
        {
            var length = GetShortestSequenceForCode(s, depth + 1, maxDepth);
            if (currentBest > length)
            {
                currentBest = length;
            }
        }

        bestLength += currentBest;

        prevChar = c;
    }

    shortestSequenceMemo[(code, depth)] = bestLength;

    return bestLength;
}

void ComputeSequences(char[,] keypad)
{
    foreach (var c in keypad)
    {
        foreach (var oc in keypad)
        {
            shortestSequences[(c, oc)] = new List<string>();

            if (c == oc)
            {
                shortestSequences[(c, oc)].Add("A");
                continue;
            }

            FindShortestSequence(c, oc, keypad);
            
        }
    }
}

void FindShortestSequence(char from, char to, char[,] keypad)
{
    var fromPos = FindChar(from, keypad);
    var toPos = FindChar(to, keypad);

    var shortestPath = Math.Abs(fromPos.row - toPos.row) + Math.Abs(fromPos.col - toPos.col);

    var queue = new Queue<(int row, int col, string seq)>();
    queue.Enqueue((fromPos.row, fromPos.col, ""));

    while (queue.Count > 0)
    {
        var (row, col, seq) = queue.Dequeue();

        if ((row, col) == toPos)
        {
            if(!shortestSequences.ContainsKey((from, to)))
                shortestSequences[(from,to)] = new List<string>();
            shortestSequences[(from, to)].Add(seq + 'A');
            continue;
        }

        if(seq.Length >= shortestPath)
        {
            continue;
        }

        foreach (var (dr, dc) in new[] { (-1,0),(0,1),(1,0),(0,-1)})
        {
            var nr = row + dr;
            var nc = col + dc;
            if(nr < 0 || nc < 0 || nr >= keypad.GetLength(0) || nc >= keypad.GetLength(1)) continue;
            if (keypad[nr,nc] == '#') continue;
            var dirChar = (dr,dc) switch
            {
                (-1,0) => '^',
                (0,1) => '>',
                (1,0) => 'v',
                (0,-1) => '<',
                _ => throw new Exception("Invalid direction")
            };
            queue.Enqueue((nr, nc, seq + dirChar));
        }
    }
}

(int row, int col) FindChar(char charToFind, char[,] keypad)
{
    for (int row = 0; row < keypad.GetLength(0); row++)
    {
        for (int col = 0; col < keypad.GetLength(1); col++)
        {
            if (keypad[row, col] == charToFind)
            {
                return (row, col);
            }
        }
    }
    return (-1, -1);
}



