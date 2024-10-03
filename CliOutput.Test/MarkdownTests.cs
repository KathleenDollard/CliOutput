// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine.Renderers;
using OutputEngine.Primitives;
using OutputEngine;
using System.Collections;

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
        var container =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        renderer.RenderTextContainer(container);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Group()
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        BlockContainer textGroup =
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
        table.Rows.Add([new Paragraph("Alice"), new Paragraph("25")]);
        table.Rows.Add([new Paragraph("Bob"), new Paragraph("30")]);
        table.IncludeHeaders = true;
        renderer.RenderTable(table);
        var result = renderer.GetBuffer();
        result.Should()
            .Be($"|Name|Age|{Environment.NewLine}|---|---|{Environment.NewLine}|Alice|25|{Environment.NewLine}|Bob|30|{Environment.NewLine}");
    }


    public class ParagraphData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data;

        public ParagraphData()
        {
            _data = [
                [BlockStyle.SectionHeading, $"## Hello World:"],
                [BlockStyle.CodeBlock, $"```{Environment.NewLine}Hello World{Environment.NewLine}```"],
                [BlockStyle.Quote, $"> Hello World"],
                [BlockStyle.Heading1, $"# Hello World"],
                [BlockStyle.Heading2, $"## Hello World"],
                [BlockStyle.Heading3, $"### Hello World"],
                [BlockStyle.Error, $"**<span style = 'color: Red ;'>Hello World</span>**"],
                [BlockStyle.Warning, $"**<span style = 'color: Yellow ;'>Hello World</span>**"],];
        }


        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(ParagraphData))]
    public void Outputs_Paragraph_with_style(string style, string expected)
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        var heading = new Paragraph("Hello World");
        heading.AddStyle(style);

        renderer.RenderTextContainer(heading);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}{Environment.NewLine}{Environment.NewLine}");

    }

    [Theory]
    [InlineData(InlineStyle.Normal, "Hello world")]
    [InlineData(InlineStyle.Important, "**Hello world**")]
    [InlineData(InlineStyle.SlightlyImportant, "_Hello world_")]
    [InlineData(InlineStyle.CodeInline, "`Hello world`")]
    [InlineData(InlineStyle.Argument, "<Hello world>")]
    [InlineData(InlineStyle.Optional, "[Hello world]")]
    [InlineData(InlineStyle.LinkText, "Hello world", Skip = "WIP")]
    public void Outputs_TextPart_with_style(string? style, string expected)
    {
        var renderer = new MarkdownRenderer(new OutputContext(true));
        var textPart = new TextPart("Hello world", style);

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}");

    }
}