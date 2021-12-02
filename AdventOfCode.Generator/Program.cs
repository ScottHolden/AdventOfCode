string basePath = @"C:\code\AdventOfCode\";

Console.Write("Year to generate: ");
string? yearText = Console.ReadLine();
if (!int.TryParse(yearText, out int year) ||
    year < 1000 || year > 9999)
{
    Console.Write("Invalid year.");
    return;
}

string name = "AdventOfCode." + year;

string path = Path.Join(basePath, name);
if (Directory.Exists(path))
{
    Console.Write(name + " already exists.");
    return;
}

Directory.CreateDirectory(path);

string daysPath = Path.Join(path, "Days");
Directory.CreateDirectory(daysPath);

string dataPath = Path.Join(path, "Data");
Directory.CreateDirectory(dataPath);

File.WriteAllText(Path.Join(path, "Program.cs"), @$"
using AdventOfCode;

AdventRunner.Run({year});
".Trim());

File.WriteAllText(Path.Join(path, name + ".csproj"), @$"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>AdventOfCode{year}</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <EmbeddedResource Include=""Data\*.txt"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\AdventOfCode.Core\AdventOfCode.Core.csproj"" />
  </ItemGroup>
</Project>
".Trim());

for (int i = 1; i <= 25; i++)
{
    File.WriteAllText(Path.Join(daysPath, $"Day{i:D2}.cs"), @$"
namespace AdventOfCode;

//[PuzzleInput(""Day{i:D2}.txt"")]
//[PuzzleInput(""Day{i:D2}-Sample.txt"")]
public class Day{i:D2} : IDay
{{
    public long Part1(string input) => -1;
    public long Part2(string input) => -1;
}}".Trim());
}

File.WriteAllText(Path.Join(dataPath, $"Day01.txt"), string.Empty);

Console.WriteLine("Done!");
