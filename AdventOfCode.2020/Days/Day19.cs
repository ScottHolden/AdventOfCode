using System.Text.RegularExpressions;

namespace AdventOfCode;

[PuzzleInput("Day19.txt", 118, 246)]
[PuzzleInput("Day19-Sample01.txt", 2, 2)]
[PuzzleInput("Day19-Sample02.txt", 3, 12)]
public class Day19 : IDay
{
    private const int RecursionDepth = 10;
    public long Part1(string input)
        => Solve(input, 0, new());

    public long Part2(string input)
        => Solve(input, 0, new()
        {
            { 8, "42 | 42 8" },
            { 11, "42 31 | 42 11 31" }
        });

    private static long Solve(string input, int index, Dictionary<int, string> ruleModifications)
    {
        string[] sections = input.SplitDoubleNewlineSections();

        Dictionary<int, string[]> ruleSet = BuildRules(sections[0]);

        foreach ((int key, string mod) in ruleModifications)
        {
            ruleSet[key] = mod.Split(" | ");
        }

        Regex ruleRegex = RuleRegex(index, ruleSet);

        return sections[1].SplitNonEmptyLines().Count(ruleRegex.IsMatch);
    }

    private static Dictionary<int, string[]> BuildRules(string input)
        => input.Replace("\"", "").Split("\n")
                .Select(x => x.Split(new[] { ": ", " | " }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => int.Parse(x[0]), x => x.Skip(1).ToArray());

    private static Regex RuleRegex(int index, Dictionary<int, string[]> ruleSet)
        => new("^" + SolveRule(index, ruleSet, new Dictionary<int, int>()) + "$");

    private static string SolveRule(int index, Dictionary<int, string[]> ruleSet, Dictionary<int, int> visited)
    {
        string[] rules = ruleSet[index];
        if (rules.Length == 1 && rules[0].Length == 1 && char.IsLetter(rules[0][0])) return rules[0];
        if (!visited.ContainsKey(index)) visited[index] = 0;
        if (visited[index] >= RecursionDepth) return "Z";
        visited[index]++;
        string output = "";
        if (rules.Length > 1) output += "(?:";
        foreach (string rule in rules)
        {
            if (rule.Length == 1 && char.IsLetter(rule[0]))
            {
                output += rule;
            }
            else
            {
                foreach (int nextRule in rule.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                {
                    output += SolveRule(nextRule, ruleSet, visited);
                }
            }
            if (rules.Length > 1) output += '|';
        }
        output = output.Trim('|');
        if (rules.Length > 1) output += ")";
        visited[index]--;
        return output;
    }
}
