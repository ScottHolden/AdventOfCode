namespace AdventOfCode;

[PuzzleInput("Day21.txt", 1882, -1)] // xgtj,ztdctgq,bdnrnx,cdvjp,jdggtft,mdbq,rmd,lgllb
[PuzzleInput("Day21-Sample.txt", 5, -1)] // mxmxvkd,sqjhc,fvjkl
public class Day21 : IDay
{
    public long Part1(string input)
    {
        Dictionary<string, List<string>> map = new();
        Dictionary<string, int> count = new();
        List<string> everything = new();

        foreach (string line in input.SplitNonEmptyLines())
        {
            string[] parts = line.Split(new[] { " (", ")" }, StringSplitOptions.RemoveEmptyEntries);
            string[] allergens = parts[1].Split(new[] { "contains ", ", " }, StringSplitOptions.RemoveEmptyEntries);
            string[] ingredients = parts[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            everything.AddRange(ingredients);
            foreach (string a in allergens)
            {
                if (!map.ContainsKey(a)) map[a] = new List<string>();
                if (!count.ContainsKey(a)) count[a] = 0;
                map[a].AddRange(ingredients);
                count[a]++;
            }
        }

        while (map.Values.Any(x => x.Count > 1))
        {
            foreach (string key in map.Keys)
            {
                if (map[key].Count == 1) continue;
                var items = map[key].GroupBy(x => x).Where(x => x.Count() == count[key]).Select(x => x.Key).ToArray();
                if (items.Length > 1 || items.Length < 1)
                    continue;
                foreach (List<string> b in map.Values)
                {
                    b.RemoveAll(x => x.Equals(items[0]));
                }
                map[key].Clear();
                map[key].Add(items[0]);
                break;
            }
        }

        Dictionary<string, string> words = map.ToDictionary(x => x.Key, x => x.Value[0]);

        everything.RemoveAll(x => words.ContainsValue(x));

        return everything.Count;
    }
    public long Part2(string input)
    {
        Dictionary<string, List<string>> map = new();
        Dictionary<string, int> count = new();
        List<string> everything = new();

        foreach (string line in input.SplitNonEmptyLines())
        {
            string[] parts = line.Split(new[] { " (", ")" }, StringSplitOptions.RemoveEmptyEntries);
            string[] allergens = parts[1].Split(new[] { "contains ", ", " }, StringSplitOptions.RemoveEmptyEntries);
            string[] ingredients = parts[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            everything.AddRange(ingredients);
            foreach (string a in allergens)
            {
                if (!map.ContainsKey(a)) map[a] = new List<string>();
                if (!count.ContainsKey(a)) count[a] = 0;
                map[a].AddRange(ingredients);
                count[a]++;
            }
        }

        while (map.Values.Any(x => x.Count > 1))
        {
            foreach (string key in map.Keys)
            {
                if (map[key].Count == 1) continue;
                var items = map[key].GroupBy(x => x).Where(x => x.Count() == count[key]).Select(x => x.Key).ToArray();
                if (items.Length > 1 || items.Length < 1)
                    continue;
                foreach (List<string> b in map.Values)
                {
                    b.RemoveAll(x => x.Equals(items[0]));
                }
                map[key].Clear();
                map[key].Add(items[0]);
                break;
            }
        }

        Dictionary<string, string> words = map.ToDictionary(x => x.Key, x => x.Value[0]);

        string answer = string.Join(',', words.Keys.OrderBy(x => x).Select(x => words[x]));
        Console.WriteLine("Answer: " + answer);
        return -1;
    }
}
