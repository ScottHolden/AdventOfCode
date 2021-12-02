namespace AdventOfCode;

[PuzzleInput("Day20.txt", 8581320593371, 2031)]
[PuzzleInput("Day20-Sample.txt", 20899048083289, 273)]
public class Day20 : IDay
{
    public long Part1(string input)
        => MultiplyCorners(Solve(input));

    private static long MultiplyCorners(Image[][] picture)
        => picture[0][0].Name * picture[^1][0].Name *
            picture[0][^1].Name * picture[^1][^1].Name;

    public long Part2(string input)
        => CountNonMonsterRough(Image.FromImageArray(Solve(input)));

    private static long CountNonMonsterRough(Image picture)
        => picture.CountRough() - CountMonsters(picture) * 15;

    private static long CountMonsters(Image picture)
        => picture.AllVariants().Max(FindMonsters);

    private static long FindMonsters(Image picture)
    {
        long count = 0;
        for (int y = 0; y < picture.Size - s_monster.Length; y++)
        {
            for (int x = 0; x < picture.Size - s_monster[1].Length; x++)
            {
                bool found = true;
                for (int yOffset = 0; found && yOffset < s_monster.Length; yOffset++)
                {
                    for (int xOffset = 0; found && xOffset < s_monster[1].Length; xOffset++)
                    {
                        if (s_monster[yOffset][xOffset] != ' ' &&
                            !picture.IsAt(x + xOffset, y + yOffset, s_monster[yOffset][xOffset]))
                        {
                            found = false;
                        }
                    }
                }
                if (found) count++;
            }
        }
        return count;
    }

    private static readonly string[] s_monster = new[] {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };

