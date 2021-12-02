using System.Numerics;

namespace AdventOfCode;

[PuzzleInput("Day12.txt", 1424, 63447)]
[PuzzleInput("Day12-Sample.txt", 25, 286)]
public class Day12 : IDay
{
    public long Part1(string input)
        => ManhattanDistance(input.SplitNonEmptyLines().Aggregate(Vector3.Zero, DriveShip));

    private static Vector3 DriveShip(Vector3 ship, string command)
        => DriveShip(ship, command[0], int.Parse(command[1..]));

    private static Vector3 DriveShip(Vector3 ship, char op, int value)
        => ship + op switch
        {
            'N' => new Vector3(0, -value, 0),
            'S' => new Vector3(0, value, 0),
            'E' => new Vector3(value, 0, 0),
            'W' => new Vector3(-value, 0, 0),
            'L' => new Vector3(0, 0, -value),
            'R' => new Vector3(0, 0, value),
            'F' => value * VectorAngle(ship.Z * MathF.PI / 180F),
            _ => throw new Exception("Woah!")
        };

    private static Vector3 VectorAngle(float angle)
        => new(MathF.Cos(angle), MathF.Sin(angle), 0);

    private static long ManhattanDistance(Vector3 value)
        => (long)(Math.Abs(value.X) + Math.Abs(value.Y));


    private static readonly Vector4 s_part2StartPosition = new(0, 0, 10, -1);

    public long Part2(string input)
        => ManhattanDistance(input.SplitNonEmptyLines().Aggregate(s_part2StartPosition, DriveShip));

    private static Vector4 DriveShip(Vector4 ship, string command)
        => DriveShip(ship, command[0], int.Parse(command[1..]));

    private static Vector4 DriveShip(Vector4 ship, char op, int value)
        => ship + op switch
        {
            'N' => new Vector4(0, 0, 0, -value),
            'S' => new Vector4(0, 0, 0, value),
            'E' => new Vector4(0, 0, value, 0),
            'W' => new Vector4(0, 0, -value, 0),
            'L' => Rotate(ship, -value),
            'R' => Rotate(ship, value),
            'F' => MoveForwards(ship, value),
            _ => throw new Exception("Woah!")
        };

    private static Vector4 Rotate(Vector4 input, int value)
        => ToWaypointVector(RotateToVector2(input, value) + new Vector2(input.X - input.Z, input.Y - input.W));

    private static Vector2 RotateToVector2(Vector4 input, int value)
        => Vector2.Transform(new Vector2(input.Z - input.X, input.W - input.Y), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, value * MathF.PI / 180.0f));

    private static Vector4 ToWaypointVector(Vector2 input)
        => new(0, 0, input.X, input.Y);

    private static Vector4 ExpandToVector(float xz, float yw)
        => new(xz, yw, xz, yw);

    private static Vector4 MoveForwards(Vector4 input, int value)
        => value * ExpandToVector(input.Z - input.X, input.W - input.Y);

    private static long ManhattanDistance(Vector4 input)
    => (long)(Math.Abs(input.X) + Math.Abs(input.Y));
}
