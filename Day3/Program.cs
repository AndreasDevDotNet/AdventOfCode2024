﻿using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Mull It Over ---");
var input = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {SumMul(input)}");
Console.WriteLine($"Part 2: {SumMulWithConditionals(input)}");

static int SumMul(string input)
{
    string pattern = @"mul\((\d+),(\d+)\)";
    var matches = Regex.Matches(input, pattern).ToList();
    return matches.Sum(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value));
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