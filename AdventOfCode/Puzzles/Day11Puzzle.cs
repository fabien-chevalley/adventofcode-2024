namespace AdventOfCode.Puzzles;

public class Day11Puzzle : Puzzle
{
    private readonly Dictionary<string, long> _cache = new();

    public override async ValueTask<long> PartOne()
    {
        var line = await File.ReadAllTextAsync(Filename);
        var stones = line.Split(' ').Select(long.Parse).ToList();

        for (var i = 0; i < 25; i++)
        {
            stones = Blink(stones);
        }

        return stones.Count;
    }

    public override async ValueTask<long> PartTwo()
    {
        var line = await File.ReadAllTextAsync(Filename);
        var stones = line.Split(' ').Select(long.Parse).ToList();

        long count = stones.Count;
        foreach (var stone in stones)
        {
            count += Solve(stone);
        }

        return count;
    }

    private List<long> Blink(List<long> stones)
    {
        var afterBlinkStones = new List<long>();
        for (var i = 0; i < stones.Count; i++)
        {
            if (stones[i] == 0)
            {
                afterBlinkStones.Add(1);
                continue;
            }

            var stringValue = stones[i].ToString();
            if (stringValue.Length % 2 == 0)
            {
                afterBlinkStones.Add(int.Parse(stringValue.Substring(0, stringValue.Length / 2)));
                afterBlinkStones.Add(int.Parse(stringValue.Substring(stringValue.Length / 2,
                    stringValue.Length - stringValue.Length / 2)));
                continue;
            }

            afterBlinkStones.Add(stones[i] * 2024);
        }

        return afterBlinkStones;
    }

    private long Solve(long stone, int level = 0)
    {
        if (_cache.ContainsKey($"{stone}-{level}")) return _cache[$"{stone}-{level}"];

        if (level == 75) return 0;

        if (stone == 0)
        {
            var result0 = Solve(1, level + 1);
            _cache[$"{stone}-{level}"] = result0;
            return result0;
        }

        var stringValue = stone.ToString();
        if (stringValue.Length % 2 == 0)
        {
            var splittedStoneOne = int.Parse(stringValue.Substring(0, stringValue.Length / 2));
            var count = Solve(splittedStoneOne, level + 1);

            var splittedStoneTwo =
                int.Parse(stringValue.Substring(stringValue.Length / 2, stringValue.Length - stringValue.Length / 2));
            count += Solve(splittedStoneTwo, level + 1);

            _cache[$"{stone}-{level}"] = count + 1;
            return count + 1;
        }

        var result = Solve(stone * 2024, level + 1);
        _cache[$"{stone * 2024}-{level + 1}"] = result;
        return result;
    }
}