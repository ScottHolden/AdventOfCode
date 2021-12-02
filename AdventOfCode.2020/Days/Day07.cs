namespace AdventOfCode;

[PuzzleInput("Day07.txt", 124, 34862)]
[PuzzleInput("Day07-Sample01.txt", 4, 32)]
[PuzzleInput("Day07-Sample02.txt", 0, 126)]
public class Day07 : IDay
{
    private const string Target = "shiny gold";
    public long Part1(string input) => CountAnyGold(ParseBags(input.SplitNonEmptyLines()));

    // Remove 1 as we don't count the outer bag!
    public long Part2(string input) => Count(ParseBags(input.SplitNonEmptyLines()), Target) - 1;

    private static long Count(Dictionary<string, (string Bag, int Count)[]> bags, string current)
        => 1 + bags[current].Sum(x => x.Count * Count(bags, x.Bag));

    private static int CountAnyGold(Dictionary<string, (string Bag, int Count)[]> bags)
        => bags.Keys.Count(x => x != Target && AnyGold(bags, x));

    private static bool AnyGold(Dictionary<string, (string Bag, int Count)[]> bags, string current)
        => current == Target || bags[current].Any(x => AnyGold(bags, x.Bag));

    private static Dictionary<string, (string Bag, int Count)[]> ParseBags(string[] lines)
        => lines.Select(x => x.Split(new string[] { " bags contain ", " bag, ", " bags, ", " bag.", " bags.", "no other" }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[0], x => x.Skip(1).Select(y => y.Split(' ', 2)).Select(y => (y[1], int.Parse(y[0]))).ToArray());
}
