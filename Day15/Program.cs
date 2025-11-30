
using AoCToolbox;

Console.WriteLine("--- Day 15: Warehouse Woes ---");

var dir = new RowCol(0, 0);

Dictionary<char, RowCol> Moves = new Dictionary<char, RowCol>
{
    {'^', dir.Up()},
    {'v', dir.Down()},
    {'<', dir.Left()},
    {'>', dir.Right()}
};

var input = File.ReadAllText("test.txt").SplitByDoubleNewline();
var map = input[0].SplitByNewline();
var moves = input[1].Replace("\n", "").Replace("\r", "");

//Console.WriteLine($"Part 1: {CalculateWarehouseGSPSum(Moves, map, moves)}");
//Console.WriteLine($"Part 2: {CalculateLargerWarehouseGSPSum(map, moves)}");
Console.WriteLine($"Part 2: {CalculateLargerWarehouseGSPSum2(Moves,map, moves)}");

static int CalculateLargerWarehouseGSPSum(List<string> map, string moves)
{
    var grid = CreateLargerWarehouse(map);

    //DrawGrid(grid);

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

        //DrawGrid(grid);
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

static int CalculateLargerWarehouseGSPSum2(Dictionary<char, RowCol> Moves, List<string> map, string moves)
{
    var grid = CreateLargerWarehouse(map);

    var rows = grid.Length;
    var cols = grid[0].Length;

    RowCol botRowCol = null;

    var boxes = new HashSet<(RowCol leftEdge, RowCol rightEdge)>();

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (grid[row][col] == '@')
            {
                botRowCol = new RowCol(row, col);
            }
            else if (grid[row][col] == '[')
            {
                boxes.Add((new RowCol(row, col), new RowCol(row, col+1)));
            }
        }
    }

    DrawLargeMap(grid, boxes, botRowCol);

    foreach (var move in moves)
    {
        var dir = Moves[move];

        var nextRowCol = botRowCol + dir;

        if (IsBoxInBotPath(boxes, nextRowCol, move))
        {
            if (CanMoveLargeBoxChain(nextRowCol, move, boxes, grid))
            {
                MoveLargeBoxChain(nextRowCol, dir, move, boxes);
                botRowCol = nextRowCol;
            }
        }
        else
        {
            if (IsValidMove(nextRowCol, grid))
            {
                botRowCol = nextRowCol;
            }
        }

        DrawLargeMap(grid, boxes, botRowCol, move.ToString());
    }

    var gpsSum = boxes.Sum(b => 100 * b.leftEdge.Row + b.leftEdge.Col);
    return gpsSum;
}

static void MoveLargeBoxChain(RowCol nextPos, RowCol dir, char move, HashSet<(RowCol leftEdge, RowCol rightEdge)> boxes)
{
    var boxPositions = new HashSet<(RowCol leftEdge, RowCol rightEdge)>();

    while (IsBoxInBotPath(boxes, nextPos, move))
    {
        boxPositions.Add(GetBoxInBotPath(nextPos, boxes, move));

        nextPos = nextPos + dir;
    }

    foreach (var boxPos in boxPositions)
    {
        boxes.TryGetValue(boxPos, out var box);
        box.leftEdge = box.leftEdge + dir;
        box.rightEdge = box.rightEdge + dir;
    }
}

static (RowCol leftEdge, RowCol rightEdge) MoveBoxToNewPos((RowCol leftEdge, RowCol rightEdge) boxPos, char move)
{
    switch (move)
    {
        case '^':
            return (new RowCol(boxPos.leftEdge.Row - 1, boxPos.leftEdge.Col), new RowCol(boxPos.rightEdge.Row - 1, boxPos.rightEdge.Col));
        case 'v':
            return (new RowCol(boxPos.leftEdge.Row + 1, boxPos.leftEdge.Col), new RowCol(boxPos.rightEdge.Row + 1, boxPos.rightEdge.Col));
        case '<':
            return (new RowCol(boxPos.leftEdge.Row, boxPos.leftEdge.Col - 1), new RowCol(boxPos.rightEdge.Row, boxPos.rightEdge.Col - 1));
        case '>':
            return (new RowCol(boxPos.leftEdge.Row, boxPos.leftEdge.Col + 1), new RowCol(boxPos.rightEdge.Row, boxPos.rightEdge.Col + 1));
    }

    throw new Exception("This should not happen");
}

