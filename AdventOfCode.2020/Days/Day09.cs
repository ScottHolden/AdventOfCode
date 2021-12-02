namespace AdventOfCode;

[PuzzleInput("Day09.txt", 18272118, 2186361)]
[PuzzleInput("Day09-Sample.txt", 127, 62)]
public class Day09 : IDay
{
    public long Part1(string input)
    {
        long[] lines = input.ParseLinesAsLong();
        int preamble = lines.Length < 25 ? 5 : 25;
        for (int i = preamble; i < lines.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < preamble; j++)
            {
                if (Array.IndexOf(lines, lines[i] - lines[i - (j + 1)], i - preamble, preamble) >= 0)
                {
                    found = true;
                    break;
                }
            }
            if (!found) return lines[i];
        }
        throw new Exception("Not found");
    }
    public long Part2(string input)
    {
        long[] lines = input.ParseLinesAsLong();
        long Target = Part1(input);
        int tail = 0;
        int head = 0;
        long current = 0;

        while (current != Target)
        {
            while (current < Target) current += lines[head++];
            if (current == Target) break;
            while (current > Target) current -= lines[tail++];
        }

        long min = long.MaxValue;
        long max = long.MinValue;

        for (int i = tail; i < head; i++)
        {
            if (lines[i] < min) min = lines[i];
            if (lines[i] > max) max = lines[i];
        }

        return min + max;
    }
}
