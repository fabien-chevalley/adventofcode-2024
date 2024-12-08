using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace aoc_2024.Puzzles;

public class Day7Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var datas = lines.Select(l => new Data(
                long.Parse(l.Split(":")[0]),
                l.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray()))
            .ToArray();

        var sum = datas.Sum(x => ComputeSum(x));
        Debug.Assert(sum == 267566105056);
        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var datas = lines.Select(l => new Data(
                long.Parse(l.Split(":")[0]),
                l.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray()))
            .ToArray();

        var sum = datas.Sum(x => ComputeSum(x, true));
        Debug.Assert(sum == 116094961956019);
        return sum;
    }

    private static long ComputeSum(Data data, bool part2 = false)
    {
        long result = 0;
        long sum = data.Numbers[0];
        foreach (var operators in GenerateCombinations(data.Numbers.Length - 1, part2))
        {  
            for (int i = 1; i < data.Numbers.Length; i++)
            {
                if(operators[i-1] == Operators.Addition)
                {
                    sum += data.Numbers[i];
                }
                if(operators[i-1] == Operators.Multiplication)
                {
                    sum *= data.Numbers[i];
                }
                if(operators[i-1] == Operators.Concatanation)
                {
                    sum = long.Parse($"{sum}{data.Numbers[i]}");
                }
            }

            if (data.Sum == sum)
            {
                result += sum;
                break;
            }

            sum = data.Numbers[0];
        }

        return result;
    }

    static List<Operators[]> GenerateCombinations(int n, bool part2)
    {
        var combinations = new List<Operators[]>();
        var bytes = new Operators[n];

        GenerateCombinations(n, combinations, bytes, 0, part2);
        return combinations;
    }

    static void GenerateCombinations(int n, List<Operators[]> combinations, Operators[] bytes, int i, bool part2)
    {
        if (i == n)
        {
            var combination = new Operators[n];
            Array.Copy(bytes, combination, n);
            combinations.Add(combination);
        }
        else
        {
            bytes[i] = Operators.Addition;
            GenerateCombinations(n, combinations, bytes, i + 1, part2);

            bytes[i] = Operators.Multiplication;
            GenerateCombinations(n, combinations, bytes, i + 1, part2);
            
            if(part2)
            {
                bytes[i] = Operators.Concatanation;
                GenerateCombinations(n, combinations, bytes, i + 1, part2);
            }
        }
    }

    public record Data(long Sum, long[] Numbers);
}

public enum Operators
{
    Addition,
    Multiplication,
    Concatanation,
}