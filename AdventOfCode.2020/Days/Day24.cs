namespace AdventOfCode;

[PuzzleInput("Day24.txt", 232, 3519)]
[PuzzleInput("Day24-Sample.txt", 10, 2208)]
public class Day24 : IDay
{
    public long Part1(string input)
    {
        return ParseTiles(input).Count;
    }

    private static HashSet<(int Q, int R)> ParseTiles(string input)
    {
        string[] lines = input.SplitNonEmptyLines();
        HashSet<(int Q, int R)> flipped = new();
        foreach (string line in lines)
        {
            (int Q, int R) key = Move(line);
            if (flipped.Contains(key)) flipped.Remove(key);
            else flipped.Add(key);
        }
        return flipped;
    }

    private static (int Q, int R) Move(string input)
    {
        int q = 0;
        int r = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 'e' || input[i] == 'w')
            {
                if (input[i] == 'e')
                {
                    q++;
                }
                if (input[i] == 'w')
                {
                    q--;
                }
            }
            else
            {
                var c1 = input[i];
                i++;
                if (c1 == 'n')
                {
                    if (input[i] == 'e')
                    {
                        q++;
                        r--;
                    }
                    if (input[i] == 'w')
                    {
                        r--;
                    }
                }
                else
                {
                    if (input[i] == 'e')
                    {
                        r++;
                    }
                    if (input[i] == 'w')
                    {
                        r++;
                        q--;
                    }
                }
            }
        }
        return (q, r);
    }


    public long Part2(string input)
    {
        HashSet<(int Q, int R)> flipped = ParseTiles(input);

        for (int day = 0; day < 100; day++)
        {
            HashSet<(int Q, int R)> res = new();
            int minQ = flipped.Min(x => x.Q) - 1;
            int maxQ = flipped.Max(x => x.Q) + 1;
            int minR = flipped.Min(x => x.R) - 1;
            int maxR = flipped.Max(x => x.R) + 1;

            for (int q = minQ; q <= maxQ; q++)
            {
                for (int r = minR; r <= maxR; r++)
                {
                    int count = Count(flipped, q, r);
                    bool isFlip = flipped.Contains((q, r));
                    if (isFlip && (count > 0 && count <= 2))
                    {
                        res.Add((q, r));
                    }
                    else if (!isFlip && count == 2)
                    {
                        res.Add((q, r));
                    }
                }
            }
            flipped = res;
        }

        return flipped.Count;
    }

    private static int Count(HashSet<(int Q, int R)> flipped, int q, int r)
    {
        return (flipped.Contains((q + 1, r)) ? 1 : 0) +
                (flipped.Contains((q, r + 1)) ? 1 : 0) +
                (flipped.Contains((q - 1, r + 1)) ? 1 : 0) +
                (flipped.Contains((q - 1, r)) ? 1 : 0) +
                (flipped.Contains((q, r - 1)) ? 1 : 0) +
                (flipped.Contains((q + 1, r - 1)) ? 1 : 0);
    }
}
