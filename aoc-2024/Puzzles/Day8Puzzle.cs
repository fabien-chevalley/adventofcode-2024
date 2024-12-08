namespace aoc_2024.Puzzles;

public class Day8Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix2(ConvertJaggedToRectangular(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()));

        List<Matrix2.Coordinates> references = new List<Matrix2.Coordinates>();
        foreach (var value in matrix.Distinct().Where(x => x != '.')) 
        {
            references.AddRange(Compute(matrix, value));
        }
        
        var sum = references
            .Where(x => !matrix.IsOutOfBox(x))
            .DistinctBy(x => new { x.X, x.Y })
            .Count();
        
        return sum;
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);
        var matrix = new Matrix2(ConvertJaggedToRectangular(lines
            .Select(l => l.Select(c => c).ToArray())
            .ToArray()));

        List<Matrix2.Coordinates> references = new List<Matrix2.Coordinates>();
        foreach (var value in matrix.Distinct().Where(x => x != '.')) 
        {
            references.AddRange(Compute(matrix, value, true));
        }
        
        var distinct = references
            .Where(x => !matrix.IsOutOfBox(x))
            .DistinctBy(x => new { x.X, x.Y })
            ;
        
        return distinct.Count();
    }

    private static List<Matrix2.Coordinates> Compute(Matrix2 matrix, char value, bool part2 = false)
    {
        var result = new List<Matrix2.Coordinates>();
        var coordinates = matrix.GetCoordinates(value);
        foreach (var first in coordinates)
        foreach (var second in coordinates.Where(x => x != first))
        {
            var distance = new Matrix2.Coordinates(
                second.X - first.X,
                second.Y - first.Y);
            
            result.Add(new Matrix2.Coordinates(first.X - distance.X, first.Y - distance.Y));

            if (part2)
            {
                while (!matrix.IsOutOfBox(result.Last()))
                {
                    result.Add(new Matrix2.Coordinates(result.Last().X - distance.X, result.Last().Y - distance.Y));
                }
            }
            
            result.Add(new Matrix2.Coordinates(second.X + distance.X, second.Y + distance.Y));
            
            if (part2)
            {
                while (!matrix.IsOutOfBox(result.Last()))
                {
                    result.Add(new Matrix2.Coordinates(result.Last().X + distance.X, result.Last().Y + distance.Y));
                }
            }
        }

        if (part2)
        {
            result.AddRange(coordinates);
        }

        return result;
    }
    
    private char[,] ConvertJaggedToRectangular(char[][] jaggedArray)
    {
        var rows = jaggedArray.Length;
        var columns = jaggedArray[0].Length;

        var rectangularArray = new char[rows, columns];
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
            rectangularArray[i, j] = jaggedArray[i][j];

        return rectangularArray;
    }
}

public class Matrix2
{
    private readonly char[,] _data;

    public Matrix2(char[,] data)
    {
        _data = data;
    }

    public List<char> Distinct()
    {
        return _data.Cast<char>().Distinct().ToList();
    }
    
    public List<Coordinates> GetCoordinates(char value)
    {
        var result = new List<Coordinates>();
        var w = _data.GetLength(0); // width
        var h = _data.GetLength(1); // height

        for (var x = 0; x < w; ++x)
        for (var y = 0; y < h; ++y)
            if (_data[x, y].Equals(value))
                result.Add(new Coordinates(x, y));

        return result;
    }

    public char GetValue(Coordinates position)
    {
        return _data[position.X, position.Y];
    }

    public void SetValue(Coordinates position, char value)
    {
        _data[position.X, position.Y] = value;
    }

    public bool IsOutOfBox(Coordinates position)
    {
        return position.X < 0 || position.X >= _data.GetLength(0) ||
               position.Y < 0 || position.Y >= _data.GetLength(1);
    }

    public record Coordinates(int X, int Y);
}