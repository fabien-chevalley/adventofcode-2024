using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day20Puzzle : Puzzle
{
    private readonly Dictionary<string, long> _counts = new();
    private readonly HashSet<string> _foundDesigns = new();

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        var start = matrix.GetCoordinates('S').Single();
        var end = matrix.GetCoordinates('E').Single();

        var position = start;

        var path = new Dictionary<Coordinates, int>
        {
            [position] = 0
        };

        while (position != end)
        {
            var direction = Direction.Up;
            while (matrix.GetValueInFront(direction, position) == null ||
                   matrix.GetValueInFront(direction, position) == '#' ||
                   path.ContainsKey(matrix.Move(direction, position)))
            {
                direction = direction.RotateClockwise();
            }

            position = matrix.Move(direction, position);
            path[position] = path.Count;
        }

        return CheatUsed(path, 2, 100);
    }

    private static long CheatUsed(Dictionary<Coordinates, int> path, int distance, int score)
    {
        var sum = 0;
        foreach (var kpv in path)
        {
            foreach (var neighbor in kpv.Key.Neighbors(distance))
            {
                if (path.TryGetValue(neighbor, out var value))
                {
                    if (value - kpv.Value - distance >= score) sum++;
                }
            }
        }

        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        var start = matrix.GetCoordinates('S').Single();
        var end = matrix.GetCoordinates('E').Single();

        var position = start;

        var path = new Dictionary<Coordinates, int>
        {
            [position] = 0
        };

        while (position != end)
        {
            var direction = Direction.Up;
            while (matrix.GetValueInFront(direction, position) == null ||
                   matrix.GetValueInFront(direction, position) == '#' ||
                   path.ContainsKey(matrix.Move(direction, position)))
            {
                direction = direction.RotateClockwise();
            }

            position = matrix.Move(direction, position);
            path[position] = path.Count;
        }

        return CheatUsed2(path, 20, 100);
    }
    
    private static long CheatUsed2(Dictionary<Coordinates, int> path, int distance, int score)
    {
        var sum = 0;
        foreach (var kpv in path)
        {
            sum += path
                .Where(x => x.Key.ManhattanDistance(kpv.Key) <= distance)
                .Where(neighbor => path.ContainsKey(neighbor.Key))
                .Count(neighbor => 
                    neighbor.Value - kpv.Value - neighbor.Key.ManhattanDistance(kpv.Key) >= score);
        }

        return sum;
    }
}