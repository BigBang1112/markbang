using System.Collections;

namespace Markbang;

public class MdList : IMdList
{
    private readonly IList<IMdListItem> items;

    public int Count => items.Count;
    public bool IsReadOnly => items.IsReadOnly;

    public MdList(IList<IMdListItem> items)
    {
        this.items = items;
    }

    public MdList(IEnumerable<IMdListItem> items) : this(items.ToList())
    {
        
    }

    public MdList(params string[] items) : this(items.Select(x => (IMdListItem)new MdListItem(x, 0)))
    {
        
    }

    public IMdListItem this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }

    internal static bool TryParse(ref ReadOnlySpan<char> span, int level, TextReader reader, out IMdBlock? value)
    {
        var items = new List<IMdListItem>();

        var parsed = RecurseItems(ref span, reader, level, items);
        
        value = new MdList(items);

        return parsed;
    }

    private static bool RecurseItems(ref ReadOnlySpan<char> span, TextReader reader, int level, IList<IMdListItem> items)
    {
        if (!MdListItem.TryParse(in span, level, out IMdListItem? item))
        {
            return false;
        }

        items.Add(item);

        span = reader.ReadLine();

        if (span.IsEmpty)
        {
            return true;
        }

        for (var i = 1; i >= 0; i--)
        {
            _ = RecurseItems(ref span, reader, level + i, items);
        }

        return true;
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
}
