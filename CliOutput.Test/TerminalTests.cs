// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine.Renderers;
using OutputEngine.Primitives;
using OutputEngine;
using System.Collections;

namespace CliOutput.Test;

public class TerminalTests
{
    [Fact]
    public void Outputs_string()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.Render("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderLine("Hello world");

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        renderer.RenderParagraph(paragraph);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Group()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        Group textGroup =
            [
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                },
                new Paragraph()
                {
                    new TextPart($"See you later"),
                }
            ];

        renderer.RenderGroup(textGroup);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}See you later{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Section()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        Section section = new("Greeting")
        {
            new Paragraph()
            {
                new TextPart("Hello"),
                new TextPart("world")
            },
            new Paragraph()
            {
                new TextPart($"See you later"),
            }
        };

        renderer.RenderSection(section);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Greeting:{Environment.NewLine}  Hello world{Environment.NewLine}  See you later{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Extra_newline_preserved_in_paragraph()
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        Group textGroup =
            [
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                },
                new Paragraph()
                {
                    new TextPart($"See you later{Environment.NewLine}"),
                }
            ];

        renderer.RenderGroup(textGroup);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}See you later{Environment.NewLine}{Environment.NewLine}");
    }

    public class ParagraphData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data;

        public ParagraphData()
        {
            _data = [
                [ParagraphStyle.SectionHeading, $"Hello World:"],
                [ParagraphStyle.CodeBlock, $"Hello World"],
                [ParagraphStyle.Quote, $"Hello World"],
                [ParagraphStyle.Heading1, $"Hello World"],
                [ParagraphStyle.Heading2, $"Hello World"],
                [ParagraphStyle.Heading3, $"Hello World"],
                [ParagraphStyle.Error, $"Hello World"],
                [ParagraphStyle.Warning, $"Hello World"],];
        }


        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(ParagraphData))]
    public void Outputs_Paragraph_with_style(string? style, string expected)
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        var heading = new Paragraph("Hello World")
        {
            Style = style
        };

        renderer.RenderParagraph(heading);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}{Environment.NewLine}");

    }

    [Theory]
    [InlineData(TextStyle.Normal, "Hello world")]
    [InlineData(TextStyle.Important, "Hello world")]
    [InlineData(TextStyle.SlightlyImportant, "Hello world")]
    [InlineData(TextStyle.CodeInline, "Hello world")]
    [InlineData(TextStyle.Argument, "<Hello world>")]
    [InlineData(TextStyle.Optional, "[Hello world]")]
    [InlineData(TextStyle.LinkText, "Hello world")]
    public void Outputs_TextPart_with_style(string? style, string expected)
    {
        var renderer = new TerminalRenderer(new OutputContext(true));
        var textPart = new TextPart("Hello world", style);

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"{expected}");

    }
}