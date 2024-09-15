using CliOutput.Primitives;
using FluentAssertions;
using CliOutput.Help;
using CliOutput.Targets;

namespace CliOutput.Test;

public class TerminalTests
{
    [Fact]
    public void Outputs_string()
    {
        var writer = new PlainHelpTerminal(true);

        writer.Write("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_string_with_newline()
    {
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine("Hello world");

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_TextPart()
    {
        var writer = new PlainHelpTerminal(true);
        var textPart = new TextPart("Hello world");

        writer.Write(textPart);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_TextGroup()
    {
        var writer = new PlainHelpTerminal(true);
        TextGroup textGroup = [
            new TextPart("Hello"),
            new TextPart("world")
            ];

        writer.Write(textGroup);

        var result = writer.GetBuffer();
        result.Should()
            .Be("Hello world");
    }

    [Fact]
    public void Outputs_TextGroup_with_newline()
    {
        var writer = new PlainHelpTerminal(true);
        TextGroup textGroup = [
            new TextPart("Hello"),
            new TextPart("world")
            ];

        writer.WriteLine(textGroup);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Hello world{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_description()
    {
        var command = new CliCommand("Hello", "World");
        var help = Help.Help.Create(command);
        var usage = help.Sections.OfType<HelpDescription>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Description:{Environment.NewLine}  World{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage()
    {
        var command = new CliCommand("Hello", "World");
        var help = Help.Help.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_subcommands()
    {
        var command = new CliCommand("Hello", "World");
        var subcommand1 = new CliCommand("Welcome", "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        command.AddSubCommand(subcommand2);
        var help = Help.Help.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello Welcome Brrr{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_parents()
    {
        var command = new CliCommand("Hello", "World");
        var subcommand1 = new CliCommand("Welcome", "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        subcommand1.AddSubCommand(subcommand2);
        var help = Help.Help.Create(subcommand2);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello Welcome Brrr{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_arguments()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("Evening", "Now, it is the evening"));
        var help = Help.Help.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello <MORNING><EVENING>{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_options()
    {
        var command = new CliCommand("Hello", "World");
        command.Options.Add(new CliOption("Morning", "It is the morning."));
        command.Options.Add(new CliOption("Evening", "Now, it is the evening"));
        var help = Help.Help.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello [OPTIONS]{Environment.NewLine}");
    }


    [Fact]
    public void Outputs_arguments()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("Evening", "Now, it is the evening"));
        var help = Help.Help.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var writer = new PlainHelpTerminal(true);

        writer.WriteLine(arguments);

        var result = writer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello [OPTIONS]{Environment.NewLine}");
    }

}