namespace Markbang.Tests.Unit;

public class MdListTests
{
    [Fact]
    public void TryParse_Mixed_ShouldCreateMdList()
    {
        var str =
"- hello" + Environment.NewLine +
"  1. how are you doing" + Environment.NewLine +
"  2. ok" + Environment.NewLine +
"    - bruh" + Environment.NewLine +
"3. kek" + Environment.NewLine +
"  - I am doing great";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello"),
            new MdListItem("how are you doing", Level: 2, Rank: 1),
            new MdListItem("ok", Level: 2, Rank: 2),
            new MdListItem("bruh", Level: 4),
            new MdListItem("kek", Rank: 3),
            new MdListItem("I am doing great", Level: 2),
        };

        AssertJustList(str, trimOffset: 0, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_OneItemUnordered_ShouldCreateMdList()
    {
        var str = "- hello";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello")
        };

        AssertJustList(str, trimOffset: 0, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_MultipleItemsUnordered_ShouldCreateMdList()
    {
        var str =
"- hello" + Environment.NewLine +
"- how are you doing" + Environment.NewLine +
"- I am doing great";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello"),
            new MdListItem("how are you doing"),
            new MdListItem("I am doing great"),
        };

        AssertJustList(str, trimOffset: 0, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_MultipleItemsUnorderedSpaced_ShouldCreateMdList()
    {
        var str =
" - hello" + Environment.NewLine +
" - how are you doing" + Environment.NewLine +
" - I am doing great";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello"),
            new MdListItem("how are you doing"),
            new MdListItem("I am doing great"),
        };

        AssertJustList(str, trimOffset: 1, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_OneItemOrdered_ShouldCreateMdList()
    {
        var str = "1. hello";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello", Rank: 1)
        };

        AssertJustList(str, trimOffset: 0, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_MultipleItemsOrdered_ShouldCreateMdList()
    {
        var str =
"1. hello" + Environment.NewLine +
"2. how are you doing" + Environment.NewLine +
"3. I am doing great";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello", Rank: 1),
            new MdListItem("how are you doing", Rank: 2),
            new MdListItem("I am doing great", Rank: 3),
        };

        AssertJustList(str, trimOffset: 0, out IMdList? actualList);
        Assert.Equal(expectedList, actualList);
    }

    [Fact]
    public void TryParse_MultipleItemsUnorderedHeaderAfter_ShouldCreateMdList()
    {
        var str =
"- hello" + Environment.NewLine +
"- how are you doing" + Environment.NewLine +
"- I am doing great" + Environment.NewLine +
"# header";

        var expectedList = new IMdListItem[]
        {
            new MdListItem("hello"),
            new MdListItem("how are you doing"),
            new MdListItem("I am doing great"),
        };

        AssertAnyList(str, trimOffset: 0, out IMdList? actualList, out ReadOnlySpan<char> line);
        Assert.Equal(expectedList, actualList);
        Assert.True(MdHeading.TryParse(line, trimOffset: 0, out _));
    }

    private static void AssertJustList(string str, int trimOffset, out IMdList? actualList)
    {
        AssertAnyList(str, trimOffset, out actualList, out ReadOnlySpan<char> line);
        Assert.True(line.IsEmpty);
    }

    private static void AssertAnyList(string str, int trimOffset, out IMdList? actualList, out ReadOnlySpan<char> line)
    {
        var r = new StringReader(str);
        line = r.ReadLine();
        var parsed = MdList.TryParse(ref line, level: 0, trimOffset, r, out IMdBlock? mdBlock);

        actualList = mdBlock as IMdList;

        Assert.NotNull(actualList);
        Assert.True(parsed);
    }
}
