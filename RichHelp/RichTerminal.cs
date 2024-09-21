using OutputEngine.Primitives;
using Spectre.Console;
using System;

namespace OutputEngine.Targets;

/* All calls to console in this class should go to the Console property.
 * This also means no calls that resolve to a base class Write should be used.
 * This is the Specter way of supporting a TestConsole and the terminal.
 */
public class RichTerminal : Terminal
{
    public RichTerminal(OutputContext outputContext, IAnsiConsole? console = null, bool isTest = false)
        : base(outputContext)
    {
        // An empty AnsiConsoleSettings indicates to detect
        Console = console ?? AnsiConsole.Create(new AnsiConsoleSettings());
        IsTest = isTest;
        OutputStyles = outputContext.OutputStyles is RichTerminalStyles styles
                        ? styles
                        : new RichTerminalStyles();
    }

    public IAnsiConsole Console { get; }
    private bool IsTest { get; }

    public override void Write(string? text)
    {
        if (text is not null)
        {
            Console.Markup(text);
        }
    }


    public override void Write(Section section, int indentCount = 0)
    {
        if (IsTest) // Apparently, we can't create a rule
        {
            Write($"[olive bold]{section.Title.Text}[/]");
            WriteLine();
        }
        else
        {
            var rule = new Rule($"[olive bold]{section.Title.Text}[/]");
            rule.RuleStyle("olive bold");
            rule.LeftJustified();
            // Note that we are deliberately going directly to the console
            AnsiConsole.Write(rule);
        }
        Write((Group)section, 1);
        WriteLine();
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

        var style = GetSpectreStyle(paragraph.Appearance);

        foreach (var line in lines)
        {
            Console.Write(Markup.Escape(line), style);
            Console.WriteLine();
        }
    }

    private Style? GetSpectreStyle(string? appearance)
    {
        if (string.IsNullOrWhiteSpace(appearance) || OutputStyles is not RichTerminalStyles styles)
        {
            return null;
        }
        return styles.GetSpectreStyle(appearance);
    }

    public override void Write(Primitives.Table table, int indentCount = 0)
    {
        var outputTable = createTable(table);
        Console.Write(outputTable);
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
            result.AddColumn(new Spectre.Console.TableColumn(col.Header != null ? col.Header.ToString() : string.Empty));
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

    public override void Write(TextPart textPart, int indentCount = 0)
    {
        (string? open, string? close) = OutputStyles?.GetStyle(textPart.Appearance) ?? (null, null);
        if (!string.IsNullOrEmpty(open))
        {
            Write($"{open}{textPart.Text}{close}");
        }
        else
        {
            Write(textPart.Text);
        }

    }

}
