using OutputEngine.Primitives;
using System.Text;

namespace OutputEngine.Targets;

public abstract class CliOutput : TextWriter
{
    /// <summary>
    /// Base constructor for writers
    /// </summary>
    /// <param name="outputContext"></param>
    /// <remarks>
    /// Implementing classes should set an appropriate default 
    /// OutputStyles if it is null.
    /// </remarks>
    protected CliOutput(OutputContext outputContext)
    {
        Redirecting = outputContext.ShouldRedirect;
        OutputContext = outputContext;
        Width = outputContext.Width;
        IndentSize = outputContext.IndentSize;
        OutputStyles = outputContext.OutputStyles ?? new OutputStyles(); ;
    }
    public OutputContext OutputContext { get; }

    public override Encoding Encoding => OutputContext.Encoding;
    protected bool Redirecting { get; }
    protected int Width { get; }
    protected int IndentSize { get; }
    protected OutputStyles?  OutputStyles { get; set;}

    private readonly StringBuilder buffer = new();
    public string GetBuffer() => buffer.ToString();
    public void ClearBuffer() => buffer.Clear();

    public void WriteLine<T>(T? output)
    {
        if (output is not null)
        {
            Write(output);
        }
        // This needs to call the parameterless form because
        // a line break is not be Environment.NewLine in HTML
        // or markdown
        WriteLine();
    }
    public override void WriteLine()
    {
        Write(Environment.NewLine);
    }

    public override void Write(string? text)
    {
        if (Redirecting)
        {
            buffer.Append(text);
        }
        else
        {
            Console.Write(text);
        }
    }

    public void Write(Layout layout, int indentCount = 0)
    {
        foreach (var section in layout.Sections)
        {
            Write(section, indentCount);
        }
    }

    public virtual void Write(Section section, int indentCount = 0)
    {
        Write(section.Title);
        WriteLine(":");
        Write((Group)section, 1);
    }

    public virtual void Write(Group group, int indentCount = 0)
    {
        if (!group.Any())
        {
            return;
        }
        var last = group.Last();
        foreach (var element in group)
        {
            switch (element)
            {
                case Table table: Write(table, indentCount); break;
                case Paragraph paragraph: Write(paragraph, indentCount); break;
                default: throw new InvalidOperationException("Unknown element type");
            }
        }
    }

    public virtual void Write(Paragraph paragraph, int indentCount = 0)
    {
        var useWidth = Width - indentCount * IndentSize;
        var useIndent = new string(' ', indentCount * IndentSize);
        if (paragraph.Count() == 0)
        {
            return;
        }
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);
        var lastLine = lines.Last();
        (string? open, string? close) = GetStyle(paragraph.Appearance);
        if (!string.IsNullOrEmpty(open))
        {
            Write(open);
        }
        foreach (var line in lines)
        {
            Write(useIndent + line);
            WriteLine();
        }
        if (!string.IsNullOrEmpty(close))
        {
            Write(close);
        }

    }

    protected static string CreateParagraphText(TextPart[] parts)
    {
        var lastNonEmptyPartEmittedSpaceOrAtStart = true;
        var sb = new StringBuilder();
        var last = parts.Last();
        for (int i = 0; i < parts.Length; i++)
        {
            var part = parts[i];
            // TODO: Determine whether a part where text is whitespace should be emitted
            if (string.IsNullOrEmpty(part.Text))
            {
                continue;
            }
            if (!lastNonEmptyPartEmittedSpaceOrAtStart && part.Whitespace.HasFlag(Whitespace.Before))
            {
                sb.Append(" ");
            }
            sb.Append(part);
            // TODO: Determine whether a part that emits only whitespace should add an extra space
            if (part != last && part.Whitespace.HasFlag(Whitespace.After))
            {
                sb.Append(" ");
                lastNonEmptyPartEmittedSpaceOrAtStart = true;
            }
        }
        return sb.ToString();
    }

    public virtual void Write(Table table, int indentCount = 0)
    {
        // TODO: Add IncludeHeaders to the Table class
        var includeHeaders = false;
        var useWidth = Width - indentCount * IndentSize;
        var indent = new string(' ', indentCount * IndentSize);
        var fixedWidthTable = new FixedWidthTable(table);
        var layout = fixedWidthTable.LayoutTable(useWidth, includeHeaders);
        if (layout is null)
        {
            return;
        }
        // This writer chooses to ignore the header
        foreach (var row in layout)
        {
            foreach (var line in row)
            {
                Write(indent);
                Write(line);
                WriteLine();
            }
        }
    }

    public void Write(TextPart textPart, int indentCount = 0)
    {
        (string? open, string? close) = GetStyle(textPart.Appearance);
        if (!string.IsNullOrEmpty(open))
        {
            Write(open);
        }
        Write(textPart.Text);
        if (!string.IsNullOrEmpty(close))
        {
            Write(close);
        }
    }

    protected (string? open, string? close) GetStyle(string? name)
        => OutputContext.OutputStyles?.GetStyle(name) ?? (null, null);
}
