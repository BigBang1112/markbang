namespace Markbang.Tests.Integration;

public class MarkdownTests
{
    [Fact]
    public void GBXNETmd_ShouldParse()
    {
        var md = Markdown.Parse("Realistic/GBX.NET.md");
    }
}