static (RowCol leftEdge, RowCol rightEdge) GetBoxInBotPath(RowCol nextPos, HashSet<(RowCol leftEdge, RowCol rightEdge)> boxes, char move)
{
    switch (move)
    {
        case '^':
        case 'v':
            return boxes.FirstOrDefault(x => x.leftEdge == nextPos || x.rightEdge == nextPos);

        case '<':
            return boxes.FirstOrDefault(x => x.rightEdge.Col == nextPos.Col || x.rightEdge.Col == nextPos.Col - 1);
        case '>':
            return boxes.FirstOrDefault(x => x.leftEdge.Col == nextPos.Col || x.leftEdge.Col == nextPos.Col + 1);
    }

    throw new Exception("This should not happen");
}

static bool CanMoveLargeBoxChain(RowCol nextPos, char move, HashSet<(RowCol leftEdge, RowCol rightEdge)> boxes, char[][] grid)
{
    while (IsBoxInBotPath(boxes, nextPos, move))
    {
        nextPos = move switch
        {
            '^' => nextPos.Up(),
            'v' => nextPos.Down(),
            '<' => nextPos.Left(),
            '>' => nextPos.Right()
        };
        if (!IsValidMove(nextPos, grid))
        {
            return false;
        }
    }

    return true;
}

static bool IsBoxInBotPath(HashSet<(RowCol leftEdge, RowCol rightEdge)> boxes, RowCol botPos, char move)
{
    switch (move)
    {
        case '^':
        case 'v':
            return boxes.Any(x => x.leftEdge == botPos || x.rightEdge == botPos);
            
        case '<':
            return boxes.Any(x => x.rightEdge == botPos || x.rightEdge == botPos.Left());
        case '>':
            return boxes.Any(x => x.leftEdge == botPos || x.leftEdge == botPos.Right());
    }

    return false;
}

static int CalculateWarehouseGSPSum(Dictionary<char, RowCol> Moves, List<string> map, string moves)
{
    var grid = CreateWarehouse(map);

    var rows = grid.Length;
    var cols = grid[0].Length;

    RowCol botRowCol = null;

    var boxes = new HashSet<RowCol>();

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (grid[row][col] == '@')
            {
                botRowCol = new RowCol(row, col);
            }
            else if (grid[row][col] == 'O')
            {
                boxes.Add(new RowCol(row, col));
            }
        }
    }

    //DrawMap(grid, boxes, botRowCol);

    foreach (var move in moves)
    {
        var dir = Moves[move];

        var nextRowCol = botRowCol + dir;

        if (boxes.Contains(nextRowCol))
        {
            if (CanMoveBoxChain(nextRowCol, move, boxes, grid))
            {
                MoveBoxChain(nextRowCol, dir, boxes);
                botRowCol = botRowCol + dir;
            }
        }
        else
        {
            if (IsValidMove(nextRowCol, grid))
            {
                botRowCol = nextRowCol;
            }
        }

       //DrawMap(grid, boxes, botRowCol, move.ToString());
    }

    var gpsSum = boxes.Sum(b => 100 * b.Row + b.Col);
    return gpsSum;
}

static void MoveBoxChain(RowCol nextPos, RowCol dir, HashSet<RowCol> boxes)
{
    var boxPositions = new List<RowCol>();

    while (boxes.Contains(nextPos))
    {
        boxPositions.Add(nextPos);

        nextPos = nextPos + dir;
    }

    boxPositions.Reverse();

    foreach (var boxPos in boxPositions)
    {
        boxes.Remove(boxPos);
        boxes.Add(boxPos + dir);
    }
}

static bool CanMoveBoxChain(RowCol nextPos, char direction, HashSet<RowCol> boxes, char[][] grid)
{
    while (boxes.Contains(nextPos))
    {
        nextPos = direction switch
        {
            '^' => nextPos.Up(),
            'v' => nextPos.Down(),
            '<' => nextPos.Left(),
            '>' => nextPos.Right()
        };
        if (!IsValidMove(nextPos, grid))
        {
            return false;
        }
    }

    return true;
}

