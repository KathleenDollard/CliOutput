// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace OutputEngine.Renderers;

public class TerminalRenderer(OutputContext outputContext, OutputStyles? defaultOutputStyles = null, CliWriter? defaultWriter = null) 
    : CliRenderer(outputContext, defaultOutputStyles ?? new TerminalStyles(), defaultWriter)
{
    public override void RenderParagraph(Paragraph paragraph, int indentCount = 0)
    {
        var useWidth = Width - (indentCount * IndentSize);
        var useIndent = new string(' ', indentCount * IndentSize);
        if (paragraph.Count() == 0)
        {
            return;
        }
        var parts = paragraph.Where(part => !string.IsNullOrEmpty(part.Text)).ToArray();
        var output = CreateParagraphText(parts);
        var lines = output.Wrap(useWidth);
        var lastLine = lines.Last();

        (string? open, string? close) = OutputStyles?.GetStyle(paragraph.Style) ?? (null, null);
        if (!string.IsNullOrEmpty(open))
        {
            Render(open);
        }
        var last = lines.Last();
        foreach (var line in lines)
        {
            Render(useIndent + line);
            if (line == last)
            {
            Render(close);
            }
            if (!(line == lastLine && paragraph.NoNewLineAfter))
            {
                RenderLine();
            }
        }
    }

    public override void RenderTable(Table table, int indentCount = 0)
    {
        // TODO: Add IncludeHeaders to the Table class
        var includeHeaders = false;
        var useWidth = Width - (indentCount * IndentSize);
        var indent = new string(' ', indentCount * IndentSize);
        var fixedWidthTable = new FixedWidthTable(table);
        var layout = fixedWidthTable.LayoutTable(useWidth, includeHeaders);
        if (layout is null)
        {
            return;
        }
        // This writer chooses to ignore the header
        foreach (var row in layout)
        {
            foreach (var line in row)
            {
                Render(indent);
                Render(line);
                RenderLine();
            }
        }
    }


}
