
// Example
//Register A: 729
//Register B: 0
//Register C: 0
//
//Program: 0,1,5,4,3,0

// Real input
//Register A: 22571680
//Register B: 0
//Register C: 0

// Program: 2, 4, 1, 3, 7, 5, 0, 3, 4, 3, 1, 5, 5, 5, 3, 0

using AoCToolbox;

var a = 22571680;

List<long> program = [2, 4, 1, 3, 7, 5, 0, 3, 4, 3, 1, 5, 5, 5, 3, 0];

Console.WriteLine("--- Day 17: Chronospatial Computer ---");

Console.WriteLine($"Part 1: {string.Join(",",RunProgram(program, a))}");
var target = new List<long>(program);
target.Reverse();
Console.WriteLine($"Part 2: {FindA(program, target)}");

static List<long> RunProgram(List<long> program, long a, bool doLogging = false)
{
    int instructionPointer = 0;
    long b = 0; 
    long c = 0;
    var output = new List<long>();

    while (instructionPointer >= 0 && instructionPointer < program.Count)
    {
        long literalVal = program[instructionPointer + 1];
        long combo = getCombo(program[instructionPointer + 1]);

        switch (program[instructionPointer])
        {
            case 0: // adv
                a = a / (long)Math.Pow(2, combo);
                if(doLogging) Console.WriteLine($"a = adv(a,combo) => {a} / {(long)Math.Pow(2, combo)} = {a}");
                break;
            case 1: // bxl
                b ^= literalVal;
                if (doLogging) Console.WriteLine($"b = bxl(literal) => {b} ^= {literalVal} = {b}");
                break;
            case 2: // bst
                b = combo % 8;
                if (doLogging) Console.WriteLine($"b = bst(combo) => {combo} % 8 = {b}");
                break;
            case 3: // jnz
                instructionPointer = (int)(a == 0 ? instructionPointer : literalVal - 2);
                if (doLogging) Console.WriteLine($"instructionPointer = a == 0 ? {instructionPointer} : {literalVal - 2} a = {a}");
                break;
            case 4: // bxc
                b ^= c;
                if (doLogging) Console.WriteLine($"b = bxc(c) => {b} ^= {c} = {b}");
                break;
            case 5: // out
                output.Add(combo % 8);
                if (doLogging) Console.WriteLine($"out(combo % 8) => output =  [{string.Join(",", output)}]");
                break;
            case 6: // bdv
                b = a / (long)Math.Pow(2, combo);
                if (doLogging) Console.WriteLine($"b = bdv(a,combo) => {a} / {(long)Math.Pow(2, combo)} = {b}");
                break;
            case 7: // cdv
                c = a / (long)Math.Pow(2, combo);
                if (doLogging) Console.WriteLine($"c = cdv(a,combo) => {a} / {(long)Math.Pow(2, combo)} = {c}");
                break;
        }
        instructionPointer += 2;
    }
    return output;

    long getCombo(long operand)
    {
        if (operand >= 0 && operand <= 3)
            return operand;

        return operand switch
        {
            4 => a,
            5 => b,
            6 => a,
            _ => throw new NotImplementedException("Unrecognized cobo operand"),
        };
    }
}

static long FindA(List<long> program, List<long> target, long a = 0, int depth = 0)
{
    if (depth == target.Count)
    {
        return a;
    }

    for (long i = 0; i < 8; i++)
    {
        var output = RunProgram(program, a * 8 + i);
        if (output.Count > 0 && output[0] == target[depth])
        {
            long result = FindA(program, target, a * 8 + i, depth + 1);
            if (result != 0)
            { 
                return result;

            }
        }
    }
    return 0;
}








