using System.Text.RegularExpressions;

namespace aoc_2024.Puzzles;

public class Day5Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var lines = await File.ReadAllTextAsync(Filename);

        var data = GetData(lines);

        return data.Updates.Where(x => !Fail(x, data)).Sum(update => update[update.Length / 2]);
    }

    public override async ValueTask<long> PartTwo()
    {
        var lines = await File.ReadAllTextAsync(Filename);

        var data = GetData(lines);
        var sum = 0;

        foreach (var update in data.Updates.Where(u => Fail(u, data)))
        {
            var ordered = new List<int>();
            while(ordered.Count < update.Length)
            {
                foreach (var page in update.Where(x => !ordered.Contains(x)))
                {
                    var copy = update.ToList();
                    copy.Remove(page);
                    copy.RemoveAll(x => ordered.Contains(x));
                    if (copy.All(x => data.AfterPagesLookup.ContainsKey(page) && data.AfterPagesLookup[page].Contains(x)))
                    {
                        ordered.Add(page);
                        break;
                    }
                }
            }
            
            sum += ordered[ordered.Count / 2];
        }

        return sum;
    }

    private static bool Fail(int[] update, Data data)
    {
        var fail = false;
        for (var i = 0; i < update.Length; i++)
        {
            var page = update[i];

            if (i + 1 < update.Length)
                for (var j = i + 1; j < update.Length; j++)
                {
                    if (data.AfterPagesLookup.ContainsKey(update[j]) && data.AfterPagesLookup[update[j]].Contains(page))
                    {
                        fail = true;
                        break;
                    }
                }

            if (i - 1 > 0)
                for (var j = i - 1; j > 0; j--)
                {
                    if (data.BeforePagesLookup.ContainsKey(update[j]) &&
                        data.BeforePagesLookup[update[j]].Contains(page))
                    {
                        fail = true;
                        break;
                    }
                }

            if (fail) break;
        }

        return fail;
    }

    private static Data GetData(string lines)
    {
        var updates = new List<int[]>();
        var afterPagesLookup = new Dictionary<int, List<int>>();
        var beforePagesLookup = new Dictionary<int, List<int>>();
        var matches = Regex.Matches(lines, @"(?<tuple>\d{2}\|\d{2})|(?<update>[0-9,]+)");
        foreach (Match match in matches)
        {
            if (match.Groups["tuple"].Success)
            {
                var tuple = match.Groups["tuple"].Value.Split("|").Select(int.Parse).ToArray();
                if (!afterPagesLookup.ContainsKey(tuple[0])) afterPagesLookup.Add(tuple[0], new List<int>());

                if (!beforePagesLookup.ContainsKey(tuple[1])) beforePagesLookup.Add(tuple[1], new List<int>());

                afterPagesLookup[tuple[0]].Add(tuple[1]);
                beforePagesLookup[tuple[1]].Add(tuple[0]);
            }

            if (match.Groups["update"].Success)
                updates.Add(match.Groups["update"].Value.Split(",").Select(int.Parse).ToArray());
        }

        return new Data(updates, afterPagesLookup, beforePagesLookup);
    }

    private record Data(
        List<int[]> Updates,
        Dictionary<int, List<int>> AfterPagesLookup,
        Dictionary<int, List<int>> BeforePagesLookup);
}