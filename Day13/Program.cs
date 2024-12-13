using AoCToolbox;

Console.WriteLine("--- Day 13: Claw Contraption ---");

var input = File.ReadAllText("testinput.txt").SplitByDoubleNewline();
var clawMachines = Enumerable.Range(0, input.Count).Select(i => new ClawMachine(input[i], false)).ToList();

long fewestTokens = clawMachines.Sum(m => m.CalculateTokensRequired()); 

Console.WriteLine($"Part 1: {fewestTokens}");

clawMachines = Enumerable.Range(0, input.Count).Select(i => new ClawMachine(input[i], true)).ToList();

fewestTokens = clawMachines.Sum(m => m.CalculateTokensRequired());

Console.WriteLine($"Part 2: {fewestTokens}");


class ClawMachine
{
    public int ButtonA_X { get; private set; }
    public int ButtonA_Y { get; private set; }
    public int ButtonB_X { get; private set; }
    public int ButtonB_Y { get; private set; }
    public long Prize_X { get; private set; }
    public long Prize_Y { get; private set; }
    private const int BUTTON_A_COST = 3;
    private const int BUTTON_B_COST = 1;
    private const long PRIZE_OFFSET = 10000000000000;

    public ClawMachine(string machineConfig, bool isPart2)
    {
        var splittedConfig = machineConfig.SplitByNewline();

        var parsedButtonA = splittedConfig[0].ParseInts();
        var parsedButtonB = splittedConfig[1].ParseInts();
        var parsedPrize = splittedConfig[2].ParseInts();

        ButtonA_X = parsedButtonA[0];
        ButtonA_Y = parsedButtonA[1];
        ButtonB_X = parsedButtonB[0];
        ButtonB_Y = parsedButtonB[1];
        if (isPart2)
        {
            Prize_X = parsedPrize[0] + PRIZE_OFFSET;
            Prize_Y = parsedPrize[1] + PRIZE_OFFSET; 
        }
        else
        {
            Prize_X = parsedPrize[0];
            Prize_Y = parsedPrize[1];
        }
    }

    public long CalculateTokensRequired()
    {
        // Calculates the determinant of the coefficient matrix
        long denominator = ButtonA_X * ButtonB_Y - ButtonA_Y * ButtonB_X;
        // Part of Cramer's rule for solving linear equations
        long numerator = Prize_Y * ButtonA_X - Prize_X * ButtonA_Y;
        
        double buttonBPresses = (double)numerator / denominator;

        if (!buttonBPresses.IsWholeNumber())
            return 0;

        double buttonAPresses = (Prize_X - ButtonB_X * buttonBPresses) / ButtonA_X;

        if (!buttonAPresses.IsWholeNumber())
            return 0;

        return (long)(BUTTON_A_COST * buttonAPresses + BUTTON_B_COST * buttonBPresses);
    }

    public override string ToString()
    {
        return $"ButtonA_XY: ({ButtonA_X},{ButtonA_Y}), ButtonB_XY: ({ButtonB_X},{ButtonB_Y}), Prize_XY: ({Prize_X},{Prize_Y})";
    }
}
