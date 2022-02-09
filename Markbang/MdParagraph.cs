using System.Collections;

namespace Markbang;

public class MdParagraph : IMdParagraph
{
    private readonly IList<string> lines;

    public int Count => lines.Count;
    public bool IsReadOnly => lines.IsReadOnly;

    public string this[int index]
    {
        get => lines[index];
        set => lines[index] = value;
    }

    public MdParagraph(ReadOnlySpan<char> span)
    {
        lines = new List<string>();

        foreach (var line in span.EnumerateLines())
        {
            lines.Add(line.ToString());
        }
    }

    public MdParagraph(string text, bool readOnly = false)
    {
        if (readOnly)
        {
            lines = text.Split(Environment.NewLine);
        }
        else
        {
            lines = text.Split(Environment.NewLine).ToList();
        }
    }

    public MdParagraph(IEnumerable<string> lines)
    {
        this.lines = lines.ToList();
    }

    public MdParagraph(IList<string> lines)
    {
        this.lines = lines;
    }

    public int IndexOf(string item)
    {
        return lines.IndexOf(item);
    }

    public void Insert(int index, string item)
    {
        lines.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        lines.RemoveAt(index);
    }

    public void Add(string item)
    {
        lines.Add(item);
    }

    public void Clear()
    {
        lines.Clear();
    }

    public bool Contains(string item)
    {
        return lines.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        lines.CopyTo(array, arrayIndex);
    }

    public bool Remove(string item)
    {
        return lines.Remove(item);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return lines.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return lines.GetEnumerator();
    }
}
