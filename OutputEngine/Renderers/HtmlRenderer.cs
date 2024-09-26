// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// TODO: Styling has not yet been done for HTML and decisions are needed, such as how much to use CSS, and possibly there is a "write your own CSS" and a "here's our styling" version needed.

using OutputEngine.Primitives;
using System.Text;
using System.Web;

namespace OutputEngine.Renderers;

public class HtmlRenderer(OutputContext outputContext) 
    : CliRenderer(outputContext, new HtmlStyles() )
{
    public override void RenderLine()
    {
        Render("<br/>");
    }

    public void WriteEscaped(string? text)
    {
        Render(HttpUtility.HtmlEncode(text));
    }

    public override void RenderSectionTitle(Section section)
    {
        var paragraph = CreateParagraphText(section.Heading.ToArray());
        Render($"<h2>{paragraph}</h2>");
    }

    private string GetIndentString(int indentCount)
    {
        return indentCount == 0
            ? string.Empty
            : new StringBuilder(indentCount * IndentSize * "&nbsp;".Length)
                    .Insert(0, "&nbsp;", indentCount * IndentSize)
                    .ToString();
    }

    public override void RenderSection(Section section, int indentCount = 0)
    {
        RenderSectionTitle(section);
        RenderGroup((Group)section, 1);
    }

    public override void RenderParagraph(Paragraph paragraph, int indentCount = 0)
    {
        var useWidth = Width - (indentCount * IndentSize);
        if (paragraph.Count() == 0)
        {
            return;
        }

        var indentString = GetIndentString(indentCount);
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);
        var (tag, style, parentTag) = ParseParagraphStyleToTags(paragraph);

        // Add parent tag if paragraph is a list
        if (parentTag != null)
        {
            Render($"<{parentTag}>");
        }

        // Add items
        foreach (var line in lines)
        {
            Render($"<{tag}>{indentString}{HttpUtility.HtmlEncode(line)}</{tag}>");
        }

        if (parentTag != null)
        {
            Render($"</{parentTag}>");
        }
    }

    private static (string tag, string style, string? parentTag) ParseParagraphStyleToTags(Paragraph paragraph) =>
        paragraph.Style switch
        {

            ParagraphStyle.Warning => ("p", "style=\"color:orange;\"", null),
            ParagraphStyle.Error => ("p", "style=\"color:red;\"", null),
            /*
            ParagraphStyle.Heading1 => ("h1", string.Empty, null),
            ParagraphStyle.Heading2 => ("h2", string.Empty, null),
            ParagraphStyle.Heading3 => ("h3", string.Empty, null),
            ParagraphStyle.Heading4 => ("h4", string.Empty, null),
            ParagraphStyle.Heading5 => ("h5", string.Empty, null),
            ParagraphStyle.Heading6 => ("h6", string.Empty, null),
            ParagraphStyle.BlockQuote => ("blockquote", string.Empty, null),
            ParagraphStyle.NumberedList => ("li", string.Empty, "ol"),
            ParagraphStyle.BulletedList => ("li", string.Empty, "ul"),
            */
            _ => ("p", string.Empty, null), // might want to use <div> instead of <p>
        };

    public override void RenderTable(Table table, int indentCount = 0)
    {
        if (table.TableData.Count == 0)
        {
            return;
        }

        var headers = table.IncludeHeaders
            ? $"<tr><th>{string.Join("</th><th>", table.Columns.Select(col => col.Header))}</th></tr>"
            : string.Empty;

        Render($"<table>{headers}");
        if (table.Title != null)
        {
            Render($"<caption>{HttpUtility.HtmlEncode(table.Title)}</caption>");
        }

        foreach (var row in table.TableData)
        {
            Render("<tr>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Render("<td>");
                WriteEscaped(row[i].ToString());
                Render("</td>");
            }
            Render("</tr>");
        }
        Render("</table>");
    }
}
