using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Day17Puzzle : Puzzle
{
    private readonly Dictionary<int, Action<int, int>> _operations;

    private readonly Dictionary<string, int> _registers = new()
    {
    };

    public Day17Puzzle()
    {
        _operations = new Dictionary<int, Action<int, int>>
        {
            [0] = (operand, comboOperand) => _registers["A"] = _registers["A"] >> comboOperand,
            [1] = (operand, comboOperand) => _registers["B"] ^= operand,
            [2] = (operand, comboOperand) => _registers["B"] = (comboOperand % 8) & 0b111,
            [3] = (_,_) => { },
            [4] = (_,_) => { _registers["B"] ^= _registers["C"]; },
            [5] = (_,_) => { },
            [6] = (operand, comboOperand) => _registers["B"] = _registers["A"] >> comboOperand,
            [7] = (operand, comboOperand) => _registers["C"] = _registers["A"] >> comboOperand
        };
    }

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(lines,
            @"Register A: (\d+)|Register B: (\d+)|Register C: (\d+)|Program: ([0-9,]+)");

        int[] opertions = [];
        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                _registers["A"] = int.Parse(match.Groups[1].Value);
            }
            if (match.Groups[2].Success)
            {
                _registers["B"] = int.Parse(match.Groups[2].Value);
            }
            if (match.Groups[3].Success)
            {
                _registers["C"] = int.Parse(match.Groups[3].Value);
            }
            if (match.Groups[4].Success)
            {
                opertions = match.Groups[4].Value.Split(",").Select(int.Parse).ToArray();
            }
        }
        
        var output = new List<int>();
        var operationIndex = 0;
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
                7 => 0,
            };


            _operations[opCode].Invoke(operand, comboOperand);

            operationIndex = opCode == 3 && _registers["A"] != 0 ? 
                operand : 
                operationIndex + 2;

            if(opCode == 5)
            {
                output.Add(comboOperand % 8);
            }

            // Console.WriteLine($"{_operationIndex}: {_registers["A"]},{_registers["B"]},{_registers["C"]}");
        }

        Console.WriteLine(string.Join(',', output));
        // 4,3,2,6,4,5,3,2,4

        return 0;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);

        return 0;
    }
}