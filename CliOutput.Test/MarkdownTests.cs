// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine.Renderers;
using OutputEngine.Primitives;
using OutputEngine;

namespace CliOutput.Test;

public class MarkdownTests
{
    [Fact]
    public void Outputs_string()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.Render("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderLine("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        renderer.RenderParagraph(paragraph);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Group()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        Group textGroup =
            [
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                },
                new Paragraph()
                {
                    new TextPart("See you later"),
                }
            ];

        renderer.RenderGroup(textGroup);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}See you later{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Section()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        //renderer.OutputContext.OutputStyles = new MarkdownStyles();
        Section section =
            new("Goodnight moon")
            {
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                },
                new Paragraph()
                {
                    new TextPart("See you later"),
                }
            };

        renderer.RenderSection(section);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Goodnight moon{Environment.NewLine}{Environment.NewLine}Hello world{Environment.NewLine}{Environment.NewLine}See you later{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Table()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        var table = new Table(
            [
                new TableColumn("Name"),
                new TableColumn("Age")
            ])
        {
            IncludeHeaders = true
        };
        table.TableData.Add([new Paragraph("Alice"), new Paragraph("25")]);
        table.TableData.Add([new Paragraph("Bob"), new Paragraph("30")]);
        table.IncludeHeaders = true;
        renderer.RenderTable(table);
        var result = renderer.GetBuffer();
        result.Should()
            .Be($"|Name|Age|{Environment.NewLine}|---|---|{Environment.NewLine}|Alice|25|{Environment.NewLine}|Bob|30|{Environment.NewLine}");
    }

    /* Rethinking styles
    [Theory]
    [InlineData(ParagraphAppearance.Heading1, "# Hello world")]
    [InlineData(ParagraphAppearance.Heading2, "## Hello world")]
    [InlineData(ParagraphAppearance.Heading3, "### Hello world")]
    [InlineData(ParagraphAppearance.Heading4, "#### Hello world")]
    [InlineData(ParagraphAppearance.Heading5, "##### Hello world")]
    [InlineData(ParagraphAppearance.Heading6, "###### Hello world")]
    [InlineData(ParagraphAppearance.BlockQuote, "> Hello world")]
    [InlineData(ParagraphAppearance.BlockQuoteDoubled, "> > Hello world")]
    [InlineData(ParagraphAppearance.BlockQuoteTripled, "> > > Hello world")]
    [InlineData(ParagraphAppearance.NumberedList, "1 Hello world")]
    [InlineData(ParagraphAppearance.BulletedList, "- Hello world")]
    //[InlineData(ParagraphAppearance.DefinitionList, "#Hello world")]
    [InlineData(ParagraphAppearance.TaskItemUnchecked, "[ ] Hello world")]
    [InlineData(ParagraphAppearance.TaskItemChecked, "[x] Hello world")]
    public void Outputs_Paragraph_with_style(string? appearance, string expected)
    {
        var renderer = new Markdown(new OutputContext(true));
        var heading = new Paragraph("Hello world")
        {
            Appearance = appearance
        };

        renderer.Write(heading);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}{Environment.NewLine}{Environment.NewLine}");

    }
    */

}