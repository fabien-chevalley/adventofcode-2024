namespace aoc_2024;

public interface IPuzzle;

public abstract class Puzzle : IPuzzle
{
    protected virtual string Filename => $"./Inputs/{GetType().Name}.input";

    public abstract ValueTask<long> PartOne();

    public abstract ValueTask<long> PartTwo();
}