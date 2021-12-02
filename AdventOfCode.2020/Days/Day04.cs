using System.Globalization;

namespace AdventOfCode;

[PuzzleInput("Day04.txt", 245, 133)]
[PuzzleInput("Day04-Sample01.txt", 2, 2)]
[PuzzleInput("Day04-Sample02.txt", 8, 4)]
public class Day04 : IDay
{
    private static readonly string[] s_validEyeValues = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

    public long Part1(string input)
        => ParsePassports(input).Count(Part1Valid);

    private bool Part1Valid(Dictionary<string, string> passport)
        => s_validationRules.Keys.All(x => passport.ContainsKey(x));

    public long Part2(string input)
        => ParsePassports(input)
            .Count(Part2Valid);

    private bool Part2Valid(Dictionary<string, string> passport)
        => s_validationRules.All(x => passport.ContainsKey(x.Key) &&
                                        x.Value(passport[x.Key]));

    private static readonly Dictionary<string, Func<string, bool>> s_validationRules = new()
    {
        { "byr", x => InRange(x, 1920, 2002) },
        { "iyr", x => InRange(x, 2010, 2020) },
        { "eyr", x => InRange(x, 2020, 2030) },
        {
            "hgt",
            x => x switch
               {
                   string inch when inch.EndsWith("in") &&
                                       int.TryParse(inch[0..^2], out int hIn) &&
                                       hIn >= 59 &&
                                       hIn <= 76 => true,
                   string cm when cm.EndsWith("cm") &&
                                       int.TryParse(cm[0..^2], out int hCm) &&
                                       hCm >= 150 &&
                                       hCm <= 193 => true,
                   _ => false
               }
        },
        { "hcl", x => x.Length == 7 && x.StartsWith('#') && int.TryParse(x[1..], NumberStyles.HexNumber, null, out int _) },
        { "ecl", x => s_validEyeValues.Contains(x) },
        { "pid", x => x.Length == 9 && int.TryParse(x, out int _) }
    };

    private static bool InRange(string value, int min, int max)
        => int.TryParse(value, out int val) && val >= min && val <= max;

    private static Dictionary<string, string>[] ParsePassports(string input)
        => input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(y => y.Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(z => z.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(a => a[0], a => a[1])).ToArray();
}
