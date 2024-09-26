// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine;
using OutputEngine.Primitives;
using OutputEngine.Renderers;
using Spectre.Console;

namespace SpectreCliOutput;

public class RichTerminalOld(OutputContext outputContext) 
    : TerminalRenderer(outputContext, new SpectreStyles())
{
    public override void RenderSectionTitle(Section section)
    {
        var paragraph = CreateParagraphText(section.Heading.ToArray());
        AnsiConsole.Write(new Markup(Markup.Escape(paragraph), new Style(decoration: Decoration.Bold)));
        AnsiConsole.WriteLine();
    }

    public override void RenderParagraph(OutputEngine.Primitives.Paragraph paragraph, int indentCount = 0)
    {
        if (paragraph.Count() == 0)
        {
            return;
        }

        var useWidth = Width - (indentCount * IndentSize);
        var useIndent = new string(' ', indentCount * IndentSize);
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);

        // TO-DO: Test different styles on different terminal backgrounds
        // to ensure colors work for different environments
        var style = paragraph.Style switch
        {
            ParagraphStyle.Error => new Style(foreground: Color.Red, background: Color.Black, decoration: Decoration.Bold),
            ParagraphStyle.Warning => new Style(foreground: Color.Yellow, background: Color.Black, decoration: Decoration.Bold),
            _ => new Style(decoration: Decoration.None),
        };

        foreach (var line in lines)
        {
            AnsiConsole.Write(new Markup(Markup.Escape(line), style));
            AnsiConsole.WriteLine();
        }
    }

    public override void RenderTable(OutputEngine.Primitives.Table table, int indentCount = 0)
    {
        var outputTable = CreateTable(table);
        AnsiConsole.Write(outputTable);
    }

    private static Spectre.Console.Table CreateTable(OutputEngine.Primitives.Table table)
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
}
