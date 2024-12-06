namespace aoc_2024.Puzzles;

public class Day6Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix(ConvertJaggedToRectangular(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()));


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
        var lines = await File.ReadAllTextAsync(Filename);

        var sum = 0;

        return sum;
    }

    private char[,] ConvertJaggedToRectangular(char[][] jaggedArray)
    {
        var rows = jaggedArray.Length;
        var columns = jaggedArray[0].Length;

        var rectangularArray = new char[rows, columns];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                rectangularArray[i, j] = jaggedArray[i][j];
            }
        }

        return rectangularArray;
    }
}

public class Matrix
{
    private readonly char[,] _data;
    private readonly List<Coordinates> _visited = new();
    private Direction _direction;
    private Coordinates _position;

    public Matrix(char[,] data)
    {
        _data = data;
        _position = GetCoordinates('^');
        _direction = Direction.Up;
        _visited.Add(_position);
    }

    public int PathLength => _visited.Count;

    public Coordinates GetCoordinates(char value)
    {
        var w = _data.GetLength(0); // width
        var h = _data.GetLength(1); // height

        for (var x = 0; x < w; ++x)
        {
            for (var y = 0; y < h; ++y)
            {
                if (_data[x, y].Equals(value)) return new Coordinates(x, y);
            }
        }

        return new Coordinates(-1, -1);
    }

    public char GetValue()
    {
        return GetValue(_position);
    }

    public char GetValue(Coordinates position)
    {
        return _data[position.X, position.Y];
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

    public bool IsOutOfBox(Coordinates position)
    {
        return position.X < 0 || position.X >= _data.GetLength(0) ||
               position.Y < 0 || position.Y >= _data.GetLength(1);
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

    public record Coordinates(int X, int Y);
}

internal enum Direction
{
    Up,
    Right,
    Down,
    Left
}