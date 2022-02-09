namespace Markbang;

public interface IMdHeading : IMdBlock
{
    int Level { get; init; }
    string Text { get; init; }
}
