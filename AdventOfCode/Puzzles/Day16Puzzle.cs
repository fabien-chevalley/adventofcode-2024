using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day16Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        Queue<Cell> nodesToProcess = new();
        Dictionary<Cell, long> visited = new(new CellComparer());

        var startPosition = matrix.GetCoordinates('S').Single();
        var endPosition = matrix.GetCoordinates('E').Single();

        var start = new Cell(startPosition, Direction.Right, 0);
        visited[start] = 0;
        nodesToProcess.Enqueue(start);

        var scores = new List<long>();
        while (nodesToProcess.TryDequeue(out var cell))
        {
            if (cell.Position == endPosition)
            {
                scores.Add(cell.Cost!.Value);
                continue;
            }

            if (visited.GetValueOrDefault(cell, int.MaxValue) < cell.Cost) continue;
            visited[cell] = cell.Cost!.Value;

            List<Cell> nextCells =
            [
                new(matrix.Move(cell.Direction, cell.Position),
                    cell.Direction),
                new(matrix.Move(matrix.RotateClockwise(cell.Direction), cell.Position),
                    matrix.RotateClockwise(cell.Direction)),
                new(matrix.Move(matrix.RotateCounterClockwise(cell.Direction), cell.Position),
                    matrix.RotateCounterClockwise(cell.Direction))
            ];

            foreach (var nextCell in nextCells.Where(nextCell => matrix.GetValue(nextCell.Position) != '#'))
                if (nextCell.Direction == cell.Direction)
                    nodesToProcess.Enqueue(nextCell with { Cost = cell.Cost + 1 });
                else
                    nodesToProcess.Enqueue(nextCell with { Cost = cell.Cost + 1001 });
        }

        return scores.Min();
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        Queue<Cell> nodesToProcess = new();
        Dictionary<Cell, long> visited = new(new CellComparer());

        var startPosition = matrix.GetCoordinates('S').Single();
        var endPosition = matrix.GetCoordinates('E').Single();

        var start = new Cell(startPosition, Direction.Right, 0, new List<Coordinates>(){startPosition});
        visited[start] = 0;
        nodesToProcess.Enqueue(start);
        
        var scores = new List<(long Score, List<Coordinates> Path)>();
        while (nodesToProcess.TryDequeue(out var cell))
        {
            if (cell.Position == endPosition)
            {
                cell.Path!.Add(cell.Position);
                scores.Add(new (
                    cell.Cost!.Value,
                    cell.Path));
                continue;
            }

            if (visited.GetValueOrDefault(cell, int.MaxValue) < cell.Cost) continue;
            visited[cell] = cell.Cost!.Value;

            List<Cell> nextCells =
            [
                new(matrix.Move(cell.Direction, cell.Position),
                    cell.Direction),
                new(matrix.Move(matrix.RotateClockwise(cell.Direction), cell.Position),
                    matrix.RotateClockwise(cell.Direction)),
                new(matrix.Move(matrix.RotateCounterClockwise(cell.Direction), cell.Position),
                    matrix.RotateCounterClockwise(cell.Direction))
            ];

            foreach (var nextCell in nextCells.Where(nextCell => matrix.GetValue(nextCell.Position) != '#'))
            {
                var nextPath = cell.Path!.ToList();
                nextPath.Add(nextCell.Position);
                
                if (nextCell.Direction == cell.Direction)
                    nodesToProcess.Enqueue(nextCell with { Cost = cell.Cost + 1, Path = nextPath});
                else
                    nodesToProcess.Enqueue(nextCell with { Cost = cell.Cost + 1001, Path = nextPath });
            }
        }

        var cells = new List<Coordinates>();
        var minScore = scores.MinBy(x => x.Score).Score;
        foreach (var path in scores.Where(x => x.Score == minScore).Select(x => x.Path))
        {
            cells.AddRange(path);
        }
        
        return cells.DistinctBy(c => new { c.X, c.Y }).Count();
    }

    private class CellComparer : IEqualityComparer<Cell>
    {
        public bool Equals(Cell a, Cell b)
        {
            return a.Position.X == b.Position.X &&
                   a.Position.Y == b.Position.Y &&
                   a.Direction == b.Direction;
        }

        public int GetHashCode(Cell a)
        {
            return a.Position.X.GetHashCode() ^
                   a.Position.Y.GetHashCode() ^
                   a.Direction.GetHashCode();
        }
    }

    private record Cell(Coordinates Position, Direction Direction, long? Cost = null, List<Coordinates>? Path = null);

    public class Matrix : Models.Matrix
    {
        public Matrix(char[,] data) : base(data)
        {
        }

        public void Print()
        {
            for (var row = 0; row < Data.GetLength(0); row++)
            {
                for (var col = 0; col < Data.GetLength(1); col++) Console.Write(Data[row, col]);
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