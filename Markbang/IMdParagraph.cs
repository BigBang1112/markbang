namespace Markbang;

public interface IMdParagraph : IMdBlock
{
    IList<string> Lines { get; init; }
}