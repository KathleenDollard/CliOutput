using FluentAssertions;
using OutputEngine;
using OutputEngine.Primitives;
using OutputEngine.Targets;

namespace CliOutput.Test;

public class HtmlTests
{
    [Fact]
    public void Outputs_string()
    {
        var writer = new Html(new OutputContext(true));

        writer.Write("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var writer = new Html(new OutputContext(true));

        writer.WriteLine("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world<br/><br/>");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var writer = new Html(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        writer.Write(textPart);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var writer = new Html(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                new TextPart("Hello"),
                new TextPart("world")
                };

        writer.Write(paragraph);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<p>Hello world</p>");
    }

    [Fact]
    public void Outputs_Group()
    {
        var writer = new Html(new OutputContext(true));
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
            .Be($"<p>Hello world</p><p>See you later</p>");
    }


    [Fact]
    public void Outputs_Section()
    {
        var writer = new Html(new OutputContext(true));
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
            .Be($"<h2>Goodnight moon</h2><br/><br/><p>&nbsp;&nbsp;Hello world</p><p>&nbsp;&nbsp;See you later</p>");
    }
}
