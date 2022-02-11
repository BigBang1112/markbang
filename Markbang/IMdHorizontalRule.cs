namespace Markbang;

public interface IMdHorizontalRule : IMdBlock
{
    char Char { get; init; }
    int Length { get; init; }
}