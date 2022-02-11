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
                    DefineColumns(trimmedCell, ref columns[columnCounter]);
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

    /// <returns>True if no issues.</returns>
    private static bool DefineColumns(ReadOnlySpan<char> trimmedCell, ref Column column)
    {
        if (trimmedCell.IsEmpty)
        {
            return false;
        }

        var rightAlign = trimmedCell[^1] == ':';
        var leftAlign = trimmedCell[0] == ':' || !rightAlign;
        var centerAlign = leftAlign && rightAlign;

        var offsetLeft = leftAlign ? 1 : 0;
        var offsetRight = rightAlign ? 1 : 0;

        var length = 1;

        for (var i = offsetLeft; i < trimmedCell.Length - offsetRight; i++)
        {
            if (trimmedCell[i] != '-')
            {
                return false;
            }

            length++;
        }

        if (centerAlign)
        {
            column = new Column(length, Alignment.Center);
            return true;
        }

        if (rightAlign)
        {
            column = new Column(length, Alignment.Right);
            return true;
        }

        if (length > 0)
        {
            column = new Column(length, Alignment.Left);
        }

        return true;
    }

    public void Write(TextWriter writer)
    {
        var lineCount = (int)MathF.Ceiling(Cells.Count / (float)Columns.Length);

        for (var y = 0; y < lineCount; y++)
        {
            var lineLength = 2;

            for (var x = 0; x < Columns.Length; x++)
            {
                lineLength += Cells[y * Columns.Length + x].Length;

                if (x < Columns.Length - 1)
                {
                    lineLength += 3;
                }
            }

            var line = new char[lineLength];
            line[0] = '|';
            line[1] = ' ';

            var i = 2;

            for (var x = 0; x < Columns.Length; x++)
            {
                var cell = Cells[y * Columns.Length + x];

                for (var j = 0; j < cell.Length; j++)
                {
                    line[i + j] = cell[j];
                }

                i += cell.Length;

                if (x < Columns.Length - 1)
                {
                    line[i] = ' ';
                    i++;
                    line[i] = '|';
                    i++;
                    line[i] = ' ';
                    i++;
                }
            }

            writer.WriteLine(line);

            if (y == 0)
            {
                writer.WriteLine(WriteHeaderSeparator());
            }
        }
    }

    private char[] WriteHeaderSeparator()
    {
        var hLength = 2;

        for (var x = 0; x < Columns.Length; x++)
        {
            var column = Columns[x];

            hLength += column.Length;

            switch (column.Alignment)
            {
                case Alignment.Center:
                    hLength += 2;
                    break;
                case Alignment.Right:
                    hLength++;
                    break;
            }

            if (x < Columns.Length - 1)
            {
                hLength += 3;
            }
        }

        var hLine = new char[hLength];

        var iH = 2;
        hLine[0] = '|';
        hLine[1] = ' ';

        for (var x = 0; x < Columns.Length; x++)
        {
            var column = Columns[x];

            if (column.Alignment == Alignment.Center)
            {
                hLine[iH] = ':';
                iH++;
            }

            for (var j = 0; j < column.Length; j++)
            {
                hLine[iH + j] = '-';
            }

            iH += column.Length;

            if (column.Alignment == Alignment.Right || column.Alignment == Alignment.Center)
            {
                hLine[iH] = ':';
                iH++;
            }

            if (x < Columns.Length - 1)
            {
                hLine[iH] = ' ';
                iH++;
                hLine[iH] = '|';
                iH++;
                hLine[iH] = ' ';
                iH++;
            }
        }

        return hLine;
    }
}
