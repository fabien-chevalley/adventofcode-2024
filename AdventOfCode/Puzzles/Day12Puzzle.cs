using AdventOfCode.Helpers;
using AdventOfCode.Models;

using Area = AdventOfCode.Models.Coordinates[];
namespace AdventOfCode.Puzzles;

public class Day12Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        
        var areas = new List<Area>();
        var visited = new List<Coordinates>();
        foreach (var coordinates in matrix)
        {
            if (!visited.Contains(coordinates))
            {
                var area = new List<Coordinates>{coordinates};
                Walk(matrix, coordinates, area);
                visited.AddRange(area);
                areas.Add(area.ToArray());
            }
        }

        return areas.Sum(x => Price(matrix, x));
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);


        return 0;
    }

    private void Walk(Matrix matrix, Coordinates coordinates, List<Coordinates> area)
    {
        var direction = Direction.Up;
        var value = matrix.GetValue(coordinates);
        do
        {
            var newCoordinates = matrix.Move(direction, coordinates);
            if (!matrix.IsOutOfBox(newCoordinates) && 
                matrix.GetValue(newCoordinates) == value &&
                !area.Contains(newCoordinates))
            {
                area.Add(newCoordinates);
                Walk(matrix, newCoordinates, area);
            }

            direction = (Direction)(((int)direction + 1) % 4);
        } while (direction != Direction.Up);
    }

    private long Price(Matrix matrix, Area area)
    {
        var perimeter = 0L;
        foreach (var coordinates in area)
        {
            var direction = Direction.Up;
            var value = matrix.GetValue(coordinates);
            do
            {
                if (matrix.GetValueInFront(direction, coordinates) != value)
                {
                    perimeter++;
                }

                direction = (Direction)(((int)direction + 1) % 4);
            } while (direction != Direction.Up);
        }

        return perimeter * area.Length;
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