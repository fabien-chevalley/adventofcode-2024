using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.Puzzles;

public class Day6Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        while (!matrix.IsOutOfBox())
        {
            var value = matrix.GetValueInFront();
            while (value == '#')
            {
                matrix.Rotate();
                value = matrix.GetValueInFront();
            }

            matrix.Move();
        }

        return matrix.PathLength;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()
            .ConvertJaggedToRectangular());

        Walk(matrix);

        var coordinates = matrix.Visited;

        var loops = 0;
        foreach (var coordinate in coordinates.Where(x => matrix.GetValue(x) == '.'))
        {
            var matrixCopy = new Matrix(matrix);
            matrixCopy.SetValue(coordinate, '#');

            if (Walk(matrixCopy, true)) loops++;

            matrixCopy.SetValue(coordinate, '.');
        }

        return loops;
    }

    private static bool Walk(Matrix matrix, bool detectLoop = false)
    {
        var count = 0;
        while (!matrix.IsOutOfBox())
        {
            var value = matrix.GetValueInFront();
            while (value == '#')
            {
                matrix.Rotate();
                value = matrix.GetValueInFront();
            }

            if (detectLoop && count++ > 100) return true;

            matrix.Move();
        }

        return false;
    }

    private class Matrix : Models.Matrix
    {
        private readonly List<Coordinates> _visited = new();
        private Direction _direction;
        private Coordinates _position;

        public Matrix(char[,] data) : base(data)
        {
            _position = GetCoordinate('^');
            _direction = Direction.Up;
            _visited.Add(_position);
        }

        public Matrix(Matrix matrix) : base(matrix.Data)
        {
            _position = GetCoordinate('^');
            _direction = Direction.Up;
            _visited.Add(_position);
        }

        public int PathLength => _visited.Count;

        public List<Coordinates> Visited => _visited.ToList();

        public Coordinates GetCoordinate(char value)
        {
            var w = Data.GetLength(0); // width
            var h = Data.GetLength(1); // height

            for (var x = 0; x < w; ++x)
            for (var y = 0; y < h; ++y)
                if (Data[x, y].Equals(value))
                    return new Coordinates(x, y);

            return new Coordinates(-1, -1);
        }

        public char? GetValueInFront()
        {
            Coordinates? position = null;
            switch (_direction)
            {
                case Direction.Up:
                    position = new Coordinates(_position.X - 1, _position.Y);
                    break;
                case Direction.Right:
                    position = new Coordinates(_position.X, _position.Y + 1);
                    break;
                case Direction.Down:
                    position = new Coordinates(_position.X + 1, _position.Y);
                    break;
                case Direction.Left:
                    position = new Coordinates(_position.X, _position.Y - 1);
                    break;
            }

            if (position == null || IsOutOfBox(position)) return null;

            return GetValue(position);
        }

        public bool IsOutOfBox()
        {
            return IsOutOfBox(_position);
        }

        public void Move()
        {
            switch (_direction)
            {
                case Direction.Up:
                    _position = new Coordinates(_position.X - 1, _position.Y);
                    break;
                case Direction.Right:
                    _position = new Coordinates(_position.X, _position.Y + 1);
                    break;
                case Direction.Down:
                    _position = new Coordinates(_position.X + 1, _position.Y);
                    break;
                case Direction.Left:
                    _position = new Coordinates(_position.X, _position.Y - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!_visited.Contains(_position) && !IsOutOfBox()) _visited.Add(_position);
        }

        public void Rotate()
        {
            _direction = (Direction)(((int)_direction + 1) % 4);
        }
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}