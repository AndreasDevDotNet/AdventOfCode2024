﻿using System.Numerics;

Console.WriteLine("--- Day 4: Ceres Search ---");

var lines = File.ReadAllLines("input.txt");

var grid = new Dictionary<Complex, char>();

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        grid[new Complex(i, j)] = lines[i][j];
    }
}

int part1Count = 0;

// Directions: complex components of 1, -1, 1j, etc.
var directions = new List<Complex>
        {
            new Complex(1, 0),  // Down
            new Complex(-1, 0), // Up
            new Complex(0, 1),  // Right
            new Complex(0, -1), // Left
            new Complex(1, 1),  // Down-right
            new Complex(1, -1), // Down-left
            new Complex(-1, -1), // Up-left
            new Complex(-1, 1)   // Up-right
        };

foreach (var start in grid.Keys)
{
    // Try to find XMAS in all directions
    foreach (var dir in directions)
    {
        if (grid.TryGetValue(start, out char c1) && c1 == 'X' &&
            grid.TryGetValue(start + dir, out char c2) && c2 == 'M' &&
            grid.TryGetValue(start + dir * 2, out char c3) && c3 == 'A' &&
            grid.TryGetValue(start + dir * 3, out char c4) && c4 == 'S')
        {
            part1Count++;
        }
    }
}

Console.WriteLine($"Part 1: {part1Count}");

int part2Count = 0;

foreach (var start in grid.Keys)
{
    if (grid.TryGetValue(start, out char center) && center == 'A')
    {
        // Check for "X-MAS" patterns in diagonals
        bool condition1 =
            (grid.TryGetValue(start - new Complex(1, 1), out char c1a) && c1a == 'M' &&
             grid.TryGetValue(start + new Complex(1, 1), out char c1b) && c1b == 'S') ||
            (grid.TryGetValue(start - new Complex(1, 1), out char c2a) && c2a == 'S' &&
             grid.TryGetValue(start + new Complex(1, 1), out char c2b) && c2b == 'M');

        bool condition2 =
            (grid.TryGetValue(start - new Complex(1, -1), out char c3a) && c3a == 'M' &&
             grid.TryGetValue(start + new Complex(1, -1), out char c3b) && c3b == 'S') ||
            (grid.TryGetValue(start - new Complex(1, -1), out char c4a) && c4a == 'S' &&
             grid.TryGetValue(start + new Complex(1, -1), out char c4b) && c4b == 'M');

        if (condition1 && condition2)
        {
            part2Count++;
        }
    }
}

Console.WriteLine($"Part 2: {part2Count}");