using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Day17Puzzle : Puzzle
{
    private readonly Dictionary<long, Action<long, long>> _operations;

    private readonly Dictionary<string, long> _registers = new();

    public Day17Puzzle()
    {
        _operations = new Dictionary<long, Action<long, long>>
        {
            [0] = (operand, comboOperand) => _registers["A"] = _registers["A"] >> (int)comboOperand,
            [1] = (operand, comboOperand) => _registers["B"] ^= operand,
            [2] = (operand, comboOperand) => _registers["B"] = (comboOperand % 8) & 0b111,
            [3] = (_, _) => { },
            [4] = (_, _) => { _registers["B"] ^= _registers["C"]; },
            [5] = (_, _) => { },
            [6] = (operand, comboOperand) => _registers["B"] = _registers["A"] >> (int)comboOperand,
            [7] = (operand, comboOperand) => _registers["C"] = _registers["A"] >> (int)comboOperand
        };
    }

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(lines,
            @"Register A: (\d+)|Register B: (\d+)|Register C: (\d+)|Program: ([0-9,]+)");

        long[] opertions = [];
        foreach (Match match in matches)
        {
            if (match.Groups[1].Success) _registers["A"] = int.Parse(match.Groups[1].Value);
            if (match.Groups[2].Success) _registers["B"] = int.Parse(match.Groups[2].Value);
            if (match.Groups[3].Success) _registers["C"] = int.Parse(match.Groups[3].Value);
            if (match.Groups[4].Success) opertions = match.Groups[4].Value.Split(",").Select(long.Parse).ToArray();
        }

        var output = RunProgram(opertions);
        Console.WriteLine(string.Join(',', output));

        return 0;
    }

    private List<long> RunProgram(long[] opertions)
    {
        var output = new List<long>();
        long operationIndex = 0;
        while (operationIndex < opertions.Length)
        {
            var opCode = opertions[operationIndex];
            var operand = opertions[operationIndex + 1];

            var comboOperand = operand switch
            {
                < 4 => operand,
                4 => _registers["A"],
                5 => _registers["B"],
                6 => _registers["C"],
                7 => 0
            };

            _operations[opCode].Invoke(operand, comboOperand);

            operationIndex = opCode == 3L && _registers["A"] != 0L ? operand : operationIndex + 2;

            if (opCode == 5) output.Add(comboOperand % 8);
        }

        return output;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(lines,
            @"Register A: (\d+)|Register B: (\d+)|Register C: (\d+)|Program: ([0-9,]+)");

        long[] opertions = [];
        foreach (Match match in matches)
        {
            if (match.Groups[1].Success) _registers["A"] = int.Parse(match.Groups[1].Value);
            if (match.Groups[2].Success) _registers["B"] = int.Parse(match.Groups[2].Value);
            if (match.Groups[3].Success) _registers["C"] = int.Parse(match.Groups[3].Value);
            if (match.Groups[4].Success) opertions = match.Groups[4].Value.Split(",").Select(long.Parse).ToArray();
        }

        return Search(opertions);
    }

    private long Search(long[] operations, long register = 0, int depth = 1)
    {
        var result = long.MaxValue;
        var nextRegister = register * 8;
        for (var i = 0; i < 8; i++)
        {
            _registers["A"] = nextRegister + i;
            var output = RunProgram(operations);
            if (output.SequenceEqual(operations.TakeLast(depth)))
            {
                if (depth > operations.Length) return result;
                if (depth == operations.Length) result = Math.Min(result, nextRegister + i);

                result = Math.Min(result, Search(operations, nextRegister + i, depth + 1));
            }
        }

        return result;
    }
}