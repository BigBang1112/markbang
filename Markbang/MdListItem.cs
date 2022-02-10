using System.Diagnostics.CodeAnalysis;

namespace Markbang;

public record MdListItem(string Text, int Level = 0, int? Rank = null) : IMdListItem
{
    public override string ToString()
    {
        if (Level > 0)
        {
            var spacing = new string(' ', Level * 2);

            if (Rank is null)
            {
                return $"{spacing}- {Text}";
            }

            return $"{spacing}{Rank}. {Text}";
        }

        if (Rank is null)
        {
            return $"- {Text}";
        }

        return $"{Rank}. {Text}";
    }

    internal static bool TryParse(in ReadOnlySpan<char> span, int level, int trimOffset, [NotNullWhen(true)] out IMdListItem? item)
    {
        if (trimOffset > span.Length)
        {
            item = null;
            return false;
        }        

        var slice = level == 0 && trimOffset == 0 ? span : span[(level + trimOffset)..];

        // Can happen if next level of identation is attempted
        if (slice.IsEmpty)
        {
            item = null;
            return false;
        }

        var isUnorderedItem = IsUnorderedItem(in slice);
        var isOrderedItem = false;

        var tempRank = 0;

        if (!isUnorderedItem)
        {
            isOrderedItem = IsOrderedItem(in slice, out tempRank);

            if (!isOrderedItem)
            {
                item = null;
                return false;
            }
        }

        var text = GetTextFromListItem(in slice, isUnorderedItem, isOrderedItem).ToString();
        var rank = isOrderedItem ? tempRank : default(int?);

        item = new MdListItem(text, level, rank);
        return true;
    }

    private static bool IsOrderedItem(in ReadOnlySpan<char> span, out int rank)
    {
        // if the first char is even numeric
        if (span[0] < 48 || span[0] > 57)
        {
            rank = 0;
            return false;
        }

        var indexOfDot = span.IndexOf('.');

        if (indexOfDot == -1)
        {
            rank = 0;
            return false;
        }

        return int.TryParse(span[..indexOfDot], out rank);
    }

    private static bool IsUnorderedItem(in ReadOnlySpan<char> span)
    {
        return span[0] == '-';
    }

    private static ReadOnlySpan<char> GetTextFromListItem(in ReadOnlySpan<char> span, bool isUnorderedItem, bool isOrderedItem)
    {
        if (isUnorderedItem)
        {
            return GetTextFromUnorderedListItem(in span);
        }

        if (isOrderedItem)
        {
            return GetTextFromOrderedListItem(in span);
        }

        throw new Exception();
    }

    private static ReadOnlySpan<char> GetTextFromOrderedListItem(in ReadOnlySpan<char> span)
    {
        var index = span.IndexOf(". ");

        if (index < 0)
        {
            return "";
        }

        return span[(index + 2)..];
    }

    private static ReadOnlySpan<char> GetTextFromUnorderedListItem(in ReadOnlySpan<char> span)
    {
        return span[2..].ToString();
    }
}