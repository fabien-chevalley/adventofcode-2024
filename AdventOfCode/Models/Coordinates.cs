namespace AdventOfCode.Models;

public record Coordinates(int X, int Y)
{
    public static readonly Coordinates Zero = new(0, 0);

    public List<Coordinates> Neighbors(int distance)
    {
        return
        [
            this with { X = X - distance },
            this with { X = X + distance },
            this with { Y = Y - distance },
            this with { Y = Y + distance }
        ];
    }

    public int ManhattanDistance(Coordinates coordinate)
    {
        return Math.Abs(X - coordinate.X) + Math.Abs(Y - coordinate.Y);
    }
}