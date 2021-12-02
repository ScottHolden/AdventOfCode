using System.Reflection;

namespace AdventOfCode;

public static class PuzzleHelpers
{
    public static IEnumerable<Type> GetDayTypes(Assembly assembly)
        => assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && typeof(IDay).IsAssignableFrom(x))
                .OrderByDescending(x => int.Parse(x.Name.Substring(3)));

    public static IDay? CreateInstance(Type t)
        => PuzzleDisplay.MeasureWithMessage(() => (IDay?)Activator.CreateInstance(t), t.Name + ".ctor");

    public static string ReadFile(Assembly assembly, string filename)
        => PuzzleDisplay.MeasureWithMessage(() =>
        {
            string resourcePath = assembly.GetManifestResourceNames().Single(x => x.EndsWith(filename));
            using Stream stream = assembly.GetManifestResourceStream(resourcePath) ?? throw new FileNotFoundException(filename);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("\r", "");
        }, " " + Path.GetFileNameWithoutExtension(filename));
}
