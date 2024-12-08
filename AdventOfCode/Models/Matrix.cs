namespace AdventOfCode.Models;

public class Matrix
{
    protected readonly char[,] Data;

    public Matrix(char[,] data)
    {
        Data = data;
    }

    public List<char> Distinct()
    {
        return Data.Cast<char>().Distinct().ToList();
    }

    public List<Coordinates> GetCoordinates(char value)
    {
        var result = new List<Coordinates>();
        var w = Data.GetLength(0); // width
        var h = Data.GetLength(1); // height

        for (var x = 0; x < w; ++x)
        for (var y = 0; y < h; ++y)
            if (Data[x, y].Equals(value))
                result.Add(new Coordinates(x, y));

        return result;
    }

    public char GetValue(Coordinates position)
    {
        return Data[position.X, position.Y];
    }

    public void SetValue(Coordinates position, char value)
    {
        Data[position.X, position.Y] = value;
    }

    public bool IsOutOfBox(Coordinates position)
    {
        return position.X < 0 || position.X >= Data.GetLength(0) ||
               position.Y < 0 || position.Y >= Data.GetLength(1);
    }
}