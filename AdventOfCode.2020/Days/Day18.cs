using System.Text.RegularExpressions;

namespace AdventOfCode;

[PuzzleInput("Day18.txt", 98621258158412, 241216538527890)]
[PuzzleInput("Day18-Sample.txt", 26457, 694173)]
public class Day18 : IDay
{
    private static readonly Regex s_solveBoth = new(@"(\d+)([+*])(\d+)", RegexOptions.Compiled);
    private static readonly Regex s_solveAdd = new(@"(\d+)([+])(\d+)", RegexOptions.Compiled);
    private static readonly Regex s_solveMultiply = new(@"(\d+)([*])(\d+)", RegexOptions.Compiled);

    public long Part1(string input)
        => input.SplitNonEmptyLines().Sum(SolvePart1);

    private static long SolvePart1(string line)
        => long.Parse(SolveInner(line.Replace(" ", ""), s_solveBoth));

    public long Part2(string input)
        => input.SplitNonEmptyLines().Sum(SolvePart2);

    private static long SolvePart2(string line)
        => long.Parse(SolveInner(line.Replace(" ", ""), s_solveAdd, s_solveMultiply));

    private static string SolveInner(string line, params Regex[] solvers)
    {
        while (line.Contains('('))
        {
            line = SolveBackets(line, solvers);
        }
        while (line.Contains('+') || line.Contains('*'))
        {
            foreach (Regex r in solvers)
            {
                if (r.IsMatch(line))
                {
                    line = r.Replace(line, x => Solve(x.Groups[1].Value, x.Groups[2].Value, x.Groups[3].Value), 1);
                    break;
                }
            }
        }
        return line;
    }

    private static string SolveBackets(string line, Regex[] solvers)
    {
        int start = line.IndexOf('(');
        int end = start + 1;
        for (int count = 1; count > 0; end++)
        {
            if (line[end] == '(') count++;
            if (line[end] == ')') count--;
        }
        return line.Substring(0, start) + SolveInner(line.Substring(start + 1, end - start - 2), solvers) + line[end..];
    }

    private static string Solve(string a, string op, string b)
        => Solve(long.Parse(a), op[0], long.Parse(b)).ToString();

    private static long Solve(long a, char op, long b)
        => op switch
        {
            '+' => a + b,
            '*' => a * b,
            _ => throw new Exception("Unknown op " + op)
        };
}
