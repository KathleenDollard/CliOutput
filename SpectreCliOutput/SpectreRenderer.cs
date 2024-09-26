// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine;
using OutputEngine.Primitives;
using OutputEngine.Renderers;
using Spectre.Console;
using SpectreStyle = Spectre.Console.Style;

namespace SpectreCliOutput;

/* All calls to console in this class should go to the Console property.
 * This also means no calls that resolve to a base class Write should be used.
 * This is the Specter way of supporting a TestConsole and the terminal.
 */
public class SpectreRenderer(OutputContext outputContext)
    : TerminalRenderer(outputContext, new SpectreStyles(), new SpectreWriter(outputContext))
{
    public void Render(string text, SpectreStyle style)
    {
        var markup = new Markup(Markup.Escape(text), style);
        var writer = Advanced.WriterFromRenderer<SpectreWriter>(this);
        if (writer is null)
        {
            throw new InvalidOperationException("Invalid writer for Spectre console");
        }
        writer.Write(markup);
    }

    public override void RenderSectionTitle(Section section)
    {
        var paragraph = CreateParagraphText(section.Heading.ToArray());
        if (Redirecting) // Apparently, we can't create a rule
        {
            Render($"[olive bold]{paragraph}[/]");
            RenderLine();
        }
        else
        {
            var rule = new Rule($"[olive bold]{paragraph}[/]");
            rule.RuleStyle("olive bold");
            rule.LeftJustified();
            // Note that we are deliberately going directly to the console
            AnsiConsole.Write(rule);
        }
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

        var style = GetSpectreStyle(paragraph.Style);

        foreach (var line in lines)
        {
            if (style is null)
            {
                Render(Markup.Escape(line));
            }
            else
            {
                Render(Markup.Escape(line), style);
            }
            RenderLine();
        }
    }

    private SpectreStyle? GetSpectreStyle(string? style)
    {
        return string.IsNullOrWhiteSpace(style) || OutputStyles is not SpectreStyles styles
            ? null
            : styles.GetSpectreStyle(style);
    }

    public override void RenderTable(OutputEngine.Primitives.Table table, int indentCount = 0)
    {
        var outputTable = CreateTable(table);
        var writer = Advanced.WriterFromRenderer<SpectreWriter>(this);
        if (writer is null)
        {
            throw new InvalidOperationException("Invalid writer for Spectre console");
        }
        writer.Write(outputTable);
        //Console.Write(outputTable);
    }

    private static Spectre.Console.Table CreateTable(OutputEngine.Primitives.Table table)
    {
        var result = new Spectre.Console.Table();
        result.Border = TableBorder.None;
        if (!table.IncludeHeaders)
        {
            result.HideHeaders();
        }

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
