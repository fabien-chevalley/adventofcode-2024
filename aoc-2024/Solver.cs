using System.Diagnostics;
using Spectre.Console;

namespace aoc_2024;

public class Solver
{
    private readonly Table _table;

    public Solver()
    {
        _table = new Table()
            .AddColumns(
                "[bold]Puzzle[/]",
                "[bold]Part one[/]",
                "[bold]Part two[/]",
                "[bold]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
    }

    public async Task SolveLast()
    {
        var type = typeof(Puzzle<>).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPuzzle)))
            .OrderBy(x => x.Name)
            .Last();

        await Solve(type);
    }

    public async Task SolveAll()
    {
        var types = typeof(Puzzle<>).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPuzzle)))
            .OrderBy(x => x.Name);

        foreach (var type in types) await Solve(type);
    }

    public async Task Solve(Type type)
    {
        var puzzle = Activator.CreateInstance(type) as Puzzle<long>;

        await AnsiConsole.Live(_table)
            .StartAsync(async ctx =>
            {
                var stopwatch = Stopwatch.StartNew();
                var partOneResult = await puzzle.PartOne();
                var partOneDuration = stopwatch.Elapsed;
                _table.AddRow(puzzle.Name, partOneResult.ToString(), string.Empty, $"{partOneDuration.Milliseconds}ms");
                ctx.Refresh();

                stopwatch.Restart();
                var partTwoResult = await puzzle.PartTwo();
                var partTwoDuration = stopwatch.Elapsed;
                _table.UpdateCell(_table.Rows.Count - 1, 2, partTwoResult.ToString());
                _table.UpdateCell(_table.Rows.Count - 1, 3,
                    $"{partOneDuration.Milliseconds}ms / {partTwoDuration.Milliseconds}ms");
                ctx.Refresh();
            });
    }


    public async Task Solve<TPuzzle, TResult>()
        where TPuzzle : Puzzle<TResult>, new()
        where TResult : new()
    {
        await Solve(typeof(TPuzzle));
    }
}