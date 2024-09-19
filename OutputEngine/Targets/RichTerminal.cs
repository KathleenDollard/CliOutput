using OutputEngine.Primitives;
using Spectre.Console;

namespace OutputEngine.Targets;

public class RichTerminal : Terminal
{
    public RichTerminal(OutputContext outputContext)
        : base(outputContext)
    {  }

    public override void Write(Section section, int indentCount = 0)
    {
        AnsiConsole.WriteLine($"[bold]{section.Title}[/]");
        Write((Group)section, 1);
    }

    public override void Write(Primitives.Paragraph paragraph, int indentCount = 0)
    {
        if (paragraph.Count() == 0)
        {
            return;
        }

        var useWidth = Width - indentCount * IndentSize;
        var useIndent = new string(' ', indentCount * IndentSize);
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);

        foreach (var line in lines)
        {
            AnsiConsole.Write(line);
            AnsiConsole.WriteLine();
        }
    }

    public override void Write(Primitives.Table table, int indentCount = 0)
    {
        var outputTable = createTable(table);
        AnsiConsole.Write(outputTable);
    }

    private Spectre.Console.Table createTable(Primitives.Table table)
    {
        var result = new Spectre.Console.Table();

        if (table.Columns.Count == 0)
        {
            return result;
        }

        foreach (var col in table.Columns)
        {
            result.AddColumn(new Spectre.Console.TableColumn(col.Header.ToString()));
        }

        foreach (var row in table.TableData)
        {
            var rowData = new List<Spectre.Console.Paragraph>();

            foreach (var paragraph in row)
            {
                rowData.Add(new Spectre.Console.Paragraph(paragraph.ToString()));
            }
            
            result.AddRow(rowData);
        }

        return result;
    }
}
