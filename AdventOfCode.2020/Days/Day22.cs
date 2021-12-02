using System.Text;

namespace AdventOfCode;

[PuzzleInput("Day22.txt", 32199, 33780)]
[PuzzleInput("Day22-Sample01.txt", 306, 291)]
public class Day22 : IDay
{
    public long Part1(string input)
    {
        List<byte>[] cards = ParseCards(input);
        while (!cards.Any(x => x.Count == 0))
        {
            byte a = cards[0][0];
            cards[0].RemoveAt(0);
            byte b = cards[1][0];
            cards[1].RemoveAt(0);
            if (a > b)
            {
                cards[0].Add(a);
                cards[0].Add(b);
            }
            else
            {
                cards[1].Add(b);
                cards[1].Add(a);
            }
        }
        return SumCards(cards[0].Count > 0 ? cards[0] : cards[1]);
    }

    private static long SumCards(List<byte> hand)
    {
        byte[] solution = hand.ToArray();
        Array.Reverse(solution);
        long sum = 0;
        for (int i = 0; i < solution.Length; i++)
        {
            sum += (i + 1) * solution[i];
        }
        return sum;
    }

    private static List<byte>[] ParseCards(string input)
        => input.Split("\n\n").Select(x => new List<byte>(x.Split('\n').Skip(1).Select(byte.Parse))).ToArray();

    public long Part2(string input)
    {
        List<byte>[] cards = ParseCards(input);
        (List<byte>[] finalCards, int winner) = PlayGame(cards);
        return SumCards(finalCards[winner]);
    }

    private static (List<byte>[] Cards, int Winner) PlayGame(List<byte>[] cards)
    {
        HashSet<string> previous = new();
        while (!cards.Any(x => x.Count == 0))
        {
            var prev = ToPrev(cards);
            if (previous.Contains(prev)) return (cards, 0);
            previous.Add(prev);

            byte a = cards[0][0];
            cards[0].RemoveAt(0);
            byte b = cards[1][0];
            cards[1].RemoveAt(0);

            if (a <= cards[0].Count &&
                b <= cards[1].Count)
            {
                int winner = PlayGame(new[]
                {
                        new List<byte>(cards[0].Take(a)),
                        new List<byte>(cards[1].Take(b))
                    }).Winner;
                cards[winner].Add(winner == 0 ? a : b);
                cards[winner].Add(winner == 0 ? b : a);
            }
            else if (a > b)
            {
                cards[0].Add(a);
                cards[0].Add(b);
            }
            else
            {
                cards[1].Add(b);
                cards[1].Add(a);
            }
        }
        return (cards, cards[0].Count > 0 ? 0 : 1);
    }
    private static string ToPrev(List<byte>[] cards)
    {
        var a = cards[0];
        var b = cards[1];
        byte[] c = new byte[a.Count + 1 + b.Count];
        a.CopyTo(c, 0);
        b.CopyTo(c, a.Count + 1);
        return Encoding.ASCII.GetString(c);
    }
}
