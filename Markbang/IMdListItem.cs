namespace Markbang;

public interface IMdListItem
{
    string Text { get; init; }
    int Level { get; init; }
    int? Rank { get; init; }
}
