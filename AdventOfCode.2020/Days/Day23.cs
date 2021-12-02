namespace AdventOfCode;

[PuzzleInput("Day23.txt", 78569234, 565615814504)]
[PuzzleInput("Day23-Sample.txt", 67384529, 149245887792)]
public class Day23 : IDay
{
    private const int Part1Rounds = 100;
    public long Part1(string input)
        => FormatPart1Answer(Solve(ParseCups(input), Part1Rounds));

    private static long FormatPart1Answer(List<int> cups)
        => long.Parse(string.Join("", string.Join("", cups).Split('1').Reverse()));

    private const int Part2Rounds = 10000000;
    private const int Part2Cups = 1000000;
    public long Part2(string input)
    {
        List<int> cups = Solve(IncrementalPadList(ParseCups(input), Part2Cups), Part2Rounds);

        int i = cups.IndexOf(1);
        return (long)cups[i + 1] * cups[i + 2];
    }

    private static List<int> IncrementalPadList(List<int> list, int totalSize)
        => list.Concat(Enumerable.Range(list.Max() + 1, totalSize - list.Count)).ToList();

    private static List<int> ParseCups(string input)
        => input.Trim().ToCharArray().Select(x => int.Parse(x.ToString())).ToList();

    private static List<int> Solve(List<int> startingCups, int itterations)
    {
        (int Value, int Next)[] cups = new (int Value, int Next)[startingCups.Count];
        int[] lookup = new int[startingCups.Count];

        int maxCup = 0;
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i] = (startingCups[i], (i + 1) % cups.Length);
            lookup[startingCups[i] - 1] = i;
            if (startingCups[i] > maxCup) maxCup = startingCups[i];
        }

        int currentIndex = 0;

        for (int i = 0; i < itterations; i++)
        {
            int currentValue = cups[currentIndex].Value;

            int nextAIndex = cups[currentIndex].Next;
            int nextAValue = cups[nextAIndex].Value;

            int nextBIndex = cups[nextAIndex].Next;
            int nextBValue = cups[nextBIndex].Value;

            int nextCIndex = cups[nextBIndex].Next;
            int nextCValue = cups[nextCIndex].Value;

            cups[currentIndex].Next = cups[nextCIndex].Next;

            int dest = ModWrap(currentValue - 1, maxCup);
            while (dest == nextAValue || dest == nextBValue || dest == nextCValue) dest = ModWrap(dest - 1, maxCup);

            int destTail = cups[lookup[dest - 1]].Next;
            cups[lookup[dest - 1]].Next = nextAIndex;
            cups[nextCIndex].Next = destTail;
            currentIndex = cups[lookup[currentValue - 1]].Next;
        }

        List<int> result = new(cups.Length);

        for (int i = 0; i < cups.Length; i++)
        {
            result.Add(cups[currentIndex].Value);
            currentIndex = cups[currentIndex].Next;
        }

        return result;

        static int ModWrap(int input, int mod)
            => input <= 0 ? mod + input : input % mod;
    }
}
