namespace AdventOfCode;

[PuzzleInput("Day10.txt", 2812, 386869246296064)]
[PuzzleInput("Day10-Sample01.txt", 35, 8)]
[PuzzleInput("Day10-Sample02.txt", 220, 19208)]
public class Day10 : IDay
{
    public long Part1(string input)
    {
        long[] lines = input.ParseLinesAsLong();
        long[] ordered = lines.Append(0).Append(lines.Max() + 3).OrderBy(x => x).ToArray();
        int[] count = new int[4];
        for (int i = 1; i < ordered.Length; i++)
        {
            count[ordered[i] - ordered[i - 1]]++;
        }
        return count[1] * count[3];
    }
    public long Part2(string input)
    {
        long[] lines = input.ParseLinesAsLong();
        HashSet<long> numbers = new(lines);
        long max = lines.Max() + 3;

        RecursiveShortcut<long> rec = new((current, recurse) =>
        {
            if (current == max) return 1;
            if (current != 0 && !numbers.Contains(current)) return 0;
            long c = 0;
            for (long i = 1; i <= 3; i++)
            {
                c += recurse(current + i);
            }
            return c;
        });

        return rec.Recurse(0);
    }
}
public class RecursiveShortcut<T> where T : struct
{
    private readonly Func<T, Func<T, T>, T> _action;
    private readonly Dictionary<T, T> _results;
    public RecursiveShortcut(Func<T, Func<T, T>, T> action)
    {
        _action = action;
        _results = new();
    }
    public T Recurse(T input) => Recurse(input, Recurse);
    public T Recurse(T input, Func<T, T> recurse)
    {
        if (_results.ContainsKey(input)) return _results[input];

        T value = _action(input, recurse);

        _results[input] = value;

        return value;
    }
}
