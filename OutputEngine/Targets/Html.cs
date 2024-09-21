using OutputEngine.Primitives;
using System;
using System.Text;
using System.Web;

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

    public void WriteEscaped(string? text)
    {
        Write(HttpUtility.HtmlEncode(text));
    }

    public override void Write(Section section, int indentCount = 0)
    {
        Write($"<h2>{section.Title.Text}</h2>");
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
        var (tag, style, parentTag) = ParseParagraphAppearanceToTags(paragraph);

        // Add parent tag if paragraph is a list
        if (parentTag != null)
        {
            Write($"<{parentTag}>");
        }

        // Add items
        foreach (var line in lines)
        {
            Write($"<{tag}>{indentString}{HttpUtility.HtmlEncode(line)}</{tag}>");
        }

        if (parentTag != null)
        {
            Write($"</{parentTag}>");
        }
    }

    private static (string tag, string style, string? parentTag) ParseParagraphAppearanceToTags(Paragraph paragraph) =>
        paragraph.Appearance switch
        {

            Appearance.Warning => ("p", "style=\"color:orange;\"", null),
            Appearance.Error => ("p", "style=\"color:red;\"", null),
            /*
            ParagraphAppearance.Heading1 => ("h1", string.Empty, null),
            ParagraphAppearance.Heading2 => ("h2", string.Empty, null),
            ParagraphAppearance.Heading3 => ("h3", string.Empty, null),
            ParagraphAppearance.Heading4 => ("h4", string.Empty, null),
            ParagraphAppearance.Heading5 => ("h5", string.Empty, null),
            ParagraphAppearance.Heading6 => ("h6", string.Empty, null),
            ParagraphAppearance.BlockQuote => ("blockquote", string.Empty, null),
            ParagraphAppearance.NumberedList => ("li", string.Empty, "ol"),
            ParagraphAppearance.BulletedList => ("li", string.Empty, "ul"),
            */
            _ => ("p", string.Empty, null), // might want to use <div> instead of <p>
        };

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
            Write($"<caption>{HttpUtility.HtmlEncode(table.Title)}</caption>");
        }

        foreach (var row in table.TableData)
        {
            Write("<tr>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Write("<td>");
                WriteEscaped(row[i].ToString());
                Write("</td>");
            }
            Write("</tr>");
        }
        Write("</table>");
    }
}
