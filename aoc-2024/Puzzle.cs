using System.Reflection;
using System.Text.RegularExpressions;

namespace aoc_2024;

public abstract class Puzzle
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

    protected virtual string Input
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = $"{GetType().FullName}.input";
            using var stream = assembly.GetManifestResourceStream(resourceName) ??
                               throw new ArgumentOutOfRangeException(nameof(resourceName),
                                   $"Can not find embedded resource named {resourceName}");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }

    public abstract ValueTask<string> PartOne();

    public abstract ValueTask<string> PartTwo();
}