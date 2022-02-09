using System.Collections;

namespace Markbang;

public class Markdown : IList<IMdBlock>
{
    private readonly IList<IMdBlock> blocks;

    public int Count => blocks.Count;
    public bool IsReadOnly => blocks.IsReadOnly;

    public Markdown(IList<IMdBlock> blocks)
    {
        this.blocks = blocks;
    }

    public Markdown(IEnumerable<IMdBlock> blocks) : this(blocks.ToList())
    {
        
    }

    public Markdown() : this(new List<IMdBlock>())
    {

    }

    public IMdBlock this[int index]
    {
        get => blocks[index];
        set => blocks[index] = value;
    }

    public static Markdown Parse(string fileName)
    {
        using var r = new StreamReader(fileName);
        return Parse(r);
    }

    public static Markdown Parse(TextReader reader)
    {
        var blocks = new List<IMdBlock>();

        var paragraph = default(IMdParagraph);

        while (TryParseBlock(reader, out IMdBlock? block, out ReadOnlySpan<char> possibleParagraphLine))
        {
            if (possibleParagraphLine.Length > 0)
            {
                if (paragraph is null)
                {
                    paragraph = new MdParagraph(possibleParagraphLine);

                    continue;
                }

                paragraph.Lines.Add(possibleParagraphLine.ToString());

                continue;
            }

            if (paragraph is not null)
            {
                blocks.Add(paragraph);
                paragraph = null;
            }

            if (block is null)
            {
                continue;
            }

            blocks.Add(block);
        }

        return new Markdown(blocks);
    }

    private static bool TryParseBlock(TextReader reader, out IMdBlock? block, out ReadOnlySpan<char> possibleParagraphLine)
    {
        var lineStr = reader.ReadLine();

        if (string.IsNullOrEmpty(lineStr))
        {
            block = null;
            possibleParagraphLine = ReadOnlySpan<char>.Empty;

            // True if empty, false if null
            return lineStr == "";
        }

        var line = lineStr.AsSpan();

        if (MdHeading.TryParse(in line, out block))
        {
            possibleParagraphLine = ReadOnlySpan<char>.Empty;
            return true;
        }

        if (MdList.TryParse(ref line, level: 0, reader, out block))
        {
            possibleParagraphLine = ReadOnlySpan<char>.Empty;
            return true;
        }

        possibleParagraphLine = line;

        return true;
    }

    public void Add(IMdBlock item)
    {
        blocks.Add(item);
    }

    public void Clear()
    {
        blocks.Clear();
    }

    public bool Contains(IMdBlock item)
    {
        return blocks.Contains(item);
    }

    public void CopyTo(IMdBlock[] array, int arrayIndex)
    {
        blocks.CopyTo(array, arrayIndex);
    }

    public IEnumerator<IMdBlock> GetEnumerator()
    {
        return blocks.GetEnumerator();
    }

    public int IndexOf(IMdBlock item)
    {
        return blocks.IndexOf(item);
    }

    public void Insert(int index, IMdBlock item)
    {
        blocks.Insert(index, item);
    }

    public bool Remove(IMdBlock item)
    {
        return blocks.Remove(item);
    }

    public void RemoveAt(int index)
    {
        blocks.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return blocks.GetEnumerator();
    }
}