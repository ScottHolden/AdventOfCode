namespace AdventOfCode;

public static class PuzzleExtentions
{
    public static string[] SplitNonEmptyLines(this string input)
        => input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitDoubleNewlineSections(this string input)
        => input.Replace("\r", "").Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

    public static long[] ParseLinesAsLong(this string input)
        => SplitNonEmptyLines(input).Select(long.Parse).ToArray();

    public static char[][] ToCharMap(this string input)
        => SplitNonEmptyLines(input).Select(x => x.ToCharArray()).ToArray();
}
