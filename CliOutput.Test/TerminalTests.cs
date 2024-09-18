using FluentAssertions;
using OutputEngine.Targets;
using OutputEngine.Primitives;
using OutputEngine;

namespace CliOutput.Test;

public class TerminalTests
{
    [Fact]
    public void Outputs_string()
    {
        var writer = new Terminal(new OutputContext(true));

        writer.Write("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var writer = new Terminal(new OutputContext(true));

        writer.WriteLine("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var writer = new Terminal(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        writer.Write(textPart);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var writer = new Terminal(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        writer.Write(paragraph);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Group()
    {
        var writer = new Terminal(new OutputContext(true));
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

        writer.Write(textGroup);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}See you later{Environment.NewLine}");
    }


    [Fact]
    public void Extra_newline_preserved_in_paragraph()
    {
        var writer = new Terminal(new OutputContext(true));
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

        writer.Write(textGroup);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}See you later{Environment.NewLine}{Environment.NewLine}");
    }
}