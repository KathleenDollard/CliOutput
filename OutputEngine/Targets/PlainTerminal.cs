using OutputEngine.Primitives;
using System.Text;

namespace OutputEngine.Targets;

public class PlainTerminal : TextWriter
{
    public PlainTerminal(bool shouldRedirect = false)
    {
        redirecting = shouldRedirect;
    }

    public override Encoding Encoding { get; } = Encoding.UTF8;
    protected readonly int indentSize = 2;
    private readonly StringBuilder buffer = new();
    private bool redirecting = false;

    public string GetBuffer() => buffer.ToString();

    public void ClearBuffer() => buffer.Clear();

    protected int Width { get; set; } = 80;

    public void WriteLine<T>(T? output)
    {
        if (output is not null)
        {
            Write(output);
        }
        // This needs to call the parameterless form because
        // a line break is not be Environment.NewLine in HTML
        // or markdown
        WriteLine();
    }
    public override void WriteLine()
    {
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

    //private void WriteIndentedLine(string text)
    //{
    //    WriteLine(indent + text);
    //}

    //private void WriteIndentedLine(Paragraph textGroup)
    //{
    //    Write(indent);
    //    WriteLine(textGroup);
    //}

    public void Write(Layout layout, int indentCount = 0)
    {
        foreach (var section in layout.Sections)
        {
            Write(section, indentCount);
        }
    }

    public virtual void Write(Section section, int indentCount = 0)
    {
        Write(section.Title);
        WriteLine(":");
        Write((Group)section, 1);
    }

    public virtual void Write(Group group, int indentCount = 0)
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
                case Table table: Write(table, indentCount); break;
                case Paragraph paragraph: Write(paragraph, indentCount); break;
                default: throw new InvalidOperationException("Unknown element type");
            }
            if (!(element == last || element.NoNewLineAfter ))
            {
                // For some reason, WriteLine is not working correctly when I send to the buffer for testing
                WriteLine();
            }
        }
    }

    public virtual void Write(Paragraph paragraph, int indentCount = 0)
    {
        var useWidth = Width - indentCount * indentSize;
        var useIndent = new string(' ', indentCount * indentSize);
        if (paragraph.Count() == 0)
        {
            return;
        }
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);
        var lastLine = lines.Last();
        foreach (var line in lines)
        {
            Write(useIndent + line);
            if (line != lastLine)
            {
                WriteLine();
            }
        }

  
    }

    protected static string CreateParagraphText(TextPart[] parts)
    {
        var lastNonEmptyPartEmittedSpaceOrAtStart = true;
        var sb = new StringBuilder();
        var last = parts.Last();
        for (int i = 0; i < parts.Length; i++)
        {
            var part = parts[i];
            // TODO: Determine whether a part where text is whitespace should be emitted
            if (string.IsNullOrEmpty(part.Text))
            {
                continue;
            }
            if (!lastNonEmptyPartEmittedSpaceOrAtStart && part.Whitespace.HasFlag(Whitespace.Before))
            {
                sb.Append(" ");
            }
            sb.Append(part);
            // TODO: Determine whether a part that emits only whitespace should add an extra space
            if (part != last && part.Whitespace.HasFlag(Whitespace.After))
            {
                sb.Append(" ");
                lastNonEmptyPartEmittedSpaceOrAtStart = true;
            }
        }
        return sb.ToString();
    }

    public virtual void Write(Table table, int indentCount = 0)
    {
        // TODO: Add IncludeHeaders to the Table class
        var includeHeaders = false;
        var useWidth = Width - indentCount * indentSize;
        var indent = new string(' ', indentCount * indentSize);
        var fixedWidthTable = new FixedWidthTable(table);
        var layout = fixedWidthTable.LayoutTable(useWidth, includeHeaders);
        // This writer chooses to ignore the header
        foreach (var row in layout)
        {
            foreach (var line in row)
            {
                Write(indent);
                Write(line);
                WriteLine();
            }
        }
    }

    public void Write(TextPart textPart, int indentCount = 0)
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
    //    Console.WriteLine());
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
