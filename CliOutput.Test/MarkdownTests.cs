using FluentAssertions;
using OutputEngine.Targets;
using OutputEngine.Primitives;
using OutputEngine;

namespace CliOutput.Test;

public class MarkdownTests
{
    [Fact]
    public void Outputs_string()
    {
        var writer = new Markdown(true);

        writer.Write("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var writer = new Markdown(true);

        writer.WriteLine("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var writer = new Markdown(true);
        var textPart = new TextPart("Hello world");

        writer.Write(textPart);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var writer = new Markdown(true);
        var paragraph =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        writer.Write(paragraph);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Group()
    {
        var writer = new Markdown(true);
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

        writer.Write(textGroup);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}See you later");
    }


    [Fact]
    public void Outputs_Section()
    {
        var writer = new Markdown(true);
        Section section =
            new Section("Goodnight moon")
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

        writer.Write(section);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"{Environment.NewLine}##Goodnight moon{Environment.NewLine}{Environment.NewLine}Hello world{Environment.NewLine}{Environment.NewLine}See you later");
    }
}