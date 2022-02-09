namespace Markbang;

public record MdHeading(int Level, string Text) : IMdHeading
{
    public static bool TryParse(ReadOnlySpan<char> span, out IMdBlock? value)
    {
        if (!span.StartsWith("#"))
        {
            value = null;
            return false;
        }

        var level = 1;

        while (span[level] == '#')
        {
            level++;
        }

        if (span[level] != ' ')
        {
            value = null;
            return false;
        }

        level++;

        value = new MdHeading(level, span[level..].ToString());

        return true;
    }
}
