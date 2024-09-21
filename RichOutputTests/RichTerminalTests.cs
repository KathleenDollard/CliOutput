using FluentAssertions;
using OutputEngine.Targets;
using OutputEngine.Primitives;
using OutputEngine;
using Spectre.Console.Testing;
using Spectre.Console;

namespace CliOutput.Test;

public class RichTerminalTests
{
    public string Expected { get; private set; }

    private TestConsole GetTestConsole()
        => new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();
    private static RichTerminal GetWriter(TestConsole testConsole)
        => new RichTerminal(new OutputContext(true), testConsole, true);

    [Theory]
    [InlineData("Hello World", "Hello World")]
    public void Outputs_string(string text, string expected)
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);

        writer.Write(text);

        var result = testConsole.Output;
        result.Should()
            .Be(expected);
    }


    [Fact]
    public void Outputs_string_with_style()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var expected = "\u001b[1mHello world\u001b[0m";

        //testConsole.Markup("[yellow]Hello world[/]");
        writer.Write("[bold]Hello world[/]");

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        string expected = $"Hello world{Environment.NewLine}";

        writer.WriteLine("Hello world");

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var textPart = new TextPart("Hello world");
        string expected = "Hello world";

        writer.Write(textPart);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_important()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var textPart = new TextPart("Hello world", Appearance.Important);
        string expected = "\u001b[1mHello world\u001b[0m";

        writer.Write(textPart);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_inline_code()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var textPart = new TextPart("Hello world", Appearance.InlineCode);
        string expected = "\u001b[1mHello world\u001b[0m";

        writer.Write(textPart);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }
    [Fact]
    public void Outputs_TextPart_as_error()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var textPart = new TextPart("Hello world", Appearance.Error);
        string expected = "\u001b[1;91mHello world\u001b[0m";

        writer.Write(textPart);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_TextPart_as_warning()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var textPart = new TextPart("Hello world", Appearance.Warning);
        string expected = "\u001b[1;33mHello world\u001b[0m";

        writer.Write(textPart);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Paragraph()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var paragraph =
                new OutputEngine.Primitives.Paragraph()
                {
                        new TextPart("Hello"),
                        new TextPart("world")
                };
        string expected = $"Hello world{Environment.NewLine}";

        writer.Write(paragraph);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Paragraph_as_Important()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        var paragraph =
                new OutputEngine.Primitives.Paragraph()
                {
                    new TextPart("Hello"),
                    new TextPart("world")
                };
        paragraph.Appearance = Appearance.Important;
        string expected = $"\u001b[1mHello world\u001b[0m{Environment.NewLine}";

        writer.Write(paragraph);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    /*
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
    */

    [Fact]
    public void Outputs_Section_header()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
        Section section = new Section("Greeting");
        string expected = $"\u001b[1;33mGreeting\u001b[0m{Environment.NewLine}{Environment.NewLine}";

        writer.Write(section);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    [Fact]
    public void Outputs_Section()
    {
        var testConsole = GetTestConsole();
        var writer = GetWriter(testConsole);
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

        writer.Write(section);

        var result = testConsole.Output;
        result.Should()
                .Be(expected);
    }

    /*
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
    */
}