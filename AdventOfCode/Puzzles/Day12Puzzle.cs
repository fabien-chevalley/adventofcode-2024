using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day12Puzzle : Puzzle
{
    private readonly Dictionary<string, long> _cache = new();

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

     
        return 0;
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);
     

        return 0;
    }
    
    
    public class Matrix : Models.Matrix
    {
        public Matrix(char[,] data) : base(data)
        {
        }

        public char? GetValueInFront(Day10Puzzle.Direction direction, Coordinates position)
        {
            switch (direction)
            {
                case Day10Puzzle.Direction.Up:
                    position = new Coordinates(position.X - 1, position.Y);
                    break;
                case Day10Puzzle.Direction.Right:
                    position = new Coordinates(position.X, position.Y + 1);
                    break;
                case Day10Puzzle.Direction.Down:
                    position = new Coordinates(position.X + 1, position.Y);
                    break;
                case Day10Puzzle.Direction.Left:
                    position = new Coordinates(position.X, position.Y - 1);
                    break;
            }

            if (IsOutOfBox(position)) return null;

            return GetValue(position);
        }


        public Coordinates Move(Day10Puzzle.Direction direction, Coordinates position)
        {
            switch (direction)
            {
                case Day10Puzzle.Direction.Up:
                    return new Coordinates(position.X - 1, position.Y);
                case Day10Puzzle.Direction.Right:
                    return new Coordinates(position.X, position.Y + 1);
                case Day10Puzzle.Direction.Down:
                    return new Coordinates(position.X + 1, position.Y);
                case Day10Puzzle.Direction.Left:
                    return new Coordinates(position.X, position.Y - 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}