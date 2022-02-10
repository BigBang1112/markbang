namespace Markbang;

public record MdHorizontalRule(int TrimOffset = 0) : IMdHorizontalRule
{
    private static readonly char[] supportedChars = new[] { '-', '_', '*' };

    /// <remarks>Parameter <paramref name="span"/> should have at least 1 character.</remarks>
    internal static bool TryParse(in ReadOnlySpan<char> span, int trimOffset, out IMdBlock? value)
    {
        var valid = Validate(in span);

        if (!valid)
        {
            value = null;
            return false;
        }

        value = new MdHorizontalRule(trimOffset);

        return true;
    }

    private static bool Validate(in ReadOnlySpan<char> span)
    {
        if (span.Length < 3)
        {
            return false;
        }

        for (var i = 0; i < supportedChars.Length; i++)
        {
            if (span[0] != supportedChars[i])
            {
                continue;
            }

            var charNotSupported = false;

            for (var j = 1; j < span.Length; j++)
            {
                if (span[j] != supportedChars[i])
                {
                    charNotSupported = true;
                    break;
                }
            }

            if (!charNotSupported)
            {
                return true;
            }
        }

        return false;
    }
}
