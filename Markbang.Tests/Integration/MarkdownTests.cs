namespace Markbang.Tests.Integration;

public class MarkdownTests
{
    private const string GBXNET_md = "Realistic/GBX.NET.md";
    private const string CGameCtnChallenge_md = "Realistic/GBX.NET.md";
    private const string Markbang_md = "Realistic/Markbang.md";

    [Fact]
    public void Parse_GBXNET_ShouldParse()
    {
        var md = Markdown.Parse(GBXNET_md);
    }

    [Fact]
    public void Parse_CGameCtnChallenge_ShouldParse()
    {
        var md = Markdown.Parse(CGameCtnChallenge_md);
    }

    [Fact]
    public void Parse_Markbang_ShouldParse()
    {
        var md = Markdown.Parse(Markbang_md);
    }

    [Fact]
    public void Save_GBXNET_ShouldSave()
    {
        var md = Markdown.Parse(GBXNET_md);

        using var w = new StringWriter();
        md.Save(w);
        var nice = w.ToString();
    }

    [Fact]
    public void Save_CGameCtnChallenge_ShouldSave()
    {
        var md = Markdown.Parse(CGameCtnChallenge_md);

        using var w = new StringWriter();
        md.Save(w);
    }

    [Fact]
    public void Save_Markbang_ShouldSave()
    {
        var md = Markdown.Parse(Markbang_md);

        using var w = new StringWriter();
        md.Save(w);
    }
}
