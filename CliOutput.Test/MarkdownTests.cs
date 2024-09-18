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
        var writer = new Markdown(new OutputContext(true));

        writer.Write("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var writer = new Markdown(new OutputContext(true));

        writer.WriteLine("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var writer = new Markdown(new OutputContext(true));
        var textPart = new TextPart("Hello world");

        writer.Write(textPart);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var writer = new Markdown(new OutputContext(true));
        var paragraph =
                new Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };

        writer.Write(paragraph);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_Group()
    {
        var writer = new Markdown(new OutputContext(true));
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
        var writer = new Markdown(new OutputContext(true));
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

    [Fact]
    public void Outputs_Table()
    {
        var writer = new Markdown(new OutputContext(true));
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
            .Be("|Name|Age|{Environment.NewLine}|---|---|{Environment.NewLine}|Alice|25|{Environment.NewLine}|Bob|30|{Environment.NewLine}");
    }

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
        var writer = new Markdown(new OutputContext(true));
        var heading = new Paragraph("Hello world")
        {
            Appearance = appearance
        };

        writer.Write(heading);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"{expected}{Environment.NewLine}{Environment.NewLine}");

    }

}