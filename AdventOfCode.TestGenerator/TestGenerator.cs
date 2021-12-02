using System.Text;
using Microsoft.CodeAnalysis;

namespace AdventOfCode;

[Generator]
public class TestGenerator : ISourceGenerator
{
    private const string AttributeSource = @"
namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class YearToTestAttribute : Attribute
{
    public YearToTestAttribute(Type typeReference) {}
}
";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization((i) => i.AddSource("YearToTestAttribute", AttributeSource));
    }

    private static IEnumerable<IAssemblySymbol> GetAdventOfCodeAssemblies(Compilation compilation)
        => compilation.SourceModule.ReferencedAssemblySymbols.Where(x => x.Name.StartsWith("AdventOfCode.2"));

    private static INamespaceSymbol GetAdventOfCodeNamespace(IAssemblySymbol assembly)
        => assembly.GlobalNamespace.GetNamespaceMembers().Single(x => x.Name == "AdventOfCode");

    private static IEnumerable<INamedTypeSymbol> GetAdventOfCodeDayTypes(INamespaceSymbol ns)
        => ns.GetMembers().OfType<INamedTypeSymbol>().Where(x => x.AllInterfaces.Any(y => y.Name == "IDay"));

    private static string GetQualifiedTypeName(ISymbol symbol)
        => symbol.ContainingNamespace + "." + symbol.Name + ", " + symbol.ContainingAssembly;

    private static PuzzleInputAttribute ExtractAttribute(AttributeData attribute)
        => (PuzzleInputAttribute?)Activator.CreateInstance(
            typeof(PuzzleInputAttribute),
            attribute.ConstructorArguments.Select(x => x.Value).ToArray()
        ) ?? throw new Exception("Unable to parse PuzzleInputAttribute");

    private static string BuildTestClass(string ns, string name, string fullName, IEnumerable<AttributeData> attributes)
    {
        (string part1Data, string part2Data) = GetPartData(attributes);

        if (part1Data.Length < 1 && part2Data.Length < 1) return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine("using AdventOfCode;");
        sb.AppendLine("using Xunit;");
        sb.AppendLine($"namespace {ns};");
        sb.AppendLine($"public class {name}");
        sb.AppendLine("{");
        if (part1Data.Length > 0)
        {
            sb.AppendLine("[Theory]");
            sb.AppendLine(part1Data);
            sb.AppendLine(@$"public void Part1(string file, long answer) => Assert.Equal(answer, TestHelpers.RunPart1(""{fullName}"", file));");
        }
        if (part2Data.Length > 0)
        {
            sb.AppendLine("[Theory]");
            sb.AppendLine(part2Data);
            sb.AppendLine(@$"public void Part2(string file, long answer) => Assert.Equal(answer, TestHelpers.RunPart2(""{fullName}"", file));");
        }
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static (string Part1Data, string Part2Data) GetPartData(IEnumerable<AttributeData> attributes)
    {
        var part1Data = new StringBuilder();
        var part2Data = new StringBuilder();

        foreach (var attribute in attributes)
        {
            var puzzleInput = ExtractAttribute(attribute);

            if (string.IsNullOrWhiteSpace(puzzleInput.Filename)) continue;

            if (!puzzleInput.SkipPart1)
            {
                part1Data.AppendLine(@$"[InlineData(""{puzzleInput.Filename}"", {puzzleInput.Part1Answer})]");
            }

            if (!puzzleInput.SkipPart2)
            {
                part2Data.AppendLine(@$"[InlineData(""{puzzleInput.Filename}"", {puzzleInput.Part2Answer})]");
            }
        }

        return (part1Data.ToString(), part2Data.ToString());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var assembly in GetAdventOfCodeAssemblies(context.Compilation))
        {
            foreach (var type in GetAdventOfCodeDayTypes(GetAdventOfCodeNamespace(assembly)))
            {
                var attributes = type.GetAttributes().Where(x => x.AttributeClass?.Name == "PuzzleInputAttribute");
                if (!attributes.Any()) continue;

                var fullName = GetQualifiedTypeName(type);

                var ns = "_" + assembly.Name.Split('.')[1];

                string source = BuildTestClass(ns, type.Name, fullName, attributes);

                if (!string.IsNullOrWhiteSpace(source))
                {
                    context.AddSource($"{assembly.Name}.{type.Name}", source);
                }
            }
        }
    }
}
