using System.Text.RegularExpressions;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day18Puzzle : Puzzle
{
    public const int Size = 70;
    public const int Length = 1024;

    public static char[,] CreateMatrix(int width, int height)
    {
        var rectangularArray = new char[width, height];
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            rectangularArray[i, j] = '.';
        }

        return rectangularArray;
    }

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllTextAsync(Filename);

        var matrix = new Matrix(CreateMatrix(Size + 1, Size + 1));

        var matches = Regex.Matches(lines, @"(\d+),(\d+)");
        var coordinates = matches.Select(m =>
                new Coordinates(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value)))
            .ToArray();

        foreach (var coordinate in coordinates.Take(Length))
        {
            matrix.SetValue(coordinate, '#');
        }

        return GetBestPath(new Coordinates(Size, Size), matrix);
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllTextAsync(Filename);


        return 0;
    }

    private long GetBestPath(Coordinates endPosition, Matrix matrix)
    {
        var bestScore = long.MaxValue;
        var visited = new Dictionary<Coordinates, long>();
        var cellsToProcess = new Queue<Cell>([Cell.Zero]);

        while (cellsToProcess.TryDequeue(out var cell))
        {
            if (cell.Score >= bestScore) continue;
            if (cell.Position == endPosition)
            {
                bestScore = cell.Score;
                continue;
            }

            var nextScore = cell.Score + 1;
            List<Cell> nextCells =
            [
                new(matrix.Move(Direction.Up, cell.Position), nextScore),
                new(matrix.Move(Direction.Right, cell.Position), nextScore),
                new(matrix.Move(Direction.Down, cell.Position), nextScore),
                new(matrix.Move(Direction.Left, cell.Position), nextScore)
            ];

            foreach (var nextCell in nextCells)
            {
                if (matrix.IsOutOfBox(nextCell.Position)) continue;
                if (matrix.GetValue(nextCell.Position) == '#') continue;

                var best = visited.GetValueOrDefault(nextCell.Position, long.MaxValue);
                if (nextScore >= best) continue;

                visited[nextCell.Position] = Math.Min(nextScore, best);
                cellsToProcess.Enqueue(nextCell);
            }
        }

        return bestScore;
    }

    private record Cell(Coordinates Position, long Score)
    {
        public static readonly Cell Zero = new(Coordinates.Zero, 0);
    }

    public class Matrix : Models.Matrix
    {
        public Matrix(char[,] data) : base(data)
        {
        }

        public void Print()
        {
            for (var row = 0; row < Data.GetLength(0); row++)
            {
                for (var col = 0; col < Data.GetLength(1); col++)
                {
                    Console.Write(Data[row, col]);
                }

                Console.WriteLine();
            }
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

        public Direction RotateClockwise(Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public Direction RotateCounterClockwise(Direction direction)
        {
            return (Direction)(((int)direction + 3) % 4);
        }
    }
}