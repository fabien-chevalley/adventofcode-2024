using System.Text.RegularExpressions;

namespace aoc_2024;

public interface IPuzzle;

public abstract class Puzzle<TResult> : IPuzzle
    where TResult : new()
{
    protected virtual string Filename => $"./Inputs/{GetType().Name}.input";

    public abstract ValueTask<TResult> PartOne();

    public abstract ValueTask<TResult> PartTwo();
}