using System.Text.RegularExpressions;

namespace aoc_2024.Puzzles;

public class Day3Puzzle : Puzzle<long>
{
    public override async ValueTask<long> PartOne()
    {
        var line = await File.ReadAllTextAsync(Filename);

        var sum = 0;
        var matches = Regex.Matches(line, @"(mul\((\d{1,3}),(\d{1,3})\))");
        foreach (Match match in matches)
        {
            sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
        }
        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);

        var sum = 0;
        var matches = Regex.Matches(line, @"(mul\((\d{1,3}),(\d{1,3})\))");
        var doIndexes = Regex.Matches(line, @"do\(\)").Select(m => m.Index).ToArray();
        var dontIndexes = Regex.Matches(line, @"don't\(\)").Select(m => m.Index).ToArray();
        
        foreach (Match match in matches)
        {
            var index = match.Index;

            var previousDo = doIndexes.LastOrDefault(x => x < index);
            var previousDont = dontIndexes.LastOrDefault(x => x < index);

            if (previousDont == 0 || previousDo > previousDont)
            {
                sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }
        return sum;
    }
}