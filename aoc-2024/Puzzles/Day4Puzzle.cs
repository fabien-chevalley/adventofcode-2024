using System.Text;
using System.Text.RegularExpressions;

namespace aoc_2024.Puzzles;

public class Day4Puzzle : Puzzle<long>
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
            var row = matrix[i];
            columns.Add(string.Join("", row));
        }
        
        var firstDiagonal = new List<string>();
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            var diagonal = new StringBuilder();
            for (var j = 0; i < matrix.GetLength(1); j++)
            {
                diagonal.Append(matrix[i][j]);
            }
            firstDiagonal.Add(diagonal.ToString());
        }

        var sum = 0;
        
        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);

        var sum = 0;
        
        return sum;
    }
}