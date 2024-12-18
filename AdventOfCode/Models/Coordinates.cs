namespace AdventOfCode.Models;

public record Coordinates(int X, int Y)
{
    public static readonly Coordinates Zero = new(0, 0);
}