namespace AdventOfCode;

[PuzzleInput("Day03.txt", 282, 958815792)]
[PuzzleInput("Day03-Sample.txt", 7, 336)]
public class Day03 : IDay
{
    private const char Tree = '#';

    public long Part1(string input)
        => CountTrees(input.SplitNonEmptyLines(), 3, 1);

    public long Part2(string input) => Part2(input.SplitNonEmptyLines());

    private static long Part2(string[] lines)
        => s_part2Steps.Select(x => CountTrees(lines, x.XStep, x.YStep))
                        .Aggregate(1L, (x, y) => x * y);

    private static readonly (int XStep, int YStep)[] s_part2Steps = new[] {
            (1, 1), (3, 1), (5, 1), (7, 1), (1, 2)
        };

    private static long CountTrees(string[] lines, int xStep, int yStep)
        => Enumerable.Range(0, lines.Length / yStep)
                        .Count(i => lines[i * yStep][(i * xStep) % lines[i * yStep].Length] == Tree);
}