    private static Image[][] Solve(string input)
        => Solve(input.Replace("\r", "")
                        .Split(new string[] { "\n\n", "Tile " }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(Image.Parse)
                        .ToArray());

    private static Image[][] Solve(Image[] images)
    {
        IEnumerable<IGrouping<int, int>> edgeCounts = images.SelectMany(x => x.AllVariants()).SelectMany(x => x.GetEdgeNumbers()).GroupBy(x => x);
        int[] outerEdges = edgeCounts.Where(x => x.Count() == 4).Select(x => x.Key).ToArray();
        int[] innerEdges = edgeCounts.Where(x => x.Count() == 8).Select(x => x.Key).ToArray();
        List<Image> cornerPieces = images.Where(x => x.GetEdgeNumbers().Count(y => outerEdges.Contains(y)) == 2).ToList();
        List<Image> edgePieces = images.Where(x => x.GetEdgeNumbers().Count(y => outerEdges.Contains(y)) == 1).ToList();
        List<Image> innerPieces = images.Where(x => x.GetEdgeNumbers().Count(y => innerEdges.Contains(y)) == 4).ToList();

        // Place a corner
        int width = (int)Math.Sqrt(images.Length);
        Image[][] picture = new Image[width][];
        for (int i = 0; i < width; i++) picture[i] = new Image[width];

        picture[0][0] = cornerPieces[0].AllVariants().First(x => outerEdges.Contains(x.Top) && outerEdges.Contains(x.Left));
        cornerPieces.RemoveAt(0);

        List<Image> pieces = cornerPieces.Concat(edgePieces).Concat(innerPieces).ToList();

        // Top edge
        for (int i = 1; i < width; i++)
        {
            Image next = pieces.SelectMany(x => x.AllVariants()).First(x => outerEdges.Contains(x.Top) && x.Left == picture[i - 1][0].Right);
            picture[i][0] = next;
            pieces.RemoveAll(x => x.Name == next.Name);
        }

        for (int y = 1; y < width; y++)
        {
            Image next = pieces.SelectMany(x => x.AllVariants()).First(x => outerEdges.Contains(x.Left) && x.Top == picture[0][y - 1].Bottom);
            picture[0][y] = next;
            pieces.RemoveAll(x => x.Name == next.Name);
            for (int x = 1; x < width; x++)
            {
                next = pieces.SelectMany(x => x.AllVariants()).First(z => z.Top == picture[x][y - 1].Bottom && z.Left == picture[x - 1][y].Right);
                picture[x][y] = next;
                pieces.RemoveAll(x => x.Name == next.Name);
            }
        }

        return picture;
    }

    private class Image
    {
        public long Name => _name;
        public int Size => _data.Length;
        private readonly long _name;
        private readonly string[] _data;
        private readonly int[] _edges;
        public int Top => _edges[0];
        public int Bottom => _edges[1];
        public int Left => _edges[2];
        public int Right => _edges[3];

        public Image(long name, string[] data)
        {
            _name = name;
            _data = data;
            if (data.Length <= 12)
                _edges = GetEdges().Select(ToNumber).ToArray();
            else
                _edges = new int[] { -1, -1, -1, -1 };
        }

        public IEnumerable<string> GetEdges()
        {
            yield return _data[0];
            yield return _data[^1];
            yield return Slice(_data, 0);
            yield return Slice(_data, _data.Length - 1);
        }
        private static int ToNumber(string x)
            => Convert.ToInt32(x.Replace('.', '0').Replace('#', '1'), 2);

        public int[] GetEdgeNumbers() => _edges;

        public string[] Trim()
        {
            string[] lines = new string[_data.Length - 2];
            for (int i = 1; i < _data.Length - 1; i++)
            {
                lines[i - 1] = _data[i][1..^1];
            }
            return lines;
        }

        public IEnumerable<Image> AllVariants()
        {
            yield return this;
            yield return FlipHoz();
            yield return FlipVert();
            yield return FlipHoz().FlipVert();
            Image b = RotateClockwise();
            yield return b;
            yield return b.FlipHoz();
            yield return b.FlipVert();
            yield return b.FlipHoz().FlipVert();
        }

        public long CountRough() => _data.Sum(x => x.Count(c => c == '#'));

        public bool IsAt(int x, int y, char c) => _data[y][x] == c;

        private static string Slice(string[] input, int index)
        {
            string e = "";
            for (int i = 0; i < input.Length; i++)
            {
                e += input[i][index];
            }
            return e;
        }

        public Image FlipHoz()
        {
            string[] dest = new string[_data.Length];
            for (int i = 0; i < _data.Length; i++)
            {
                dest[i] = Reverse(_data[i]);
            }
            return new Image(_name, dest);
        }

        public Image FlipVert()
        {
            string[] dest = new string[_data.Length];
            for (int i = 0; i < _data.Length; i++)
            {
                dest[i] = _data[_data.Length - 1 - i];
            }
            return new Image(_name, dest);
        }

        public Image RotateClockwise()
        {
            string[] dest = new string[_data.Length];
            for (int y = 0; y < _data.Length; y++)
            {
                dest[y] = "";
                for (int x = 0; x < _data.Length; x++)
                {
                    dest[y] += _data[_data.Length - 1 - x][y];
                }
            }
            return new Image(_name, dest);
        }

        private static string Reverse(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        public static Image Parse(string input)
        {
            string[] parts = input.Split(':');
            return new Image(long.Parse(parts[0]), parts[1].Trim().Split('\n'));
        }

        public static Image FromImageArray(Image[][] picture)
        {
            int size = picture.GetLength(0);
            int inner = picture[0][0].Size - 2;
            string[] result = new string[size * inner];
            for (int yImage = 0; yImage < size; yImage++)
            {
                for (int xImage = 0; xImage < size; xImage++)
                {
                    string[] lines = picture[xImage][yImage].Trim();
                    for (int yLine = 0; yLine < inner; yLine++)
                    {
                        result[yImage * inner + yLine] += lines[yLine];
                    }
                }
            }
            return new Image(0, result);
        }

        public override string? ToString() => string.Join('\n', _data);
    }
}
