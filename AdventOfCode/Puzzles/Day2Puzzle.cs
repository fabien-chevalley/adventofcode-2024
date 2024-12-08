using System.Diagnostics;
using Levels = System.Collections.Generic.List<long>;

namespace AdventOfCode.Puzzles;

public class Day2Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await GetLines();

        var sum = 0;
        foreach (var levels in lines)
            if (IsSafe(levels))
                sum++;

        Debug.Assert(sum == 218);
        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await GetLines();

        var sum = 0;
        foreach (var levels in lines)
            if (IsSafe(levels))
                sum++;
            else
                for (var i = 0; i < levels.Count; i++)
                {
                    var levelsCopy = levels.ToList();
                    levelsCopy.RemoveAt(i);
                    if (IsSafe(levelsCopy))
                    {
                        sum++;
                        break;
                    }
                }

        Debug.Assert(sum == 290);
        return sum;
    }

    private static List<long> FirstDerivative(List<int> levels)
    {
        var result = new List<long>();

        return result;
    }

    private static bool IsSafe(Levels levels)
    {
        if (levels.Distinct().Count() == 1) return false;

        var fail = false;
        long? previousDiff = null;
        for (var i = 0; i < levels.Count - 1; i++)
        {
            var diff = levels[i] - levels[i + 1];
            if (Math.Abs(diff) > 3)
            {
                fail = true;
                break;
            }

            if (previousDiff != null && Math.Sign(previousDiff.Value) != Math.Sign(diff))
            {
                fail = true;
                break;
            }

            previousDiff = diff;
        }

        return !fail;
    }

    private async Task<List<Levels>> GetLines()
    {
        var lines = new List<Levels>();

        foreach (var line in await File.ReadAllLinesAsync(Filename))
            lines.Add(line.Split(' ').Select(long.Parse).ToList());

        return lines;
    }
}