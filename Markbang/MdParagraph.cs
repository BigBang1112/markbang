namespace Markbang;

public class MdParagraph : IMdParagraph
{
    public IList<string> Lines { get; init; }
    public int TrimOffset { get; init; }

    public MdParagraph(ReadOnlySpan<char> span, bool singleLine = false, int trimOffset = 0)
    {
        TrimOffset = trimOffset;

        if (singleLine)
        {
            Lines = new List<string> { span.ToString() };
            return;
        }

        Lines = new List<string>();

        foreach (var line in span.Enumerate(Environment.NewLine))
        {
            Lines.Add(line.ToString());
        }
    }

    public MdParagraph(string text, bool readOnly = false, bool singleLine = false, int trimOffset = 0)
    {
        TrimOffset = trimOffset;

        if (singleLine)
        {
            if (readOnly)
            {
                Lines = new[] { text };
            }
            else
            {
                Lines = new List<string> { text };
            }

            return;
        }

        if (readOnly)
        {
            Lines = text.Split(Environment.NewLine);
        }
        else
        {
            Lines = text.Split(Environment.NewLine).ToList();
        }
    }

    public MdParagraph(IEnumerable<string> lines)
    {
        Lines = lines.ToList();
    }

    public MdParagraph(IList<string> lines)
    {
        Lines = lines;
    }

    public static implicit operator string(MdParagraph m)
    {
        return string.Join(Environment.NewLine, m.Lines);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Lines);
    }

    public void Write(TextWriter writer)
    {
        foreach (var line in Lines)
        {
            writer.WriteLine(line);
        }
    }
}