static bool IsValidMove(RowCol move, char[][] grid)
{
    return move.Row >= 0 && move.Col >= 0 && move.Row < grid.Length && move.Col < grid[0].Length && grid[move.Row][move.Col] != '#';
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

static char[][] CreateWarehouse(List<string> map)
{
    var charMap = new List<char[]>();

    foreach (var row in map)
    {
        var charRow = new List<char>();
        foreach (var col in row)
        {
            charRow.Add(col);
        }
        charMap.Add(charRow.ToArray());
    }

    return charMap.ToArray();
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

static void DrawMap(char[][] grid, HashSet<RowCol> boxes, RowCol botRowCol, string move = null)
{
    Console.Clear();

    if (move != null)
    {
        string moveDir = move switch
        {
            "^" => "Up",
            "v" => "Down",
            "<" => "Left",
            ">" => "Right"
        };

        Console.WriteLine(moveDir);

    }
    else
    {
        Console.WriteLine("Starting state");
    }

    for (int row = 0; row < grid.Length; row++)
    {
        for (int col = 0; col < grid[0].Length; col++)
        {
            if (col == botRowCol.Col && row == botRowCol.Row)
                Console.Write('@');
            else if (boxes.Contains(new RowCol(row,col)))
                Console.Write('O');
            else
            {
                if (grid[row][col] == '@' || grid[row][col] == 'O')
                    Console.Write('.');
                else
                    Console.Write(grid[row][col]);
            }

        }
        Console.WriteLine();
    }
    Console.WriteLine();

    Thread.Sleep(500);
}

static void DrawLargeMap(char[][] grid, HashSet<(RowCol leftEdge, RowCol rightEdge)> boxes, RowCol botRowCol, string move = null)
{
    Console.Clear();

    if (move != null)
    {
        string moveDir = move switch
        {
            "^" => "Up",
            "v" => "Down",
            "<" => "Left",
            ">" => "Right"
        };

        Console.WriteLine(moveDir);

    }
    else
    {
        Console.WriteLine("Starting state");
    }

    for (int row = 0; row < grid.Length; row++)
    {
        for (int col = 0; col < grid[0].Length; col++)
        {
            if (col == botRowCol.Col && row == botRowCol.Row)
                Console.Write('@');
            else if (boxes.Contains((new RowCol(row, col), new RowCol(row, col + 1))))
            {
                Console.Write('[');
                Console.Write(']');
                col++;
            }
            else
            {
                if (grid[row][col] == '@' || grid[row][col] == '[' || grid[row][col] == ']')
                    Console.Write('.');
                else
                    Console.Write(grid[row][col]);
            }

        }
        Console.WriteLine();
    }
    Console.WriteLine();

    Thread.Sleep(500);
}

class Box
{
    public RowCol LeftEdge { get; private set; }
    public RowCol RightEdge { get; private set; }

    public Box(RowCol leftEdge, RowCol rightEdge)
    {
        LeftEdge = leftEdge;
        RightEdge = rightEdge;
    }

    public HashSet<Box> GetAdjecentBoxes(List<Box> allBoxes, char direction)
    {
        var boxes = new HashSet<Box>();

        List<Box> rightBoxes = new();
        List<Box> leftBoxes = new();

        switch (direction)
        {
            case '^':
                rightBoxes = allBoxes.Where(x => x.RightEdge == RightEdge.Up()).ToList();
                leftBoxes = allBoxes.Where(x => x.LeftEdge == LeftEdge.Up()).ToList();
                foreach (Box box in rightBoxes)
                    boxes.Add(box);
                foreach (Box box in leftBoxes)
                    boxes.Add(box);
                break;
            case 'v':
                rightBoxes = allBoxes.Where(x => x.RightEdge == RightEdge.Down()).ToList();
                leftBoxes = allBoxes.Where(x => x.LeftEdge == LeftEdge.Down()).ToList();
                foreach (Box box in rightBoxes)
                    boxes.Add(box);
                foreach (Box box in leftBoxes)
                    boxes.Add(box);
                break;
            case '<':
                leftBoxes = allBoxes.Where(x => x.RightEdge == LeftEdge.Left()).ToList();
                foreach (Box box in leftBoxes)
                    boxes.Add(box);
                break;
            case '>':
                rightBoxes = allBoxes.Where(x => x.LeftEdge == RightEdge.Right()).ToList();
                foreach(Box box in rightBoxes)
                    boxes.Add(box);
                break;

            default:
                throw new Exception("Invild direction marker");
        }

        return boxes;
    }

}

