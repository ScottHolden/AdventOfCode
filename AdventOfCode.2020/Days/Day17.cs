namespace AdventOfCode;

[PuzzleInput("Day17.txt", 306, 2572)]
[PuzzleInput("Day17-Sample.txt", 112, 848)]
public class Day17 : IDay
{
    public long Part1(string input)
        => Solve(input, 3);

    public long Part2(string input)
        => Solve(input, 4);

    private static long Solve(string input, int dimensions)
    {
        Space space = Space.Build(input, dimensions);
        for (int i = 0; i < 6; i++)
        {
            space = Simulate(space);
        }
        return space.Count();
    }

    private static Space Simulate(Space space)
    {
        (int[] min, int[] max) = space.FindEdges();
        Space dest = new(space.Dimensions);
        ResursePopulateDestinationSpace(min, max, space, dest);
        return dest;
    }

    private static int ResursePopulateDestinationSpace(int[] start, int[] end, Space source, Space destination, int index = 0, bool inner = false)
    {
        if (index < source.Dimensions)
        {
            int startValue = start[index];
            int sum = 0;
            for (; start[index] <= end[index]; start[index]++)
            {
                sum += ResursePopulateDestinationSpace(start, end, source, destination, index + 1, inner);
            }
            start[index] = startValue;
            return sum;
        }

        bool active = source.Contains(start);

        if (inner)
        {
            return active ? 1 : 0;
        }

        (int[] min, int[] max) = OffsetArrayValuesBy(start, 1);
        int count = ResursePopulateDestinationSpace(min, max, source, destination, 0, true);

        if (count == 3 || (active && count == 4))
        {
            destination.Add(start);
        }

        return 1;
    }

    private static (int[] Min, int[] Max) OffsetArrayValuesBy(int[] source, int offset)
    {
        int[] min = new int[source.Length];
        int[] max = new int[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            min[i] = source[i] - offset;
            max[i] = source[i] + offset;
        }
        return (min, max);
    }

    internal class Space
    {
        private readonly Dictionary<ulong, int[]> _cubes;
        public int Dimensions { get; }

        public Space(int dimensions)
        {
            _cubes = new Dictionary<ulong, int[]>();
            this.Dimensions = dimensions;
        }
        public Space(IEnumerable<int[]> values, int dimensions)
        {
            _cubes = values.ToDictionary(x => BuildKey(x), x => x);
            this.Dimensions = dimensions;
        }
        public void Add(int[] value)
        {
            int[] copy = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                copy[i] = value[i];
            }
            _cubes.Add(BuildKey(copy), copy);
        }
        private static ulong BuildKey(int[] value)
        {
            // I know this is meant to be an infinite grid,
            //  but it's just too easy to assume +-500 in each dimension
            ulong key = 0;
            for (int i = 0; i < value.Length; i++)
            {
                key *= 1000;
                key += (ulong)(500 + value[i]);
            }
            return key;
        }
        public IEnumerable<int[]> All() => _cubes.Values;
        public int Count() => _cubes.Count;
        public bool Contains(int[] value) => _cubes.ContainsKey(BuildKey(value));

        public static Space Build(string input, int dimensions)
            => new(input.SplitNonEmptyLines()
                    .SelectMany((a, y) => a.Select((b, x) => (x, y, b == '#')))
                    .Where(x => x.Item3)
                    .Select(x => BuildCubeFromSlice(x.x, x.y, dimensions)), dimensions);

        private static int[] BuildCubeFromSlice(int x, int y, int dimensions)
        {
            int[] value = new int[dimensions];
            value[0] = x;
            value[1] = y;
            return value;
        }

        public (int[] Min, int[] Max) FindEdges(int offset = 1)
        {
            int[] min = new int[this.Dimensions];
            int[] max = new int[this.Dimensions];
            foreach (int[] value in _cubes.Values)
            {
                for (int i = 0; i < this.Dimensions; i++)
                {
                    if (value[i] - offset < min[i]) min[i] = value[i] - offset;
                    if (value[i] + offset > max[i]) max[i] = value[i] + offset;
                }
            }
            return (min, max);
        }
    }
}
