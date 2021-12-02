namespace AdventOfCode;

[PuzzleInput("Day05.txt", 989, 548)]
[PuzzleInput("Day05-Sample.txt", 820, skipPart2: true)]
public class Day05 : IDay
{
    public long Part1(string input)
        => input.SplitNonEmptyLines().Select(FindSeat).OrderByDescending(x => x).First();

    private static int FindSeat(string input)
        => (FindNumber(input[..7], 128, 'F') * 8) +
            FindNumber(input.Substring(7, 3), 8, 'L');

    private static int FindNumber(string input, int max, char upper)
    {
        int min = 0;
        for (int i = 0; i < input.Length; i++)
        {
            int step = (max - min) / 2;
            if (input[i] == upper)
            {
                max -= step;
            }
            else
            {
                min += step;
            }
        }
        return min;
    }
    public long Part2(string input)
    {
        var seats = input.SplitNonEmptyLines().Select(FindSeat).ToHashSet();
        int max = seats.Max() - 1;
        for (int i = seats.Min() + 1; i < max; i++)
        {
            if (!seats.Contains(i))
            {
                return i;
            }
        }
        throw new Exception("Massive error!");
    }
}
