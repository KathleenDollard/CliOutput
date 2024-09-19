using System.Web;
using OutputEngine.Primitives;

namespace OutputEngine.Targets;

public class Markdown : CliOutput
{
    public Markdown(OutputContext outputContext)
        : base(outputContext)
    { }

    public override void WriteLine()
    {
        Write(Environment.NewLine);
        Write(Environment.NewLine);
    }

    public override void Write(Section section, int indentCount = 0)
    {
        Write(Environment.NewLine);
        WriteLine($"## {section.Title.Text}");
        Write((Group)section);
    }

    public override void Write(Paragraph paragraph, int indentCount = 0)
    {
        if (paragraph.Count() == 0)
        {
            return;
        }
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        output = HttpUtility.HtmlEncode(output);
        WriteLine(output);
    }

    public override void Write(Table table, int indentCount = 0)
    {
        if (table.TableData.Count == 0)
        {
            return;
        }
        var headers = table.IncludeHeaders
            ? "|" + string.Join('|', table.Columns.Select(col => col.Header)) + "|"
            : new string('|', table.Columns.Count + 1);
        Write(headers);
        Write(Environment.NewLine);
        var fence = "|" + string.Join('|', table.Columns.Select(col => "---")) + "|";
        Write(fence);
        Write(Environment.NewLine);
        foreach (var row in table.TableData)
        {
            Write("|");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var parts = row[i].Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
                var output = CreateParagraphText(parts);
                output = HttpUtility.HtmlEncode(output);
                Write(output);
                Write("|");
            }
            Write(Environment.NewLine);
        }
    }
}
