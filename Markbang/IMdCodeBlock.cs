namespace Markbang;

public interface IMdCodeBlock : IMdBlock
{
    IList<string> CodeLines { get; init; }
    string? Language { get; init; }
    bool IsIndented { get; init; }
}
