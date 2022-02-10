namespace Markbang;

public interface IMdBlock : IWriteable
{
    int TrimOffset { get; init; }
}
