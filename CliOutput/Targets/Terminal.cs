using CliOutput.Primitives;
using System.Text;
using CliOutput.Help;
using static System.Net.Mime.MediaTypeNames;

namespace CliOutput.Targets;

public class PlainHelpTerminal : TextWriter
{
    public PlainHelpTerminal(bool shouldRedirect = false)
    {
        redirecting = shouldRedirect;
    }

    public override Encoding Encoding { get; } = Encoding.UTF8;
    private readonly string indent = "  ";
    private readonly StringBuilder buffer = new();
    private bool redirecting = false;

    public string GetBuffer() => buffer.ToString();

    public void ClearBuffer() => buffer.Clear();

    public void WriteLine(Help.Help help)
    {
        foreach (var section in help.Sections)
        {
            switch (section)
            {
                case HelpDescription description:
                    WriteLine(description); break;
                case HelpUsage usage:
                    WriteLine(usage); break;
                case HelpExamples examples:
                    WriteLine(examples); break;
                case HelpArguments arguments:
                    WriteLine(arguments); break;
                case HelpOptions options:
                    WriteLine(options); break;
                case HelpSubcommands subCommands:
                    WriteLine(subCommands); break;
            };
        }
    }

    public void WriteSectionHead(HelpSection section)
        => WriteLine(section.Title + ":");

    public void WriteLine(HelpDescription description)
    {
        WriteSectionHead(description);
        WriteIndentedLine(description.Command.Description);
    }

    public void WriteLine(HelpUsage usage)
    {
        WriteSectionHead(usage);
        if (usage.Command.SubCommands.Any())
        {
            // This is wrong, you'll just get the command twice
            foreach (var command in usage.Command.SubCommands)
            {
                WriteIndentedLine(usage.GetHelpUse());
            }
        }
        else
        {
            WriteIndentedLine(usage.GetHelpUse());
        }
    }

    public void WriteLine(HelpExamples examples)
    {
        WriteSectionHead(examples);
    }

    public void WriteLine(HelpArguments arguments, int width)
    {
        WriteSectionHead(arguments);
        WriteTable(arguments.GetHelp(), width, 1);
    }

    public void WriteLine(HelpOptions options)
        => WriteSectionHead(options);

    public void WriteLine(HelpSubcommands subCommands)
    {
        WriteSectionHead(subCommands);
    }

    public override void WriteLine(string? output)
    {
        if (output is not null)
        {
            Write(output);
        }
        Write(Environment.NewLine);
    }

    public override void Write(string? text)
    {
        if (redirecting)
        {
            buffer.Append(text);
        }
        else
        {
            Console.WriteLine(text);
        }
    }

    private void WriteIndentedLine(string text)
    {
        WriteLine(indent + text);
    }

    private void WriteIndentedLine(TextGroup textGroup)
    {
        Write(indent);
        WriteLine(textGroup);
    }

    public void WriteLine(TextGroup textGroup)
    {
        Write(textGroup);
        Write(Environment.NewLine);
    }

    public void WriteTable(Table table, int width, int indent)
    {
        width = width - indent;
        var fixedWidthTable = new FixedWidthTable(table);
        fixedWidthTable.LayoutTable(width);
        // This writer chooses to ignore the header
        foreach (var row in table.TableData)
        {
            for (var i = 0; i < table.Columns.Count; i++)
            {
                Write(indent);
                Write(row[i]);
                WriteLine();
            }
        }
    }

    public void Write(TextGroup textGroup)
    {
        if (textGroup.Count == 0)
        {
            return;
        }
        var lastNonEmptyPartEmittedSpaceOrAtStart = true;
        var last = textGroup.Last(); ;
        for (int i = 0; i < textGroup.Count; i++)
        {
            var part = textGroup[i];
            // TODO: Determine whether a part where text is whitespace should be emitted
            if (string.IsNullOrEmpty(part.Text))
            {
                continue;
            }
            if (!lastNonEmptyPartEmittedSpaceOrAtStart && part.Whitespace.HasFlag(Whitespace.Before))
            {
                Write(" ");
            }
            Write(part);
            // TODO: Determine whether a part that emits only whitespace should add an extra space
            if (part != last && part.Whitespace.HasFlag(Whitespace.After) && MoreNonEmptyParts(textGroup[(i + 1)..]))
            {
                Write(" ");
                lastNonEmptyPartEmittedSpaceOrAtStart = true;
            }
        }

        static bool MoreNonEmptyParts(IEnumerable<TextPart> parts)
            => parts.Any(part => !string.IsNullOrWhiteSpace(part.Text));
    }

    public void Write(TextPart textPart)
        => Write(textPart.Text);






    //private void WriteLineHelpSectionHead(string? text) => Console.WriteLine(text + ":");




    //private void WriteLineHelpPart<T>(HelpPart<T> helpPart)
    //    where T : HelpItem
    //{
    //    if (!helpPart.Items.Any())
    //    {
    //        return 0;
    //    }
    //    WriteLineHelpSectionHead(helpPart.PartName);

    //    var anyAliases = helpPart.SelectMany(x => x.Aliases).Any(x => !string.IsNullOrWhiteSpace(x));
    //    Spectre.Console.Table helpPartTable = buildHelpPartTable(helpPart, anyAliases);
    //    SetHelpPartTableCharacteristics(helpPartTable);
    //    var panel = new Spectre.Console.Panel(helpPartTable);
    //    // Patrik: Apparent bug in Spectre console. This causes the columns wrapping to be off
    //    //panel.NoBorder().Padding(0, 0, 0, 0);
    //    //AnsiConsole.WriteLine(panel);
    //    AnsiConsole.WriteLine(helpPartTable);
    //    Console.WriteLine(Environment.NewLine));
    //    return 0;

