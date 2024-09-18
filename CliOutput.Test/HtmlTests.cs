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
            .Be($"Hello world<br/>");
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
            .Be($"<h2>Goodnight moon</h2><br/><p>&nbsp;&nbsp;Hello world</p><p>&nbsp;&nbsp;See you later</p>");
    }

    [Fact]
    public void Outputs_Table()
    {
        var writer = new Html(new OutputContext(true));
        var table = new Table(
            [
            new TableColumn("Name"),
            new TableColumn("Age")
            ]);
        table.TableData.Add([new Paragraph("Alice"), new Paragraph("25")]);
        table.TableData.Add([new Paragraph("Bob"), new Paragraph("30")]);
        writer.Write(table);
        var result = writer.GetBuffer();
        result.Should()
            .Be("<table><tr><td>Name</td><td>Age</td></tr><tr><td>Alice</td><td>25</td></tr><tr><td>Bob</td><td>30</td><tr></table>");
    }

    [Theory]
    [InlineData(ParagraphAppearance.Heading1, "<h1>Hello world</h1>")]
    [InlineData(ParagraphAppearance.Heading2, "<h2>Hello world</h2>")]
    [InlineData(ParagraphAppearance.Heading3, "<h3>Hello world</h3>")]
    [InlineData(ParagraphAppearance.Heading4, "<h4>Hello world</h4>")]
    [InlineData(ParagraphAppearance.Heading5, "<h5>Hello world</h5>")]
    [InlineData(ParagraphAppearance.Heading6, "<h6>Hello world</h6>")]
    [InlineData(ParagraphAppearance.BlockQuote, "<blockquote>Hello world</blockquote>")]
    [InlineData(ParagraphAppearance.BlockQuoteDoubled, "<blockquote>Hello world</blockquote>")]
    [InlineData(ParagraphAppearance.BlockQuoteTripled, "<blockquote>Hello world</blockquote>")]
    //[InlineData(ParagraphAppearance.NumberedList, "<p>Hello world</p>")]
    //[InlineData(ParagraphAppearance.BulletedList, "<p>Hello world</p>")]
    //[InlineData(ParagraphAppearance.DefinitionList, "#Hello world")]
    //[InlineData(ParagraphAppearance.TaskItemUnchecked, "[ ] Hello world")]
    //[InlineData(ParagraphAppearance.TaskItemChecked, "[x] Hello world")]
    public void Outputs_Paragraph_with_style(string? appearance, string expected)
    {
        var writer = new Html(new OutputContext(true));
        var heading = new Paragraph("Hello world")
        {
            Appearance = appearance
        };

        writer.Write(heading);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"{expected}");

    }
}
