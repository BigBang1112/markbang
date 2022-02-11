namespace Markbang;

public class MdCodeBlock : IMdCodeBlock
{
    public IList<string> CodeLines { get; init; }
    public string? Language { get; init; }
    public int TrimOffset { get; init; }
    public bool IsIndented { get; init; }

    public MdCodeBlock(IList<string> codeLines, string? language = null, bool isIndented = false, int trimOffset = 0)
    {
        CodeLines = codeLines;
        Language = language;
        IsIndented = isIndented;
        TrimOffset = trimOffset;
    }

    public MdCodeBlock(string code, string? language = null, bool isIndented = false, bool readOnly = false, int trimOffset = 0)
        : this(readOnly ? SplitCode(code) : SplitCode(code).ToList<string>(), language, isIndented, trimOffset)
    {

    }

    private static string[] SplitCode(string code)
    {
        return code.Split(Environment.NewLine);
    }

    public MdCodeBlock(ReadOnlySpan<char> code, string? language = null, bool isIndented = false, int trimOffset = 0)
    {
        Language = language;
        IsIndented = isIndented;
        TrimOffset = trimOffset;
        CodeLines = new List<string>();

        foreach (var line in code.Enumerate(Environment.NewLine))
        {
            CodeLines.Add(line.ToString());
        }
    }

    /// <remarks>Parameter <paramref name="span"/> should have at least 1 character.</remarks>
    internal static bool TryParse(ref ReadOnlySpan<char> span, int trimOffset, TextReader reader, out IMdBlock? value)
    {
        if (trimOffset == 4)
        {
            return TryParseIndented(ref span, trimOffset, reader, out value);
        }

        if (span[0] != '`' || (span.StartsWith("```") && span.Length >= 6 && span.EndsWith("```")))
        {
            value = null;
            return false;
        }

        var language = span.Length > 3 ? span[3..].ToString() : null;
        var codeLines = new List<string>();

        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
            {
                span = line;
                break;
            }

            var lineSpan = line.AsSpan().TrimEnd();

            if (lineSpan.IsEmpty)
            {
                codeLines.Add("");
                continue;
            }
            
            if (lineSpan.StartsWith("```"))
            {
                span = reader.ReadLine();
                break;
            }

            if (line.Length == lineSpan.Length)
            {
                codeLines.Add(line);
            }
            else
            {
                codeLines.Add(lineSpan.ToString());
            }
        }

        value = new MdCodeBlock(codeLines, language, isIndented: false, trimOffset);
        return true;
    }

    private static bool TryParseIndented(ref ReadOnlySpan<char> span, int trimOffset, TextReader reader, out IMdBlock? value)
    {
        var codeLines = new List<string>
        {
            span.ToString()
        };

        while (true)
        {
            span = reader.ReadLine().AsSpan();

            if (span.IsEmpty || span.Trim().IsEmpty)
            {
                codeLines.Add("");
                continue;
            }

            if (span.Length < 5 || !span.StartsWith("    "))
            {
                break;
            }

            codeLines.Add(span[4..].ToString());
        }

        // Maybe temporary, maybe the best solution
        while (codeLines[^1] == "")
        {
            codeLines.RemoveAt(codeLines.Count - 1);
        }

        span = span.TrimStart();

        value = new MdCodeBlock(codeLines, language: null, isIndented: true, trimOffset);
        return true;
    }

    public void Write(TextWriter writer)
    {
        if (IsIndented)
        {
            throw new NotImplementedException();
        }

        if (string.IsNullOrWhiteSpace(Language))
        {
            writer.WriteLine("```");
        }
        else
        {
            writer.WriteLine($"```{Language}");
        }

        foreach (var line in CodeLines)
        {
            writer.WriteLine(line);
        }

        writer.WriteLine("```");
    }
}
