using OutputEngine.Primitives;
using System.Text;

namespace OutputEngine.Targets;

public class Html : CliOutput
{
    public Html(OutputContext outputContext)
        : base(outputContext)
    { }

    public override void WriteLine()
    {
        Write("<br/>");
    }

    public override void Write(Section section, int indentCount = 0)
    {
        Write($"<h3>{section.Title.Text}</h3>");
        Write((Group)section, 1);
    }

    private string GetIndentString(int indentCount)
    {
        if (indentCount == 0)
        {
            return string.Empty;
        }

        return new StringBuilder(indentCount * IndentSize * "&nbsp;".Length).
            Insert(0, "&nbsp;", indentCount * IndentSize).ToString();
    }

    public override void Write(Paragraph paragraph, int indentCount = 0)
    {
        var useWidth = Width - indentCount * IndentSize;
        if (paragraph.Count() == 0)
        {
            return;
        }

        var indentString = GetIndentString(indentCount);
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);
        var (tag, style) = paragraph.Appearance switch
        {
            ParagraphAppearance.Warning => ("p", "style=\"color:orange;\""),
            ParagraphAppearance.Error => ("p", "style=\"color:red;\""),
            ParagraphAppearance.Heading1 => ("h1", string.Empty),
            ParagraphAppearance.Heading2 => ("h2", string.Empty),
            ParagraphAppearance.Heading3 => ("h3", string.Empty),
            ParagraphAppearance.Heading4 => ("h4", string.Empty),
            ParagraphAppearance.Heading5 => ("h5", string.Empty),
            ParagraphAppearance.Heading6 => ("h6", string.Empty),
            ParagraphAppearance.BlockQuoteDoubled => ("blockquote", string.Empty),
            ParagraphAppearance.BlockQuoteTripled => ("blockquote", string.Empty),
            ParagraphAppearance.BlockQuote => ("blockquote", string.Empty),
            /*ParagraphAppearance.NumberedList => "h6",
            ParagraphAppearance.BulletedList => "h6",
            ParagraphAppearance.DefinitionList => "h6",*/
            _ => ("p", string.Empty),
        };

        foreach (var line in lines)
        {
            Write($"<{tag}>{indentString}{line}</{tag}>");
        }
    }

    public override void Write(Table table, int indentCount = 0)
    {
        if (table.TableData.Count == 0)
        {
            return;
        }

        var headers = table.IncludeHeaders
            ? $"<tr><th>{string.Join("</th><th>", table.Columns.Select(col => col.Header))}</th></tr>"
            : string.Empty;

        Write($"<table>{headers}");
        if (table.Title != null)
        {
            Write($"<caption>{table.Title}</caption>");
        }

        foreach (var row in table.TableData)
        {
            Write("<tr>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Write("<td>");
                Write(row[i].ToString());
                Write("</td>");
            }
            Write("</tr>");
        }
        Write("</table>");
    }
}
