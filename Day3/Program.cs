using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Mull It Over ---");
var input = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {SumMul(input)}");
Console.WriteLine($"Part 2: {SumMulWithConditionals(input)}");

static int SumMul(string input)
{
    var total = 0;

    string pattern = @"mul\((\d+),(\d+)\)";

    var matches = Regex.Matches(input, pattern);

    foreach (Match match in matches)
    {
        int x = int.Parse(match.Groups[1].Value);
        int y = int.Parse(match.Groups[2].Value);

        total += x * y;
    }

    return total;
}

static int SumMulWithConditionals(string input)
{
    var total = 0;
    var pattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";

    var allMatches = Regex.Matches(input, pattern);
    bool mulEnabled = true;

    foreach (Match match in allMatches)
    {
        if (match.Value == "do()")
            mulEnabled = true;
        if (match.Value == "don't()")
            mulEnabled = false;

        if (match.Value.Contains("mul"))
        {
            if (mulEnabled)
            {
                total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            } 
        }
    }

    return total;
}