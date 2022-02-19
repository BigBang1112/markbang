using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Markbang.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net50)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
public class MarkdownBenchmarks
{
    private readonly string gbxnetMdString = File.ReadAllText("Realistic/GBX.NET.md");
    private readonly Markdown gbxnetMd = Markdown.Parse("Realistic/GBX.NET.md");
    private readonly string cgamectnchallengeMdString = File.ReadAllText("Realistic/CGameCtnChallenge.md");
    private readonly Markdown cgamectnchallengeMd = Markdown.Parse("Realistic/CGameCtnChallenge.md");
    private readonly string markbangMdString = File.ReadAllText("Realistic/Markbang.md");
    private readonly Markdown markbangMd = Markdown.Parse("Realistic/Markbang.md");

    [Benchmark]
    public Markdown Parse_GBXNET_Md()
    {
        using var reader = new StringReader(gbxnetMdString);
        return Markdown.Parse(reader);
    }

    [Benchmark]
    public void Save_GBXNET_Md()
    {
        using var writer = new StringWriter();
        gbxnetMd.Save(writer);
    }

    [Benchmark]
    public Markdown Parse_CGameCtnChallenge_Md()
    {
        using var reader = new StringReader(cgamectnchallengeMdString);
        return Markdown.Parse(reader);
    }

    [Benchmark]
    public void Save_CGameCtnChallenge_Md()
    {
        using var writer = new StringWriter();
        cgamectnchallengeMd.Save(writer);
    }

    [Benchmark]
    public Markdown Parse_Markbang_Md()
    {
        using var reader = new StringReader(markbangMdString);
        return Markdown.Parse(reader);
    }

    [Benchmark]
    public void Save_Markbang_Md()
    {
        using var writer = new StringWriter();
        markbangMd.Save(writer);
    }
}
