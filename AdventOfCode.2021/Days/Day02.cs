namespace AdventOfCode;

[PuzzleInput("Day02.txt", 1840243, 1727785422)]
[PuzzleInput("Day02-Sample.txt", 150, 900)]
public class Day02 : IDay
{
    public long Part1(string input)
    {
        long hoz = 0, depth = 0;

        foreach ((string action, int value) in ParseInput(input))
        {
            if (action == "forward") hoz += value;
            else if (action == "down") depth += value;
            else if (action == "up") depth -= value;
        }

        return hoz * depth;
    }

    public long Part2(string input)
    {
        long hoz = 0, depth = 0, aim = 0;

        foreach ((string action, int value) in ParseInput(input))
        {
            if (action == "forward")
            {
                hoz += value;
                depth += aim * value;
            }
            else if (action == "down")
            {
                aim += value;
            }
            else if (action == "up")
            {
                aim -= value;
            }
        }

        return hoz * depth;
    }

    private static IEnumerable<(string, int)> ParseInput(string input)
        => input.SplitNonEmptyLines()
                .Select(x => x.Split(' '))
                .Select(x => (x[0], int.Parse(x[1])));
}
