using System.Diagnostics;

namespace AdventOfCode;

public static class PuzzleDisplay
{
    private const int Width = 48;
    private const int TimeDigits = 4;

    public static bool DisplayAnswer(Func<string, long> func, string input, long? answer, bool skip)
    {
        if (skip)
        {
            WriteTimedOutput($"  {func.Method.Name}: Skipped", 0);
            return true;
        }

        (long output, long elapsedMilliseconds) = Measure(() => func(input));

        char correct = answer.HasValue switch
        {
            true when output == answer => 'Y',
            true => 'X',
            false => '?'
        };

        WriteStatusOutput($"  {func.Method.Name}: {output}", correct, elapsedMilliseconds);

        return answer.HasValue && output == answer;
    }

    public static T MeasureWithMessage<T>(Func<T> func, string message)
    {
        (T result, long elapsedMilliseconds) = Measure(func);
        WriteTimedOutput(message, elapsedMilliseconds);
        return result;
    }

    private static (T Result, long ElapsedMilliseconds) Measure<T>(Func<T> func)
    {
        var sw = Stopwatch.StartNew();
        T result = func();
        sw.Stop();
        return (result, sw.ElapsedMilliseconds);
    }

    public static long Measure(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private static void WriteStatusOutput(string line, char status, long time)
        => WriteOutput($"{line,8 + TimeDigits - Width}[{status}][{time,TimeDigits} ms]");

    public static void WriteTimedOutput(string line, long time)
        => WriteOutput($"{line,5 + TimeDigits - Width}[{time,TimeDigits} ms]");

    public static void WriteOutput(string line)
        => Console.WriteLine(line);

    public static void WriteHeader(string input)
    {
        int length = Width - input.Length - 2;
        string left = new('-', length / 2);
        string right = new('-', length - left.Length);
        WriteOutput($"\n{left} {input} {right}\n");
    }

    public static void WriteFooter()
        => WriteOutput($"\n{new string('-', Width)}\n");

    public static void Pause() => Console.ReadLine();
}
