Console.WriteLine("--- Day 20: Race Condition ---");

var grid = File.ReadAllLines("input.txt").ToList();

var rows = grid.Count;
var cols = grid[0].Length;

int currentRow = 0, currentCol = 0;
bool found = false;

for (currentRow = 0; currentRow < rows && !found; currentRow++)
{
    for (currentCol = 0; currentCol < cols; currentCol++)
    {
        if (grid[currentRow][currentCol] == 'S')
        {
            found = true;
            break;
        }
    }
}

if (found) currentRow--;

var distances = CalculateDistances(grid, rows, cols, currentRow, currentCol);

Console.WriteLine($"Part 1: {GetCheatCount(grid, rows, cols, currentRow, currentCol, distances, false)}");
Console.WriteLine($"Part 2: {GetCheatCount(grid,rows, cols, currentRow, currentCol, distances, true)}");

static int[,] CalculateDistances(List<string> grid, int rows, int cols, int currentRow, int currentCol)
{
    var distances = new int[rows, cols];
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            distances[i, j] = -1;
        }
    }

    distances[currentRow, currentCol] = 0;

    while (grid[currentRow][currentCol] != 'E')
    {
        foreach (var (dr, dc) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
        {
            int nr = currentRow + dr;
            int nc = currentCol + dc;

            if (nr < 0 || nc < 0 || nr >= rows || nc >= cols) continue;
            if (grid[nr][nc] == '#') continue;
            if (distances[nr, nc] != -1) continue;

            distances[nr, nc] = distances[currentRow, currentCol] + 1;
            currentRow = nr;
            currentCol = nc;
        }
    }

    return distances;
}

static int GetCheatCount(List<string> grid, int rows, int cols, int currentRow, int currentCol, int[,] distances, bool newCheatLength)
{
    var cheatLengths = new Dictionary<(int startRow, int startCol, int endRow, int endCol),List<int>>();

    int cheatCount = 0;
    for (currentRow = 0; currentRow < rows; currentRow++)
    {
        for (currentCol = 0; currentCol < cols; currentCol++)
        {
            if (grid[currentRow][currentCol] == '#') continue;
            if (!newCheatLength)
            {
                foreach (var (dr, dc) in new[] { (2, 0), (1, 1), (0, 2), (-1, 1) })
                {
                    int nr = currentRow + dr;
                    int nc = currentCol + dc;

                    if (nr < 0 || nc < 0 || nr >= rows || nc >= cols) continue;
                    if (grid[nr][nc] == '#') continue;
                    if (Math.Abs(distances[currentRow, currentCol] - distances[nr, nc]) >= 102) cheatCount++;
                } 
            }
            else
            {
                for (int radius = 2; radius <= 20; radius++)
                {
                    for (int dr = 0; dr <= radius; dr++)
                    {
                        int dc = radius - dr;
                
                        foreach (var (drow, dcol) in new[] {(dr, dc), (dr, -dc), (-dr, dc), (-dr, -dc)})
                        {
                            int nr = currentRow + drow;
                            int nc = currentCol + dcol;

                            if (nr < 0 || nc < 0 || nr >= rows || nc >= cols) continue;
                            if (grid[nr][nc] == '#') continue;
                            if (distances[currentRow, currentCol] - distances[nr,nc] >= 100 + radius)
                            {
                                if (!cheatLengths.ContainsKey((currentRow, currentCol, nr, nc)))
                                    cheatLengths[(currentRow, currentCol, nr, nc)] = new List<int>();

                                cheatLengths[(currentRow, currentCol, nr, nc)].Add(distances[currentRow, currentCol] - distances[nr, nc]);

                            }

                        }
                    }
                }
            }
        }
    }

    if(!newCheatLength)
        return cheatCount;
    else
        return cheatLengths.Values.Distinct().Count();
}