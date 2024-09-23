using OutputEngine.Primitives;

namespace OutputEngine.Renderers;

public class MarkdownRenderer(OutputContext outputContext) 
    : CliRenderer(outputContext, new MarkdownStyles())
{
    public override void RenderLine()
    {
        Render($"{Environment.NewLine}{Environment.NewLine}");
    }

    public override void RenderSection(Section section, int indentCount = 0)
    {
        RenderSectionTitle(section);
        RenderGroup((Group)section, 1);
    }

    public override void RenderSectionTitle(Section section)
    {
        var paragraph = CreateParagraphText(section.Heading.ToArray());
        Render($"## {paragraph}");
        RenderLine();
    }

    public override void RenderParagraph(Paragraph paragraph, int indentCount = 0)
    {
        if (paragraph.Count() == 0)
        {
            return;
        }
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        output = MarkdownEncode(output);
        RenderLine(output);
    }

    public override void RenderTable(Table table, int indentCount = 0)
    {
        if (table.TableData.Count == 0)
        {
            return;
        }
        var headers = table.IncludeHeaders
            ? "|" + string.Join('|', table.Columns.Select(col => col.Header)) + "|"
            : new string('|', table.Columns.Count + 1);
        Render(headers);
        Render(Environment.NewLine);
        var fence = "|" + string.Join('|', table.Columns.Select(col => "---")) + "|";
        Render(fence);
        Render(Environment.NewLine);
        foreach (var row in table.TableData)
        {
            Render("|");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var parts = row[i].Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
                var output = CreateParagraphText(parts);
                output = MarkdownEncode(output);
                Render(output);
                Render("|");
            }
            Render(Environment.NewLine);
        }
    }

    private static string MarkdownEncode(string input)
    {
        return input.Replace("<", "\\<").Replace(">", "\\>");
        // const string escapeCharacterPattern = @"([<>])";
        // return System.Text.RegularExpressions.Regex.Replace(input, escapeCharacterPattern, "\\$1");
    }
}
