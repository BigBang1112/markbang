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

        while (TryParseBlock(reader, out IMdBlock? block,
            out string? possibleParagraphLine,
            out int possibleParagraphLineTrimOffset))
        {
            if (block is not null)
            {
                blocks.Add(block);
            }

            if (possibleParagraphLine is not null)
            {
                if (paragraph is null)
                {
                    paragraph = new MdParagraph(possibleParagraphLine, readOnly: false, singleLine: true, possibleParagraphLineTrimOffset);

                    continue;
                }

                paragraph.Lines.Add(possibleParagraphLine);

                continue;
            }

            if (paragraph is not null)
            {
                blocks.Add(paragraph);
                paragraph = null;
            }
        }

        return new Markdown(blocks);
    }

    private static bool TryParseBlock(TextReader reader, out IMdBlock? block, out string? possibleParagraphLine, out int possibleParagraphLineTrimOffset)
    {
        var lineStr = reader.ReadLine();

        if (string.IsNullOrEmpty(lineStr))
        {
            block = null;
            possibleParagraphLine = null;
            possibleParagraphLineTrimOffset = 0;

            // True if empty, false if null
            return lineStr == "";
        }

        var line = lineStr.AsSpan();
        var lineTrimmed = line.Trim();

        if (lineTrimmed.IsEmpty)
        {
            block = null;
            possibleParagraphLine = null;
            possibleParagraphLineTrimOffset = 0;
            return true;
        }

        var trimLength = line.TrimStartLength();

        if (TryParseBlock_In(in lineTrimmed, trimLength, out block))
        {
            possibleParagraphLine = null;
            possibleParagraphLineTrimOffset = 0;
            return true;
        }

        if (TryParseBlock_Ref(ref lineTrimmed, trimLength, reader, out block) || lineTrimmed.IsEmpty)
        {
            possibleParagraphLine = null;
            possibleParagraphLineTrimOffset = 0;
            return true;
        }

        possibleParagraphLine = trimLength > 0 ? lineTrimmed.ToString() : lineStr;
        possibleParagraphLineTrimOffset = trimLength;

        return true;
    }

    /// <summary>
    /// Try parsing a block that cannot change the line after its execution.
    /// </summary>
    /// <param name="lineTrimmed"></param>
    /// <param name="trimLength"></param>
    /// <param name="reader"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    private static bool TryParseBlock_In(in ReadOnlySpan<char> lineTrimmed,
                                         int trimLength,
                                         out IMdBlock? block)
    {
        if (MdHeading.TryParse(in lineTrimmed, trimLength, out block))
        {
            return true;
        }

        if (MdHorizontalRule.TryParse(in lineTrimmed, trimLength, out block))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Try parsing a block that could change the line after its execution.
    /// </summary>
    /// <param name="lineTrimmed"></param>
    /// <param name="trimLength"></param>
    /// <param name="reader"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    private static bool TryParseBlock_Ref(ref ReadOnlySpan<char> lineTrimmed,
                                         int trimLength,
                                         TextReader reader,
                                         out IMdBlock? block)
    {
        if (MdCodeBlock.TryParse(ref lineTrimmed, trimLength, reader, out block))
        {
            return true;
        }
        
        if (MdList.TryParse(ref lineTrimmed, level: 0, trimLength, reader, out block))
        {
            return true;
        }

        return false;
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