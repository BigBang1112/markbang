namespace Markbang;

public class MdTable : IMdTable
{
    public IList<string> Cells { get; init; }
    public Column[] Columns { get; init; }
    public int TrimOffset { get; init; }

    public MdTable(IList<string> cells, Column[] columns, int trimOffset = 0)
    {
        Cells = cells;
        Columns = columns;
        TrimOffset = trimOffset;
    }

    /// <remarks>Parameter <paramref name="span"/> should have at least 1 character.</remarks>
    internal static bool TryParse(ref ReadOnlySpan<char> span, int trimOffset, TextReader reader, out IMdBlock? value)
    {
        if (span[0] != '|')
        {
            value = null;
            return false;
        }

        var cells = default(List<string>);

        foreach (var cell in span[1..].Enumerate('|'))
        {
            cells ??= new List<string>();
            cells.Add(cell.Trim().ToString());
        }

        if (cells is null)
        {
            value = null;
            return false;
        }

        var columns = new Column[cells.Count];
        var columnsDefined = false;
        var hasIssueWithColumnDefinition = false;

        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
            {
                span = line;
                break;
            }

            var lineSpan = line.AsSpan().Trim();

            if (lineSpan.IsEmpty || lineSpan[0] != '|')
            {
                span = lineSpan;
                break;
            }

            var columnCounter = 0;

            foreach (var cell in lineSpan[1..].Enumerate('|'))
            {
                if (!columnsDefined && cell.IsEmpty)
                {
                    hasIssueWithColumnDefinition = true;
                    break;
                }

                var trimmedCell = cell.Trim();

                if (!columnsDefined)
                {
                    if (trimmedCell.IsEmpty)
                    {
                        hasIssueWithColumnDefinition = true;
                        break;
                    }

                    var rightAlign = trimmedCell[^1] == ':';
                    var leftAlign = trimmedCell[0] == ':' || !rightAlign;
                    var centerAlign = leftAlign && rightAlign;

                    var offsetLeft = leftAlign ? 1 : 0;
                    var offsetRight = rightAlign ? 1 : 0;

                    for (var i = offsetLeft; i < trimmedCell.Length - offsetRight; i++)
                    {
                        if (trimmedCell[i] != '-')
                        {
                            hasIssueWithColumnDefinition = true;
                            break;
                        }
                    }

                    if (!hasIssueWithColumnDefinition)
                    {
                        if (centerAlign)
                        {
                            columns[columnCounter] = new Column(Alignment.Center);
                        }
                        else if (rightAlign)
                        {
                            columns[columnCounter] = new Column(Alignment.Right);
                        }
                    }
                }
                else
                {
                    cells.Add(trimmedCell.ToString());
                }

                columnCounter++;
            }

            if (hasIssueWithColumnDefinition)
            {
                break;
            }

            columnsDefined = true;
        }

        value = new MdTable(cells, columns, trimOffset);
        return true;
    }
}
