namespace AdventOfCode;

[PuzzleInput("Day08.txt", 1949, 2092)]
[PuzzleInput("Day08-Sample.txt", 5, 8)]
public class Day08 : IDay
{
    public long Part1(string input)
        => RunCode(ParseFile(input.SplitNonEmptyLines())).Acc;

    private static (long Acc, bool Success) RunCode((Instruction Instruction, int Data)[] code)
    {
        bool[] visited = new bool[code.Length];
        long acc = 0;
        for (int pos = 0; pos < code.Length; pos++)
        {
            if (visited[pos]) return (acc, false);

            visited[pos] = true;

            if (code[pos].Instruction == Instruction.Acc)
            {
                acc += code[pos].Data;
            }
            else if (code[pos].Instruction == Instruction.Jmp)
            {
                pos += code[pos].Data - 1;
            }
        }
        return (acc, true);
    }

    public long Part2(string input)
    {
        (Instruction Instruction, int Data)[] code = ParseFile(input.SplitNonEmptyLines());
        for (int i = 0; i < code.Length; i++)
        {
            var originalInstruction = code[i].Instruction;

            code[i].Instruction = code[i].Instruction switch
            {
                Instruction.Nop => Instruction.Jmp,
                Instruction.Jmp => Instruction.Nop,
                Instruction x => x
            };

            (long acc, bool success) = RunCode(code);

            if (success) return acc;

            code[i].Instruction = originalInstruction;
        }

        throw new Exception("No solution found!");
    }

    private static (Instruction Instruction, int Data)[] ParseFile(string[] data)
        => data.Select(x => x.Split(' '))
                .Select(x => (Enum.Parse<Instruction>(x[0], true), int.Parse(x[1])))
                .ToArray();

    private enum Instruction
    {
        Acc,
        Jmp,
        Nop
    }
}
