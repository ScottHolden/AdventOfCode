namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PuzzleInputAttribute : Attribute
{
    public string Filename { get; }
    public long? Part1Answer { get; }
    public bool SkipPart1 { get; }
    public long? Part2Answer { get; }
    public bool SkipPart2 { get; }

    public PuzzleInputAttribute(string filename)
    {
        this.Filename = filename;
        this.Part1Answer = null;
        this.Part2Answer = null;
    }
    public PuzzleInputAttribute(string filename, long part1Answer, bool skipPart2 = false)
    {
        this.Filename = filename;
        this.Part1Answer = part1Answer;
        this.Part2Answer = null;
        this.SkipPart2 = skipPart2;
    }
    public PuzzleInputAttribute(string filename, long part1Answer, long part2Answer, bool skipPart1 = false, bool skipPart2 = false)
    {
        this.Filename = filename;
        this.Part1Answer = part1Answer;
        this.Part2Answer = part2Answer;
        this.SkipPart1 = skipPart1;
        this.SkipPart2 = skipPart2;
    }
}
