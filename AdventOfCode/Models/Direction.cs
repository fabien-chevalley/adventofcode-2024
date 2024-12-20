namespace AdventOfCode.Models;

public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public static class DirectionExtensions
{
    public static Direction RotateClockwise(this Direction direction)
    {
        return (Direction)(((int)direction + 1) % 4);
    }

    public static Direction RotateCounterClockwise(this Direction direction)
    {
        return (Direction)(((int)direction + 3) % 4);
    }
}