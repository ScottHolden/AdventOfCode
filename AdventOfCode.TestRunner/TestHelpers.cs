namespace AdventOfCode;

internal class TestHelpers
{
    public static long RunPart1(string name, string filename)
        => RunPart1(ActivateByName(name), filename);

    public static long RunPart2(string name, string filename)
        => RunPart2(ActivateByName(name), filename);

    private static long RunPart1(IDay day, string filename)
        => day.Part1(PuzzleHelpers.ReadFile(day.GetType().Assembly, filename));

    private static long RunPart2(IDay day, string filename)
        => day.Part2(PuzzleHelpers.ReadFile(day.GetType().Assembly, filename));

    private static IDay ActivateByName(string name)
        => (IDay?)Activator.CreateInstance(
            Type.GetType(name) ?? throw new Exception("Couldn't get type: " + name)
        ) ?? throw new Exception("Couldn't activate type: " + name);
}
