using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class AdventRunner
{
    private const int RunCount = 1;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Run(int year)
    {
        PuzzleDisplay.WriteOutput($"Advent Of Code {year} - ScottDev");
        long elapsed = RunAllPuzzles(Assembly.GetCallingAssembly());
        PuzzleDisplay.WriteFooter();
        PuzzleDisplay.WriteOutput($"AoC {year} Completed in {elapsed} ms!");
    }

    private static long RunAllPuzzles(Assembly assembly)
    {
        var stopwatch = Stopwatch.StartNew();
        foreach (Type t in PuzzleHelpers.GetDayTypes(assembly))
        {
            if (RunPuzzle(t))
            {
                stopwatch.Stop();
                PuzzleDisplay.Pause();
                stopwatch.Start();
            }
        }
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    private static bool RunPuzzle(Type t)
    {
        bool pauseOnNext = false;

        try
        {
            var puzzleAttributes = t.GetCustomAttributes<PuzzleInputAttribute>();

            if (!puzzleAttributes.Any()) return false;

            PuzzleDisplay.WriteHeader(t.Name);

            IDay? day = PuzzleHelpers.CreateInstance(t);

            if (day is null) return false;

            foreach (PuzzleInputAttribute puzzle in puzzleAttributes.OrderByDescending(x => x.Filename.Length))
            {
                string input = PuzzleHelpers.ReadFile(t.Assembly, puzzle.Filename);

                if (string.IsNullOrWhiteSpace(input))
                {
                    PuzzleDisplay.WriteOutput("  Empty file found, skipping...");
                    continue;
                }
                for (int i = 0; i < RunCount; i++)
                {
                    pauseOnNext |= !PuzzleDisplay.DisplayAnswer(day.Part1, input, puzzle.Part1Answer, puzzle.SkipPart1);
                    pauseOnNext |= !PuzzleDisplay.DisplayAnswer(day.Part2, input, puzzle.Part2Answer, puzzle.SkipPart2);
                }
            }
        }
        catch (Exception e)
        {
            pauseOnNext = true;
            PuzzleDisplay.WriteOutput(e.ToString());
        }

        return pauseOnNext;
    }
}
