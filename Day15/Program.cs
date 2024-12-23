
using AoCToolbox;

Console.WriteLine("--- Day 15: Warehouse Woes ---");

Dictionary<char, (int row, int col)> Moves = new Dictionary<char, (int row, int col)>
{
    {'^', (-1,0)},
    {'v', (1,0)},
    {'<', (0,-1)},
    {'>', (0,1)}
};

var input = File.ReadAllText("testinput.txt").SplitByDoubleNewline();
var map = input[0].SplitByNewline();
var moves = input[1].Replace("\n", "").Replace("\r", "");

Console.WriteLine($"Part 1: {CalculateWarehouseGSPSum(Moves, map, moves)}");
Console.WriteLine($"Part 2: {CalculateLargerWarehouseGSPSum(map, moves)}");

static int CalculateWarehouseGSPSum2(List<string> map, string moves)
{
    var grid = CreateWarehouse(map);

    var rows = grid.Length;
    var cols = grid[0].Length;

    var botRow = -1;
    var botCol = -1;

    for (var r = 0; r < rows; r++)
    {
        for (var c = 0; c < cols; c++)
        {
            if (grid[r][c] == '@')
            {
                botRow = r;
                botCol = c;
                break;
            }
        }
        if (botRow != -1)
            break;
    }

    foreach (var move in moves)
    {
        int dr = move switch
        {
            '^' => -1,
            'v' => 1,
            _ => 0,
        };

        int dc = move switch
        {
            '<' => -1,
            '>' => 1,
            _ => 0,
        };

        var moveTargets = new List<(int, int)> { (botRow, botCol) };

        var cr = botRow;
        var cc = botRow;

        bool canMove = true;

        while (true)
        {
            cr += dr;
            cc += dc;

            var currentChar = grid[cr][cc];

            if(currentChar == '#')
            {
                canMove = false;
                break;
            }

            if(currentChar == 'O')
                moveTargets.Add((cr, cc));

            if (currentChar == '.')
                break;
        }

        if(!canMove) continue;

        if ((botRow + dr > 0 && botRow + dr <= grid.Length - 1) && (botCol + dc > 0 && botCol + dc <= grid[0].Length - 1) && grid[botRow + dr][botCol + dc] != '#')
        {
            grid[botRow][botCol] = '.';
            grid[botRow + dr][botCol + dc] = '@'; 
        }

        foreach (var (br,bc) in moveTargets.Skip(1))
        {
            grid[br + dr][bc + dc] = 'O';
        }

        botRow += dr;
        botCol += dc;

        Console.WriteLine(move);
        DrawGrid(grid);
    }

    var gpsSum = 0;

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (grid[r][c] == 'O')
            {
                gpsSum += 100 * r + c;
            }
        }
    }

    return gpsSum;
}

static int CalculateLargerWarehouseGSPSum(List<string> map, string moves)
{
    var grid = CreateLargerWarehouse(map);

    var rows = grid.Length;
    var cols = grid[0].Length;

    var botRow = -1;
    var botCol = -1;

    for ( var r = 0;r < rows;r++)
    {
        for ( var c = 0;c < cols;c++)
        {
            if (grid[r][c] == '@')
            {
                botRow = r; 
                botCol = c;
                break;
            }
        }
        if (botRow != -1)
            break;
    }

    foreach (var move in moves)
    {
        int dr = move switch
        {
            '^' => -1,
            'v' => 1,
            _ => 0,
        };

        int dc = move switch
        {
            '<' => -1,
            '>' => 1,
            _ => 0,
        };

        var moveTargets = new List<(int, int)> { (botRow, botCol) };

        bool canMove = true;

        for(int i = 0; i < moveTargets.Count;i++) 
        {
            var (cr, cc) = moveTargets[i];

            int nr = cr + dr;
            int nc = cc + dc;

            if (moveTargets.Contains((nr, nc))) continue;

            char currentChar = grid[nr][nc];

            if (currentChar == '#')
            {
                canMove = false;
                break;
            }

            if(currentChar == '[')
            {
                moveTargets.Add((nr, nc));
                moveTargets.Add((nr, nc + 1));
            }

            if (currentChar == ']')
            {
                moveTargets.Add((nr, nc));
                moveTargets.Add((nr, nc - 1));
            }
        }

        if (!canMove) continue;

        var gridCopy = grid.Select(row => row.ToArray()).ToArray();

        grid[botRow][botCol] = '.';
        grid[botRow + dr][botCol + dc] = '@';

        foreach (var (br, bc) in moveTargets.Skip(1))
        {
            grid[br][bc] = '.';
        }

        foreach (var (br, bc) in moveTargets.Skip(1))
        {
            grid[br + dr][bc + dc] = gridCopy[br][bc];
        }

        botRow += dr;
        botCol += dc;

        DrawGrid(grid);
    }

    var gpsSum = 0;

    for (int r = 0;r < rows; r++) 
    {
        for (int c = 0;c < cols; c++)
        {
            if (grid[r][c] == '[')
            {
                gpsSum += 100 * r + c;
            }
        }
    }

    return gpsSum;
}

