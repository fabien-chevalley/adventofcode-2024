using AdventOfCode.Puzzles;

namespace AdventOfCode.Test;

public class ResolutionTests
{
    [Test]
    [Arguments(typeof(Day1Puzzle), 1319616, 27267728)]
    [Arguments(typeof(Day2Puzzle), 218, 290)]
    [Arguments(typeof(Day3Puzzle), 188116424, 104245808)]
    [Arguments(typeof(Day4Puzzle), 2560, 1910)]
    [Arguments(typeof(Day5Puzzle), 6034, 6305)]
    [Arguments(typeof(Day6Puzzle), 4988, 4985)]
    [Arguments(typeof(Day7Puzzle), 267566105056, 116094961956019)]
    [Arguments(typeof(Day8Puzzle), 256, 1005)]
    [Arguments(typeof(Day9Puzzle), 6395800119709, 6418529470362)]
    [Arguments(typeof(Day11Puzzle), 184927, 220357186726677)]
    public async Task Year2024(Type type, long partOneResult, long partTwoResult)
    {
        if (Activator.CreateInstance(type) is not Puzzle puzzle) throw new InvalidOperationException();

        puzzle.Filename = $"../../../../AdventOfCode/Inputs/{type.Name}.input";
        await Assert.That(await puzzle.PartOne()).IsEqualTo(partOneResult);
        await Assert.That(await puzzle.PartTwo()).IsEqualTo(partTwoResult);
    }
}