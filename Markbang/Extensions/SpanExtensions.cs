namespace Markbang.Extensions;

internal static class SpanExtensions
{
    internal static int TrimStartLength(this in ReadOnlySpan<char> line)
    {
        var length = 0;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == ' ')
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
}
