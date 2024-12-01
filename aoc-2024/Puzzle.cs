using System.Reflection;
using System.Text.RegularExpressions;

namespace aoc_2024;

public class Result<TResult>(TResult value)
{
    private readonly TResult _value = value;
}

public abstract class Puzzle<TResult>
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

    protected virtual StreamReader Reader
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = $"{GetType().FullName}.input";
            var stream = assembly.GetManifestResourceStream(resourceName) ??
                         throw new ArgumentOutOfRangeException(nameof(resourceName),
                             $"Can not find embedded resource named {resourceName}");

            return new StreamReader(stream);
        }
    }

    public abstract ValueTask<TResult> PartOne();

    public abstract ValueTask<TResult> PartTwo();
}