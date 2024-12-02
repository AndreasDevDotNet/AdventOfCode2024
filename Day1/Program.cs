using AoCToolbox;

Console.WriteLine("--- Day 1: Historian Hysteria ---");
var input = File.ReadAllLines("input.txt").ToList().Select(x => x.ParseInts()).Select(x => (x[0], x[1]));

var leftList = input.Select(x => x.Item1).OrderBy(x => x).ToList();
var rightList = input.Select(x => x.Item2).OrderBy(x => x).ToList();

var sumDist = Enumerable.Range(0, leftList.Count).Sum(x => Math.Abs(rightList[x] - leftList[x]));
var countSimmilar = leftList.Sum(x => x * rightList.Count(c => c == x));

Console.WriteLine($"Part 1: {sumDist}");
Console.WriteLine($"Part 2: {countSimmilar}");



