using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day8Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        List<Coordinates> references = new();
        foreach (var value in matrix.Distinct().Where(x => x != '.')) references.AddRange(Compute(matrix, value));

        var sum = references
            .Where(x => !matrix.IsOutOfBox(x))
            .DistinctBy(x => new { x.X, x.Y })
            .Count();

        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        List<Coordinates> references = new();
        foreach (var value in matrix.Distinct().Where(x => x != '.')) references.AddRange(Compute(matrix, value, true));

        var distinct = references
            .Where(x => !matrix.IsOutOfBox(x))
            .DistinctBy(x => new { x.X, x.Y });

        return distinct.Count();
    }

    private static List<Coordinates> Compute(Matrix matrix, char value, bool part2 = false)
    {
        var result = new List<Coordinates>();
        var coordinates = matrix.GetCoordinates(value);
        foreach (var first in coordinates)
        foreach (var second in coordinates.Where(x => x != first))
        {
            var distance = new Coordinates(
                second.X - first.X,
                second.Y - first.Y);

            result.Add(new Coordinates(first.X - distance.X, first.Y - distance.Y));

            if (part2)
                while (!matrix.IsOutOfBox(result.Last()))
                    result.Add(new Coordinates(result.Last().X - distance.X, result.Last().Y - distance.Y));

            result.Add(new Coordinates(second.X + distance.X, second.Y + distance.Y));

            if (part2)
                while (!matrix.IsOutOfBox(result.Last()))
                    result.Add(new Coordinates(result.Last().X + distance.X, result.Last().Y + distance.Y));
        }

        if (part2) result.AddRange(coordinates);

        return result;
    }
}