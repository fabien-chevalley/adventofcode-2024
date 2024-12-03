using System.Diagnostics;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace aoc_2024;

public static class Solver
{
    public static async Task Solve<TPuzzle, TResult>()
        where TPuzzle : Puzzle<TResult>, new()
        where TResult : new()
    {
        var results = await Solve<long>(typeof(TPuzzle));
        
        var table = CreateTable();
        table.AddRow(PuzzleName(typeof(TPuzzle)), results.PartOne.ToString(), results.PartTwo.ToString());
        AnsiConsole.Write(table);
    }
    
    public static async Task SolveLast()
    {
        var type = typeof(Puzzle<>).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPuzzle)))
            .OrderBy(x => x.Name)
            .Last();
        
        var results = await Solve<long>(type);
        
        var table = CreateTable();
        table.AddRow(PuzzleName(type), results.PartOne.ToString(), results.PartTwo.ToString());
        AnsiConsole.Write(table);
    }

    public static async Task SolveAll()
    {
        var types = typeof(Puzzle<>).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPuzzle)))
            .OrderBy(x => x.Name);

        var table = CreateTable();
        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                foreach (var type in types)
                {
                    var results = await Solve<long>(type);
                    table.AddRow(PuzzleName(type), results.PartOne.ToString(), results.PartTwo.ToString());
                    ctx.Refresh();
                }
            });
    }

    private static async Task<Results<TResult>> Solve<TResult>(Type type)
        where TResult : new()
    {
        if (Activator.CreateInstance(type) is not Puzzle<TResult> puzzle) throw new InvalidOperationException();

        var stopwatch = Stopwatch.StartNew();
        var partOneResult = new Result<TResult>(await puzzle.PartOne(), stopwatch.Elapsed);

        stopwatch.Restart();
        var partTwoResult = new Result<TResult>(await puzzle.PartTwo(), stopwatch.Elapsed);
        return new Results<TResult>(partOneResult, partTwoResult);
    }

    private static string PuzzleName(Type type)
    {
        if(!type.IsAssignableTo(typeof(IPuzzle))) throw new InvalidOperationException();
        
        var typeName = type.Name;
        var match = Regex.Match(typeName, "Day(?<day>[0-9]{1,2})Puzzle");
        return match.Success ? $"Day {match.Groups["day"].Value}" : typeName;
    }
    
    private static Table CreateTable()
    {
        return new Table()
            .AddColumns(
                "[bold]Puzzle[/]",
                "[bold]Part one[/]",
                "[bold]Part two[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
    }

    private sealed record Results<TResult>(Result<TResult> PartOne, Result<TResult> PartTwo);

    private sealed record Result<TResult>(TResult Value, TimeSpan Elapsed)
    {
        public override string ToString()
        {
            return $"{Value} ({Elapsed.TotalMilliseconds}ms)";
        }
    }
}