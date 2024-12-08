using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Day1Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await GetLines();

        return lines.Aggregate<Line, long>(0,
            (current, value) => current + Math.Abs(value.First - value.Second));
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await GetLines();

        return lines.Aggregate<Line, long>(0,
            (current, value) => current + lines.Count(x => x.Second == value.First) * value.First);
    }

    private async Task<Line[]> GetLines()
    {
        var input = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(input, "([0-9]+)\\s+([0-9]+)");
        var values = matches
            .Select(m =>
                new
                {
                    first = int.Parse(m.Groups[1].Value),
                    second = int.Parse(m.Groups[2].Value)
                })
            .ToArray();

        var firsts = values.Select(x => x.first).Order();
        var seconds = values.Select(x => x.second).Order();

        return firsts
            .Zip(seconds, (first, second) => new Line(first, second))
            .ToArray();
    }

    private record Line(int First, int Second);
}