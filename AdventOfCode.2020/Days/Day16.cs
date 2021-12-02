namespace AdventOfCode;

[PuzzleInput("Day16.txt", 26980, 3021381607403)]
[PuzzleInput("Day16-Sample.txt", 71, 14)]
public class Day16 : IDay
{
    public long Part1(string input)
    {
        (RangeSet[] checks, _, int[][] otherTickets) = ParseData(input);

        return otherTickets.Select(x => ValidateTicket(x, checks)).Where(x => x >= 0).Sum();
    }

    private static (RangeSet[] Checks, int[] MyTicket, int[][] OtherTickets) ParseData(string input)
    {
        string[] dataParts = input.Split("\n\n");
        return (
            dataParts[0].Split('\n').Select(RangeSet.Parse).ToArray(),
            dataParts[1].Split("\n")[1].Split(",").Select(int.Parse).ToArray(),
            dataParts[2].Split("\n").Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray()
        );
    }

    private static int ValidateTicket(int[] ticket, RangeSet[] checks)
        => ticket.Append(-1)
                    .Where(y => !checks.Any(x => x.IsWithin(y)))
                    .First();

    private static int[][] FilterInvalidTickets(int[][] tickets, RangeSet[] checks)
        => tickets.Where(x => ValidateTicket(x, checks) < 0)
                    .ToArray();

    private static (RangeSet[] Checks, int[] MyTicket, int[][] OtherTickets) ParseAndValidateData(string input)
    {
        (RangeSet[] checks, int[] myTicket, int[][] otherTickets) = ParseData(input);
        return (checks, myTicket, FilterInvalidTickets(otherTickets, checks));
    }

    public long Part2(string input)
    {
        (RangeSet[] checks, int[] myTicket, int[][] otherTickets) = ParseAndValidateData(input);

        return TrimPossibilities(BuildPossibleList(checks, otherTickets))
                    .Select((x, i) => x.Name.StartsWith("departure") ? myTicket[i] : 1L)
                    .Aggregate((x, y) => x * y);
    }

    private static RangeSet[][] BuildPossibleList(RangeSet[] checks, int[][] otherTickets)
        => Enumerable.Range(0, checks.Length)
                        .Select(i => checks.Where(x => ValidateColumnTicket(otherTickets, x, i))
                                            .ToArray())
                        .ToArray();

    private static bool ValidateColumnTicket(int[][] tickets, RangeSet check, int col)
        => tickets.All(x => check.IsWithin(x[col]));

    private static RangeSet[] TrimPossibilities(RangeSet[][] input)
    {
        List<RangeSet>[] colToCheck = input.Select(x => x.ToList()).ToArray();
        while (colToCheck.Any(x => x.Count > 1))
        {
            for (int i = 0; i < colToCheck.Length; i++)
            {
                if (colToCheck[i].Count == 1)
                {
                    for (int j = 0; j < colToCheck.Length; j++)
                    {
                        if (j == i) continue;
                        if (colToCheck[j].Contains(colToCheck[i][0])) colToCheck[j].Remove(colToCheck[i][0]);
                    }
                }
            }
        }

        return colToCheck.Select(x => x[0]).ToArray();
    }

    private record Range(int Min, int Max)
    {
        public static Range Parse(string input)
        {
            string[] parts = input.Split('-');
            return new Range(int.Parse(parts[0]), int.Parse(parts[1]));
        }
        public bool IsWithin(int value) => value >= this.Min && value <= this.Max;
    }

    private record RangeSet(string Name, Range[] Items)
    {
        public static RangeSet Parse(string input)
        {
            string[] parts = input.Split(": ");
            return new RangeSet(parts[0], parts[1].Split(" or ").Select(Range.Parse).ToArray());
        }
        public bool IsWithin(int value) => this.Items.Any(x => x.IsWithin(value));
    }
}
