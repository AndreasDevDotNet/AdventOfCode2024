using AoCToolbox;

Console.WriteLine("--- Day 1: Historian Hysteria ---");
var input = File.ReadAllLines("input.txt").ToList();

var leftList = new List<int>();
var rightList = new List<int>();
foreach (var item in input)
{
    var parts = item.ParseInts();
    leftList.Add(parts[0]);
    rightList.Add(parts[1]);
}

leftList.Sort();
rightList.Sort();

var sumDist = 0;
var numSimmilar = 0;
for (int i = 0; i < leftList.Count; i++)
{
    sumDist += Math.Max(rightList[i], leftList[i]) - Math.Min(leftList[i],rightList[i]);
    var rightListCount = rightList.Count(x => x == leftList[i]);
    numSimmilar += leftList[i] * rightListCount;
}

Console.WriteLine($"Part 1: {sumDist}");
Console.WriteLine($"Part 2: {numSimmilar}");



