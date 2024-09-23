using OutputEngine.Primitives;
using System.Text;

namespace OutputEngine.Renderers;

public abstract class CliRenderer
{
    protected static class Advanced
    {
        public static TWriter? WriterFromRenderer<TWriter>(CliRenderer renderer)
            where TWriter : CliWriter
            => renderer.Writer as TWriter;
    }

    /// <summary>
    /// Base constructor for writers
    /// </summary>
    /// <param name="outputContext"></param>
    /// <remarks>
    /// Implementing classes should set an appropriate default 
    /// OutputStyles if it is null.
    /// </remarks>
    protected CliRenderer(OutputContext outputContext, OutputStyles defaultOutputStyles, CliWriter? defaultWriter = null)
    {
        Writer = outputContext.Writer is not null
            ? outputContext.Writer
            : defaultWriter is not null
                ? defaultWriter
                : new CliWriter(outputContext);
        Width = outputContext.Width;
        IndentSize = outputContext.IndentSize;
        OutputStyles = outputContext.OutputStyles ?? defaultOutputStyles;
        BlockRenderers = new()
        {
            [typeof(Paragraph)] = RenderParagraphBlock,
            [typeof(Table)] = RenderTableBlock,
        };
        InlineRenderers = new()
        {
            [typeof(TextPart)] = RenderTextPartBlock,
        };
    }

    private void RenderParagraphBlock(BlockElement block, int indentCount)
    {
        if (block is Paragraph paragraph)
        {
            RenderParagraph(paragraph, indentCount);
        }
        else
        {
            throw new InvalidOperationException("Unknown block type");
        }
    }

    private void RenderTableBlock(BlockElement block, int indentCount)
    {
        if (block is Table table)
        {
            RenderTable(table, indentCount);
        }
        else
        {
            throw new InvalidOperationException("Unknown block type");
        }
    }

    private void RenderTextPartBlock(InlineElement inline)
    {
        if (inline is TextPart textPart)
        {
            RenderTextPart(textPart);
        }
        else
        {
            throw new InvalidOperationException("Unknown inline type");
        }
    }


    private CliWriter Writer { get; }
    public string? GetBuffer()
        => Writer.Redirecting
                ? Writer.GetBuffer()
                : null;
    protected bool Redirecting => Writer.Redirecting;
    protected int Width { get; }
    protected int IndentSize { get; }
    protected OutputStyles? OutputStyles { get; }
    protected Dictionary<Type, Action<BlockElement, int>> BlockRenderers { get; }
    protected Dictionary<Type, Action<InlineElement>> InlineRenderers { get; }

    public abstract void RenderParagraph(Paragraph paragraph, int indentCount = 0);
    public abstract void RenderTable(Table table, int indentCount);

    /// <summary>
    /// Writes a line to the output
    /// </summary>
    /// <remarks>
    /// This is a rendering concern because different renderers need different new line syntax.
    /// Examples: double new line for markdown and \<\/br\> for HTML.
    /// </remarks>
    public virtual void RenderLine()
    {
        Writer.WriteLine();
    }

    public virtual void Render(string? text)
    {
        if (text is not null)
        {
            Writer.Write(text);
        }
    }

    public virtual void RenderLine(string text)
    {
        Render(text);
        RenderLine();
    }


    public void RenderLayout(Layout layout, int indentCount = 0)
    {
        foreach (var section in layout.Sections)
        {
            RenderSection(section, indentCount);
        }
    }

    public virtual void RenderSection(Section section, int indentCount = 0)
    {
        RenderSectionTitle(section);
        RenderGroup(section, 1);
        RenderLine();
    }

    public virtual void RenderSectionTitle(Section section)
    {
        RenderParagraph(section.Heading);
        RenderLine();
    }

    public virtual void RenderGroup(Group group, int indentCount = 0)
    {
        if (!group.Any())
        {
            return;
        }
        foreach (var element in group)
        {
            if (BlockRenderers.TryGetValue(element.GetType(), out var renderer))
            {
                renderer(element, indentCount);
            }
            else
            {
                throw new InvalidOperationException("Unknown element type");
            }
        }
    }

    public virtual void RenderTextPart(TextPart textPart)
    {
        (string? open, string? close) = OutputStyles?.GetStyle(textPart.Appearance) ?? (null, null);
        Render($"{open ?? ""}{textPart.Text}{close ?? ""}");

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
                sb.Append(' ');
            }
            sb.Append(part);
            // TODO: Determine whether a part that emits only whitespace should add an extra space
            if (part != last && part.Whitespace.HasFlag(Whitespace.After))
            {
                sb.Append(' ');
                lastNonEmptyPartEmittedSpaceOrAtStart = true;
            }
        }
        return sb.ToString();
    }
}

