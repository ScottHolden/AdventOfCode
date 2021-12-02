namespace AdventOfCode;

[PuzzleInput("Day11.txt", 2418, 2144)]
[PuzzleInput("Day11-Sample.txt", 37, 26)]
public class Day11 : IDay
{
    public long Part1(string input)
        => PlayGame(input.ToCharMap(), DirectCheck, 4);

    public long Part2(string input)
        => PlayGame(input.ToCharMap(), CastCheck, 5);

    private static char GetNext(char m, int count, int maxPeeps) => m switch
    {
        '.' => '.',
        'L' when count < 1 => '#',
        '#' when count >= maxPeeps => 'L',
        char c => c
    };

    private static int PlayGame(char[][] map, CheckFunc func, int peepCount)
    {
        bool settled = false;
        while (!settled) (map, settled) = PlayRound(map, func, peepCount);
        return map.SelectMany(x => x).Count(x => x == '#');
    }

    private static (char[][] Map, bool Settled) PlayRound(char[][] map, CheckFunc func, int peepCount)
    {
        char[][] newMap = new char[map.Length][];
        bool settled = true;
        for (int y = 0; y < map.Length; y++)
        {
            newMap[y] = new char[map[y].Length];
            for (int x = 0; x < map[y].Length; x++)
            {
                char next = GetNext(map[y][x], GetCount(map, x, y, func), peepCount);
                if (map[y][x] != next) settled = false;
                newMap[y][x] = next;
            }
        }
        return (newMap, settled);
    }

    private static int GetCount(char[][] map, int x, int y, CheckFunc func)
        => CountAllDirections((xOff, yOff) => func(map, x, y, xOff, yOff));

    private delegate bool CheckFunc(char[][] map, int x, int y, int xOff, int yOff);

    private static bool DirectCheck(char[][] map, int x, int y, int xOff, int yOff)
        => !OutOfBounds(map, x + xOff, y + yOff) && map[y + yOff][x + xOff] == '#';

    private static bool CastCheck(char[][] map, int x, int y, int xOff, int yOff)
        => GetCountCast2(map, x, y, xOff, yOff) == '#';

    private static int CountAllDirections(Func<int, int, bool> action)
        => CountAllDirections((x, y) => action(x, y) ? 1 : 0);
    private static int CountAllDirections(Func<int, int, int> action)
        => action(-1, -1) + action(-1, 0) + action(-1, 1) +
            action(0, -1) + action(0, 1) +
            action(1, -1) + action(1, 0) + action(1, 1);

    private static char GetCountCast2(char[][] map, int x, int y, int xOffi, int yOffi)
    {
        for (int d = 1; ; d++)
        {
            int xOff = d * xOffi;
            int yOff = d * yOffi;
            if (OutOfBounds(map, x + xOff, y + yOff)) return '.';
            if (map[y + yOff][x + xOff] != '.') return map[y + yOff][x + xOff];
        }
    }
    private static bool OutOfBounds(char[][] map, int x, int y)
        => (x < 0) ||
            (y < 0) ||
            (y >= map.Length) ||
            (x >= map[y].Length);
}