static char[][] CreateWarehouse(List<string> map)
{
    var scaledMap = new List<char[]>();

    foreach (var row in map)
    {
        scaledMap.Add(row.ToArray());
    }

    return scaledMap.ToArray();
}

static char[][] CreateLargerWarehouse(List<string> map)
{
    var scaledMap = new List<char[]>();

    foreach (var row in map)
    {
        var scaledRow = new List<char>();

        foreach (var col in row)
        {
            switch (col)
            {
                case '#':
                    scaledRow.AddRange(new[] { '#', '#' });
                    break;
                case 'O':
                    scaledRow.AddRange(new[] { '[', ']' });
                    break;
                case '.':
                    scaledRow.AddRange(new[] { '.', '.' });
                    break;
                case '@':
                    scaledRow.AddRange(new[] { '@', '.' });
                    break;
            }
        }

        scaledMap.Add(scaledRow.ToArray());
    }

    return scaledMap.ToArray();
}



static int CalculateWarehouseGSPSum(Dictionary<char, (int row, int col)> Moves, List<string> map, string moves)
{
    var rows = map.Count;
    var cols = map.First().Length;

    int botRow = 0;
    int botCol = 0;

    var boxes = new HashSet<(int row, int col)>();

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (map[row][col] == '@')
            {
                botRow = row;
                botCol = col;
            }
            else if (map[row][col] == 'O')
            {
                boxes.Add((row, col));
            }
        }
    }

    foreach (var move in moves)
    {
        (int dr, int dc) = Moves[move];

        int nr = botRow + dr;
        int nc = botCol + dc;

        if (boxes.Contains((nr, nc)))
        {
            if (CanMoveBoxChain(nr, nc, dr, dc, boxes, map))
            {
                MoveBoxChain(nr, nc, dr, dc, boxes);
                botRow = nr;
                botCol = nc;
            }
        }
        else
        {
            if (IsValidMove(nr, nc, map))
            {
                botRow = nr;
                botCol = nc;
            }
        }

        //DrawMap(map, boxes, botRow, botCol);
    }

    var gpsSum = boxes.Sum(b => 100 * b.row + b.col);
    return gpsSum;
}

static void MoveBoxChain(int row, int col, int dr, int dc, HashSet<(int row, int col)> boxes)
{
    var boxPositions = new List<(int, int)>();

    while (boxes.Contains((row, col)))
    {
        boxPositions.Add((row, col));

        row += dr;
        col += dc;
    }

    boxPositions.Reverse();

    foreach (var (boxRow, boxCol) in boxPositions)
    {
        boxes.Remove((boxRow, boxCol));
        boxes.Add((boxRow + dr, boxCol + dc));
    }
}

static bool CanMoveBoxChain(int row, int col, int dr, int dc, HashSet<(int row, int col)> boxes, List<string> map)
{
    while (boxes.Contains((row, col)))
    {
        row += dr;
        col += dc;
        if (!IsValidMove(row, col, map))
        {
            return false;
        }
    }

    return true;
}

static bool IsValidMove(int row, int col, List<string> map)
{
    return row >= 0 && col >= 0 && row < map.Count && col < map[0].Length && map[row][col] != '#';
}

static void DrawGrid(char[][] grid)
{
    Console.Clear();
    for (int r = 0; r < grid.Length; r++)
    {
        for (int c = 0; c < grid[r].Length; c++)
        {
            Console.Write(grid[r][c]);
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

static void DrawMap(List<string> map, HashSet<(int row, int col)> boxes, int botRow, int botCol)
{
    Console.Clear();
    for (int row = 0; row < map.Count; row++)
    {
        for (int col = 0; col < map[0].Length; col++)
        {
            if (col == botCol && row == botRow)
                Console.Write('@');
            else if (boxes.Contains((row, col)))
                Console.Write('O');
            else
            {
                if (map[row][col] == '@' || map[row][col] == 'O')
                    Console.Write('.');
                else
                    Console.Write(map[row][col]);
            }

        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

