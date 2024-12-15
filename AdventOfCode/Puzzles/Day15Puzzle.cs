using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day15Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Where(l => l.StartsWith('#'))
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        var directions =
            lines
                .SkipWhile(l => l.StartsWith('#'))
                .Skip(1)
                .Aggregate((line, next) => line + next);


        foreach (var direction in directions)
        {
            // Console.WriteLine(direction);
            var valuesInFront = matrix.GetValuesInFront(GetDirection(direction)).ToList();
            if (valuesInFront.Contains('.'))
            {
                var index = valuesInFront.IndexOf('.');
                var wallindex = valuesInFront.IndexOf('#');
                if (wallindex == -1 || index < wallindex)
                    matrix.Push(GetDirection(direction), index);
            }

            // matrix.Print();
        }

        return matrix.GetCoordinates('O').Select(c => 100L * c.X + c.Y).Sum();
    }

    private static Direction GetDirection(char direction)
    {
        switch (direction)
        {
            case '^': return Direction.Up;
            case '>': return Direction.Right;
            case 'v': return Direction.Down;
            case '<': return Direction.Left;
            default: throw new Exception($"Invalid direction {direction}");
        }
    }

    public char[] Expand(char character)
    {
        switch (character)
        {
            case '#': return new[] { '#', '#' };
            case 'O': return new[] { '[', ']' };
            case '.': return new[] { '.', '.' };
            case '@': return new[] { '@', '.' };
            default: throw new Exception($"Invalid character {character}");
        }
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Where(l => l.StartsWith('#'))
            .Select(l => l.SelectMany(Expand).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        var directions =
            lines
                .SkipWhile(l => l.StartsWith('#'))
                .Skip(1)
                .Aggregate((line, next) => line + next);

        foreach (var direction in directions)
        {
            if (matrix.GetValueInFront(GetDirection(direction)) == '.')
            {
                matrix.Push(GetDirection(direction), 0);
                continue;
            }

            if (GetDirection(direction) == Direction.Left || GetDirection(direction) == Direction.Right)
            {
                var valuesInFront = matrix.GetValuesInFront(GetDirection(direction)).ToList();
                if (valuesInFront.Contains('.'))
                {
                    var index = valuesInFront.IndexOf('.');
                    var wallindex = valuesInFront.IndexOf('#');
                    if (wallindex == -1 || index < wallindex)
                        matrix.Push2(GetDirection(direction), index);
                }

                continue;
            }

            var boxes = new List<Coordinates>();
            var canPush = matrix.CanPush(GetDirection(direction), matrix.RobotPosition, boxes);
            if (canPush) matrix.Push3(GetDirection(direction), boxes);
        }

        return matrix.GetCoordinates('[').Select(c => 100L * c.X + c.Y).Sum();
    }


    public class Matrix : Models.Matrix
    {
        public Matrix(char[,] data) : base(data)
        {
            RobotPosition = GetCoordinates('@').Single();
        }

        public Coordinates RobotPosition { get; set; }

        public Coordinates? AdjacentBox(Direction direction, Coordinates position)
        {
            var value = GetValue(position);

            if (value == '[') return new Coordinates(position.X, position.Y + 1);
            if (value == ']') return new Coordinates(position.X, position.Y - 1);

            return null;
        }

        public bool CanPush(Direction direction, Coordinates position, List<Coordinates> boxes)
        {
            var value = GetValueInFront(direction, position);
            if (value == '#') return false;

            var nextPosition = Move(direction, position);
            var adjacentBox = AdjacentBox(direction, nextPosition);
            if (adjacentBox != null)
            {
                if (!boxes.Any(b => b.X == nextPosition.X && b.Y == nextPosition.Y)) boxes.Add(nextPosition);

                if (!boxes.Any(b => b.X == adjacentBox.X && b.Y == adjacentBox.Y)) boxes.Add(adjacentBox);

                var canPush = CanPush(direction, nextPosition, boxes);
                canPush &= CanPush(direction, adjacentBox, boxes);

                return canPush;
            }

            return true;
        }


        public void Print()
        {
            for (var row = 0; row < Data.GetLength(0); row++)
            {
                for (var col = 0; col < Data.GetLength(1); col++) Console.Write(Data[row, col]);
                Console.WriteLine();
            }
        }

        public char[] GetValuesInFront(Direction direction)
        {
            var values = new List<char>();
            var position = RobotPosition;
            while (!IsOutOfBox(Move(direction, position)))
            {
                values.Add(GetValueInFront(direction, position)!.Value);
                position = Move(direction, position);
            }

            return values.ToArray();
        }

        public char? GetValueInFront(Direction direction)
        {
            return GetValueInFront(direction, RobotPosition);
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

        public void Move(Direction direction)
        {
            RobotPosition = Move(direction, RobotPosition);
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

        public void Push(Direction direction, int index)
        {
            var position = RobotPosition;
            SetValue(position, '.');


            position = Move(direction, position);
            SetValue(position, '@');
            RobotPosition = position;

            if (index == 0) return;

            for (var i = 0; i < index; i++)
            {
                position = Move(direction, position);
                SetValue(position, 'O');
            }
        }

        public void Push2(Direction direction, int index)
        {
            var position = RobotPosition;
            SetValue(position, '.');

            position = Move(direction, position);
            SetValue(position, '@');
            RobotPosition = position;

            if (index == 0) return;

            for (var i = 0; i < index; i = i + 2)
            {
                if (direction == Direction.Right)
                {
                    position = Move(direction, position);
                    SetValue(position, '[');
                    position = Move(direction, position);
                    SetValue(position, ']');
                }

                if (direction == Direction.Left)
                {
                    position = Move(direction, position);
                    SetValue(position, ']');
                    position = Move(direction, position);
                    SetValue(position, '[');
                }
            }
        }

        public void Push3(Direction direction, List<Coordinates> boxes)
        {
            var position = RobotPosition;

            if (direction == Direction.Up)
                foreach (var coordinate in boxes.OrderBy(b => b.X))
                {
                    var value = GetValue(coordinate);
                    SetValue(coordinate, '.');
                    SetValue(Move(direction, coordinate), value);
                }

            if (direction == Direction.Down)
                foreach (var coordinate in boxes.OrderByDescending(b => b.X))
                {
                    var value = GetValue(coordinate);
                    SetValue(coordinate, '.');
                    SetValue(Move(direction, coordinate), value);
                }

            SetValue(Move(direction, position), '@');
            SetValue(position, '.');

            RobotPosition = Move(direction, position);
        }

        public List<Coordinates> ConnectedBoxes(Direction direction)
        {
            if (direction == Direction.Left || direction == Direction.Right) Enumerable.Empty<Coordinates>();

            throw new NotImplementedException();
        }
    }
}