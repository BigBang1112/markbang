using System.Collections;

namespace Markbang;

public class MdList : IMdList
{
    private readonly IList<IMdListItem> items;

    public int Count => items.Count;
    public bool IsReadOnly => items.IsReadOnly;

    public int TrimOffset { get; init; }

    public IMdListItem this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }

    public MdList(IList<IMdListItem> items, int trimOffset = 0)
    {
        this.items = items;
        TrimOffset = trimOffset;
    }

    public MdList(IEnumerable<IMdListItem> items, int trimOffset = 0) : this(items.ToList(), trimOffset)
    {
        
    }

    public MdList(params string[] items) : this(items.Select(x => (IMdListItem)new MdListItem(x, 0)))
    {
        
    }

    public override string ToString()
    {
        if (Count == 0)
        {
            return "Empty list";
        }

        return $"{this[0]} + ({Count - 1} more)";
    }

    internal static bool TryParse(ref ReadOnlySpan<char> span, int level, int trimOffset, TextReader reader, out IMdBlock? value)
    {
        if (!MdListItem.TryParse(in span, level, trimOffset, out IMdListItem? item))
        {
            value = null;
            return false;
        }

        var items = new List<IMdListItem> { item };

        RecurseItems(ref span, reader, level, trimOffset, items, firstItemAdded: true);
        
        value = new MdList(items, trimOffset);

        return true;
    }

    private static void RecurseItems(ref ReadOnlySpan<char> span, TextReader reader, int level, int trimOffset, IList<IMdListItem> items, bool firstItemAdded = false)
    {
        while (true)
        {
            if (!firstItemAdded)
            {
                if (span.IsEmpty || !MdListItem.TryParse(in span, level, trimOffset, out IMdListItem? item))
                {
                    break;
                }

                items.Add(item);
            }

            span = reader.ReadLine();

            if (span.IsEmpty)
            {
                break;
            }

            var nextLevel = span.TrimStartLength() - trimOffset;

            RecurseItems(ref span, reader, nextLevel, trimOffset, items);

            firstItemAdded = false;
        }
    }

    public void Add(IMdListItem item)
    {
        items.Add(item);
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool Contains(IMdListItem item)
    {
        return items.Contains(item);
    }

    public void CopyTo(IMdListItem[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<IMdListItem> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    public int IndexOf(IMdListItem item)
    {
        return items.IndexOf(item);
    }

    public void Insert(int index, IMdListItem item)
    {
        items.Insert(index, item);
    }

    public bool Remove(IMdListItem item)
    {
        return items.Remove(item);
    }

    public void RemoveAt(int index)
    {
        items.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }

    public void Write(TextWriter writer)
    {
        foreach (var item in items)
        {
            item.Write(writer);
        }
    }
}
