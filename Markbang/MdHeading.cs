﻿namespace Markbang;

public record MdHeading(int Level, string Text, int TrimOffset = 0) : IMdHeading
{
    internal static bool TryParse(in ReadOnlySpan<char> span, int trimOffset, out IMdBlock? value)
    {
        if (span.IsEmpty || span[0] != '#')
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
