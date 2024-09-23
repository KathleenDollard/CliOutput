using FluentAssertions;
using OutputEngine.Primitives;
using OutputEngine;
using Spectre.Console.Testing;
using Spectre.Console;
using SpectreCliOutput;
using OutputEngine.Renderers;

namespace RichOutputTests;

public class SpectreTests
{

    private static SpectreRenderer GetRenderer()
        => new SpectreRenderer(new OutputContext(true));

    [Theory]
    [InlineData("Hello World", "Hello World")]
    [InlineData("[bold]Hello world[/]", "\u001b[1mHello world\u001b[0m")]
    public void Outputs_string(string text, string expected)
    {
        var renderer = GetRenderer();

        renderer.Render(text);

        var result = renderer.GetBuffer();
        result.Should()
            .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var renderer = GetRenderer();
        var textPart = new TextPart("Hello world");
        string expected = "Hello world";

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_important()
    {
        var renderer = GetRenderer();
        var textPart = new TextPart("Hello world", Appearance.Important);
        string expected = "\u001b[1mHello world\u001b[0m";

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_inline_code()
    {
        var renderer = GetRenderer();
        var textPart = new TextPart("Hello world", Appearance.InlineCode);
        string expected = "\u001b[1mHello world\u001b[0m";

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }
    [Fact]
    public void Outputs_TextPart_as_error()
    {
        var renderer = GetRenderer();
        var textPart = new TextPart("Hello world", Appearance.Error);
        string expected = "\u001b[1;91mHello world\u001b[0m";

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_warning()
    {
        var renderer = GetRenderer();
        var textPart = new TextPart("Hello world", Appearance.Warning);
        string expected = "\u001b[1;33mHello world\u001b[0m";

        renderer.RenderTextPart(textPart);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var renderer = GetRenderer();
        var paragraph =
                new OutputEngine.Primitives.Paragraph()
                {
                        new TextPart("Hello"),
                        new TextPart("world")
                };
        string expected = $"Hello world{Environment.NewLine}";

        renderer.RenderParagraph(paragraph);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Paragraph_as_Important()
    {
        var renderer = GetRenderer();
        var paragraph =
                new OutputEngine.Primitives.Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };
        paragraph.Appearance = Appearance.Important;
        string expected = $"\u001b[1mHello world\u001b[0m{Environment.NewLine}";

        renderer.RenderParagraph(paragraph);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Group()
    {
        var renderer = GetRenderer();
        Group textGroup =
            [
                new OutputEngine.Primitives.Paragraph()
                    {
                        new TextPart("Hello"),
                        new TextPart("world")
                    },
                    new OutputEngine.Primitives.Paragraph()
                    {
                        new TextPart($"See you later"),
                    }
            ];
        string expected = $"Hello world{Environment.NewLine}See you later{Environment.NewLine}";

        renderer.RenderGroup(textGroup);

        var result = renderer.GetBuffer();
        result.Should()
            .Be(expected);
    }

    [Fact]
    public void Outputs_Section_header()
    {
        var renderer = GetRenderer();
        Section section = new Section("Greeting");
        string expected = $"\u001b[1;33mGreeting\u001b[0m{Environment.NewLine}{Environment.NewLine}";

        renderer.RenderSection(section);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Section()
    {
        var renderer = GetRenderer();
        Section section = new Section("Greeting")
            {
                new OutputEngine.Primitives.Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                },
                new OutputEngine.Primitives.Paragraph()
                {
                    new TextPart($"See you later"),
                }
            };
        string expected = "\u001b[1;33mGreeting\u001b[0m\r\nHello world\r\nSee you later\r\n\r\n";

        renderer.RenderSection(section);

        var result = renderer.GetBuffer();
        result.Should()
                .Be(expected);
    }


    [Fact]
    public void Extra_newline_preserved_in_paragraph()
    {
        var renderer = GetRenderer();
        Group textGroup =
            [
                new OutputEngine.Primitives.Paragraph()
                    {
                        new TextPart("Hello"),
                        new TextPart($"world{Environment.NewLine}")
                    },
                    new OutputEngine.Primitives.Paragraph()
                    {
                        new TextPart("See you later"),
                    }
            ];
        string expected = $"Hello world{Environment.NewLine}{Environment.NewLine}See you later{Environment.NewLine}";

        renderer.RenderGroup(textGroup);

        var result = renderer.GetBuffer();
        result.Should()
            .Be(expected);
    }

    [Fact]
    public void Outputs_Table()
    {
        var renderer = GetRenderer();
        var table = new OutputEngine.Primitives.Table(
            [
                new OutputEngine.Primitives.TableColumn("Name"),
                new OutputEngine.Primitives.TableColumn("Age")
            ])
        {
            IncludeHeaders = true
        };
        table.TableData.Add([new OutputEngine.Primitives.Paragraph("Alice"), new OutputEngine.Primitives.Paragraph("25")]);
        table.TableData.Add([new OutputEngine.Primitives.Paragraph("Bob"), new OutputEngine.Primitives.Paragraph("30")]);
        table.IncludeHeaders = true;
        var expected = "Name  Age\r\nAlice 25 \r\nBob   30 \r\n";


        renderer.RenderTable(table);

        var result = renderer.GetBuffer();
        result.Should()
            .Be(expected);
    }
}