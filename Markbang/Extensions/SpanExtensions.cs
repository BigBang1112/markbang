namespace Markbang.Extensions;

internal static class SpanExtensions
{
    internal static int TrimStartLength(this in ReadOnlySpan<char> span)
    {
        var length = 0;

        for (var i = 0; i < span.Length; i++)
        {
            if (span[i] == ' ')
            {
                length++;
            }
            else
            {
                break;
            }
        }

        return length;
    }

    internal static SpanSplitEnumerator Enumerate(this in ReadOnlySpan<char> span, ReadOnlySpan<char> separator)
    {
        return new SpanSplitEnumerator(span, separator);
    }

    internal static SpanSplitEnumerator Enumerate(this in ReadOnlySpan<char> span, char separator)
    {
        return new SpanSplitEnumerator(span, separator);
    }
}
