namespace AdventOfCode.Puzzles;

public class Day19Puzzle : Puzzle
{
    private readonly HashSet<string> _foundDesigns = new();

    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllLinesAsync(Filename);

        var towels = lines[0].Split(", ").OrderByDescending(x => x.Length).ToArray();
        var designs = lines[2..].ToArray();

        return designs.Count(design => Search(design, towels));
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

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllTextAsync(Filename);

        return 0;
    }
}