using System.Diagnostics;
using Spectre.Console;

namespace aoc_2024;

public static class Solver
{
    public static async Task Solve<TPuzzle, TResult>() where TPuzzle : Puzzle<TResult>, new()
    {
        var puzzle = new TPuzzle();

        var table = new Table()
            .AddColumns(
                "[bold]Puzzle[/]",
                "[bold]Part[/]",
                "[bold]Solution[/]",
                "[bold]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                var stopwatch = Stopwatch.StartNew();
                var partOneResult = await puzzle.PartOne();
                table.AddRow(puzzle.Name, "one", partOneResult.ToString(), stopwatch.ElapsedMilliseconds.ToString());

                ctx.Refresh();

                stopwatch.Restart();
                var partTwoResult = await puzzle.PartTwo();
                table.AddRow(puzzle.Name, "two", partTwoResult.ToString(), stopwatch.ElapsedMilliseconds.ToString());

                ctx.Refresh();
            });
    }
}