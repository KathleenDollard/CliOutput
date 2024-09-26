// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine;
using OutputEngine.Primitives;
using OutputEngine.Renderers;

namespace CliOutput.Test;

public class HtmlTests
{
    [Fact]
    public void Outputs_string()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));

        renderer.Render("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));

        renderer.RenderLine("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world<br/>");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                new TextPart("Hello"),
                new TextPart("world")
                };

        renderer.RenderParagraph(paragraph);

        var result = renderer.GetBuffer();
        result.Should()
            .Be("<p>Hello world</p>");
    }

    [Fact]
    public void Outputs_Group()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));
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
            .Be($"<p>Hello world</p><p>See you later</p>");
    }


    [Fact]
    public void Outputs_Section()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));
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
            .Be($"<h2>Goodnight moon</h2><p>&nbsp;&nbsp;Hello world</p><p>&nbsp;&nbsp;See you later</p>");
    }

    [Fact]
    public void Outputs_Table()
    {
        var renderer = new HtmlRenderer(new OutputContext(true));
        var table = new Table(
            [
            new TableColumn("Name"),
            new TableColumn("Age")
            ]);
        table.TableData.Add([new Paragraph("Alice"), new Paragraph("25")]);
        table.TableData.Add([new Paragraph("Bob"), new Paragraph("30")]);
        table.IncludeHeaders = true;
        renderer.RenderTable(table);
        var result = renderer.GetBuffer();
        result.Should()
            .Be("<table><tr><th>Name</th><th>Age</th></tr><tr><td>Alice</td><td>25</td></tr><tr><td>Bob</td><td>30</td></tr></table>");
    }

    /* Rethinking styles
    [Theory]
    [InlineData(ParagraphStyle.Heading1, "<h1>Hello world</h1>")]
    [InlineData(ParagraphStyle.Heading2, "<h2>Hello world</h2>")]
    [InlineData(ParagraphStyle.Heading3, "<h3>Hello world</h3>")]
    [InlineData(ParagraphStyle.Heading4, "<h4>Hello world</h4>")]
    [InlineData(ParagraphStyle.Heading5, "<h5>Hello world</h5>")]
    [InlineData(ParagraphStyle.Heading6, "<h6>Hello world</h6>")]
    [InlineData(ParagraphStyle.BlockQuote, "<blockquote>Hello world</blockquote>")]
    //[InlineData(ParagraphStyle.BlockQuoteDoubled, "<blockquote><blockquote>Hello world</blockquote></blockquote>")]
    //[InlineData(ParagraphStyle.BlockQuoteTripled, "<blockquote><blockquote><blockquote>Hello world</blockquote></blockquote></blockquote>")]
    [InlineData(ParagraphStyle.NumberedList, "<ol><li>Hello world</li></ol>")]
    [InlineData(ParagraphStyle.BulletedList, "<ul><li>Hello world</li></ul>")]
    //[InlineData(ParagraphStyle.DefinitionList, "#Hello world")] This needs more complex structure
    //[InlineData(ParagraphStyle.TaskItemUnchecked, "[ ] Hello world")]
    //[InlineData(ParagraphStyle.TaskItemChecked, "[x] Hello world")]
    public void Outputs_Paragraph_with_style(string? style, string expected)
    {
        var renderer = new Html(new OutputContext(true));
        var heading = new Paragraph("Hello world")
        {
            Style = style
        };

        renderer.Write(heading);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}");

    }
    */
}
