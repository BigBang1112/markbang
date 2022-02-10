namespace Markbang;

public record MdHeading(int Level, string Text, int TrimOffset = 0) : IMdHeading
{
    public void Write(TextWriter writer)
    {
        writer.WriteLine(ToCharArray());
    }

    public char[] ToCharArray()
    {
        var array = new char[TrimOffset + Level + 1 + Text.Length];

        for (var i = 0; i < array.Length; i++)
        {
            if (i < TrimOffset)
            {
                array[i] = ' ';
                continue;
            }

            var fromTrim = i - TrimOffset;

            if (fromTrim < Level)
            {
                array[i] = '#';
                continue;
            }

            if (fromTrim == Level)
            {
                array[i] = ' ';
                continue;
            }

            array[i] = Text[fromTrim - Level - 1];
        }

        return array;
    }

    public static bool TryParse(in ReadOnlySpan<char> span, out IMdBlock? value)
    {
        if (span.IsEmpty)
        {
            value = null;
            return false;
        }

        var trimmedSpan = span.Trim();

        if (trimmedSpan.IsEmpty)
        {
            value = null;
            return false;
        }

        var trimOffset = trimmedSpan.TrimStartLength();

        return TryParse(in span, trimOffset, out value);
    }

    /// <remarks>Parameter <paramref name="span"/> should have at least 1 character.</remarks>
    internal static bool TryParse(in ReadOnlySpan<char> span, int trimOffset, out IMdBlock? value)
    {
        if (span[0] != '#')
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

        value = new MdHeading(level, span[level..].Trim().ToString(), trimOffset);

        return true;
    }
}
