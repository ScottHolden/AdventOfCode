namespace AdventOfCode;

[PuzzleInput("Day01.txt", 1655, 1683)]
[PuzzleInput("Day01-Sample.txt", 7, 5)]
public class Day01 : IDay
{
    public long Part1(string input) => Part1(input.ParseLinesAsLong());
    private static long Part1(long[] input) => input.Skip(1).Select((x, i) => x - input[i]).Count(x => x > 0);
    public long Part2(string input) => Part2(input.ParseLinesAsLong());
    private static long Part2(long[] input) => Part1(input.Skip(2).Select((x, i) => x + input[i] + input[i + 1]).ToArray());
}
