namespace Markbang;

internal ref struct SpanSplitEnumerator
{
    private readonly char? separatorChar;
    private readonly ReadOnlySpan<char> separator;

    private ReadOnlySpan<char> rest;
    private ReadOnlySpan<char> current;
    private bool isActive;

    internal SpanSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator)
    {
        rest = span;
        current = default;
        isActive = true;
        separatorChar = null;
        this.separator = separator;
    }

    internal SpanSplitEnumerator(ReadOnlySpan<char> span, char separator)
    {
        rest = span;
        current = default;
        isActive = true;
        separatorChar = separator;
        this.separator = default;
    }

    public SpanSplitEnumerator GetEnumerator() => this;
    public ReadOnlySpan<char> Current => current;

    public bool MoveNext()
    {
        if (!isActive)
        {
            return false;
        }

        var index = separatorChar is null ? rest.IndexOf(separator) : rest.IndexOf(separatorChar.Value);

        if (index >= 0)
        {
            current = rest[..index];
            rest = rest[(index + (separatorChar is null ? separator.Length : 1))..];

            return true;
        }

        isActive = false;
        current = rest;
        rest = default;

        return true;
    }
}
