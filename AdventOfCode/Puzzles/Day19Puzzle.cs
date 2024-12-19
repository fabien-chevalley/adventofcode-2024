namespace AdventOfCode.Puzzles;

public class Day19Puzzle : Puzzle
{
    private readonly Dictionary<string, long> _counts = new();
    private readonly HashSet<string> _foundDesigns = new();

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);

        var towels = lines[0].Split(", ").OrderByDescending(x => x.Length).ToArray();
        var designs = lines[2..].ToArray();

        return designs.Count(design => Search(design, towels));
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllLinesAsync(Filename);

        var towels = lines[0].Split(", ").OrderByDescending(x => x.Length).ToArray();
        var designs = lines[2..].ToArray();

        return designs.Sum(design => Search2(design, towels));
    }

    private bool Search(string design, string[] towels)
    {
        if (_foundDesigns.Contains(design)) return true;

        if (towels.Contains(design)) _foundDesigns.Add(design);

        foreach (var towel in towels.Where(design.StartsWith))
        {
            if (Search(design.Substring(towel.Length), towels)) _foundDesigns.Add(design);
        }

        return _foundDesigns.Contains(design);
    }

    private long Search2(string design, string[] towels)
    {
        if (_counts.TryGetValue(design, out var count)) return count;

        if (towels.Contains(design)) _counts.Add(design, 1L);

        foreach (var towel in towels.Where(design.StartsWith))
        {
            _counts.TryAdd(design, 0L);
            _counts[design] += Search2(design.Substring(towel.Length), towels);
        }

        return _counts.GetValueOrDefault(design, 0);
    }
}