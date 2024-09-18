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
        Write("<br/>");
    }

    public override void Write(Section section, int indentCount = 0)
    {
        WriteLine($"<h2>{section.Title.Text}</h2>");
        Write((Group)section, 1);
    }

    private string GetIndentString(int indentCount)
    {
        if (indentCount == 0)
        {
            return string.Empty;
        }

        return new StringBuilder(indentCount * IndentSize * "&nbsp;".Length).Insert(0, "&nbsp;", indentCount * IndentSize).ToString();
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
        foreach (var line in lines)
        {
            Write($"<p>{indentString}{line}</p>");
        }
    }

    public override void Write(Table table, int indentCount = 0)
    {
        // TODO: Add IncludeHeaders to the Table class
        var includeHeaders = false;
        var headers = includeHeaders
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
                Write(row[i]);
                Write("</td>");
            }
            Write("</tr>");
        }
        Write("</table>");
    }
}
