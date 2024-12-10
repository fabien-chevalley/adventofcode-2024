using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day10Puzzle : Puzzle
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        var startCoordinates = matrix.GetCoordinates('0');
        var sum = 0;

        foreach (Coordinates start in startCoordinates)
        {
            var submitReached = new List<Coordinates>();
            Walk(matrix, start, 1, submitReached);

            sum += submitReached.DistinctBy(x => new { x.X, x.Y }).Count();
        }

        return sum;
    }


    public void Walk(Matrix matrix, Coordinates coordinates, int value, List<Coordinates> submitReached)
    {
        if (value == 10)
        {
            submitReached.Add(coordinates);
            // Console.WriteLine($"Submit reached: {coordinates.X}, {coordinates.Y} --> {submitReached}");
            return;
        }

        var direction = Direction.Up;

        // Console.WriteLine($"{matrix.GetValue(coordinates)}: {coordinates.X}, {coordinates.Y}");

        do
        {
            if (matrix.GetValueInFront(direction, coordinates) == value.ToString().ToCharArray()[0])
            {
                Walk(matrix, matrix.Move(direction, coordinates), value + 1, submitReached);
            }

            direction = (Direction)(((int)direction + 1) % 4);
        } while (direction != Direction.Up);
    }


    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());
       
        var startCoordinates = matrix.GetCoordinates('0');
        var sum = 0;

        foreach (Coordinates start in startCoordinates)
        {
            var submitReached = new List<Coordinates>();
            Walk(matrix, start, 1, submitReached);

            sum += submitReached.Count;
        }

        return sum;
    }

    public class Matrix : Models.Matrix
    {
        public Matrix(char[,] data) : base(data)
        {
        }

        public char? GetValueInFront(Direction direction, Coordinates position)
        {
            switch (direction)
            {
                case Direction.Up:
                    position = new Coordinates(position.X - 1, position.Y);
                    break;
                case Direction.Right:
                    position = new Coordinates(position.X, position.Y + 1);
                    break;
                case Direction.Down:
                    position = new Coordinates(position.X + 1, position.Y);
                    break;
                case Direction.Left:
                    position = new Coordinates(position.X, position.Y - 1);
                    break;
            }

            if (IsOutOfBox(position)) return null;

            return GetValue(position);
        }


        public Coordinates Move(Direction direction, Coordinates position)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Coordinates(position.X - 1, position.Y);
                case Direction.Right:
                    return new Coordinates(position.X, position.Y + 1);
                case Direction.Down:
                    return new Coordinates(position.X + 1, position.Y);
                case Direction.Left:
                    return new Coordinates(position.X, position.Y - 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}