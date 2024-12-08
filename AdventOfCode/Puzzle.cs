namespace AdventOfCode;

public interface IPuzzle;

public abstract class Puzzle : IPuzzle
{
    public string Filename { get; set; }

    public abstract ValueTask<long> PartOne();

    public abstract ValueTask<long> PartTwo();
}