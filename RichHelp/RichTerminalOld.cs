using OutputEngine.Primitives;
using Spectre.Console;

namespace OutputEngine.Targets;

public class RichTerminalOld : Terminal
{
    public RichTerminalOld(OutputContext outputContext)
        : base(outputContext)
    {  }

    public override void Write(Section section, int indentCount = 0)
    {
        AnsiConsole.Write(new Markup(Markup.Escape(section.Title.Text), new Style(decoration: Decoration.Bold)));
        AnsiConsole.WriteLine();
        Write((Group)section, 1);
    }

    public override void Write(Group group, int indentCount = 0)
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
                case Primitives.Table table: Write(table, indentCount); break;
                case Primitives.Paragraph paragraph: Write(paragraph, indentCount); break;
                default: throw new InvalidOperationException("Unknown element type");
            }
        }
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

        // TO-DO: Test different appearances on different terminal backgrounds
        // to ensure colors work for different environments
        var style = paragraph.Appearance switch
        {
            Appearance.Error => new Style(foreground: Spectre.Console.Color.Red, background: Spectre.Console.Color.Black, decoration: Decoration.Bold),
            Appearance.Warning => new Style(foreground: Spectre.Console.Color.Yellow, background: Spectre.Console.Color.Black, decoration: Decoration.Bold),
            _ => new Style(decoration: Decoration.None),
        };

        foreach (var line in lines)
        {
            AnsiConsole.Write(new Markup(Markup.Escape(line), style));
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
            result.AddColumn(new Spectre.Console.TableColumn(col.Header != null? col.Header.ToString(): string.Empty));
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
