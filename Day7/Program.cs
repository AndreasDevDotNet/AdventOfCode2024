using AoCToolbox;

Console.WriteLine("--- Day 7: Bridge Repair ---");
var calibrationEquations = File.ReadAllLines("input.txt").ToList().Select(x => ParseInput(x)).ToList();

long totalSum = 0;

foreach (var calibrationEq in calibrationEquations)
{
    for(int mask = 0; mask < (1 << (calibrationEq.Numbers.Count - 1)); mask++)
    {
        var result = EvaluatEquation(calibrationEq.Numbers, mask);
        if (result == calibrationEq.TestSum)
        {
            totalSum += calibrationEq.TestSum;
            break;
        }
    }
}

Console.WriteLine($"Part 1: {totalSum}");

totalSum = 0;

foreach (var calibrationEq in calibrationEquations)
{
    int operatorCount = calibrationEq.Numbers.Count - 1;

    for (int mask = 0; mask < Math.Pow(3, operatorCount); mask++)
    {
        var result = EvaluatEquationExtended(calibrationEq.Numbers, ConvertToBaseThree(mask, operatorCount));
        if (result == calibrationEq.TestSum)
        {
            totalSum += calibrationEq.TestSum;
            break;
        }
    }
}

Console.WriteLine($"Part 2: {totalSum}");

static long EvaluatEquation(List<long> numbers, int operatorMask)
{
    long result = numbers[0];
    for (int i = 1; i < numbers.Count; i++)
    {
        bool isAdd = ((operatorMask & (1 << (i - 1))) != 0);
        result = isAdd ? result + numbers[i] : result * numbers[i];
    }

    return result;
}

static long EvaluatEquationExtended(List<long> numbers, int[] operatorMask)
{
    long result = numbers[0];
    for (int i = 1; i < numbers.Count; i++)
    {
        switch (operatorMask[i - 1])
        {
            case 0: // Multiply
                result *= numbers[i];
                break;
            case 1: // Add
                result += numbers[i];
                break;
            case 2: // Concatenate
                result = Concatenate(result, numbers[i]);
                break;
        }
    }

    return result;
}

static long Concatenate(long a, long b)
{
    return long.Parse($"{a}{b}");
}

static int[] ConvertToBaseThree(int number, int length)
{
    var operators = new int[length];
    for (int i = 0; i < length; i++)
    {
        operators[i] = number % 3;
        number /= 3;
    }
    return operators;
}

static (long TestSum, List<long> Numbers) ParseInput(string input)
{
    var part = input.Split(':');
    var sum = part[0];
    var nums = part[1].ParseLongs().ToList();

    return (long.Parse(sum), nums);
}