namespace AdventOfCode;

[PuzzleInput("Day06.txt", 6911, 3473)]
[PuzzleInput("Day06-Sample.txt", 11, 6)]
public class Day06 : IDay
{
    public long Part1(string input)
        => SumChars(input, (x, _) => x.Distinct().Count());
    public long Part2(string input)
        => SumChars(input, (x, c) => x.GroupBy(y => y).Count(y => y.Count() == c + 1));

    public static long SumChars(string input, Func<char[], int, long> action)
        => input.Replace("\r", "")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Sum(x => action(x.Replace("\n", "").ToCharArray(), x.Count(z => z == '\n')));
}
