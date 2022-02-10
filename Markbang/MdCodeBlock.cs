using System.Text;

namespace Markbang;

public class MdCodeBlock : IMdCodeBlock
{
    public IList<string> CodeLines { get; init; }
    public string? Language { get; init; }
    public int TrimOffset { get; init; }

    public MdCodeBlock(IList<string> codeLines, string? language = null, int trimOffset = 0)
    {
        CodeLines = codeLines;
        Language = language;
        TrimOffset = trimOffset;
    }

    public MdCodeBlock(string code, string? language = null, bool readOnly = false, int trimOffset = 0)
        : this(readOnly ? SplitCode(code) : SplitCode(code).ToList<string>(), language, trimOffset)
    {

    }

    private static string[] SplitCode(string code)
    {
        return code.Split(Environment.NewLine);
    }

    public MdCodeBlock(ReadOnlySpan<char> code, string? language = null, int trimOffset = 0)
    {
        CodeLines = new List<string>();

        foreach (var line in code.EnumerateLines())
        {
            CodeLines.Add(line.ToString());
        }
    }

    internal static bool TryParse(ref ReadOnlySpan<char> span, int trimOffset, TextReader reader, out IMdBlock? value)
    {
        if (trimOffset == 4)
        {
            return TryParseIndented(ref span, trimOffset, reader, out value);
        }

        if (span[0] != '`' || (!span.StartsWith("```") && span.Length > 3 && !span.EndsWith("```")))
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

        value = new MdCodeBlock(codeLines, language, trimOffset);
        return false;
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

        span = span.TrimStart();

        value = new MdCodeBlock(codeLines, language: null, trimOffset);
        return true;
    }
}