    //    Spectre.Console.Table buildHelpPartTable<TItem>(HelpPart<TItem> helpPart, bool anyAliases)
    //       where TItem : HelpItem
    //    {
    //        var helpPartTable = new Spectre.Console.Table().HideHeaders();
    //        var nameColumn = new Spectre.Console.TableColumn("");
    //        //.PadLeft(5).PadRight(2);
    //        helpPartTable.AddColumn(nameColumn);
    //        if (anyAliases)
    //        {
    //            helpPartTable.AddColumn("");
    //        }
    //        helpPartTable.AddColumn("");
    //        foreach (var item in helpPart.Items)
    //        {
    //            helpPartTable.AddRow(helpRowForItem(item, anyAliases).ToArray());
    //        }

    //        return helpPartTable;
    //    }

    //    IEnumerable<string> helpRowForItem(HelpItem option, bool anyAliases)
    //       => anyAliases
    //           ? new List<string>
    //               {
    //                    "  " + option.DisplayName,
    //                    string.Join(", ", option.Aliases),
    //                    option.Description ?? "",
    //               }
    //           : new List<string>
    //               {
    //                    "  " + option.DisplayName,
    //                    option.Description ?? "",
    //               };

    //}


    //private void BuildTable(Spectre.Console.Table WriteLineTable, Table table)
    //{
    //    foreach (var column in table.Columns)
    //    {
    //        if (!column.Hide)
    //        {
    //            var WriteLineColumn = new Spectre.Console.TableColumn(column.Header)
    //            {
    //                NoWrap = true
    //            };
    //            WriteLineTable.AddColumn(WriteLineColumn);
    //        }
    //    }

    //    foreach (var row in table.Rows)
    //    {
    //        var values = new List<Markup>();
    //        for (int i = 0; i < table.Columns.Count; i++)
    //        {
    //            if (!table.Columns[i].Hide)
    //            {
    //                values.Add(new Markup(row[i], null).Overflow(Overflow.Ellipsis));
    //            }
    //        }

    //        WriteLineTable.AddRow(new Spectre.Console.TableRow(values));
    //    }
    //}

    //private void SetTableCharacteristics(Spectre.Console.Table WriteLineTable, int uxLevel)
    //{
    //    switch (uxLevel)
    //    {
    //        case 0: return;
    //        case 1:
    //            WriteLineTable.Border(TableBorder.None);
    //            WriteLineTable.Expand();
    //            return;
    //        case 2:
    //            WriteLineTable.Border(TableBorder.Minimal);
    //            WriteLineTable.BorderColor(Color.Olive);
    //            WriteLineTable.Expand();
    //            return;
    //        case 3:
    //            WriteLineTable.Border(TableBorder.Horizontal);
    //            WriteLineTable.BorderColor(Color.Green);
    //            WriteLineTable.Expand();
    //            return;
    //        case 4:
    //            WriteLineTable.Border(TableBorder.Rounded);
    //            WriteLineTable.BorderColor(Color.Grey);
    //            WriteLineTable.Collapse();
    //            return;
    //        case 5:
    //            WriteLineTable.Border(TableBorder.Horizontal);
    //            WriteLineTable.BorderColor(Color.Navy);
    //            WriteLineTable.Collapse();
    //            return;

    //        default:
    //            break;
    //    }
    //}

    //private void SetHelpPartTableCharacteristics(Spectre.Console.Table helpPartTable)
    //{
    //    switch (uxLevel)
    //    {
    //        case 0: return;
    //        case 1:
    //            helpPartTable.Border(TableBorder.None);
    //            helpPartTable.Expand();
    //            return;
    //        case 2:
    //            helpPartTable.Border(TableBorder.None);
    //            helpPartTable.Expand();
    //            return;
    //        case 3:
    //            helpPartTable.Border(TableBorder.Horizontal);
    //            helpPartTable.BorderColor(Color.Olive);
    //            helpPartTable.Expand();
    //            return;
    //        case 4:
    //            helpPartTable.Border(TableBorder.None);
    //            helpPartTable.Collapse();
    //            return;
    //        case 5:
    //            helpPartTable.Border(TableBorder.Horizontal);
    //            helpPartTable.BorderColor(Color.Olive);
    //            helpPartTable.Collapse();
    //            return;

    //        default:
    //            break;
    //    }
    //}

    //private void SetHelpUsageTableCharacteristics(Spectre.Console.Table helpUsageTable)
    //{
    //    switch (uxLevel)
    //    {
    //        case 0: return;
    //        case 1:
    //            helpUsageTable.Border(TableBorder.None);
    //            helpUsageTable.Expand();
    //            return;
    //        case 2:
    //            helpUsageTable.Border(TableBorder.None);
    //            helpUsageTable.Expand();
    //            return;
    //        case 3:
    //            helpUsageTable.Border(TableBorder.None);
    //            helpUsageTable.Expand();
    //            return;
    //        case 4:
    //            helpUsageTable.Border(TableBorder.None);
    //            helpUsageTable.BorderColor(Color.Blue);
    //            helpUsageTable.Collapse();
    //            return;
    //        case 5:
    //            helpUsageTable.Border(TableBorder.None);
    //            helpUsageTable.BorderColor(Color.Yellow);
    //            helpUsageTable.Collapse();
    //            return;

    //        default:
    //            break;
    //    }
    //}
}
