namespace AdventOfCode;

[PuzzleInput("Day14.txt", 14553106347726, 2737766154126)]
[PuzzleInput("Day14-Sample01.txt", 165, -1, skipPart2: true)]
[PuzzleInput("Day14-Sample02.txt", 51, 208)]
public class Day14 : IDay
{
    public long Part1(string input)
            => Solve(new Part1Computer(), input);

    public long Part2(string input)
        => Solve(new Part2Computer(), input);

    private static long Solve(Computer computer, string input)
        => computer.Execute(input.SplitNonEmptyLines()).Sum();

    private abstract class Computer
    {
        protected readonly Dictionary<long, long> Memory = new();
        public abstract void Mask(string mask);
        public abstract void Mem(long address, long value);
        public long Sum() => Memory.Values.Sum();
        public Computer Execute(string[] lines)
        {
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ' ', '=', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts[0] == "mask") Mask(parts[1]);
                else if (parts[0] == "mem") Mem(long.Parse(parts[1]), long.Parse(parts[2]));
            }
            return this;
        }
    }

    private class Part1Computer : Computer
    {
        private long _orMask;
        private long _andMask;

        public override void Mask(string mask)
        {
            _orMask = Convert.ToInt64(mask.Replace('X', '0'), 2);
            _andMask = Convert.ToInt64(mask.Replace('X', '1'), 2);
        }

        public override void Mem(long address, long value)
        {
            Memory[address] = value & _andMask | _orMask;
        }
    }

    private class Part2Computer : Computer
    {
        private long _orMask;
        private long _andMask;
        private long[] _buildMasks = Array.Empty<long>();
        public override void Mask(string mask)
        {
            _orMask = Convert.ToInt64(mask.Replace('X', '0'), 2);
            _andMask = Convert.ToInt64(mask.Replace('0', '1').Replace('X', '0'), 2);
            _buildMasks = BuildMasks(mask.Replace('1', '0')).ToArray();
        }

        public override void Mem(long address, long value)
        {
            foreach (long buildMask in _buildMasks)
            {
                Memory[((address | _orMask) & _andMask) | buildMask] = value;
            }
        }
        private static IEnumerable<long> BuildMasks(string input)
        {
            if (input.Contains('X'))
            {
                foreach (string s in ReplaceFirst(input))
                {
                    foreach (var l in BuildMasks(s))
                    {
                        yield return l;
                    }
                }
            }
            else
            {
                yield return Convert.ToInt64(input, 2);
            }
        }
        private static IEnumerable<string> ReplaceFirst(string text)
        {
            int pos = text.IndexOf('X');
            if (pos < 0)
            {
                yield return text;
            }
            else
            {
                yield return text[..pos] + '0' + text[(pos + 1)..];
                yield return text[..pos] + '1' + text[(pos + 1)..];
            }
        }
    }
}
