namespace Markbang;

public record MdHorizontalRule(char Char = '-', int Length = 3, int TrimOffset = 0) : IMdHorizontalRule
{
    private static readonly char[] supportedChars = new[] { '-', '_', '*' };

    /// <remarks>Parameter <paramref name="span"/> should have at least 1 character.</remarks>
    internal static bool TryParse(in ReadOnlySpan<char> span, int trimOffset, out IMdBlock? value)
    {
        var valid = Validate(in span, out int length, out char hrChar);

        if (!valid)
        {
            value = null;
            return false;
        }

        value = new MdHorizontalRule(hrChar, length, trimOffset);

        return true;
    }

    private static bool Validate(in ReadOnlySpan<char> span, out int length, out char hrChar)
    {
        if (span.Length < 3)
        {
            length = default;
            hrChar = default;
            return false;
        }

        for (var i = 0; i < supportedChars.Length; i++)
        {
            var ch = supportedChars[i];

            if (span[0] != ch)
            {
                continue;
            }

            var charNotSupported = false;

            for (var j = 1; j < span.Length; j++)
            {
                if (span[j] != ch)
                {
                    charNotSupported = true;
                    break;
                }
            }

            if (!charNotSupported)
            {
                length = span.Length;
                hrChar = ch;
                return true;
            }
        }

        length = default;
        hrChar = default;
        return false;
    }

    public void Write(TextWriter writer)
    {
        writer.WriteLine(new string(Char, Length));
    }
}
