namespace AdventOfCode;

[PuzzleInput("Day13.txt", 3269, 672754131923874)]
[PuzzleInput("Day13-Sample.txt", 295, 1068781)]
public class Day13 : IDay
{
    public long Part1(string input)
    {
        string[] lines = input.SplitNonEmptyLines();
        long timestamp = long.Parse(lines[0]);
        int[] buses = lines[1].Split(new char[] { ',', 'x' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

        for (long current = timestamp; ; current++)
        {
            for (int i = 0; i < buses.Length; i++)
            {
                if (current % buses[i] == 0)
                {
                    return (current - timestamp) * buses[i];
                }
            }
        }
    }
    public long Part2(string input)
    {
        long[] buses = input.SplitNonEmptyLines()[1]
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x[0] == 'x' ? -1 : long.Parse(x))
                            .ToArray();

        long first = 0;
        for (long i = 1, diff = buses[0]; i < buses.Length; i++)
        {
            if (buses[i] > 0)
            {
                (first, diff) = Find(diff, first, buses[i], -i);
            }
        }
        return first;
    }
    public static (long First, long Diff) Find(long aStep, long aOffset, long bStep, long bOffset)
    {
        long first = FindNext(aOffset, aStep, bOffset, bStep);
        long second = FindNext(first, aStep, first, bStep);
        return (first, second - first);
    }
    public static long FindNext(long a, long aStep, long b, long bStep)
    {
        long x = a + aStep;
        long y = b;
        while (x != y)
        {
            while (x < y)
            {
                x += aStep;
            }
            while (y < x)
            {
                long diff = x - y;
                y += diff < 3 * bStep ? bStep : (diff / bStep) * bStep;
            }
        }
        return x;
    }
}
