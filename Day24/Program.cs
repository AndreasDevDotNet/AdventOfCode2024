using AoCToolbox;
using System.Diagnostics;

Console.WriteLine("--- Day 24: Crossed Wires ---");

Dictionary<string, int> knownValues = new Dictionary<string, int>();
Dictionary<string, (string op, string x, string y)> formulas = new Dictionary<string, (string, string, string)>();
Dictionary<string, Func<int, int, int>> operators = new Dictionary<string, Func<int, int, int>>()
{
    ["OR"] = (x, y) => x | y,
    ["AND"] = (x, y) => x & y,
    ["XOR"] = (x, y) => x ^ y
};

var blocks = File.ReadAllText("input.txt").SplitByDoubleNewline();

var lines = blocks[0].SplitByNewline().ToList();
foreach (var line in lines)
{
    string[] parts = line.Split(": ");
    knownValues[parts[0]] = int.Parse(parts[1]);
}

var formulaBlock = blocks[1].SplitByNewline().ToList();

foreach (var formula in formulaBlock)
{
    string[] parts = formula.Replace(" -> ", " ").Split(' ');
    formulas[parts[3]] = (parts[1], parts[0], parts[2]);
}

List<int> z = new List<int>();
int i = 0;

while (true)
{
    string key = "z" + i.ToString().PadLeft(2, '0');
    if (!formulas.ContainsKey(key)) break;
    z.Add(Eval(key));
    i++;
}

string binaryString = string.Join("", z.AsEnumerable().Reverse().Select(x => x.ToString()));
Console.WriteLine($"Part 1: {Convert.ToInt64(binaryString, 2)}");
var sw = new Stopwatch();
sw.Start();
List<string> swaps = new List<string>();
for (int swap = 0; swap < 4; swap++)
{
    int baseline = Progress();
    bool foundSwap = false;

    foreach (string x in formulas.Keys.ToList())
    {
        if (foundSwap) break;

        foreach (string y in formulas.Keys.ToList())
        {
            if (x == y) continue;

            // Swap formulas
            var tempX = formulas[x];
            var tempY = formulas[y];
            formulas[x] = tempY;
            formulas[y] = tempX;

            if (Progress() > baseline)
            {
                swaps.Add(x);
                swaps.Add(y);
                foundSwap = true;
                break;
            }

            // Revert swap
            formulas[x] = tempX;
            formulas[y] = tempY;
        }
    }
}

Console.WriteLine($"Part 2: {string.Join(",", swaps.OrderBy(s => s))}");
sw.Stop();
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

int Eval(string wire)
{
    if (knownValues.ContainsKey(wire)) return knownValues[wire];

    var (op, x, y) = formulas[wire];
    knownValues[wire] = operators[op](Eval(x), Eval(y));
    return knownValues[wire];
}

string MakeWire(string chr, int num)
{
    return chr + num.ToString().PadLeft(2, '0');
}

bool VerifyZ(string wire, int num)
{
    if (!formulas.ContainsKey(wire)) return false;
    var (op, x, y) = formulas[wire];
    if (op != "XOR") return false;
    if (num == 0) return new[] { x, y }.OrderBy(s => s).SequenceEqual(new[] { "x00", "y00" });
    return (VerifyIntermediateXor(x, num) && VerifyCarryBit(y, num)) ||
           (VerifyIntermediateXor(y, num) && VerifyCarryBit(x, num));
}

bool VerifyIntermediateXor(string wire, int num)
{
    if (!formulas.ContainsKey(wire)) return false;
    var (op, x, y) = formulas[wire];
    if (op != "XOR") return false;
    return new[] { x, y }.OrderBy(s => s)
        .SequenceEqual(new[] { MakeWire("x", num), MakeWire("y", num) });
}

bool VerifyCarryBit(string wire, int num)
{
    if (!formulas.ContainsKey(wire)) return false;
    var (op, x, y) = formulas[wire];

    if (num == 1)
    {
        if (op != "AND") return false;
        return new[] { x, y }.OrderBy(s => s).SequenceEqual(new[] { "x00", "y00" });
    }

    if (op != "OR") return false;
    return (VerifyDirectCarry(x, num - 1) && VerifyRecarry(y, num - 1)) ||
           (VerifyDirectCarry(y, num - 1) && VerifyRecarry(x, num - 1));
}

bool VerifyDirectCarry(string wire, int num)
{
    if (!formulas.ContainsKey(wire)) return false;
    var (op, x, y) = formulas[wire];
    if (op != "AND") return false;
    return new[] { x, y }.OrderBy(s => s)
        .SequenceEqual(new[] { MakeWire("x", num), MakeWire("y", num) });
}

bool VerifyRecarry(string wire, int num)
{
    if (!formulas.ContainsKey(wire)) return false;
    var (op, x, y) = formulas[wire];
    if (op != "AND") return false;
    return (VerifyIntermediateXor(x, num) && VerifyCarryBit(y, num)) ||
           (VerifyIntermediateXor(y, num) && VerifyCarryBit(x, num));
}

bool Verify(int num)
{
    return VerifyZ(MakeWire("z", num), num);
}

int Progress()
{
    int i = 0;
    while (Verify(i)) i++;
    return i;
}
