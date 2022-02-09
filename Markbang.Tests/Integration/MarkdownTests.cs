namespace Markbang.Tests.Integration;

public class MarkdownTests
{
    [Fact]
    public void Parse_GBXNETmd_ShouldParse()
    {
        var md = Markdown.Parse("Realistic/GBX.NET.md");
    }
}
