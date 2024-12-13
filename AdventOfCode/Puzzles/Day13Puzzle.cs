using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Day13Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(lines,
            @"Button A: X\+(\d+), Y\+(\d+)|Button B: X\+(\d+), Y\+(\d+)|Prize: X=(\d+), Y=(\d+)");

        var sum = 0L;
        for (var i = 0; i < matches.Count; i = i + 3)
        {
            long ax = 0, ay = 0;
            long bx = 0, by = 0;
            long sumx = 0, sumy = 0;
            if (matches[i].Groups[2].Success)
            {
                ax = long.Parse(matches[i].Groups[1].Value);
                ay = long.Parse(matches[i].Groups[2].Value);
            }

            if (matches[i + 1].Groups[4].Success)
            {
                bx = long.Parse(matches[i + 1].Groups[3].Value);
                by = long.Parse(matches[i + 1].Groups[4].Value);
            }

            if (matches[i + 2].Groups[6].Success)
            {
                sumx = long.Parse(matches[i + 2].Groups[5].Value);
                sumy = long.Parse(matches[i + 2].Groups[6].Value);
            }

            var buttonB = (sumy * ax - sumx * ay) / (by * ax - bx * ay);
            var buttonA = (sumx - buttonB * bx) / ax;
            if (buttonA >= 0 && 
                buttonB >= 0 && 
                buttonB * bx + buttonA * ax == sumx &&
                buttonB * by + buttonA * ay == sumy) sum += buttonA * 3 + buttonB;
        }

        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllTextAsync(Filename);
        var matches = Regex.Matches(lines,
            @"Button A: X\+(\d+), Y\+(\d+)|Button B: X\+(\d+), Y\+(\d+)|Prize: X=(\d+), Y=(\d+)");

        var sum = 0L;
        for (var i = 0; i < matches.Count; i = i + 3)
        {
            long ax = 0, ay = 0;
            long bx = 0, by = 0;
            long sumx = 0, sumy = 0;
            if (matches[i].Groups[2].Success)
            {
                ax = long.Parse(matches[i].Groups[1].Value);
                ay = long.Parse(matches[i].Groups[2].Value);
            }

            if (matches[i + 1].Groups[4].Success)
            {
                bx = long.Parse(matches[i + 1].Groups[3].Value);
                by = long.Parse(matches[i + 1].Groups[4].Value);
            }

            if (matches[i + 2].Groups[6].Success)
            {
                sumx = long.Parse(matches[i + 2].Groups[5].Value) + 10000000000000;
                sumy = long.Parse(matches[i + 2].Groups[6].Value) + 10000000000000;
            }

            var buttonB = (sumy * ax - sumx * ay) / (by * ax - bx * ay);
            var buttonA = (sumx - buttonB * bx) / ax;
            if (buttonA >= 0 && buttonB >= 0 && 
                buttonB * bx + buttonA * ax == sumx &&
                buttonB * by + buttonA * ay == sumy) sum += buttonA * 3 + buttonB;
        }

        return sum;
    }
}