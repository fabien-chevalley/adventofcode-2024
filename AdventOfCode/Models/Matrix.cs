using System.Collections;

namespace AdventOfCode.Models;

public class Matrix : IEnumerable<Coordinates>
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

    public char this[Coordinates coordinates]
    {
        get => GetValue(coordinates);
        set => SetValue(coordinates, value);
    }

    public IEnumerator<Coordinates> GetEnumerator()
    {
        for (int row = 0; row < Data.GetLength(0); row++)
        {
            for (int col = 0; col < Data.GetLength(1); col++)
            {
                yield return new Coordinates(row, col);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}