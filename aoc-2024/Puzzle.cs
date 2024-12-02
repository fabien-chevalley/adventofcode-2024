using System.Text.RegularExpressions;

namespace aoc_2024;

public interface IPuzzle;

public abstract class Puzzle<TResult> : IPuzzle
    where TResult : new()
{
    public virtual string Name
    {
        get
        {
            var typeName = GetType().Name;
            var match = Regex.Match(typeName, "Day(?<day>[0-9]{1,2})Puzzle");
            return match.Success ? $"Day {match.Groups["day"].Value}" : typeName;
        }
    }

    protected virtual string Filename => $"./Inputs/{GetType().Name}.input";

    public abstract ValueTask<TResult> PartOne();

    public abstract ValueTask<TResult> PartTwo();
}