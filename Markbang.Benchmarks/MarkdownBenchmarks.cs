using BenchmarkDotNet.Attributes;

namespace Markbang.Benchmarks;

[MemoryDiagnoser]
public class MarkdownBenchmarks
{
    private readonly string gbxnetMdString = File.ReadAllText("Realistic/GBX.NET.md");
    private readonly Markdown gbxnetMd = Markdown.Parse("Realistic/GBX.NET.md");

    [Benchmark]
    public Markdown Parse_GBXNETmd()
    {
        using var reader = new StringReader(gbxnetMdString);
        return Markdown.Parse(reader);
    }

    [Benchmark]
    public void Save_GBXNETmd()
    {
        using var writer = new StringWriter();
        gbxnetMd.Save(writer);
    }
}
