using System.Runtime;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

[PuzzleInput("Day15.txt", 447, 11721679)]
[PuzzleInput("Day15-Sample.txt", 436, 175594)]
public class Day15 : IDay
{
    public long Part1(string input)
        => FindNumber(input, 2020);
    public long Part2(string input)
        => FindNumber(input, 30000000);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static long FindNumber(string input, int place)
    {
        string[] items = input.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        int current = 0, i;
        using WeirdTinyFastDictionary wtfd = new(1000, 1000, 1000);
        for (i = 0; i < items.Length; i++)
        {
            current = int.Parse(items[i]);
            wtfd.GetOrSomething(current, i + 1);
        }
        for (i = items.Length; i < place; i++)
        {
            current = wtfd.GetOrSomething(current, i);
        }
        return current;
    }
}

public sealed class WeirdTinyFastDictionary : IDisposable
{
    private readonly int _indexASize;
    private readonly int _indexBSize;
    private readonly int _indexCSize;
    private readonly int[][][] _backing;
    private readonly GCLatencyMode _oldMode;

    public WeirdTinyFastDictionary(int indexASize, int indexBSize, int indexCSize)
    {
        _indexASize = indexASize;
        _indexBSize = indexBSize;
        _indexCSize = indexCSize;
        _backing = new int[_indexASize][][];
        _oldMode = GCSettings.LatencyMode;
        GCSettings.LatencyMode = GCLatencyMode.LowLatency;
        //GC.TryStartNoGCRegion(200L * 1024 * 1024);
    }

    public void Dispose()
    {
        //GC.EndNoGCRegion();
        GCSettings.LatencyMode = _oldMode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int GetOrSomething(int index, int i)
    {
        int indexC = index % _indexCSize;
        int indexBc = index / _indexCSize;
        int indexB = indexBc % _indexBSize;
        int indexA = indexBc / _indexBSize;
        //if (indexA > _indexASize) throw new System.Exception("Too big for this race!");
        int[][] a = _backing[indexA];
        if (a == null) a = _backing[indexA] = new int[_indexBSize][];
        int[] b = a[indexB];
        if (b == null) b = a[indexB] = new int[_indexCSize];
        int val = b[indexC];
        b[indexC] = i;
        return val > 0 ? i - val : 0;
    }
}
