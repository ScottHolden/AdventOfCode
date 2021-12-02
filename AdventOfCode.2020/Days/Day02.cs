namespace AdventOfCode;

[PuzzleInput("Day02.txt", 572, 306)]
[PuzzleInput("Day02-Sample.txt", 2, 1)]
public class Day02 : IDay
{
    public long Part1(string input) => Solve(input, ValidPart1);

    public long Part2(string input) => Solve(input, ValidPart2);

    private static long Solve(string input, Func<PasswordItem, bool> check)
        => input.SplitNonEmptyLines().Select(PasswordItem.ParseString).Count(check);

    private static bool ValidPart1(PasswordItem item)
        => BetweenInclusive(item.Password.Count(x => x == item.ValidChar), item.Min, item.Max);

    private static bool BetweenInclusive(int x, int min, int max)
        => x >= min && x <= max;

    private static bool ValidPart2(PasswordItem item)
        => CheckPos(item.Password, item.ValidChar, item.Min - 1) ^
            CheckPos(item.Password, item.ValidChar, item.Max - 1);

    private static bool CheckPos(string s, char c, int i)
        => i < s.Length && s[i] == c;
}

public record PasswordItem(int Min, int Max, char ValidChar, string Password)
{
    public static PasswordItem ParseString(string input)
        => ParseString(input.Split(new char[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries));

    private static PasswordItem ParseString(string[] input)
        => new(int.Parse(input[0]), int.Parse(input[1]), input[2][0], input[3]);
}
