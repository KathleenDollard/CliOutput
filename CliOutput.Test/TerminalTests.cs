using FluentAssertions;
using OutputEngine.Renderers;
using OutputEngine.Primitives;
using OutputEngine;

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
}