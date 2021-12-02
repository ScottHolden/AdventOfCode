namespace AdventOfCode;

[PuzzleInput("Day25.txt", 7936032, true)]
[PuzzleInput("Day25-Sample.txt", 14897079, true)]
public class Day25 : IDay
{
    public long Part1(string input)
    {
        long[] lines = input.ParseLinesAsLong();
        long cardSecretLoopSize;
        long doorSecretLoopSize;

        long cardPublicKey = lines[0];
        cardSecretLoopSize = Find(7, cardPublicKey);

        long doorPublicKey = lines[1];
        doorSecretLoopSize = Find(7, doorPublicKey);

        long key1 = Transform(cardPublicKey, doorSecretLoopSize);
        long key2 = Transform(doorPublicKey, cardSecretLoopSize);

        return key1;
    }

    private static long Transform(long subjectNumber, long loopSize)
    {
        const long divValue = 20201227;
        long value = 1;

        for (long i = 0; i < loopSize; i++)
        {
            value *= subjectNumber;
            value = value % divValue;
        }

        return value;
    }
    private static long Find(long subjectNumber, long target)
    {
        const long divValue = 20201227;
        long value = 1;

        for (long i = 0; i < long.MaxValue; i++)
        {
            if (value == target) return i;
            value *= subjectNumber;
            value = value % divValue;
        }

        return -1;
    }

    public long Part2(string input)
    {
        return -1;
    }
}
