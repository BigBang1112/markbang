using BenchmarkDotNet.Attributes;

namespace Markbang.Benchmarks;

[MemoryDiagnoser]
public class MarkdownBenchmarks
{
    private readonly string gbxnetMdString = File.ReadAllText("Realistic/GBX.NET.md");

    [Benchmark]
    public Markdown Parse_GBXNETmd()
    {
        using var reader = new StringReader(gbxnetMdString);
        return Markdown.Parse(reader);
    }
}
