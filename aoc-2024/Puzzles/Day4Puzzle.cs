using System.Diagnostics;
using System.Text;

namespace aoc_2024.Puzzles;

public class Day4Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray();

        var columns = new List<string>();
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            var column = new StringBuilder();
            for (var j = 0; j < matrix[i].Length; j++)
            {
                column.Append(matrix[j][i].ToString());
            }

            columns.Add(column.ToString());
        }

        var firstDiagonal = GetDiagonal(matrix);

        var secondDiagonal = GetDiagonal(matrix.Reverse().ToArray());

        var allDimensions =
            lines
                .Concat(lines.Select(line => line.Reverse()))
                .Concat(columns)
                .Concat(columns.Select(column => column.Reverse()))
                .Concat(firstDiagonal)
                .Concat(firstDiagonal.Select(diagonal => diagonal.Reverse()))
                .Concat(secondDiagonal)
                .Concat(secondDiagonal.Select(diagonal => diagonal.Reverse()))
                .ToArray();

        var hits = allDimensions.Sum(input => Hits(input.ToArray()));
        Debug.Assert(hits == 2560);

        return hits;
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);

        var sum = 0;

        return sum;
    }
    
    private static List<string> GetDiagonal(char[][] matrix)
    {
        var firstDiagonal = new List<string>();
        var max = matrix.Length;
        for (var i = -max + 1; Math.Abs(i) < max; i++)
        {
            var diagonal = new StringBuilder();
            for (var j = 0; j <= max - Math.Abs(i) - 1; j++)
            {
                var row = i < 0 ? j : i + j;
                var col = i > 0 ? j : Math.Abs(i) + j;
                diagonal.Append(matrix[col][row]);
            }

            firstDiagonal.Add(diagonal.ToString());
        }

        return firstDiagonal;
    }

    private static int Hits(char[] input)
    {
        var hits = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (Match(input.Skip(i).ToArray())) hits++;
        }

        return hits;
    }

    private static bool Match(char[] chars)
    {
        char[] order = ['X', 'M', 'A', 'S'];

        var index = 0;
        foreach (var c in chars)
        {
            if (c == order[index])
                index++;
            else
                return false;

            if (index == order.Length) return true;
        }

        return false;
    }
}