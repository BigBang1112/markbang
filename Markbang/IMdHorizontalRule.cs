namespace Markbang;

public interface IMdHorizontalRule : IMdBlock
{
    internal static bool TryParse(in ReadOnlySpan<char> span, out IMdBlock? value)
    {
        if (span.IsEmpty || span[0] != '-')
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

        value = new MdHeading(level, span[level..].Trim().ToString());

        return true;
    }
}