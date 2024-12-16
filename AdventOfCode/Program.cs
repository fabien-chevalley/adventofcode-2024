using AdventOfCode;
using AdventOfCode.Puzzles;

await Solver.Solve<Day16Puzzle>();

// var mode = AnsiConsole.Prompt(
//     new SelectionPrompt<string>()
//         .Title("Solve which [red]puzzle[/]?")
//         .AddChoices("single", "last", "all"));
//
// if (mode == "single")
// {
//     // TODO : add prompt to select day
//     await Solver.Solve<Day1Puzzle>();
// }
//
// if (mode == "last") await Solver.SolveLast();
//
// if (mode == "all") await Solver.SolveAll();