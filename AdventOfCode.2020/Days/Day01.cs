namespace AdventOfCode;

[PuzzleInput("Day01.txt", 997899, 131248694)]
[PuzzleInput("Day01-Sample.txt", 514579, 241861950)]
public class Day01 : IDay
{
    public long Part1(string input) => Part1(input.ParseLinesAsLong());

    private static long Part1(long[] input)
        => Solve(input.SelectMany(x => input.Select(y => new[] { x, y })));

    public long Part2(string input) => Part2(input.ParseLinesAsLong());

    private static long Part2(long[] input)
        => Solve(input.SelectMany(x => input.SelectMany(y => input.Select(z => new[] { x, y, z }))));

    private static long Solve(IEnumerable<long[]> input)
        => input.First(x => x.Sum() == 2020)
                .Aggregate(1L, (x, y) => x * y);
}
