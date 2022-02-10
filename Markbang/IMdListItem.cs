namespace Markbang;

public interface IMdListItem : IWriteable
{
    string Text { get; init; }
    int Level { get; init; }
    int? Rank { get; init; }
}
